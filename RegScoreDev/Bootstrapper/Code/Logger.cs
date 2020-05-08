using System;
using System.IO;

namespace Bootstrapper.Code
{
	public static class Logger
	{
		#region Operations

		public static void WriteInfoToLog(string message)
		{
			WriteToLog(message);
		}

		public static void WriteErrorToLog(Exception ex)
		{
			WriteToLog(String.Format("{0}{1}{2}{3}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine));
		}

		public static void WriteToLog(string message)
		{
			message = DateTime.Now.ToString("dd/MM/yyyy   HH:mm:ss") + message;

			File.AppendAllText("Log.txt", message);
		}

		#endregion
	}
}