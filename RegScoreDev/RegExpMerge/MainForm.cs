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

namespace RegExpMerge
{
    public partial class MainForm : Form
    {
        private bool leftDbLoaded = false;
        private bool rightDbLoaded = false;
        private RibbonButton btnSaveRight;
        private RibbonButton btnSaveLeft;
        private RibbonButton btnSaveAll;

	    private string[] columnNames = new string[] { "Arithmetic factor", "exceptions", "lookahead" ,
        "lookbehind","neg lookahead", "neg lookbehind","prefix match","RegExpColor","score","suffix match"};

        private Font boldFont = new Font(DefaultFont, FontStyle.Bold);

        public MainForm()
        {
            InitializeComponent();

            InitPane();
            InitHistroyInCombobox();
            InitGridViews();
        }

        private void InitGridViews()
        {
            dataGridViewLeft.DataSource = this.bindingSourceLeft;
            dataGridViewRight.DataSource = this.bindingSourceRight;

            dataGridViewLeft.Columns["hdnImportanceValue"].Visible = false;
            dataGridViewLeft.Columns["hdnBoldFileds"].Visible = false;

            dataGridViewRight.Columns["hdnImportanceValue"].Visible = false;
            dataGridViewRight.Columns["hdnBoldFileds"].Visible = false;
        }

        private void InitHistroyInCombobox()
        {
            //Load history from file
            var left = Properties.Settings.Default.leftArray;
            if (left == null)
            {
                left = new string[0];
                Properties.Settings.Default.leftArray = left;
                Properties.Settings.Default.Save();
            }
            foreach (var item in left)
            {
                cmbLeftHistory.Items.Add(item);
            }

            var right = Properties.Settings.Default.rightArray;
            if (right == null)
            {
                right = new string[0];
                Properties.Settings.Default.rightArray = right;
                Properties.Settings.Default.Save();
            }
            foreach (var item in right)
            {
                cmbRightHistory.Items.Add(item);
            }
        }

        private void InitPane()
        {
            var panel = new RibbonPanel("Default");

            var btnCopyToRight = new RibbonButton("Copy to right");
            panel.Items.Add(btnCopyToRight);
            btnCopyToRight.Click += btnCopyToRight_Click;
            btnCopyToRight.Image = Properties.Resources.right_normal;
            btnCopyToRight.SmallImage = Properties.Resources.right_normal;

            var btnCopyToLeft = new RibbonButton("Copy to left");
            panel.Items.Add(btnCopyToLeft);
            btnCopyToLeft.Click += btnCopyToLeft_Click;
            btnCopyToLeft.Image = Properties.Resources.left_normal;
            btnCopyToLeft.SmallImage = Properties.Resources.left_normal;


            var delimiter1 = new RibbonSeparator();
            panel.Items.Add(delimiter1);


            btnSaveLeft = new RibbonButton("Save left");
            panel.Items.Add(btnSaveLeft);
            btnSaveLeft.Click += btnSaveLeft_Click;
            btnSaveLeft.Image = Properties.Resources.Save;
            btnSaveLeft.SmallImage = Properties.Resources.Save;
            btnSaveLeft.Enabled = false;

            btnSaveRight = new RibbonButton("Save right");
            panel.Items.Add(btnSaveRight);
            btnSaveRight.Click += btnSaveRight_Click;
            btnSaveRight.Image = Properties.Resources.Save;
            btnSaveRight.SmallImage = Properties.Resources.Save;
            btnSaveRight.Enabled = false;

            btnSaveAll = new RibbonButton("Save all");
            panel.Items.Add(btnSaveAll);
            btnSaveAll.Click += btnSaveAll_Click;
            btnSaveAll.Image = Properties.Resources.SaveAll;
            btnSaveAll.SmallImage = Properties.Resources.SaveAll;
            btnSaveAll.Enabled = false;


            var delimiter = new RibbonSeparator();
            panel.Items.Add(delimiter);


            var btnRecalculate = new RibbonButton("Re-calculate");
            panel.Items.Add(btnRecalculate);
            btnRecalculate.Click += btnRecalculate_Click;
            btnRecalculate.Image = Properties.Resources.CalcScores;
            btnRecalculate.SmallImage = Properties.Resources.CalcScores;


            this.ribbonTabMain.Panels.Add(panel);


            this.splitContainer.SplitterDistance = this.Width / 2;
        }

       
        #region "Ribbon buttons"

