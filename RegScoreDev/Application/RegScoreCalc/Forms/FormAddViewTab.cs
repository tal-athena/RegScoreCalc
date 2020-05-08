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
	public partial class FormAddViewTab : Form
	{
        #region Fields
	    
		#endregion

		#region Ctors

		public FormAddViewTab(Pane tabPane)
		{
			InitializeComponent();

            

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
            this.comboBox1.Items.Add("List view");
            this.comboBox1.Items.Add("Event calendar view");
            this.comboBox1.Items.Add("Entities view");
        }

        public string GetTabName()
        {
            return this.textBox_TabName.Text;
        }
        public DocumentViewType GetViewType()
        {
            if (this.comboBox1.Text == "List view")
                return DocumentViewType.Html_ListView;
            else if (this.comboBox1.Text == "Event calendar view")
                return DocumentViewType.Html_CalenderView;
            else if (this.comboBox1.Text == "Entities view")
                return DocumentViewType.Html_MedaCyEntityView;
            return DocumentViewType.Undefined;            
        }
        #endregion
    }
}