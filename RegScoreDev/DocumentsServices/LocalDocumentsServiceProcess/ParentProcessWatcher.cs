using System;
using System.Diagnostics;
using System.Threading;

namespace LocalDocumentsServiceProcess
{
	internal class ParentProcessWatcher
	{
		protected Logger _logger;
		private readonly DocumentsServer _server;

		protected ManualResetEvent _event;

		protected Thread _thread;

		public ParentProcessWatcher(Logger logger, DocumentsServer server)
		{
			_logger = logger;
			_server = server;
		}

		public void StartWatching(int processID)
		{
			_event = new ManualResetEvent(false);

			_thread = new Thread(ThreadFunc);
			_thread.Start(processID);
		}

		protected void ThreadFunc(object state)
		{
			try
			{
				var processID = (int) state;

				while (!_event.WaitOne(5000))
				{
					var process = Process.GetProcessById(processID);
					if (process.HasExited)
					{
						_server.CloseConnection(false);

						Process.GetCurrentProcess().Kill();
					}
				}
			}
			catch (Exception ex)
			{
				_logger.HandleException(ex);

				_server.CloseConnection(false);

				Process.GetCurrentProcess().Kill();
			}
		}

		public void Terminate()
		{
			_event.Set();

			if (!_thread.Join(1000))
				_thread.Abort();
		}
	}
}
