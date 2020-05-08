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
    public partial class AddNewFilter : Form
    {
        public string name;

        public AddNewFilter(string currentName, string title = "Add")
        {
			InitializeComponent();

            name = currentName;

            txtGroupName.Text = currentName;

			this.BackColor = MainForm.ColorBackground;
            this.Text = title;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtGroupName.Text != "")
            {
                name = txtGroupName.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(this,"Enter name!","Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
