using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
    public partial class PopUpStatisticsNewTableName : Form
    {
        public string newName = "";
        OleDbConnection _conn;

        public PopUpStatisticsNewTableName(OleDbConnection conn)
        {
            _conn = conn;
            InitializeComponent();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //First check if there is already table in DB with choosen name
            if (String.IsNullOrEmpty(txtNewName.Text))
            {
                MessageBox.Show(this, "Please choose name for new table first..", "Empty name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                bool exists = false;
                newName = txtNewName.Text;
                try
                {
                    OleDbCommand cmd = new OleDbCommand("SELECT 1 FROM " + newName + " WHERE 1 = 0", _conn);
                    cmd.ExecuteNonQuery();
                    exists = true;
                }
                catch { }

                if (exists)
                {
                    MessageBox.Show(this, "Table with this name already exists in database. Please change it.", "Already exists", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
