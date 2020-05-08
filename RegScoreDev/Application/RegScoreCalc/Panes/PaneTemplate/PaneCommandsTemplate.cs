using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace RegScoreCalc
{
	public class PaneCommandsTemplate : PaneCommands
	{
		#region Data members

		#endregion

		#region Ctors

		public PaneCommandsTemplate(ViewsManager views, RibbonPanel panel)
			: base(views, panel)
		{
			
		}

		#endregion

		#region Events

		protected void OnCustomCommand_Clicked(object sender, EventArgs e)
		{
			MessageBox.Show("Custom command");

			RaiseDataModifiedEvent();
		}

		#endregion

		#region Overrides

		protected override void InitCommands(RibbonPanel panel)
		{
			base.InitCommands(panel);

            RibbonButton btnCustomCommand = new RibbonButton("Custom command");
            panel.Items.Add(btnCustomCommand);

			btnCustomCommand.Click += new EventHandler(OnCustomCommand_Clicked);
            btnCustomCommand.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

		}

		#endregion

		#region Implementation

		#endregion
	}
}