        void btnRecalculate_Click(object sender, EventArgs e)
        {
            ProcessData();

            MessageBox.Show("Finished!");
        }

        void btnSaveAll_Click(object sender, EventArgs e)
        {
            SaveLeftDatabase();
            SaveRightDatabase();
            MessageBox.Show("Databases saved!");
        }

        void btnSaveLeft_Click(object sender, EventArgs e)
        {
            SaveLeftDatabase();
            MessageBox.Show("Left database saved!");
        }

        void btnSaveRight_Click(object sender, EventArgs e)
        {
            SaveRightDatabase();
            MessageBox.Show("Right database saved!");
        }

        private void SaveLeftDatabase()
        {
            try
            {
                dataGridViewLeft.EndEdit();
                bindingSourceLeft.EndEdit();

                adapterRegExpLeft.Update(datasetLeft.RegExp);

                dataGridViewLeft.BeginEdit(true);
                bindingSourceLeft.ResumeBinding();
            }
            catch
            {
                MessageBox.Show("Saving of the left database failed");
            }
        }
        private void SaveRightDatabase()
        {
            try
            {
                dataGridViewRight.EndEdit();
                bindingSourceRight.EndEdit();

                adapterRegExpRight.Update(datasetRight.RegExp);

                dataGridViewRight.BeginEdit(true);
                bindingSourceRight.ResumeBinding();
            }
            catch
            {
                MessageBox.Show("Saving of the right database failed");
            }
        }


        private void btnCopyToLeft_Click(object sender, EventArgs e)
        {
            if (dataGridViewRight.SelectedRows.Count > 0)
            {
                CopyRows(dataGridViewRight.SelectedRows, datasetLeft);
            }

        }
        private void btnCopyToRight_Click(object sender, EventArgs e)
        {

            if (dataGridViewLeft.SelectedRows.Count > 0)
            {
                CopyRows(dataGridViewLeft.SelectedRows, datasetRight);
            }
        }

