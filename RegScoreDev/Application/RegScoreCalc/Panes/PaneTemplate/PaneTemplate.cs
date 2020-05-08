using System;
using System.Data;
using System.Drawing;
using System.Data.OleDb;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace RegScoreCalc
{
	public partial class PaneTemplate : Pane
	{
		#region Data members

		protected PaneCommandsTemplate _commands;

		#endregion

		#region Ctors

		public PaneTemplate()
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
            RibbonPanel panel = new RibbonPanel("Template");

            tab.Panels.Add(panel);

			RibbonButton btnTestRibbon = new RibbonButton("Test");
            panel.Items.Add(btnTestRibbon);
			btnTestRibbon.Click += new EventHandler(btnTest_Click);
            btnTestRibbon.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			_commands = new PaneCommandsTemplate(_views, panel);
		}

		public override void UpdatePane()
		{
			try
			{
				int nCount = _views.MainForm.datasetMain.ColorCodes.Rows.Count;
				lblRecordsCount.Text = "Color codes table contains " + nCount.ToString() + " record(s)";
			}
			catch { }
		}

		#endregion

		#region Implementation

		#endregion
	}
}
