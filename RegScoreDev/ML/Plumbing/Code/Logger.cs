using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Plumbing.Code
{
	public class Logger
	{
		#region Constants

		protected const int _heartBeatInterval = 30000;

		#endregion

		#region Delegates

		public delegate void StdEventHandler(object sender, StdEventArgs e);

		public event StdEventHandler StdEvent;

		protected Timer _timer;

		#endregion

		#region Fields

		protected readonly string _logFilePath;

		#endregion

		#region Ctors

		public Logger(string logFilePath)
		{
			_logFilePath = logFilePath;

			_timer = new Timer(OnTimer, null, _heartBeatInterval, _heartBeatInterval);
		}

		#endregion

		#region Events

		private void OnTimer(object state)
		{
			try
			{
				LogMessage("HEARTBEAT", LogEntry.Heartbeat);
			}
			catch { }
		}

        #endregion

        #region Operations

        public void LogPython(string message)
        {
            message = message.Replace(">", "&gt;");
            
            LogMessage(message, LogEntry.Default);
        }

        public void Log(string message)
		{
			LogMessage(message, LogEntry.Default);
		}

		public void LogSection(string message, bool divide = true)
		{
			if (divide)
				LogMessage(message, LogEntry.SectionDivide);
			else
				LogMessage(message, LogEntry.Section);
		}

		public void LogError(string message)
		{
			LogMessage(message, LogEntry.Error);
		}

		protected void LogMessage(string message, LogEntry logEntry)
		{
			try
			{
				if (message == null)
					return;

				///////////////////////////////////////////////////////////////////////////////

				if (message == "Constraint")
					message = "Constraint exception:" + Environment.NewLine + "this may happen due to insertion of new row with existing ID";

				OnStdEvent(message + Environment.NewLine);

				string cssClass;
				switch (logEntry)
				{
					case LogEntry.Section:
					case LogEntry.SectionDivide:
						cssClass = "section";
						break;

					case LogEntry.Error:
						cssClass = "error";
						break;

					case LogEntry.Heartbeat:
						cssClass = "heartbeat";
						break;

					default:
						cssClass = string.Empty;
						break;
				}

				///////////////////////////////////////////////////////////////////////////////

				if (!String.IsNullOrEmpty(cssClass))
					message = "<p class=\"" + cssClass + "\">" + message + "</p>";
				else
					message = "<p>" + message + "</p>";

				message += Environment.NewLine;

				///////////////////////////////////////////////////////////////////////////////

				if (logEntry == LogEntry.SectionDivide)
					message = "<hr />" + message;

				///////////////////////////////////////////////////////////////////////////////

				lock (this)
				{
					//Debug.WriteLine(message);

					File.AppendAllText(_logFilePath, message);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("!!! ERROR !!! " + ex.Message);
			}
		}

		public void StopHeartBeat(WaitHandle wait)
		{
			try
			{
				_timer.Dispose(wait);
				_timer = null;
			}
			catch
			{
			}
		}

		#endregion

		#region Implementation

		protected void OnStdEvent(string line)
		{
			if (StdEvent != null)
				StdEvent(this, new StdEventArgs { Message = line });
		}

		#endregion
	}

	public enum LogEntry
	{
		#region Constants

		Default = 0,
		Section,
		SectionDivide,
		Error,
		Heartbeat

		#endregion
	}
}
