using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
    public partial class frmModifiedNotesPopUp : Form
    {
        private string newDatabaseName = "";

        public frmModifiedNotesPopUp(string NewDatabaseName)
        {
            InitializeComponent();
            newDatabaseName = NewDatabaseName;
        }

        public string GetNewName()
        {
            return txtNewDatabaseName.Text;
        }

        public int GetSelectedOption()
        {
            if (rbNewModified.Checked)
            {
                return 1;
            }
            else if (rbNewDatabase.Checked)
            {
                return 3;
            }
            return 2;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (rbNewModified.Checked)
            {
                if (txtNewDatabaseName.Text.Length == 0)
                {
                    txtNewDatabaseName.Focus();
                }
            }
        }

        private void frmModifiedNotesPopUp_Load(object sender, EventArgs e)
        {
            txtNewDatabaseName.Text = newDatabaseName;
        }

        private void rbNewModified_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
