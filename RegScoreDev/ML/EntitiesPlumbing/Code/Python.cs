using RegExpLib.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EntitiesPlumbing.Code
{
    public class Python
    {
        #region Fields

        protected Process _process;
        protected string _outputFolder;

        protected Logger _logger;
        protected Logger _progressor;

        protected EntitiesProcessingParam _param;
        #endregion

        #region Ctors

        public Python(EntitiesProcessingParam param, Logger logger, Logger progresser)
        {
            _outputFolder = param.WorkingFolder;

            _param = param;
            _logger = logger;
            _progressor = progresser;
        }

        #endregion

        #region Events

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (Regex.IsMatch(e.Data, @"^\d+$"))
                {
                    //_progressor.ClearLog();
                    _progressor.Log(e.Data);
                } else
                {
                    _logger.Log(e.Data);
                } 
                    
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
                {
                    _logger.LogSection(message);                    
                }
                    
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

        public int StartML(string pythonFile)
        {

            string commandPath;

            
            commandPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                                     .Location), "spaCyEntities");
            
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
                //_process.ErrorDataReceived += process_ErrorDataReceived;
            }

            _process.Exited += process_Exited;
            _process.Start();

            if (!String.IsNullOrEmpty(_param.AnacondaPath))
            {
                string activateEnv;

                activateEnv = "\"" + Path.Combine(_param.AnacondaPath, "Scripts", "activate.bat") + "\"" + " " + _param.VirtualEnv;

                _process.StandardInput.WriteLine(activateEnv);
            }

            var pythonarguments = String.Format(@"""{0}"" ""{1}""", pythonFile, _param.SqliteFilePath);
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
        public EntityLabelResult GetEntityNames (string pythonFile)
        {
            EntityLabelResult results = new EntityLabelResult();
            results.Labels = new List<string>();

            string commandPath;


            commandPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                                     .Location), "spaCyEntities");

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
                //_process.OutputDataReceived += process_OutputDataReceived;
                _process.ErrorDataReceived += process_ErrorDataReceived;
            }

            _process.Exited += process_Exited;
            _process.Start();

            if (!String.IsNullOrEmpty(_param.AnacondaPath))
            {
                string activateEnv;

                activateEnv = "\"" + Path.Combine(_param.AnacondaPath, "Scripts", "activate.bat") + "\"" + " " + _param.VirtualEnv;

                _process.StandardInput.WriteLine(activateEnv);
            }

            var pythonarguments = String.Format(@"""{0}""", pythonFile);
            _process.StandardInput.WriteLine("python " + pythonarguments);

            _process.StandardInput.WriteLine("exit");

            var output = _process.StandardOutput.ReadToEnd();

            bool start = false;

            foreach (var line in output.Replace("\r", "").Split('\n'))
            {
                if (line.Contains("--------------------------"))
                {
                    start = !start;
                    continue;
                }
                if (start)
                    results.Labels.Add(line);
            }

            _process.WaitForExit();

            return results;
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
