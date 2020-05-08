using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFToAccess
{
    public partial class Form1 : Form
    {

        private string _inputFolderPath;
        private string _outputFileName;

        private Thread _cvtThread;

        #region property        

        private bool IsConverting { get; set; }

        #endregion

        #region ctor

        public Form1()
        {
            InitializeComponent();

            _cvtThread = null;
            IsConverting = false;

            ProgressBar.CheckForIllegalCrossThreadCalls = false;
            Button.CheckForIllegalCrossThreadCalls = false;
            CheckBox.CheckForIllegalCrossThreadCalls = false;

        }
        #endregion

        #region event
        private void OnFormClosing(object sender, EventArgs e)
        {
            if (IsConverting && _cvtThread != null)
                _cvtThread.Abort();

            Properties.Settings.Default.InputFolder = _inputFolderPath;
            Properties.Settings.Default.OutputFile = _outputFileName;
            Properties.Settings.Default.RemoveLineWithSpaces = this.chkLineWithSpaces.Checked;
            Properties.Settings.Default.RemoveEmptyLine = this.chkLineWithNewLine.Checked;
            Properties.Settings.Default.TrimBeginSpaces = this.chkBeginSpaces.Checked;
            Properties.Settings.Default.TrimEndSpaces = this.chkEndSpaces.Checked;

            Properties.Settings.Default.Save();
        }


        private void OnFormLoad(object sender, EventArgs e)
        {
            _inputFolderPath = Properties.Settings.Default.InputFolder;
            _outputFileName = Properties.Settings.Default.OutputFile;

            this.lblInputFileName.Text = "Input Folder : " + _inputFolderPath;
            this.txtOutputFileName.Text = _outputFileName;

            this.chkLineWithSpaces.Checked = Properties.Settings.Default.RemoveLineWithSpaces;
            this.chkLineWithNewLine.Checked = Properties.Settings.Default.RemoveEmptyLine;
            this.chkBeginSpaces.Checked = Properties.Settings.Default.TrimBeginSpaces;
            this.chkEndSpaces.Checked = Properties.Settings.Default.TrimEndSpaces;
        }

        private void btnOpenSqlite_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();            

            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                _inputFolderPath = folderBrowser.SelectedPath;
                lblInputFileName.Text = "Input Folder : " + _inputFolderPath;
            }
        }

        private void btnSaveAccess_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();

            fileDialog.Filter = "Access File (*.accdb)|*.accdb";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                _outputFileName = fileDialog.FileName;
                txtOutputFileName.Text = _outputFileName;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (IsConverting)
            {
                return;
            }

            if (_inputFolderPath == null || _inputFolderPath == "" || _outputFileName == null || _outputFileName == "")
            {
                MessageBox.Show("Please select correct input path/ouput file");
                return;
            }

            try
            {
                btnConvert.Enabled = false;

                int count = Directory.GetFiles(_inputFolderPath, "*.pdf", SearchOption.AllDirectories).Count();
                this.progressBar1.Maximum = count;

                IsConverting = true;
                
                _cvtThread = new Thread(() => ConvertingThread(this, _inputFolderPath, _outputFileName));
                _cvtThread.Start();

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                btnConvert.Enabled = true;
                IsConverting = false;
                _cvtThread = null;
                return;
            }
            
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsConverting == true && _cvtThread != null)
            {
                _cvtThread.Abort();
                IsConverting = false;
                _cvtThread = null;
                btnConvert.Enabled = true;
            }
        }
        #endregion

        #region implement
        public void FinishConverting()
        {
            MessageBox.Show("Finished");

            IsConverting = false;
            progressBar1.Value = 0;
            btnConvert.Enabled = true;
            _cvtThread = null;
        }       

        public void SetProgressBarValue(int value)
        {
            this.progressBar1.Value = value;
        }

        public bool IsCheckedLineWithSpaces()
        {
            return chkLineWithSpaces.Checked;
        }

        public bool IsCheckedLineWithNewLine()
        {
            return chkLineWithNewLine.Checked;
        }

        public bool IsCheckedBeginSpaces()
        {
            return chkBeginSpaces.Checked;
        }

        public bool IsCheckedEndSpaces()
        {
            return chkEndSpaces.Checked;
        }


        #endregion

        #region thread

        public static void ConvertingThread(Form1 cvt, string strInputFileName, string strOutputFileName)
        {
            try
            {
                System.IO.File.Copy("template.accdb", strOutputFileName, true);

                OleDbConnection accessConnection = AccessDBHelper.CreateConnection(strOutputFileName, null);
                AccessDBHelper.CreateTable(accessConnection, "Documents");

                var commandPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                                             .Location);
                Directory.SetCurrentDirectory(commandPath);

                Process pdftotextProcess = new Process
                {
                    StartInfo = new ProcessStartInfo("pdftotext.exe")
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        RedirectStandardError = false,
                        CreateNoWindow = true,
                    },
                    EnableRaisingEvents = true
                };

                int progressValue;
                string[] fileArray = Directory.GetFiles(strInputFileName, "*.pdf", SearchOption.AllDirectories);

                for (progressValue = 0; progressValue < fileArray.Length; progressValue++)
                {
                    pdftotextProcess.StartInfo.Arguments = String.Format(@"-layout ""{0}"" {1}", fileArray[progressValue], "temp.txt");
                    pdftotextProcess.Start();
                    pdftotextProcess.WaitForExit();

                    string optimizedText = GetOptimizedText("temp.txt", cvt.IsCheckedLineWithSpaces(), cvt.IsCheckedLineWithNewLine(), cvt.IsCheckedBeginSpaces(), cvt.IsCheckedEndSpaces());
                    File.Delete("temp.txt");

                    AccessDBHelper.insertRecord(accessConnection, "Documents", new Tuple<int, string, string>(progressValue + 1, optimizedText, fileArray[progressValue]));

                    cvt.SetProgressBarValue(progressValue + 1);
                }
            }
            catch (Exception t)
            {
                MessageBox.Show(t.Message);
            }
            finally
            {
                cvt.FinishConverting();
            }
        }

        private static string GetOptimizedText(string fileName, bool willRemoveLineWithSpaces = false, bool willRemoveEmptyLine = false, bool willTrimBeginSpaces = false, bool willTrimEndSpaces = false)
        {
            string optimizedText = "";

            var lines = File.ReadLines(fileName);

            foreach (string line in lines)
            {
                string optimizedLine = line;
                if (willRemoveLineWithSpaces)
                {
                    if (string.IsNullOrWhiteSpace(optimizedLine))
                        continue;
                }
                if (willRemoveEmptyLine)
                {
                    if (string.IsNullOrEmpty(optimizedLine))
                        continue;
                }
                if (willTrimBeginSpaces)
                {
                    optimizedLine = optimizedLine.TrimStart();
                }
                if (willTrimEndSpaces)
                {
                    optimizedLine = optimizedLine.TrimEnd();
                }
                optimizedText += optimizedLine + Environment.NewLine;
            }

            return optimizedText;
        }

        #endregion
    }
    public class AccessDBHelper
    {
        #region static operations
        public static OleDbConnection CreateConnection(string databasePath, string password)
        {
            bool invalidPassword;
            return new OleDbConnection(GetConnectionString(databasePath, password, out invalidPassword));
        }

        private static bool TryConnectToDatabase(string connectionString, out bool invalidPassword)
        {
            var result = false;

            invalidPassword = false;

            try
            {
                using (var connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147217843 || ex.Message.IndexOf("password", StringComparison.InvariantCultureIgnoreCase) != -1)
                    invalidPassword = true;
            }

            return result;
        }

        public static void CreateTable(OleDbConnection oleDbConnection, string tableName)
        {
            if (oleDbConnection.State != ConnectionState.Open)
            {
                oleDbConnection.Open();
            }

            string strCommand = "CREATE TABLE " + tableName + " (ED_ENC_NUM double, NOTE_TEXT ntext, PDFPath text, Score int, Category int, Rank int, Proc1SVM int, Proc3SVM int, PRIMARY KEY(ED_ENC_NUM))";
            var command = new OleDbCommand(strCommand, oleDbConnection);
            command.ExecuteNonQuery();
        }

        public static void insertRecord(OleDbConnection oleDbConnection, string tableName, Tuple<int, string, string> row)
        {
            if (oleDbConnection.State != ConnectionState.Open)
            {
                oleDbConnection.Open();
            }

            var strCommand = "INSERT INTO " + tableName + " (ED_ENC_NUM, NOTE_TEXT, PDFPath) VALUES (@ED_ENC_NUM, @NOTE_TEXT, @PDFPath)";
            var command = new OleDbCommand(strCommand, oleDbConnection);

            command.Parameters.AddWithValue("@ED_ENC_NUM", row.Item1).DbType = DbType.Double;
            if (row.Item2 == null)
                command.Parameters.AddWithValue("@NOTE_TEXT", "");
            else
                command.Parameters.AddWithValue("@NOTE_TEXT", row.Item2);
            command.Parameters.AddWithValue("@PDFPath", row.Item3);            

            command.ExecuteNonQuery();
        }

        #endregion
        public static string GetConnectionString(string strFilePath, string password, out bool invalidPassword)
        {
            const string ace12ProviderString = "Microsoft.ACE.OLEDB.12.0";
            const string ace15ProviderString = "Microsoft.ACE.OLEDB.15.0";

            //const string connectionFormatString = "Provider={0};Data Source=\"{1}\";Jet OLEDB:Database Password=111aaa___;";

            var connectionFormatString = "Provider={0};Data Source=\"{1}\";";

            if (!String.IsNullOrEmpty(password))
                connectionFormatString += $"Jet OLEDB:Database Password={password};";

            ///////////////////////////////////////////////////////////////////////////////

            var connectionString = String.Format(connectionFormatString, ace12ProviderString, strFilePath);
            if (TryConnectToDatabase(connectionString, out invalidPassword))
                return connectionString;

            if (invalidPassword)
                return String.Empty;

            connectionString = String.Format(connectionFormatString, ace15ProviderString, strFilePath);
            if (TryConnectToDatabase(connectionString, out invalidPassword))
                return connectionString;

            ///////////////////////////////////////////////////////////////////////////////

            return String.Empty;
        }
    }
}
