using System;
using System.Windows.Forms;

using DocumentsServiceInterfaceLib;

namespace LocalDocumentsServiceProcess
{
	public class Program
	{
		public static int Main(string[] args)
		{
			var logger = new Logger();

			try
			{
				if (args.Length < 2)
					return (int) ErrorCode.InvalidPipeName;

				///////////////////////////////////////////////////////////////////////////////

				if (Properties.Settings.Default.ShowStartupMessage)
					MessageBox.Show("Documents service tool started");

				///////////////////////////////////////////////////////////////////////////////

				Console.WriteLine("Starting documents service...");

				var server = new DocumentsServer(logger);

				var watcher = new ParentProcessWatcher(logger, server);
				watcher.StartWatching(Convert.ToInt32(args[0]));

				var exitCode = server.StartServer(args[1]);

				watcher.Terminate();

				return (int) exitCode;
			}
			catch (Exception ex)
			{
				logger.HandleException(ex);

				return (int) ErrorCode.SeeLogs;
			}
		}
	}
}
