using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQLite;


namespace ConvertSqliteToAccess
{
    public partial class Converter : Form
    {
        private string strInputFileName;
        private string strOutputFileName;

        private string strIdsFileName;

        public List<int> lstId;

        public Converter()
        {
            InitializeComponent();
            lstId = new List<int>();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnOpenSqlite_Click(object sender, EventArgs e)
        {            
            OpenFileDialog fileDialog = new OpenFileDialog();
                        
            fileDialog.Filter = "Sqlite3 file (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                strInputFileName = fileDialog.FileName;
                lblInputFileName.Text = "Input File : " + strInputFileName;
                
                strOutputFileName = strInputFileName + ".accdb";
                txtOutputFileName.Text = strOutputFileName;

            }
       }

        private void btnSaveAccess_Click(object sender, EventArgs e)
        {

            SaveFileDialog fileDialog = new SaveFileDialog();

            fileDialog.Filter = "Access File (*.accdb)|*.accdb";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                strOutputFileName = fileDialog.FileName;
                txtOutputFileName.Text = strOutputFileName;
            }
        }
        public static void ConvertingThread(Converter cvt, string strInputFileName, string strOutputFileName, bool isChecked = false)
        {
            try
            {
                var sqlConnection = new SQLiteConnection(strInputFileName);

                TableQuery<Records> query;
                
                query = sqlConnection.Table<Records>();
                
                // create access db file
                System.IO.File.Copy("template.accdb", strOutputFileName, true);
                OleDbConnection accessConnection = AccessDBHelper.CreateConnection(strOutputFileName, null);
                AccessDBHelper.CreateTable(accessConnection, "Documents");

                var count = query.Count();

                cvt.setProgressBarRange(0, count);
                                
                foreach (var row in query)
                {
                    if ( isChecked && cvt.lstId.Exists(x => x == (int)row.Id))
                        AccessDBHelper.insertRecord(accessConnection, "Documents", row);

                    cvt.increseProgressBarValue();
                }

                MessageBox.Show("Completed");
                cvt.enableConvertButton();
            }
            catch (Exception t)
            {
                MessageBox.Show(t.Message);                
            }
            
        }

        public void enableConvertButton()
        {
            progressBar1.Value = 0;
            btnConvert.Enabled = true;
        }

        public void increseProgressBarValue()
        {
            progressBar1.Value += 1;
        }

        public void setProgressBarRange(int min, int max)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = max;
            progressBar1.Value = 0;
        }


        private void btnConvert_Click(object sender, EventArgs e)
        {
            btnConvert.Enabled = false;

            Thread childThread = new Thread(() => ConvertingThread(this, strInputFileName, strOutputFileName, this.checkBox1.Checked));
            childThread.Start();            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            lstId.Clear();
            if (this.checkBox1.Checked == true)
            {                
                OpenFileDialog fileDialog = new OpenFileDialog();

                fileDialog.Filter = "Plain text file (*.*)|*.txt";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    strIdsFileName = fileDialog.FileName;

                    string[] lines = File.ReadAllLines(strIdsFileName);
                    foreach (string line in lines)
                    {
                        try
                        {
                            lstId.Add(Int32.Parse(line));
                        }catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }
        }
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

            string strCommand = "CREATE TABLE " + tableName + " (ED_ENC_NUM double, NOTE_TEXT ntext, PdfLink text, DocumentUrl text, Score int, Category int, Rank int, Proc1SVM int, Proc3SVM int, PRIMARY KEY(ED_ENC_NUM))";
            var command = new OleDbCommand(strCommand, oleDbConnection);
            command.ExecuteNonQuery();
            
        }

        public static void insertRecord(OleDbConnection oleDbConnection, string tableName, Records row)
        {
            if (oleDbConnection.State != ConnectionState.Open)
            {
                oleDbConnection.Open();
            }

            var strCommand = "INSERT INTO " + tableName + " (ED_ENC_NUM, NOTE_TEXT, PdfLink, DocumentUrl) VALUES (@ED_ENC_NUM, @NOTE_TEXT, @PdfLink, @DocumentUrl)";
            var command = new OleDbCommand(strCommand, oleDbConnection);
                        
            command.Parameters.AddWithValue("@ED_ENC_NUM", row.Id).DbType = DbType.Double;
            if (row.PdfText == null)
                command.Parameters.AddWithValue("@NOTE_TEXT", "");
            else 
                command.Parameters.AddWithValue("@NOTE_TEXT", row.PdfText);
            command.Parameters.AddWithValue("@PdfLink", row.PdfLink);
            command.Parameters.AddWithValue("@DocumentUrl", row.DocumentUrl);
            
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

    public class Records
    {
        #region Fields

        [PrimaryKey]
        public double Id { get; set; }

        public string PdfLink { get; set; }

        public string PdfText { get; set; }

        public string DocumentUrl { get; set; }

        #endregion
    }
}