        private void CopyRows(DataGridViewSelectedRowCollection SelectedRows, Data.DataSetMain dataset)
        {
            foreach (DataGridViewRow item in SelectedRows)
            {
                try
                {
                    bool newRowCreated = false;
                    var newRow = dataset.RegExp.FindByRegExp(item.Cells["RegExp"].Value.ToString());
                    if (newRow == null)
                    {
                        newRow = dataset.RegExp.NewRegExpRow();
                        newRowCreated = true;

                        newRow.RegExp = item.Cells["RegExp"].Value.ToString();
                    }
                    //var newRow = dataset.RegExp.NewRegExpRow();
                    newRow.hdnImportanceValue = 1;
                    newRow.hdnBoldFileds = "";
                    if (!String.IsNullOrEmpty(item.Cells["Arithmetic factor"].Value.ToString()))
                    {
                        newRow.Arithmetic_factor = (int)item.Cells["Arithmetic factor"].Value;
                    }
                    if (!String.IsNullOrEmpty(item.Cells["exceptions"].Value.ToString()))
                    {
                        newRow.exceptions = item.Cells["exceptions"].Value.ToString();
                    }
                    if (!String.IsNullOrEmpty(item.Cells["lookahead"].Value.ToString()))
                    {
                        newRow.lookahead = item.Cells["lookahead"].Value.ToString();
                    }
                    if (!String.IsNullOrEmpty(item.Cells["lookbehind"].Value.ToString()))
                    {
                        newRow.lookbehind = item.Cells["lookbehind"].Value.ToString();
                    }
                    if (!String.IsNullOrEmpty(item.Cells["neg lookahead"].Value.ToString()))
                    {
                        newRow.neg_lookahead = item.Cells["neg lookahead"].Value.ToString();
                    }
                    if (!String.IsNullOrEmpty(item.Cells["neg lookbehind"].Value.ToString()))
                    {
                        newRow.neg_lookbehind = item.Cells["neg lookbehind"].Value.ToString();
                    }
                    if (!String.IsNullOrEmpty(item.Cells["prefix match"].Value.ToString()))
                    {
                        newRow.prefix_match = item.Cells["prefix match"].Value.ToString();
                    }
                    if (!String.IsNullOrEmpty(item.Cells["RegExpColor"].Value.ToString()))
                    {
                        newRow.RegExpColor = item.Cells["RegExpColor"].Value.ToString();
                    }
                    if (!String.IsNullOrEmpty(item.Cells["score"].Value.ToString()))
                    {
                        newRow.score = (int)item.Cells["score"].Value;
                    }
                    if (!String.IsNullOrEmpty(item.Cells["suffix match"].Value.ToString()))
                    {
                        newRow.suffix_match = item.Cells["suffix match"].Value.ToString();
                    }

                    if (newRowCreated)
                    {
                        dataset.RegExp.AddRegExpRow(newRow);
                    }
                    else
                    {
                        //Copy from second to the first, so they can now have the same elements
                        if (!newRow.IsArithmetic_factorNull())
                        {
                            item.Cells["Arithmetic factor"].Value = newRow.Arithmetic_factor;
                        }
                        if (!newRow.IsexceptionsNull())
                        {
                            item.Cells["exceptions"].Value = newRow.exceptions;
                        }
                        if (!newRow.IslookaheadNull())
                        {
                            item.Cells["lookahead"].Value = newRow.lookahead;
                        }
                        if (!newRow.IslookbehindNull())
                        {
                            item.Cells["lookbehind"].Value = newRow.lookbehind;
                        }
                        if (!newRow.Isneg_lookaheadNull())
                        {
                            item.Cells["neg lookahead"].Value = newRow.neg_lookahead;
                        }
                        if (!newRow.Isneg_lookbehindNull())
                        {
                            item.Cells["neg lookbehind"].Value = newRow.neg_lookbehind;
                        }
                        if (!newRow.Isprefix_matchNull())
                        {
                            item.Cells["prefix match"].Value = newRow.prefix_match;
                        }
                        if (!newRow.IsRegExpColorNull())
                        {
                            item.Cells["RegExpColor"].Value = newRow.RegExpColor;
                        }
                        if (!newRow.IsscoreNull())
                        {
                            item.Cells["score"].Value = newRow.score;
                        }
                        if (!newRow.Issuffix_matchNull())
                        {
                            item.Cells["suffix match"].Value = newRow.suffix_match;
                        }
                    }

                    //This row now is not different from the row in the other table
                    item.Cells["hdnImportanceValue"].Value = 1;
                    item.Cells["hdnBoldFileds"].Value = "";

                    //Sort and color rows
                    SortAndOrderRows();
                }
                catch { }
            }
        }



        #endregion

        private void btnOpenLeft_Click(object sender, EventArgs e)
        {
            //Open database left
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MS Access database (*.accdb;*.mdb)|*.accdb;*.mdb|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var filename = openFileDialog.FileName;
                //Save to history??
                OpenLeftDatabase(filename);
            }
        }

