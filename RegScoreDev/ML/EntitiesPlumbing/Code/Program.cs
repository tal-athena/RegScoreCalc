using System;
using System.Windows.Forms;

using EntitiesPlumbing.Forms;

namespace EntitiesPlumbing.Code
{
	public static class Program
	{
		#region Entry point

		[STAThread]
		public static int Main(string[] arguments)
		{
			int result = 3;

			PlumbingCore plumbingCore = null;

			try
			{
				if (Properties.Settings.Default.ShowMessageOnStartup)
					MessageBox.Show("PLUMBING TOOL STARTED", "Plumbing Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);

				///////////////////////////////////////////////////////////////////////////////

				plumbingCore = new PlumbingCore(arguments);

				if (Properties.Settings.Default.ShowMainWindow)
				{
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);

					var formMain = new FormMain(plumbingCore);
					Application.Run(formMain);

					result = formMain.ProcessingResult;
				}
				else
				{
					result = plumbingCore.StartProcessing();
				}

			}
			catch (EmptyDatasetException ex)
			{
				if (plumbingCore != null && plumbingCore.Logger != null)
					plumbingCore.Logger.LogError(ex.Message);

				result = 3;
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message);

				result = 1;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);

				result = 2;
			}
			finally
			{
				if (plumbingCore != null)
					plumbingCore.Dispose();
			}

			return result;
		}

		#endregion
	}
}
