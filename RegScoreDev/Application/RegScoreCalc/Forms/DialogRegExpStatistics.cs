using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
    public partial class DialogRegExpStatistics : Form
    {

        public string selectedRegExp = "";
        public bool replace = false;
        public string replaceRegExp = "";
        ViewsManager _views;
        public DialogRegExpStatistics(ViewsManager views)
        {
            _views = views;
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ChooseAndExit();
        }

        private void lvRegExp_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChooseAndExit();
        }

        private void DialogRegExpStatistics_Load(object sender, EventArgs e)
        {
            //Load all items
            try
            {
                string cmdText = "SELECT * FROM [RegExpStatistics]";
                OleDbCommand cmd = new OleDbCommand(cmdText, _views.MainForm.adapterDocuments.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var regExp = reader[1].ToString().Trim();
                    var replace = reader[2].ToString();
                    //var regExpString = replace ? "Yes" : "";
                    var replaceText = reader[3].ToString().Trim();

                    //lvRegExp.Items.Add(new object[] { regExp, replace, replaceText });
                    ListViewItem listItem = new ListViewItem(new[] { regExp, replace, replaceText });
                    listItem.Tag = reader[0].ToString();
                    lvRegExp.Items.Add(listItem);
                }

            }
            catch
            {
                MessageBox.Show("No table in database", "Creating table", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
                string cmdText = "CREATE TABLE RegExpStatistics (ID AUTOINCREMENT, Regexp CHAR(255),IsReplace BIT, ReplacementText CHAR(255),Primary Key(ID))";
                OleDbCommand cmd = new OleDbCommand(cmdText, _views.MainForm.adapterDocuments.Connection);
                cmd.ExecuteNonQuery();

            }
        }

        private void ChooseAndExit()
        {
            //Check if there is a item selected
            if (lvRegExp.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvRegExp.SelectedItems[0];
                selectedRegExp = selectedItem.SubItems[0].Text;
                var replaceTxt = selectedItem.SubItems[1].Text;
                replace = true;
                if (replaceTxt == "False")
                {
                    replace = false;
                }
                replaceRegExp = selectedItem.SubItems[2].Text;


                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCodesEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvRegExp.SelectedItems.Count > 0)
                {
                    ListViewItem selectedItem = lvRegExp.SelectedItems[0];
                    var id = selectedItem.Tag.ToString();
                    var regExpFromList = selectedItem.SubItems[0].Text;
                    var replaceT = selectedItem.SubItems[1].Text;
                    var replaceFromList = true;
                    if (replaceT == "False")
                    {
                        replaceFromList = false;
                    }
                    var replaceTextFromList = selectedItem.SubItems[2].Text;

                    //Open new pop up for adding the data
                    AddRegExpStatistics popUp = new AddRegExpStatistics(regExpFromList, replaceFromList, replaceTextFromList);
                    if (popUp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        var regExp = popUp.regExp;
                        var replace = popUp.replace;
                        var replaceText = popUp.replaceText;
                        //Add to listbox

                        lvRegExp.SelectedItems[0].SubItems[0].Text = regExp;
                        lvRegExp.SelectedItems[0].SubItems[1].Text = replace.ToString();
                        lvRegExp.SelectedItems[0].SubItems[2].Text = replaceText;

                        //Update database
                        string cmdText = "UPDATE [RegExpStatistics] SET [Regexp] = ?,[IsReplace] = ?, [ReplacementText] = ? WHERE ID = " + id + ";";
                        OleDbCommand cmd = new OleDbCommand(cmdText, _views.MainForm.adapterDocuments.Connection);
                        cmd.Parameters.Add("Regexp", OleDbType.Char).Value = regExp;
                        cmd.Parameters.Add("IsReplace", OleDbType.Boolean).Value = replace;
                        cmd.Parameters.Add("ReplacementText", OleDbType.Char).Value = replaceText;
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch { }
        }

        private void btnCodesAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //Open new pop up for adding the data
                AddRegExpStatistics popUp = new AddRegExpStatistics("", false, "");
                if (popUp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    var regExp = popUp.regExp;
                    var replace = popUp.replace;
                    var replaceText = popUp.replaceText;

                    //Insert to database
                    string cmdText = "INSERT INTO [RegExpStatistics] ([Regexp],[IsReplace], [ReplacementText]) VALUES (?,?,?);";
                    OleDbCommand cmd = new OleDbCommand(cmdText, _views.MainForm.adapterDocuments.Connection);
                    cmd.Parameters.Add("RegExpStatistics", OleDbType.Char).Value = regExp;
                    cmd.Parameters.Add("IsReplace", OleDbType.Boolean).Value = replace;
                    cmd.Parameters.Add("ReplacementText", OleDbType.Char).Value = replaceText;
                    cmd.ExecuteNonQuery();

                    string query2 = "Select @@Identity";
                    cmd.CommandText = query2;
                    var tag = (int)cmd.ExecuteScalar();


                    //Add to listbox
                    ListViewItem listItem = new ListViewItem(new[] { regExp, replace.ToString(), replaceText });
                    listItem.Tag = tag.ToString();
                    lvRegExp.Items.Add(listItem);

                }
            }
            catch { }
        }

        private void btnCodesDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvRegExp.SelectedItems.Count > 0)
                {
                    var id = lvRegExp.SelectedItems[0].Tag.ToString();
                    lvRegExp.Items.Remove(lvRegExp.SelectedItems[0]);

                    //Remove from database
                    string cmdText = "DELETE FROM RegExpStatistics WHERE ID = " + id + ";";
                    OleDbCommand cmd = new OleDbCommand(cmdText, _views.MainForm.adapterDocuments.Connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }


    }
}
