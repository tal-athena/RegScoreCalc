using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Threading;
using System.Windows;

namespace RegScoreCalc.Code
{
	public class PythonNotebookServer
	{
		protected const string _urlPrefix = "http://localhost:";

		protected Process _process;
		protected ManualResetEvent _event;
		protected string _folder;
		protected int _defaultPort;
		protected int _actualPort;
		protected int _refCount;

        protected string _token;

		public PythonNotebookServer(string folder, int defaultPort)
		{
			_folder = folder;
			_defaultPort = defaultPort;
			_actualPort = defaultPort;
        }

        #region properties

        public string AccessToken
        {
            get { return _token;  }
        }

        #endregion

        public string StartServer(string anacondaPath, string pythonEnv, int pythonVersion)
		{
			Interlocked.Increment(ref _refCount);

            _token = null;

            var baseUrl = GetBaseUrl();

			if (_process != null && !_process.HasExited)
				return baseUrl;

			_process = new Process
			           {
				           StartInfo = new ProcessStartInfo
				                       {
					                       FileName = "cmd.exe",					                       
					                       WorkingDirectory = _folder,
										   RedirectStandardOutput = true,
					                       RedirectStandardError = true,
                                           RedirectStandardInput = true,
                                           UseShellExecute = false,
					                       CreateNoWindow = true
				                       },
				           EnableRaisingEvents = true
			           };

            ///////////////////////////////////////////////////////////////////////////////
            
			_process.ErrorDataReceived += process_ErrorDataReceived;
			_process.OutputDataReceived += (sender, args) => Debug.WriteLine(args.Data);

			if (_event == null)
				_event = new ManualResetEvent(false);
			else
				_event.Reset();

			if (!_process.Start())
				throw new Exception("Failed to start IPython");

            if (!String.IsNullOrEmpty(anacondaPath) && pythonEnv != "NULL")
            {
                var activateEnv = "\"" + Path.Combine(anacondaPath, "Scripts", "activate.bat") + "\"" + " " + pythonEnv;
                _process.StandardInput.WriteLine(activateEnv);
            }

            if (pythonVersion == 3)
            {
                var Arguments = "notebook --no-browser --port=" + _defaultPort;
                _process.StandardInput.WriteLine("jupyter " + Arguments);
            } else
            {
                var Arguments = "notebook --no-browser --port=" + _defaultPort;
                _process.StandardInput.WriteLine("ipython " + Arguments);
            }
            
            _process.StandardInput.WriteLine("exit");

            _process.BeginErrorReadLine();

			if (!_event.WaitOne(30000))
				throw new Exception("Failed to start IPython");

			baseUrl = GetBaseUrl();

			return baseUrl;
		}

		public bool StopServer()
		{
			try
			{
				if (_process == null || _process.HasExited)
				{
					Debug.WriteLine("IPYTHON exited");

					Interlocked.Exchange(ref _refCount, 0);
					return true;
				}

				var count = Interlocked.Decrement(ref _refCount);
				if (count <= 0)
				{
					Debug.WriteLine("NO REFS, killing...");

					_process.Kill();
					_process = null;
				}
				else
				{
					Debug.WriteLine(count + " refs");
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);

				return false;
			}

			return false;
		}

		private void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			try
			{
				if (!String.IsNullOrEmpty(e.Data))
				{
					var pos = e.Data.IndexOf(_urlPrefix, StringComparison.OrdinalIgnoreCase);
					if (pos != -1)
					{
						var port = String.Empty;

						var data = e.Data.Remove(0, pos + _urlPrefix.Length);
						foreach (var c in data)
						{
							if (Char.IsDigit(c))
								port += c;
							else
								break;
						}
                                                
                        var tokenPos = e.Data.IndexOf("?token=", StringComparison.OrdinalIgnoreCase);

                        if (tokenPos != -1)
                        {
                            _token = e.Data.Remove(0, tokenPos + 7);
                        }

						if (!String.IsNullOrEmpty(port))
						{
							Debug.WriteLine(_actualPort);
							_actualPort = Convert.ToInt32(port);
							_event.Set();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected string GetBaseUrl()
		{
			return _urlPrefix + _actualPort;
		}
	}
}