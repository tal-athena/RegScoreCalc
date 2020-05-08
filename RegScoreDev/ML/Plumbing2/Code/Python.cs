using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Plumbing2.Code
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
				_logger.Log(e.Data);
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
                
				var message = "Python script process exited with code: " + _process.ExitCode;

                if (_process.ExitCode == 0)
				    _logger.LogSection(message);
                if (_process.ExitCode != 0)
                    _logger.LogError(message);
            }
			catch (Exception ex)
			{                
				Debug.WriteLine(ex.Message);
			}
		}

		#endregion

		#region Operations

		public int StartML(string pythonFile, string trainFileName)
		{
            
			
            var arguments = String.Format("run -it --rm -v {0}:/temp ubuntu/medacy:version1 /bin/bash -c '/temp/execute.sh'", _outputFolder);

            var fs = File.CreateText("execute.sh");                        
            fs.Write(String.Format("/root/anaconda3/bin/python '{0}' '{1}'", "/temp/"+ Path.GetFileName(pythonFile), "/temp/train.sqlite3"));            
            fs.Close();
            /*
            var arguments = String.Format("/C docker run -it -v {0}:/temp ubuntu/medacy:version1 /bin/bash", _outputFolder);
            arguments += " & " + String.Format("export PATH=$PATH:/root/anaconda3/bin");
            arguments += " & " + String.Format("source activate MyEnv");
            arguments += " & " + String.Format("cd /temp");
            arguments += " & " + String.Format("python {0} {1}", Path.GetFileName(pythonFile), "train.sqlite3");
            arguments += " & " + String.Format("exit & exit");
            */
            _process = new Process
			{
				StartInfo = new ProcessStartInfo("docker")
							{
								UseShellExecute = true,
                                Arguments = arguments,                                
								//RedirectStandardOutput = redirectConsole,
								//RedirectStandardError = redirectConsole,
                                //RedirectStandardInput = true,
                                CreateNoWindow = !Properties.Settings.Default.ShowPythonWindow,
                                WindowStyle = ProcessWindowStyle.Hidden
                            },
				EnableRaisingEvents = true
			};
            /*
			if (redirectConsole)
			{
				_process.OutputDataReceived += process_OutputDataReceived;
				_process.ErrorDataReceived += process_ErrorDataReceived;
			}
            */
			_process.Exited += process_Exited;
			_process.Start();
            /*
           // run docker container
           _process.StandardInput.WriteLine(String.Format("docker run -i -t -v {0}:/temp ubuntu/medacy:version1 /bin/bash", _outputFolder));
           
            // set conda path
            _process.StandardInput.WriteLine(String.Format("export PATH=$PATH:/root/anaconda3/bin"));

            // activate virtual Env
            _process.StandardInput.WriteLine(String.Format("source activate MyEnv"));

            // get into working directory
            _process.StandardInput.WriteLine(String.Format("cd /temp"));

            // run python file
            _process.StandardInput.WriteLine(String.Format("python {0} {1}", Path.GetFileName(pythonFile), "train.sqlite3"));
            
            // exit docker container
            _process.StandardInput.WriteLine("exit");
            // exit cmd
            _process.StandardInput.WriteLine("exit");

            if (redirectConsole)
			{
				_process.BeginOutputReadLine();
				_process.BeginErrorReadLine();
			}
           */
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
