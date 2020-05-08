using DocumentsServiceInterfaceLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormRegExpSettings : Form
	{
        #region Fields

        private int _autoCalc;
	    
		#endregion

		#region Ctors

		public FormRegExpSettings(int AutoCalc)
		{
			InitializeComponent();

            this.BackColor = MainForm.ColorBackground;

            _autoCalc = AutoCalc;
        }

		#endregion

		#region Events

		private void Form_Load(object sender, EventArgs e)
		{
            try
            {
                FillComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
                if (this.DialogResult == DialogResult.OK)
                {
                    if (this.comboBox1.Items.IndexOf(this.comboBox1.Text) == -1)
                    {
                        e.Cancel = true;
                        MessageBox.Show("Please select correct");
                    }
                }
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{                
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}


		#endregion

		#region Implementation

		protected void FillComboBox()
		{
            comboBox1.Items.Add("never");
            comboBox1.Items.Add("1 change");
            comboBox1.Items.Add("5 changes");
            comboBox1.Items.Add("10 changes");

            switch (_autoCalc)
            {
                case 0:
                    comboBox1.Text = "never";
                    break;

                case 1:
                    comboBox1.Text = "1 change";
                    break;

                case 5:
                    comboBox1.Text = "5 changes";
                    break;

                case 10:
                    comboBox1.Text = "10 changes";
                    break;
            }
        }

        public int GetAutoCalc()
        {
            int index = this.comboBox1.Items.IndexOf(this.comboBox1.Text);

            if (index == 1)
                return 1;
            else if (index == 2)
                return 5;
            else if (index == 3)
                return 10;

            return 0;
        }
        #endregion
    }
}