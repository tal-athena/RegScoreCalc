using System;
using System.Data;
using System.Drawing;
using System.Data.OleDb;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace RegScoreCalc
{
	public partial class PaneReviewStatistics : Pane
	{
		#region Data members


		#endregion

		#region Ctors

        public PaneReviewStatistics()
		{
			InitializeComponent();
		}

		#endregion

		#region Events

		private void btnTest_Click(object sender, EventArgs e)
		{
			try
			{
				int nCount = _views.MainForm.datasetMain.ColorCodes.Rows.Count;
				MessageBox.Show("RegExp table contains " + nCount.ToString() + " record(s)");
			}
			catch { }
		}

		#endregion

		#region Operations

		#endregion

		#region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			base.InitPane(views, ownerView, panel, tab);
		}

		protected override void InitPaneCommands(RibbonTab tab)
		{
         
		}

		public override void UpdatePane()
		{
           
		}

		#endregion

        private void groupBoxCutoff_Enter(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click_1(object sender, EventArgs e)
        {

        }

        private void tbPrevalence_TextChanged(object sender, EventArgs e)
        {
            // TODO
        }

        private void checkBox_ManualPrev_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ManualPrev.Checked)
            {
                numericPrevalence.ReadOnly = false;
            }
            else
            {
                numericPrevalence.ReadOnly = true;
            }
        }

       

      
		#region Implementation

		#endregion
	}
}
