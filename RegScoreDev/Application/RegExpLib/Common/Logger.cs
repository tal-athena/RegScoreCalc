using System;
using System.Data;
using System.IO;
using System.Threading;

using RegExpLib.Core;

namespace RegExpLib.Common
{
	public class Logger
	{
		#region Fields

		protected readonly string _logFilePath;
		protected readonly string _progressFilePath;

		protected int _progressPercentage;
		protected long _progressValue;

		#endregion

		#region Ctors

		public Logger(string progressFilePath, string logFilePath)
		{
			_progressFilePath = progressFilePath;
			_logFilePath = logFilePath;
		}

		#endregion

		#region Operations

		public bool ReportProgress(int progressPercentage, long progressValue)
		{
			try
			{
				lock (this)
				{
					if (progressValue - _progressValue < 1000)
						return true;

					_progressValue = progressValue;

					///////////////////////////////////////////////////////////////////////////////

					var isCancellationPending = IsCancellationPending();
					if (isCancellationPending)
						return false;

					///////////////////////////////////////////////////////////////////////////////

					if (progressPercentage <= _progressPercentage)
						return true;

					_progressPercentage = progressPercentage;

					File.AppendAllText(_progressFilePath, _progressPercentage + Environment.NewLine);
				}
			}
			catch { }

			return true;
		}

		public void HandleException(Exception ex)
		{
			AppendToLog(ex.Message);
		}

		public void AppendToLog(string message)
		{
			lock (this)
			{
				message += Environment.NewLine;

				File.AppendAllText(_logFilePath, message);
			}
		}

		public void HandleRegExpException(DataRow row)
		{
			HandleRegExpException(RegExpBase.GetRegExpValue(row));
		}

		public void HandleRegExpException(string regExp)
		{
			try
			{
				var message = String.Format("Processing error:    " + "failed to build regular expression '{0}'", regExp);

				AppendToLog(message);
			}
			catch { }
		}

		public bool IsCancellationPending()
		{
			var result = false;

			try
			{
				const string eventName = "RegExpProcessor.exe";

				var cancellationEvent = new EventWaitHandle(false, EventResetMode.ManualReset, eventName);
				result = cancellationEvent.WaitOne(0);
			}
			catch { }

			return result;
		}

		#endregion
	}
}
