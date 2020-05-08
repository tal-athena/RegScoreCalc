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
	public partial class FormAddBrowserTab : Form
	{
        #region Fields

        List<string> _columnIds;
	    
		#endregion

		#region Ctors

		public FormAddBrowserTab(Pane tabPane, List<string> columns)
		{
			InitializeComponent();

            _columnIds = columns;

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
                    if (this.textBox_TabName.Text == "" || this.comboBox1.Items.IndexOf(this.comboBox1.Text) == -1)
                    {
                        e.Cancel = true;
                        MessageBox.Show("Please input correct information!");
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
            this.comboBox1.Items.AddRange(_columnIds.ToArray());
        }

        public string GetTabName()
        {
            return this.textBox_TabName.Text;
        }
        public string GetColumnName()
        {            
            return this.comboBox1.Text;
        }
        #endregion
    }
}