        private void btnOpenRight_Click(object sender, EventArgs e)
        {
            //Open database left
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MS Access database (*.accdb;*.mdb)|*.accdb;*.mdb|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var filename = openFileDialog.FileName;
                //Save to history??
                OpenRightDatabase(filename);
            }
        }

        private void OpenLeftDatabase(string filename)
        {
            OleDbConnection newConnection = new OleDbConnection(Helpers.GetConnectionString(filename));

            this.adapterRegExpLeft.Connection = newConnection;
            this.adapterRegExpLeft.Fill(datasetLeft.RegExp);

            //Save connection to history
            List<string> leftHistory;
            if (Properties.Settings.Default["leftArray"] != null)
            {
                leftHistory = Properties.Settings.Default.leftArray.ToList();
            }
            else
            {
                leftHistory = new List<string>();
            }
            if (leftHistory.Count > 10)
            {
                leftHistory.RemoveAt(leftHistory.Count - 1);
            }
            leftHistory.RemoveAll(p => p == filename);
            leftHistory.Insert(0, filename);
            Properties.Settings.Default.leftArray = leftHistory.ToArray();
            Properties.Settings.Default.Save();

            //Add to combobox
            cmbLeftHistory.Items.Insert(0, filename);

            leftDbLoaded = true;

            //Process data and check for different rows
            ProcessData();

            btnSaveLeft.Enabled = true;
        }

        private void OpenRightDatabase(string filename)
        {
            OleDbConnection newConnection = new OleDbConnection(Helpers.GetConnectionString(filename));

            this.adapterRegExpRight.Connection = newConnection;
            this.adapterRegExpRight.Fill(datasetRight.RegExp);

            //Save connection to history
            List<string> rightHistory = Properties.Settings.Default.rightArray.ToList();
            if (rightHistory.Count > 10)
            {
                rightHistory.RemoveAt(rightHistory.Count - 1);
            }
            rightHistory.RemoveAll(p => p == filename);
            rightHistory.Insert(0, filename);
            Properties.Settings.Default.rightArray = rightHistory.ToArray();
            Properties.Settings.Default.Save();

            //Insert in combobox
            cmbRightHistory.Items.Insert(0, filename);
            rightDbLoaded = true;

            //Process data and check for different rows
            ProcessData();

            btnSaveRight.Enabled = true;
        }

        public void ProcessData()
        {
            //If the both databases are loaded
            if (leftDbLoaded && rightDbLoaded)
            {
                btnSaveAll.Enabled = true;

                Differences(datasetLeft.RegExp, datasetRight.RegExp);

                //var rightData = datasetRight.RegExp.Select("hdnImportanceValue IS NULL");
                //if (rightData.Length > 0)
                //{
                //    Data.DataSetMain.RegExpDataTable right = (Data.DataSetMain.RegExpDataTable)rightData.CopyToDataTable();
                //    Differences(right, datasetLeft.RegExp);
                //}

                Differences(datasetRight.RegExp, datasetLeft.RegExp);

                SortAndOrderRows();
            }
        }

        private void SortAndOrderRows()
        {
            dataGridViewLeft.Sort(dataGridViewLeft.Columns["hdnImportanceValue"], ListSortDirection.Ascending);
            dataGridViewRight.Sort(dataGridViewRight.Columns["hdnImportanceValue"], ListSortDirection.Ascending);


            //Color data in the data grid views
            ColorRows(dataGridViewLeft.Rows);
            ColorRows(dataGridViewRight.Rows);
        }

        private void ColorRows(DataGridViewRowCollection rows)
        {
            Font lboldFont = new Font(this.dataGridViewLeft.DefaultCellStyle.Font, FontStyle.Bold);
            foreach (DataGridViewRow row in rows)
            {
                if (!row.IsNewRow)
                {
                    //Return to default
                    foreach (var cell in columnNames)
                    {
                        row.Cells[cell].Style.Font = this.dataGridViewLeft.DefaultCellStyle.Font;
                    }

                    if (row.Cells["hdnImportanceValue"].Value.ToString() == "2")
                    {
                        //Get different columns and set them to bold
                        row.DefaultCellStyle.ForeColor = Color.Blue;

                        var fields = row.Cells["hdnBoldFileds"].Value.ToString();
                        var fieldsArray = fields.Split(',');

                        for (int i = 0; i < fieldsArray.Length - 1; i++)
                        {
                            row.Cells[fieldsArray[i]].Style.Font = boldFont;
                        }
                    }
                    else if (row.Cells["hdnImportanceValue"].Value.ToString() == "3")
                    {
                        //Set row color to green
                        row.DefaultCellStyle.ForeColor = Color.Green;
                    }
                    else
                    {
                        //Black normal font
                        row.DefaultCellStyle.ForeColor = Color.Black;
                     
                    }
                }
            }
        }

        private void Differences(Data.DataSetMain.RegExpDataTable table1, Data.DataSetMain.RegExpDataTable table2)
        {
            //Find differences
            foreach (var item in table1)
            {
                bool foundSimilar = false;
                foreach (var rItem in table2)
                {
                    if (item.RegExp == rItem.RegExp)
                    {
                        foundSimilar = true;
                        //They have the same regExp string, check for other fields
                        string fields = DifferentFields(item, rItem);
                        if (String.IsNullOrEmpty(fields))
                        {
                            //All fields are the same
                            item.hdnImportanceValue = 1;
                            //rItem.hdnImportanceValue = 1;
                        }
                        else
                        {
                            item.hdnImportanceValue = 2;
                            //rItem.hdnImportanceValue = 2;

                            //Store fields that should be bolded
                            item.hdnBoldFileds = fields;
                            //rItem.hdnBoldFileds = fields;
                        }

                        break;
                    }
                }
                if (!foundSimilar)
                {
                    //Not in the right
                    item.hdnImportanceValue = 3;
                }
            }
        }
        private string DifferentFields(Data.DataSetMain.RegExpRow item, Data.DataSetMain.RegExpRow rItem)
        {
            string fields = "";

            foreach (var col in columnNames)
            {
                if (item[col] != null)
                {
                    if (rItem[col] != null)
                    {
                        if (item[col].ToString() != rItem[col].ToString())
                        {
                            fields += col + ",";
                        }
                    }
                    else
                    {
                        fields += col + ",";
                    }
                }
            }

            return fields;
        }

        private void cmbLeftHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var filename = cmbLeftHistory.SelectedItem.ToString();
            OpenLeftDatabase(filename);
        }

        private void cmbRightHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var filename = cmbRightHistory.SelectedItem.ToString();
            OpenRightDatabase(filename);
        }

        private void dataGridViewLeft_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColorRows(dataGridViewLeft.Rows);
        }

        private void dataGridViewRight_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColorRows(dataGridViewRight.Rows);
        }

        private void dataGridViewLeft_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            //Update database
            //adapterRegExpLeft.Update(datasetLeft);
            datasetLeft.AcceptChanges();

            //Recalculate 
            ProcessData();
        }

        private void dataGridViewRight_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            //Update database
            //adapterRegExpRight.Update(datasetRight);
            datasetRight.AcceptChanges();

            //Recalculate 
            ProcessData();


        }

        private void dataGridViewLeft_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ProcessData();
        }

        private void dataGridViewRight_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ProcessData();
        }

        private void dataGridViewLeft_Scroll(object sender, ScrollEventArgs e)
        {
            dataGridViewRight.HorizontalScrollingOffset = dataGridViewLeft.HorizontalScrollingOffset;
            dataGridViewRight.FirstDisplayedScrollingRowIndex = dataGridViewLeft.FirstDisplayedScrollingRowIndex;
        }

        private void dataGridViewRight_Scroll(object sender, ScrollEventArgs e)
        {
            dataGridViewLeft.HorizontalScrollingOffset = dataGridViewRight.HorizontalScrollingOffset;
            dataGridViewLeft.FirstDisplayedScrollingRowIndex = dataGridViewRight.FirstDisplayedScrollingRowIndex;
        }

        private void dataGridViewLeft_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (leftDbLoaded && (e.ColumnIndex <= 10 && e.ColumnIndex >= 6))
            {
                var regExp = dataGridViewLeft.Rows[e.RowIndex].Cells[0].Value.ToString();

                //Open pop up
                RegExpMerge.Data.DataSetMain.RegExpRow row = datasetLeft.RegExp.FindByRegExp(regExp);
                ListPopUp formPopUpItems = new ListPopUp(row, e.ColumnIndex, this);
                //formPopUpItems.Location = dataGridViewLeft.PointToScreen(dataGridViewLeft.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location);
                formPopUpItems.Location = new Point(this.Left + 50, (this.Bottom / 2) - 150);
                formPopUpItems.Show();
            }
        }

        private void dataGridViewRight_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (rightDbLoaded && (e.ColumnIndex <= 10 && e.ColumnIndex >= 6))
            {
                var regExp = dataGridViewRight.Rows[e.RowIndex].Cells[0].Value.ToString();

                //Open pop up
                RegExpMerge.Data.DataSetMain.RegExpRow row = datasetRight.RegExp.FindByRegExp(regExp);
                ListPopUp formPopUpItems = new ListPopUp(row, e.ColumnIndex, this);
                formPopUpItems.Location = new Point(this.Width - 400, (this.Bottom / 2) - 150);
                //formPopUpItems.Location = dataGridViewRight.PointToScreen(dataGridViewRight.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location);
                formPopUpItems.Show();
            }
        }




    }
}
