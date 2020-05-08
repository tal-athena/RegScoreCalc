using DocumentsServiceInterfaceLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormPythonSettings : Form
	{
        #region Fields

        #endregion

        #region Properties
        public string AnacondaPath { get; set; }
        public string VirtualEnv { get; set; }
        public string PythonFullVersion { get; set; }
        public int PythonVersion
        {
            get {
                if (string.IsNullOrEmpty(PythonFullVersion))
                    return 2;
                return Int32.Parse(PythonFullVersion.Substring(0, 1));
            }
        }
        #endregion

        #region Ctors

        public FormPythonSettings(string condaPath, string Env)
		{
		    InitializeComponent();

            this.BackColor = MainForm.ColorBackground;

            AnacondaPath = condaPath;
            VirtualEnv = String.IsNullOrEmpty(Env) ? "base" : Env;

		}

		#endregion

		#region Events

		private void Form_Load(object sender, EventArgs e)
		{
			try
			{
                

                if (AnacondaPath == "")
                {
                    radioDefault.Checked = true;
                    radioSpecific.Checked = false;

                }
                else
                {
                    textAnacondaPath.Text = AnacondaPath;

                    radioSpecific.Checked = true;
                    radioDefault.Checked = false;                   
                }
            }
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void FormColumns_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					if (radioDefault.Checked == true)
                    {
                        AnacondaPath = "";
                        VirtualEnv = "base";
                        PythonFullVersion = "2";
                    }
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
        private void radioDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (radioDefault.Checked == true)
            {                
                lblPythonVersion.Text = "Python version: " + GetPythonVersion();
                lblAnacondaVersion.Text = "Anaconda version: " + GetAnacondaVersion();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
             
            FolderBrowserDialog condaDialog = new FolderBrowserDialog();            
            if (condaDialog.ShowDialog() == DialogResult.OK)
            {
                textAnacondaPath.Text = condaDialog.SelectedPath;              
            }
        }
        private void VirtualEnvironmentChanged(object sender, EventArgs e)
        {
            if (radioSpecific.Checked == true)
            {
                VirtualEnv = listEnvironment.SelectedItem.ToString();
                lblPythonVersion.Text = "Python version: " + GetPythonVersion();
            }
        }
        private void radioSpecific_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSpecific.Checked == true)
            {                
                AnacondaPath = textAnacondaPath.Text;

                if (AnacondaPath != "")
                    RefreshAnacondaPath();
                else
                {
                    lblPythonVersion.Text = "Python version: ";
                    lblAnacondaVersion.Text = "Anaconda version: ";
                }
            }
        }

        private void textAnacondaPath_TextChanged(object sender, EventArgs e)
        {
            if (AnacondaPath != textAnacondaPath.Text)
            {
                AnacondaPath = textAnacondaPath.Text;

                RefreshAnacondaPath();
            }
            
        }
        #endregion

        #region Implementation

        protected void RefreshAnacondaPath()
        {
            listEnvironment.Items.Clear();            

            List<string> envList = GetVirtualEnvironments();
            listEnvironment.Items.AddRange(envList.ToArray());

            int i = 0;
            for (i = 0; i < listEnvironment.Items.Count; i++)
            {
                if (listEnvironment.Items[i].ToString() == VirtualEnv)
                {
                    listEnvironment.SelectedIndex = i;
                    break;
                }
            }
            if (listEnvironment.Items.Count > 0 && i == listEnvironment.Items.Count)
            {
                listEnvironment.SelectedIndex = 0;
                VirtualEnv = "base";
            }

            lblAnacondaVersion.Text = "Anaconda version: " + GetAnacondaVersion();
        }

        protected List<string> GetVirtualEnvironments()
        {
            List<string> envList = new List<string>();
            var _process = new Process
            {
                StartInfo = new ProcessStartInfo("cmd.exe")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };

            
            _process.Start();

            
            if (!String.IsNullOrEmpty(AnacondaPath))
            {
                string condaEnvList;

                condaEnvList = "\"" + Path.Combine(AnacondaPath, "Scripts", "conda") + "\"" + " env list";

                _process.StandardInput.WriteLine(condaEnvList);
            }            
            
            _process.StandardInput.WriteLine("exit");
                        
            _process.WaitForExit();

            StreamReader reader = _process.StandardOutput;
            string result = reader.ReadToEnd();
            
            //MessageBox.Show(result);
            var lines = result.Replace("\r", "").Split('\n');

            int i;
            for (i = 0; i < lines.Count(); i++)
            {
                if (lines[i].Contains("# conda environments:"))
                {
                    i += 2;
                    break;                    
                }
            }
            while (i < lines.Count() && !lines[i].Contains("exit"))
            {
                if (!String.IsNullOrWhiteSpace(lines[i]))
                    envList.Add(lines[i].Split(' ')[0]);
                i++;
            }
            
            return envList;
        }
        protected string GetPythonVersion()
        {            
            var _process = new Process
            {
                StartInfo = new ProcessStartInfo("cmd.exe")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };


            _process.Start();

            if (radioSpecific.Checked == true)
            {
                if (!String.IsNullOrEmpty(AnacondaPath))
                {
                    string activateEnv;

                    activateEnv = "\"" + Path.Combine(AnacondaPath, "Scripts", "activate.bat") + "\"" + " " + VirtualEnv;

                    _process.StandardInput.WriteLine(activateEnv);
                }
            }
            _process.StandardInput.WriteLine("python --version");
            _process.StandardInput.WriteLine("exit");

            _process.WaitForExit();
                        
            string result = _process.StandardOutput.ReadToEnd() + _process.StandardError.ReadToEnd();

            //MessageBox.Show(result);

            if (!Regex.IsMatch(result, @"Python\s([\d.]+)"))
                return "python environment are not found where specified.";

            PythonFullVersion = Regex.Match(result, @"Python\s([\d.]+)").ToString().Replace("Python ", "");
                
            return PythonFullVersion;
        }
        protected string GetAnacondaVersion()
        {
            var _process = new Process
            {
                StartInfo = new ProcessStartInfo("cmd.exe")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };


            _process.Start();

            if (radioSpecific.Checked == true)
            {
                if (!String.IsNullOrEmpty(AnacondaPath))
                {
                    string activateEnv;

                    activateEnv = "\"" + Path.Combine(AnacondaPath, "Scripts", "conda.exe") + "\"" + " --version";

                    _process.StandardInput.WriteLine(activateEnv);
                }
            }
            else if (radioDefault.Checked == true)
            {
                string activateEnv;

                activateEnv = "conda --version";

                _process.StandardInput.WriteLine(activateEnv);
            }       
            _process.StandardInput.WriteLine("exit");

            _process.WaitForExit();
                        
            string result = _process.StandardOutput.ReadToEnd() + _process.StandardError.ReadToEnd();

            //MessageBox.Show(result);

          if (!Regex.IsMatch(result, @"conda\s([\d.]+)"))
                return "Anaconda environment are not found where specified.";

            string version = Regex.Match(result, @"conda\s([\d.]+)").ToString().Replace("conda ", "");           
                
            return version;
        }


        #endregion

        private void btnOpenConda_Click(object sender, EventArgs e)
        {
            Directory.SetCurrentDirectory("C:\\");
            string arguments = String.Format(@" ""/K"" {0}\Scripts\activate.bat {1}", AnacondaPath, AnacondaPath);
            var _process = new Process
            {
               
                StartInfo = new ProcessStartInfo("cmd.exe")
                {
                    UseShellExecute = true,                    
                    CreateNoWindow = false,
                    Arguments = arguments
                },                
            };

            _process.Start();
        }
    }
}