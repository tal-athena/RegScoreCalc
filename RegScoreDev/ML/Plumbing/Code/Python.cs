using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Plumbing.Code
{
	public class Python
	{
		#region Fields

		protected Process _process;
		protected string _outputFolder;

		protected Logger _logger;

		#endregion

		#region Ctors

		public Python(string outputFolder, Logger logger)
		{
			_outputFolder = outputFolder;

			_logger = logger;
		}

		#endregion

		#region Events

		private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			try
			{
				_logger.LogPython(e.Data);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		private void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			try
			{
				_logger.LogError(e.Data);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		private void process_Exited(object sender, EventArgs e)
		{
			try
			{
				var message = "Machine learning process exited with code: " + _process.ExitCode;

				_logger.LogSection(message);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		#endregion

		#region Operations

		public int StartML(string trainFileName, string testFleName, string anacondaPath, string pythonEnv, int pythonVersion = 2, string processLanguage = "en")
		{
            string commandPath;

            if (pythonVersion == 3)
            {
                commandPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                                             .Location), "BinaryClassifier35");
            }
            else
            {
                commandPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                                             .Location), "BinaryClassifier");
            }

			Directory.SetCurrentDirectory(commandPath);


			var redirectConsole = !Properties.Settings.Default.ShowPythonWindow;

			_process = new Process
			{
				StartInfo = new ProcessStartInfo("cmd.exe")
							{
								UseShellExecute = false,
								RedirectStandardOutput = redirectConsole,
								RedirectStandardError = redirectConsole,
                                RedirectStandardInput = true,
                                CreateNoWindow = redirectConsole
							},
				EnableRaisingEvents = true
			};

			if (redirectConsole)
			{
				_process.OutputDataReceived += process_OutputDataReceived;
				_process.ErrorDataReceived += process_ErrorDataReceived;
			}

			_process.Exited += process_Exited;
			_process.Start();

            if (!String.IsNullOrEmpty(anacondaPath))
            {
                string activateEnv;
                
                activateEnv = "\"" + Path.Combine(anacondaPath, "Scripts", "activate.bat") + "\"" + " " + pythonEnv;
                    
                _process.StandardInput.WriteLine(activateEnv);
            }
            
            var pythonarguments = String.Format(@"master_new.py {0} {1} {2} -t {3}", trainFileName, testFleName, processLanguage,  "\"" + _outputFolder + "\"");
            _process.StandardInput.WriteLine("python " + pythonarguments);

            _process.StandardInput.WriteLine("exit");

            if (redirectConsole)
			{
				_process.BeginOutputReadLine();
				_process.BeginErrorReadLine();
			}

			_process.WaitForExit();

			return _process.ExitCode;
		}

		#endregion
	}

	public class StdEventArgs
	{
		#region Fields

		public string Message { get; set; }

		#endregion
	}
}
