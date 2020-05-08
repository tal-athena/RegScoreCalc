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
using static SQLite.SQLiteConnection;

namespace ConvertSqliteToAccessFull
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

            ProgressBar.CheckForIllegalCrossThreadCalls = false;
            Button.CheckForIllegalCrossThreadCalls = false;
            CheckBox.CheckForIllegalCrossThreadCalls = false;
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
                
                var tableInfo = sqlConnection.GetTableInfo("Documents");
                //TableQuery<Records> query;                
                
                // create access db file
                System.IO.File.Copy("template.accdb", strOutputFileName, true);
                OleDbConnection accessConnection = AccessDBHelper.CreateConnection(strOutputFileName, null);

                AccessDBHelper.CreateTable(accessConnection, "Documents", tableInfo);
                
                string sql = "SELECT ";
                foreach (var columnInfo in tableInfo)
                {
                    sql += $"{columnInfo.Name}, ";
                }
                sql = sql.Substring(0, sql.Length - 2) + " FROM Documents";

                var cmd = sqlConnection.CreateCommand(sql);
                List<string> ImportedFiles = new List<string>();
             

                var query = sqlConnection.DynamicQuery(sql).ToList();

                cvt.setProgressBarRange(0, query.Count);
                                
                for (var i = 0; i < query.Count; i ++)
                {
                    AccessDBHelper.insertRecord(accessConnection, "Documents", query[i], tableInfo);
                    cvt.increseProgressBarValue();
                }
                accessConnection.Close();
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

        public static void CreateTable(OleDbConnection oleDbConnection, string tableName, List<ColumnInfo> tableInfo)
        {
            if (oleDbConnection.State != ConnectionState.Open)
            {
                oleDbConnection.Open();
            }
            var IDColumn = tableInfo.Find(x => x.Name == "ED_ENC_NUM");
            if (IDColumn != null)
                IDColumn.ColumnType = "double";

            string strCommand = "CREATE TABLE " + tableName + "(";
            foreach (var columnInfo in tableInfo)
            {
                strCommand += $"[{columnInfo.Name}] {columnInfo.ColumnType}, ";
            }

            strCommand += "NOTE_ENTITIES ntext, Score int, Category int, Rank int, Proc1SVM int, Proc3SVM int,";

            if (tableInfo.Find(x => x.Name == "ED_ENC_NUM") != null)
                strCommand += "PRIMARY KEY(ED_ENC_NUM)";
            else strCommand += "[defaultID] AUTOINCREMENT(1001,1) PRIMARY KEY";

            strCommand += ")";

            var command = new OleDbCommand(strCommand, oleDbConnection);
            command.ExecuteNonQuery();
            
        }

        public static void insertRecord(OleDbConnection oleDbConnection, string tableName, List<object> row, List<ColumnInfo> tableInfo)
        {
            if (oleDbConnection.State != ConnectionState.Open)
            {
                oleDbConnection.Open();
            }

            var strCommand = "INSERT INTO " + tableName + " (";
            for (var i = 0; i < tableInfo.Count; i ++)
            {
                if (i == 0)
                    strCommand += "[" + tableInfo[i].Name + "]";
                else strCommand += "," + "[" + tableInfo[i].Name + "]";
            }
            strCommand += ") VALUES (";
            for (var i = 0; i < tableInfo.Count; i++)
            {
                if (i == 0)
                    strCommand += "@" + tableInfo[i].Name;
                else strCommand += "," + "@" + tableInfo[i].Name;
            }
            strCommand += ")";
            

            var command = new OleDbCommand(strCommand, oleDbConnection);

            for (var i = 0; i < row.Count; i ++)
            {
                if (row[i] == null)
                    command.Parameters.AddWithValue("@" + tableInfo[i].Name, "");
                else
                    command.Parameters.AddWithValue("@" + tableInfo[i].Name, row[i]);
            }
            
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
