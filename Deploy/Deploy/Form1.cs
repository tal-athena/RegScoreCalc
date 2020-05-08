using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Deploy
{
    public partial class Form1 : Form
    {
        private string _solutionFile;
        private string _outputFolder;
        private string DEVENVPATH;

        public Form1()
        {
            InitializeComponent();

            TextBox.CheckForIllegalCrossThreadCalls = false;
            Button.CheckForIllegalCrossThreadCalls = false;
            
        }

        private void OnFormClose(object sender, FormClosingEventArgs e)
        {            
            Properties.Settings.Default.VSPath = Path.GetDirectoryName(Path.GetDirectoryName(DEVENVPATH));
            Properties.Settings.Default.SolutionFile = _solutionFile;
            Properties.Settings.Default.OutputDirectory = _outputFolder;

            Properties.Settings.Default.x86 = chkX86.Checked;
            Properties.Settings.Default.x64 = chkX64.Checked;

            Properties.Settings.Default.Save();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            _solutionFile = Properties.Settings.Default.SolutionFile;
            _outputFolder = Properties.Settings.Default.OutputDirectory;
            DEVENVPATH = Properties.Settings.Default.VSPath + "\\Common\\IDE";

            chkX86.Checked = Properties.Settings.Default.x86;
            chkX64.Checked = Properties.Settings.Default.x64;

            lblSolution.Text = "Solution : " + _solutionFile;
            lblOutput.Text = "Output: " + _outputFolder;
            textVSPath.Text = Properties.Settings.Default.VSPath;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Solution file (*.sln)|*.sln";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                _solutionFile = fileDialog.FileName;
                lblSolution.Text = "Solution : " + _solutionFile;
            }
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                _outputFolder = folderBrowser.SelectedPath;
                lblOutput.Text = "Output: " + _outputFolder;
            }
        }

        private void btnDeploy_Click(object sender, EventArgs e)
        {

            DEVENVPATH = textVSPath.Text + "\\Common7\\IDE";            
            if (string.IsNullOrEmpty(_solutionFile) || string.IsNullOrEmpty(_outputFolder))
            {
                MessageBox.Show("solution file/output folder is not set");
                return;
            }
            if (!chkX86.Checked && !chkX64.Checked)
            {
                MessageBox.Show("Please select build option");
                return;
            }

            textOutputPane.Text = "";

            Thread childThread = new Thread(() => StartBuild(this, chkX86.Checked,chkX64.Checked, _solutionFile, _outputFolder));
            childThread.Start();            
        }

        private void StartBuild(Form1 form, bool x86, bool x64, string _solutionFile, string _outputFolder)
        {
            form.btnDeploy.Enabled = false;

            try
            {
                if (x86)
                {   
                    var lastFolder = Path.GetDirectoryName(_solutionFile);
                    var pathWithoutLastFolder = Path.GetDirectoryName(lastFolder);

                    var word2vecSln = Path.Combine(pathWithoutLastFolder, "word2vec", "vs2017") + "\\word2vec.sln";

                    form.BuildProject(_solutionFile, "RegScoreCalc", "x86");
                    form.BuildProject(_solutionFile, "DRTAccessFileSetup", "x86");
                    form.BuildProject(word2vecSln, "distance", "win32");
                    

                    string inputFolder = Path.Combine(Path.GetDirectoryName(_solutionFile), "Application", "RegScoreCalc", "bin", "x86", "Release");
                    CopyDir.Copy(inputFolder, Path.Combine(_outputFolder, "32bit"));

                    inputFolder = Path.Combine(Path.GetDirectoryName(_solutionFile), "DRTAccessFileSetup", "bin", "x86", "Release");
                    CopyDir.Copy(inputFolder, Path.Combine(_outputFolder, "32bit"));

                    Directory.CreateDirectory(Path.Combine(_outputFolder, "32bit", "word2vec"));
                    File.Copy(Path.Combine(Path.GetDirectoryName(word2vecSln), "Release", "distance.exe"), Path.Combine(_outputFolder, "32bit", "word2vec", "distance.exe"), true);                    
                }

                if (x64)
                {
                    var lastFolder = Path.GetDirectoryName(_solutionFile);
                    var pathWithoutLastFolder = Path.GetDirectoryName(lastFolder);

                    var word2vecSln = Path.Combine(pathWithoutLastFolder, "word2vec", "vs2017") + "\\word2vec.sln";

                    form.BuildProject(_solutionFile, "RegScoreCalc", "x64");
                    form.BuildProject(_solutionFile, "DRTAccessFileSetup", "x64");

                    form.BuildProject(word2vecSln, "distance", "x64");

                    string inputFolder = Path.Combine(Path.GetDirectoryName(_solutionFile), "Application", "RegScoreCalc", "bin", "x64", "Release");
                    CopyDir.Copy(inputFolder, Path.Combine(_outputFolder, "64bit"));
                    inputFolder = Path.Combine(Path.GetDirectoryName(_solutionFile), "DRTAccessFileSetup", "bin", "x64", "Release");
                    CopyDir.Copy(inputFolder, Path.Combine(_outputFolder, "64bit"));

                    Directory.CreateDirectory(Path.Combine(_outputFolder, "64bit", "word2vec"));                    
                    File.Copy(Path.Combine(Path.GetDirectoryName(word2vecSln), "Release", "distance.exe"), Path.Combine(_outputFolder, "32bit", "word2vec", "distance.exe"), true);
                }
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            form.btnDeploy.Enabled = true;
        }

        public void BuildProject(string slnName, string projectName, string platform)
        {
            try
            {
                

                var arguments = String.Format(@"""{0}"" /ReBuild Release|{3} /Project {1} /projectconfig ""release|{2}""", slnName, projectName, platform, platform);

                Process devEnvProcess = new Process
                {
                    StartInfo = new ProcessStartInfo(DEVENVPATH + "\\devenv.com", arguments)
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                    },
                    EnableRaisingEvents = true
                };

                // Set our event handler to asynchronously read the sort output.
                devEnvProcess.OutputDataReceived += new DataReceivedEventHandler(DevEnvOutputHandler);

                //distanceProcess.Exited += DevEnvProcessExited;

                devEnvProcess.Start();

                devEnvProcess.BeginOutputReadLine();

                devEnvProcess.WaitForExit();
                
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void DevEnvProcessExited(object sender, EventArgs e)
        {
            btnDeploy.Enabled = true;
        }

        private void DevEnvOutputHandler(object sender, DataReceivedEventArgs e)
        {            
            textOutputPane.Text += e.Data + Environment.NewLine;
            textOutputPane.SelectionStart = textOutputPane.TextLength;
            textOutputPane.ScrollToCaret();
        }
    }
    class CopyDir
    {
        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
