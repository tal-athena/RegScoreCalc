using System;
using System.Windows.Forms;

using DRTAccessFileSetup.Forms;

namespace DRTAccessFileSetup.Code
{
	static class Program
	{
		public static string AppName = "DRT Access File Setup";

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
		}

		public static void ShowInfoMessage(string message)
		{
			MessageBox.Show(message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public static void HandleException(Exception ex)
		{
			MessageBox.Show(ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}
