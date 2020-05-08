using System;
using System.IO;

namespace LocalDocumentsServiceProcess
{
	internal class Logger
	{
		protected const int _maxLogSize = 1024 * 100;

		public void HandleException(Exception ex)
		{
			try
			{
				var logFilePath = Path.Combine(Environment.CurrentDirectory, "LocalDocumentsServiceProcess.log");
				var fi = new FileInfo(logFilePath);
				if (fi.Exists && fi.Length > _maxLogSize)
					File.Delete(logFilePath);

				File.AppendAllText(logFilePath, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\t" + ex.Message + Environment.NewLine);
			}
			catch { }
		}
	}
}
