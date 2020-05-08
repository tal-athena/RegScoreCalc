using System;
using System.Data;
using System.Drawing;
using System.Data.OleDb;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using RegScoreCalc.Forms;
using System.ComponentModel;
using RegScoreCalc.Helpers;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Helpers;

namespace RegScoreCalc
{
    public partial class PaneSVM : Pane
    {
        #region Data members

        public string newProcessName = "";
        bool uploadError = false;
        private bool generateCanceled = false;
        public TextBox notificationTextBox;
        private FormGenerateSVMProgress fgSVM;
        private BackgroundWorker bw;
        string folderName;

        private string _destFile;
        private string _destTestFile;
        RibbonButton runProc1;
        RibbonButton runProc2;
        RibbonButton runProc3;
        RibbonButton resultsProc1;
        RibbonButton resultsProc2;
        RibbonButton resultsProc3;

        public ViewSVM _viewSVM;


        private int lblPositiveValidationNumber = 0;
        private int lblNegativeValidationNumber = 0;


        #endregion

        #region Ctors

        public PaneSVM()
        {
            InitializeComponent();

            //Color blue = Color.FromArgb(39, 179, 204);
            //Color consoleBg = System.Drawing.Color.FromArgb(249, 252, 255);
            //Color consoleFont = System.Drawing.Color.FromArgb(163, 163, 163);

            //groupBox3.ForeColor = consoleFont;
            //groupBox3.BackColor = blue;
            //panel1.BackColor = blue;
            //panel2.BackColor = blue;
            //gbPositive.BackColor = blue;
            //gbUncategorized.BackColor = blue;
            //gbUncategorized.ForeColor = consoleBg;

            //groupBox1.BackColor = consoleBg;
            //groupBox1.ForeColor = consoleFont;

            //label2.ForeColor = consoleBg;
            //label1.ForeColor = consoleBg;
            //label4.ForeColor = consoleBg;
            //lblRangeMin.ForeColor = consoleBg;
            //lblRangeMax.ForeColor = consoleBg;

            //numericNumberUncategorized.ForeColor = blue;

            //lbPositiveAllSVM.ForeColor = consoleFont;
            //lbPositiveChoosedSVM.ForeColor = consoleFont;


            //chbSVMValidation.ForeColor = consoleBg;
            //lblPositiveValidation.ForeColor = consoleBg;
            //lblNegativeValidation.ForeColor = consoleBg;
            //label6.ForeColor = consoleBg;
            //label3.ForeColor = consoleBg;
            //numericNegativeValidation.ForeColor = consoleBg;
            //numericPositiveValidation.ForeColor = consoleBg;



            btnPositiveAdd.BackgroundImageLayout = ImageLayout.Stretch;
            btnPositiveRemove.BackgroundImageLayout = ImageLayout.Stretch;

        }


        #endregion

        #region Events

        private void closeForm()
        {
            if (fgSVM != null)
            {
                if (fgSVM.Visible)
                {
                    fgSVM.Close();
                }
            }
        }

        private void UpdateValidationValues()
        {
            var dt = new MainDataSet.DocumentsDataTable();
			var adapter = new MainDataSetTableAdapters.DocumentsTableAdapter();
			adapter.Fill(dt);

            string positiveIDs = "";
            string negativeIDs = "";
            GetPositiveAndNegativeCategories(ref positiveIDs, ref negativeIDs);


            int negativeCount;
            int positiveCount;
            if (!String.IsNullOrEmpty(positiveIDs))
            {
                DataRow[] positiveResults = dt.Select("category IN (" + positiveIDs + ")");
                positiveCount = positiveResults.Length;
            }
            else
            {
                positiveCount = 0;
            }
            if (!String.IsNullOrEmpty(negativeIDs))
            {
                DataRow[] negativeResults = dt.Select("category IN (" + negativeIDs + ")");
                negativeCount = negativeResults.Length;
            }
            else
            {
                negativeCount = 0;
            }


            int negativeRandomCount = (int)numericNumberUncategorized.Value;

            txtPositiveCounter.Text = positiveCount.ToString();
            txtNegativeCounter.Text = negativeCount.ToString();


            numericPositiveValidation.Minimum = 0;
            numericPositiveValidation.Maximum = positiveCount;

            numericNegativeValidation.Minimum = 0;
            numericNegativeValidation.Maximum = negativeCount + negativeRandomCount;


            int negativeTrainingCount = (negativeCount + negativeRandomCount) - (int)numericNegativeValidation.Value;
            int positiveTrainingCount = positiveCount - (int)numericPositiveValidation.Value;


            lblPositiveValidationNumber = positiveTrainingCount;
            lblNegativeValidationNumber = negativeTrainingCount;

            lblNegativeValidation.Text = "for validation and " + negativeTrainingCount.ToString() + " for derivation";
            lblPositiveValidation.Text = "for validation and " + positiveTrainingCount.ToString() + " for derivation";
        }

        private void btnPositiveAdd_Click(object sender, EventArgs e)
        {
            if (lbPositiveAllSVM.SelectedIndex > -1)
            {
                string categoryName = lbPositiveAllSVM.SelectedItem.ToString();

                lbPositiveChoosedSVM.Items.Add(categoryName);

                lbPositiveAllSVM.Items.Remove(categoryName);


                UpdateSavedCategoriesToDB();

                UpdateValidationValues();

            }
        }
        private void btnPositiveRemove_Click(object sender, EventArgs e)
        {
            if (lbPositiveChoosedSVM.SelectedIndex > -1)
            {
                string categoryName = lbPositiveChoosedSVM.SelectedItem.ToString();
                lbPositiveChoosedSVM.Items.Remove(categoryName);

                lbPositiveAllSVM.Items.Add(categoryName);


                UpdateSavedCategoriesToDB();

                UpdateValidationValues();
            }
        }

        void btnGenerateTraning_Click(object sender, EventArgs e)
        {
            GenerateTraningButtonClick();
        }

        private void GenerateTraningButtonClick(string processName = null)
        {
            try
            {
                runProc1.Enabled = true;
                runProc2.Enabled = true;
                runProc3.Enabled = true;
                if (!String.IsNullOrEmpty(processName))
                {
                    //Disable all buttons for Run except the processName
                    DisableRunButton(processName);


                }

                bw = new BackgroundWorker();
                bw.WorkerSupportsCancellation = true;
                bw.WorkerReportsProgress = true;

                bw.DoWork += bw_DoWork;

                bw.ProgressChanged += bw_ProgressChanged;
                bw.RunWorkerCompleted += bw_RunWorkerCompleted;

                if (fgSVM == null)
                {
                    fgSVM = new FormGenerateSVMProgress();
                }

                fgSVM.UpdateProgress(0);

                bw.RunWorkerAsync();

                fgSVM.ShowDialog();
            }
            catch { }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                try
                {
                    fgSVM.Close();
                }
                catch { }


            }
            catch { }
            finally
            {
                MessageBox.Show("Done!");
            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (fgSVM != null)
            {
                if (e.ProgressPercentage > 100)
                {
                    fgSVM.UpdateProgress(100);
                }
                else
                {
                    fgSVM.UpdateProgress(e.ProgressPercentage);
                }
            }

        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (chbSVMValidation.Checked)
            {
                GenerateTraining2();
            }
            else
            {
                GenerateTraining();
            }
        }


        private void GenerateTraining2()
        {
            try
            {
                generateCanceled = false;

                //1. Copy template of the db


                var currentPath = Path.GetDirectoryName(Application.ExecutablePath);
                var sourceFile = Path.Combine(currentPath, "trainTemplate.mdb");
                var newFilename = "training.mdb";
                var newTestFilename = "test.mdb";

                var destFile = Path.Combine(currentPath, newFilename);
                var destTestFile = Path.Combine(currentPath, newTestFilename);


                _destFile = destFile;
                _destTestFile = destTestFile;


                int positiveItemsTest = (int)numericPositiveValidation.Value;
                int negativeItemsTest = (int)numericNegativeValidation.Value;
                int positiveItemsTraining = Int32.Parse(txtPositiveCounter.Text) - positiveItemsTest;
                int negativeItemsTraining = Int32.Parse(txtNegativeCounter.Text) - negativeItemsTest + (int)numericNumberUncategorized.Value;
                int positiveItemsCount = positiveItemsTest + positiveItemsTraining;
                int negativeItemsCount = negativeItemsTest + negativeItemsTraining;// + (int)numericNumberUncategorized.Value;

                int randomScore0DocumentsCount = (int)numericNumberUncategorized.Value;


                System.IO.File.Copy(sourceFile, destFile, true);
                System.IO.File.Copy(sourceFile, destTestFile, true);

                string connectionStringTrain = _views.MainForm.GetConnectionString(destFile);

                //Make a new connection
                //
                OleDbConnection connTraini = new OleDbConnection();
                connTraini.ConnectionString = connectionStringTrain;
                connTraini.Open();

                string connectionStringTest = _views.MainForm.GetConnectionString(destTestFile);
                OleDbConnection connTestTraini = new OleDbConnection();
                connTestTraini.ConnectionString = connectionStringTest;
                connTestTraini.Open();


                //2. Get all categories ID-s
                string positiveIDs = "";
                string negativeIDs = "";

                GetPositiveAndNegativeCategories(ref positiveIDs, ref negativeIDs);


                bw.ReportProgress(10);


                // 2. Insert to new database all positive with score of 100
                OleDbConnection conn = _views.MainForm.adapterDocuments.Connection;
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                int positiveCounter = 0;
                if (positiveIDs.Length > 0)
                {

                    var strPositiveCommand = "SELECT * FROM Documents WHERE category IN (" + positiveIDs + ") ;";


                    var positiveCommand = new OleDbCommand(strPositiveCommand, conn);

                    var reader = positiveCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        var ed = reader["ED_ENC_NUM"];
                        double ned = Double.Parse(ed.ToString());
                        var note = reader["NOTE_TEXT"].ToString();
                        var category = reader["category"];

                        var strPositiveInsert = "INSERT INTO Documents "
                                                                + "(ED_ENC_NUM, NOTE_TEXT,Score,category) "
                                                                + "VALUES (?, ?,?,?)";
                        int score = 100;
                        OleDbCommand positiveInsert;
                        if (positiveCounter < positiveItemsTraining)
                        {
                            positiveInsert = new OleDbCommand(strPositiveInsert, connTraini);
                            positiveCounter++;
                        }
                        else if (positiveCounter >= positiveItemsTraining && positiveCounter < positiveItemsCount)
                        {
                            score = 0;
                            positiveInsert = new OleDbCommand(strPositiveInsert, connTestTraini);
                            positiveCounter++;
                        }
                        else
                        {
                            break;
                        }


                        positiveInsert.Parameters.Add("ED_ENC_NUM", OleDbType.Double).Value = ned;
                        positiveInsert.Parameters.Add("NOTE_TEXT", OleDbType.VarChar).Value = note;
                        positiveInsert.Parameters.Add("Score", OleDbType.Integer).Value = score;
                        positiveInsert.Parameters.Add("category", OleDbType.Integer).Value = Convert.ToInt32(category);
                        positiveInsert.ExecuteNonQuery();
                    }

                }

                bw.ReportProgress(50);


                //3. Insert all negative with score -100
                if (conn.State != ConnectionState.Open)
                    conn.Open();


                int negativeCounter = 0;
                if (negativeIDs.Length > 0)
                {
                    var strNegativeCommand = "SELECT * FROM Documents WHERE category IN (" + negativeIDs + ") ;";


                    var negativeCommand = new OleDbCommand(strNegativeCommand, conn);

                    var readerNegative = negativeCommand.ExecuteReader();

                    while (readerNegative.Read())
                    {
                        var ed = readerNegative["ED_ENC_NUM"];
                        double ned = Double.Parse(ed.ToString());
                        var note = readerNegative["NOTE_TEXT"].ToString();
                        var category = readerNegative["category"];

                        var strPositiveInsert = "INSERT INTO Documents "
                                                                + "(ED_ENC_NUM, NOTE_TEXT,Score,category) "
                                                                + "VALUES (?, ?,?,?)";

                        int score = -100;
                        OleDbCommand negativeInsert;
                        if (negativeCounter < negativeItemsTraining)
                        {
                            negativeInsert = new OleDbCommand(strPositiveInsert, connTraini);
                            negativeCounter++;
                        }
                        else if (negativeCounter >= negativeItemsTraining && negativeCounter < negativeItemsCount)
                        {
                            score = 0;
                            negativeInsert = new OleDbCommand(strPositiveInsert, connTestTraini);
                            negativeCounter++;
                        }
                        else
                        {
                            break;
                        }

                        negativeInsert.Parameters.Add("ED_ENC_NUM", OleDbType.Double).Value = ned;
                        negativeInsert.Parameters.Add("NOTE_TEXT", OleDbType.VarChar).Value = note;
                        negativeInsert.Parameters.Add("Score", OleDbType.Integer).Value = score;
                        negativeInsert.Parameters.Add("category", OleDbType.Integer).Value = Convert.ToInt32(category);
                        negativeInsert.ExecuteNonQuery();
                    }
                }









                bw.ReportProgress(70);


				var dtDocuments = new MainDataSet.DocumentsDataTable();
				var adapter = new MainDataSetTableAdapters.DocumentsTableAdapter();
				adapter.Fill(dtDocuments);

                //dtDocuments.AsEnumerable().OrderBy(r => rand.Next()).CopyToDataTable(dtDocuments,LoadOption.OverwriteChanges);
                DataRow[] negativeResults = dtDocuments.Select("category IS NULL AND Score = 0");

                //Generate random array of id-s
                Random rand = new Random();
                List<int> result = new List<int>();
                HashSet<int> check = new HashSet<int>();
                var documentsCount = negativeResults.Length;

                var length = randomScore0DocumentsCount;
                for (Int32 i = 0; i < length; i++)
                {
                    int curValue = rand.Next(0, documentsCount);
                    while (check.Contains(curValue))
                    {
                        curValue = rand.Next(0, documentsCount);
                    }
                    result.Add(curValue);
                    check.Add(curValue);
                }



                for (int i = 0; i < length; i++)
                {
                    //Get random row
                    var currentRow = negativeResults[result[i]];

                    var ed = currentRow["ED_ENC_NUM"];
                    double ned = Double.Parse(ed.ToString());
                    var note = currentRow["NOTE_TEXT"].ToString();
                    var category = currentRow["category"];

                    var strPositiveInsert = "INSERT INTO Documents "
                                                            + "(ED_ENC_NUM, NOTE_TEXT,Score) "
                                                            + "VALUES (?, ?,?)";
                    int score = -100;
                    OleDbCommand negativeInsert;
                    if (negativeCounter < negativeItemsTraining)
                    {
                        negativeInsert = new OleDbCommand(strPositiveInsert, connTraini);
                        negativeCounter++;
                    }
                    else if (negativeCounter >= negativeItemsTraining && negativeCounter < negativeItemsCount)
                    {
                        score = 0;
                        negativeInsert = new OleDbCommand(strPositiveInsert, connTestTraini);
                        negativeCounter++;
                    }
                    else
                    {
                        score = 0;
                        negativeInsert = new OleDbCommand(strPositiveInsert, connTestTraini);
                        negativeCounter++;
                    }

                    negativeInsert.Parameters.Add("ED_ENC_NUM", OleDbType.Double).Value = ned;
                    negativeInsert.Parameters.Add("NOTE_TEXT", OleDbType.VarChar).Value = note;
                    negativeInsert.Parameters.Add("Score", OleDbType.Integer).Value = score;
                    // negativeInsert.Parameters.Add("category", OleDbType.Integer).Value = (int)category;
                    negativeInsert.ExecuteNonQuery();
                }

                bw.ReportProgress(100);


                try
                {
                    if (connTraini.State == ConnectionState.Open)
                    {
                        connTraini.Close();
                        connTraini.Dispose();
                    }
                    if (connTestTraini.State == ConnectionState.Open)
                    {
                        connTestTraini.Close();
                        connTestTraini.Dispose();
                    }

                }
                catch { }


                if (generateCanceled)
                {
                    try
                    {
                        File.Delete(destFile);
                        File.Delete(destTestFile);
                    }
                    catch { }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetPositiveAndNegativeCategories(ref string positiveIDs, ref string negativeIDs)
        {
            //var dt = new RegScoreCalc.MainDataSet.CategoriesDataTable();
            //_views.MainForm.adapterCategories.Fill(dt);
            //foreach (DataRow row in dt.Rows)
            foreach (var row in _views.MainForm.datasetMain.Categories)
            {
                var id = row[0];
                var name = row[1];
                if (lbPositiveChoosedSVM.Items.IndexOf(name) > -1)
                {
                    positiveIDs += id + ",";
                }
                else //Else it is negative
                {
                    negativeIDs += id + ",";
                }
            }
            if (!String.IsNullOrEmpty(positiveIDs))
            {
                positiveIDs = positiveIDs.Substring(0, positiveIDs.Length - 1);
            }
            if (!String.IsNullOrEmpty(negativeIDs))
            {
                negativeIDs = negativeIDs.Substring(0, negativeIDs.Length - 1);
            }
        }

        private void GenerateTraining()
        {
            try
            {
                generateCanceled = false;

                //1. Copy template of the db


                var currentPath = Path.GetDirectoryName(Application.ExecutablePath);
                var sourceFile = Path.Combine(currentPath, "trainTemplate.mdb");
                var newFilename = "training.mdb";
                var newTestFilename = "test.mdb";

                var destFile = Path.Combine(currentPath, newFilename);
                var destTestFile = Path.Combine(currentPath, newTestFilename);


                _destFile = destFile;
                _destTestFile = destTestFile;




                System.IO.File.Copy(sourceFile, destFile, true);
                System.IO.File.Copy(sourceFile, destTestFile, true);

                string connectionStringTrain = _views.MainForm.GetConnectionString(destFile);

                //Make a new connection
                //
                OleDbConnection connTraini = new OleDbConnection();
                connTraini.ConnectionString = connectionStringTrain;
                connTraini.Open();

                string connectionStringTest = _views.MainForm.GetConnectionString(destTestFile);
                OleDbConnection connTestTraini = new OleDbConnection();
                connTestTraini.ConnectionString = connectionStringTest;
                connTestTraini.Open();


                //2. Get all categories ID-s
                string positiveIDs = "";
                string negativeIDs = "";
                var dt = new RegScoreCalc.MainDataSet.CategoriesDataTable();
                _views.MainForm.adapterCategories.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    var id = row[0];
                    var name = row[1];
                    if (lbPositiveChoosedSVM.Items.IndexOf(name) > -1)
                    {
                        positiveIDs += id + ",";
                    }
                    else //Else it is negative
                    {
                        negativeIDs += id + ",";
                    }
                }
                if (!String.IsNullOrEmpty(positiveIDs))
                {
                    positiveIDs = positiveIDs.Substring(0, positiveIDs.Length - 1);
                }
                if (!String.IsNullOrEmpty(negativeIDs))
                {
                    negativeIDs = negativeIDs.Substring(0, negativeIDs.Length - 1);
                }


                bw.ReportProgress(10);


                // 2. Insert to new database all positive with score of 100
                OleDbConnection conn = _views.MainForm.adapterDocuments.Connection;
                if (conn.State != ConnectionState.Open)
                    conn.Open();


                if (positiveIDs.Length > 0)
                {

                    var strPositiveCommand = "SELECT * FROM Documents WHERE category IN (" + positiveIDs + ") ;";


                    var positiveCommand = new OleDbCommand(strPositiveCommand, conn);

                    var reader = positiveCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        var ed = reader["ED_ENC_NUM"];
                        double ned = Double.Parse(ed.ToString());
                        var note = reader["NOTE_TEXT"].ToString();
                        var category = reader["category"];

                        var strPositiveInsert = "INSERT INTO Documents "
                                                                + "(ED_ENC_NUM, NOTE_TEXT,Score,category) "
                                                                + "VALUES (?, ?,?,?)";

                        var positiveInsert = new OleDbCommand(strPositiveInsert, connTraini);

                        positiveInsert.Parameters.Add("ED_ENC_NUM", OleDbType.Double).Value = ned;
                        positiveInsert.Parameters.Add("NOTE_TEXT", OleDbType.LongVarWChar).Value = note;
                        positiveInsert.Parameters.Add("Score", OleDbType.Integer).Value = 100;
                        positiveInsert.Parameters.Add("category", OleDbType.Integer).Value = Convert.ToInt32(category);
                        positiveInsert.ExecuteNonQuery();
                    }

                }

                bw.ReportProgress(30);


                //3. Insert all negative with score -100
                if (conn.State != ConnectionState.Open)
                    conn.Open();



                if (negativeIDs.Length > 0)
                {
                    var strNegativeCommand = "SELECT * FROM Documents WHERE category IN (" + negativeIDs + ") ;";


                    var negativeCommand = new OleDbCommand(strNegativeCommand, conn);

                    var readerNegative = negativeCommand.ExecuteReader();

                    while (readerNegative.Read())
                    {
                        var ed = readerNegative["ED_ENC_NUM"];
                        double ned = Double.Parse(ed.ToString());
                        var note = readerNegative["NOTE_TEXT"].ToString();
                        var category = readerNegative["category"];

                        var strPositiveInsert = "INSERT INTO Documents "
                                                                + "(ED_ENC_NUM, NOTE_TEXT,Score,category) "
                                                                + "VALUES (?, ?,?,?)";

                        var negativeInsert = new OleDbCommand(strPositiveInsert, connTraini);

                        negativeInsert.Parameters.Add("ED_ENC_NUM", OleDbType.Double).Value = ned;
                        negativeInsert.Parameters.Add("NOTE_TEXT", OleDbType.LongVarWChar).Value = note;
                        negativeInsert.Parameters.Add("Score", OleDbType.Integer).Value = -100;
                        negativeInsert.Parameters.Add("category", OleDbType.Integer).Value = Convert.ToInt32(category);
                        negativeInsert.ExecuteNonQuery();
                    }
                }



                bw.ReportProgress(50);



                // 4. Insert random number of uncategorized documents

                var numberOfItems = Int32.Parse(lblRangeMax.Text);

                double progressValue = 50;
                int pom = 1;


                double ratio = (double)25 / (double)numberOfItems;

                var strRandomCommand = "SELECT * FROM Documents WHERE category IS NULL AND Score <= 0;";
                //var strRandomCommand = "SELECT * FROM Documents WHERE category IS NULL AND Score = 0;";
                var randomCommand = new OleDbCommand(strRandomCommand, conn);

                var readerRandom = randomCommand.ExecuteReader();




                //Used to know when to start inserting in test.mdb database
                int inFirstDatabase = 0;
                OleDbCommand randomInsert;
                while (readerRandom.Read())
                {

                    inFirstDatabase++;
                    if (fgSVM.cancel)
                    {
                        //Stop and remove db file
                        connTraini.Close();
                        generateCanceled = true;


                        break;
                    }

                    var ed = readerRandom["ED_ENC_NUM"];
                    double ned = Double.Parse(ed.ToString());
                    var note = readerRandom["NOTE_TEXT"].ToString();

                    var strRandomInsert = "INSERT INTO Documents "
                                                            + "(ED_ENC_NUM, NOTE_TEXT,Score) "
                                                            + "VALUES (?,?,?)";

                    if (inFirstDatabase < numericNumberUncategorized.Value)
                    {
                        randomInsert = new OleDbCommand(strRandomInsert, connTraini);
                        randomInsert.Parameters.Add("ED_ENC_NUM", OleDbType.Double).Value = ned;
                        randomInsert.Parameters.Add("NOTE_TEXT", OleDbType.LongVarWChar).Value = note;
                        randomInsert.Parameters.Add("Score", OleDbType.Integer).Value = -100;
                    }
                    else
                    {
                        randomInsert = new OleDbCommand(strRandomInsert, connTestTraini);
                        randomInsert.Parameters.Add("ED_ENC_NUM", OleDbType.Double).Value = ned;
                        randomInsert.Parameters.Add("NOTE_TEXT", OleDbType.LongVarWChar).Value = note;
                        randomInsert.Parameters.Add("Score", OleDbType.Integer).Value = 0;
                    }


                    randomInsert.ExecuteNonQuery();

                    progressValue += ratio;
                    pom++;
                    if (pom > 5)
                    {
                        pom = 1;
                        try
                        {
                            bw.ReportProgress(Convert.ToInt32(progressValue));
                        }
                        catch { }
                    }
                }


                //Category is null and score > 0
                var strRandomCommand1 = "SELECT * FROM Documents WHERE category IS NULL AND Score > 0;";
                //var strRandomCommand1 = "SELECT * FROM Documents WHERE category IS NULL AND Score = 0;";
                var randomCommand1 = new OleDbCommand(strRandomCommand1, conn);

                var readerRandom1 = randomCommand1.ExecuteReader();


                //Used to know when to start inserting in test.mdb database
                OleDbCommand randomInsert1;


                while (readerRandom1.Read())
                {
                    if (fgSVM.cancel)
                    {
                        //Stop and remove db file
                        connTraini.Close();
                        generateCanceled = true;

                        break;
                    }



                    var ed = readerRandom1["ED_ENC_NUM"];
                    double ned = Double.Parse(ed.ToString());
                    var note = readerRandom1["NOTE_TEXT"].ToString();

                    var strRandomInsert = "INSERT INTO Documents "
                                                            + "(ED_ENC_NUM, NOTE_TEXT,Score) "
                                                            + "VALUES (?,?,?)";


                    randomInsert1 = new OleDbCommand(strRandomInsert, connTestTraini);


                    randomInsert1.Parameters.Add("ED_ENC_NUM", OleDbType.Double).Value = ned;
                    randomInsert1.Parameters.Add("NOTE_TEXT", OleDbType.LongVarWChar).Value = note;
                    randomInsert1.Parameters.Add("Score", OleDbType.Integer).Value = 0;
                    try
                    {
                        randomInsert1.ExecuteNonQuery();
                    }
                    catch { }

                    progressValue += ratio;
                    pom++;
                    if (pom > 5)
                    {
                        pom = 1;
                        try
                        {
                            bw.ReportProgress(Convert.ToInt32(progressValue));
                        }
                        catch { }
                    }
                }


                bw.ReportProgress(100);


                try
                {
                    if (connTraini.State == ConnectionState.Open)
                    {
                        connTraini.Close();
                        connTraini.Dispose();
                    }
                    if (connTestTraini.State == ConnectionState.Open)
                    {
                        connTestTraini.Close();
                        connTestTraini.Dispose();
                    }

                }
                catch { }


                if (generateCanceled)
                {
                    try
                    {
                        File.Delete(destFile);
                        File.Delete(destTestFile);
                    }
                    catch { }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void btnUploadToServer_Click(object sender, EventArgs e)
        {

            //First check if there is the template db
            var path = Path.Combine(Directory.GetCurrentDirectory(), "trainTemplate.mdb");
            if (!File.Exists(path))
            {
                MessageBox.Show(_views.MainForm, "The template db is missing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //Set process name
                RibbonButton btnSender = (RibbonButton)sender;
                newProcessName = btnSender.Tag.ToString();

                GenerateTraningButtonClick(newProcessName);


                notificationTextBox.Text = "";
                notificationTextBox.AppendText("Upload started..." + Environment.NewLine);


                bw = new BackgroundWorker();
                bw.WorkerSupportsCancellation = true;
                bw.WorkerReportsProgress = true;

                bw.DoWork += bwUpload_DoWork;

                bw.ProgressChanged += bwUpload_ProgressChanged;
                bw.RunWorkerCompleted += bwUpload_RunWorkerCompleted;

                fgSVM = new FormGenerateSVMProgress();
                fgSVM.btnCancel.Enabled = false;

                bw.RunWorkerAsync();


                fgSVM.ShowDialog();
            }
        }


        void bwUpload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (fgSVM != null)
                {
                    fgSVM.Close();
                }
                if (!uploadError)
                {
                    notificationTextBox.AppendText("Upload done!" + Environment.NewLine + Environment.NewLine);

                    //Start process at server
                    if (_viewSVM != null)
                    {
                        _viewSVM.StartProcess();
                    }
                }
                else
                {
                    //MessageBox.Show(this, "Problem with uploading files to server!\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    notificationTextBox.AppendText("Upload Failed!");


                    EnableAllStartProcessButtons();
                }



            }
            catch (Exception ex)
            {
                notificationTextBox.AppendText("Upload Failed!" + Environment.NewLine + ex.Message + Environment.NewLine);
                fgSVM.Close();

            }


        }

        void bwUpload_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage > 100)
            {
                fgSVM.UpdateProgress(100);
            }
            else
            {
                fgSVM.UpdateProgress(e.ProgressPercentage);
            }
        }

        void bwUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            //If the generation of mdb files is canseled do not upload and start process on server
            if (!generateCanceled)
            {
                UploadFilesToServer();
            }

        }


        private void UploadFilesToServer()
        {
            try
            {
                uploadError = false;

                //Connect to service and start uploading the file
                if (!File.Exists(_destFile))
                {
                    MessageBox.Show("There is no traning database!");
                    return;
                }

                // training.mdb

                var databaseName = _views.MainForm.DatabaseName;
                if (String.IsNullOrEmpty(databaseName))
                {
                    databaseName = "database";
                }
                var fileNameWithoutBlanks = Path.GetFileNameWithoutExtension(databaseName).Replace(" ", String.Empty);

                var processNumber = "1";
                if (!String.IsNullOrEmpty(newProcessName))
                {
                    processNumber = newProcessName;
                }


                if (fileNameWithoutBlanks.Length <= 7)
                {
                    folderName = processNumber + fileNameWithoutBlanks + "_" + DateTime.Now.Ticks.ToString();
                }
                else
                {
                    folderName = processNumber + fileNameWithoutBlanks.Substring(0, 6) + "_" + DateTime.Now.Ticks.ToString();
                }

                //Start service client
                using (UploadFileToServer.FileTransferServiceClient client = new UploadFileToServer.FileTransferServiceClient())
                {


                    //Get some info about the input file
                    //training.mdb
                    notificationTextBox.AppendText("Uploading file: training.mdb" + Environment.NewLine);
                    UploadFileToServer(_destFile, client);


                    // test.mdb
                    notificationTextBox.AppendText("Uploading file: test.mdb" + Environment.NewLine);
                    UploadFileToServer(_destTestFile, client);

                }


                //Save current folderName to app so user can check status from time to time
                Properties.Settings.Default.ProcessName = folderName;
                Properties.Settings.Default.Save();

            }
            catch (Exception ex)
            {
                // notificationTextBox.AppendText(ex.Message + Environment.NewLine);
                MessageBox.Show("Upload error\n" + ex.Message, "Upload error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fgSVM.Close();

                uploadError = true;
            }
        }

        private void UploadFileToServer(string filename, UploadFileToServer.FileTransferServiceClient client)
        {

            ////Open input stream for training.mdb
            //using (System.IO.FileStream stream = new System.IO.FileStream(filename, System.IO.FileMode.Open,
            //    System.IO.FileAccess.Read, FileShare.ReadWrite))
            //{
            //    using (StreamWithProgress uploadStreamWithProgress = new StreamWithProgress(stream))
            //    {

            //        //var stream1 = uploadStreamWithProgress.GetStream();
            //        //stream.CopyTo(stream1);

            //        System.IO.FileInfo fileInfo = new System.IO.FileInfo(filename);

            //        uploadStreamWithProgress.ProgressChanged += uploadStreamWithProgress_ProgressChanged;

            //        client.UploadFile(Path.GetFileName(filename), folderName, fileInfo.Length, uploadStreamWithProgress);


            //    }
            //}

            Stream fileStream = null;
            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filename);
                var fileName2 = Path.Combine(fileInfo.DirectoryName, "temp" + Path.GetFileName(filename));

                try
                {
                    fileStream = fileInfo.OpenRead();
                }
                catch
                {

                    File.Copy(fileInfo.FullName, fileName2, true);
                    fileInfo = new System.IO.FileInfo(fileName2);

                    fileStream = fileInfo.OpenRead();
                }

                StreamWithProgress streamWithProgress = new StreamWithProgress((FileStream)fileStream);

                streamWithProgress.ProgressChanged += uploadStreamWithProgress_ProgressChanged;



                if (folderName == null)
                {
                    folderName = "test" + Guid.NewGuid().ToString();
                }

                client.UploadFile(Path.GetFileName(filename), folderName, fileInfo.Length, streamWithProgress);

                byte[] buffer = new byte[2048];
                int bytesRead = fileStream.Read(buffer, 0, 2048);
                while (bytesRead > 0)
                {
                    fileStream.Write(buffer, 0, 2048);
                    bytesRead = fileStream.Read(buffer, 0, 2048);
                }


                //Check if temp file exists and delete it
                if (File.Exists(fileName2))
                {
                    try
                    {
                        File.Delete(fileName2);
                    }
                    catch { }
                }

            }
            catch
            {

                throw;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }

        }

        void uploadStreamWithProgress_ProgressChanged(object sender, StreamWithProgress.ProgressChangedEventArgs e)
        {
            int progress = (int)((e.BytesRead * 100) / e.Length);
            bw.ReportProgress(progress);
        }

        #endregion

        #region Operations


        public static readonly string m_delimiter = "@#@";
        public void UpdateSavedCategoriesToDB()
        {
            try
            {
                string positiveCategories = "";
                foreach (var positiveId in lbPositiveChoosedSVM.Items)
                {
                    positiveCategories += positiveId.ToString() + m_delimiter;
                }
                if (!String.IsNullOrEmpty(positiveCategories))
                {
                    positiveCategories = positiveCategories.Substring(0, positiveCategories.Length - m_delimiter.Length);
                }


                //Save to DB
                OleDbConnection conn = _views.MainForm.adapterDocuments.Connection;
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                var strCommand = "UPDATE SavedCategories SET categoryIDs=? WHERE PosNeg=?";


                var command = new OleDbCommand(strCommand, conn);

                command.Parameters.Add("categoryIDs", OleDbType.VarChar).Value = positiveCategories;
                command.Parameters.Add("PosNeg", OleDbType.VarChar).Value = "Positive";
                command.ExecuteNonQuery();


                if (conn.State == ConnectionState.Open)
                    conn.Close();


            }
            catch
            {

            }
        }

        protected void GetAllCategories()
        {
            try
            {
                OleDbConnection conn = _views.MainForm.adapterDocuments.Connection;
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Categories";



                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var name = reader["Category"].ToString();


                    //Add to listBoxes
                    lbPositiveAllSVM.Items.Add(name);
                }
            }
            catch
            {

            }

        }
        protected void GetSavedCategories()
        {

            try
            {
                lbPositiveChoosedSVM.Items.Clear();



                var dt = new RegScoreCalc.MainDataSet.CategoriesDataTable();
                _views.MainForm.adapterCategories.Fill(dt);



                OleDbConnection conn = _views.MainForm.adapterDocuments.Connection;
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM SavedCategories";



                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var type = reader["PosNeg"].ToString();
                    var ids = reader["categoryIDs"].ToString();
                    var idArray = ids.Split(new string[] { m_delimiter }, StringSplitOptions.RemoveEmptyEntries);

                    if (!String.IsNullOrEmpty(idArray[0]))
                    {
                        if (type == "Positive")
                        {
                            foreach (var id in idArray)
                            {
                                if (0 > lbPositiveAllSVM.Items.IndexOf(id))
                                    continue;
                                lbPositiveAllSVM.Items.Remove(id);
                                lbPositiveChoosedSVM.Items.Add(id);

                                lbPositiveAllSVM.Items.Remove(id);
                            }
                        }

                    }

                }
            }
            catch
            {
                try
                {
                    //Create table if it doesnt exist

                    OleDbConnection conn = _views.MainForm.adapterDocuments.Connection;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string strCommand = "CREATE TABLE SavedCategories (PosNeg ntext, categoryIDs ntext)";

                    OleDbCommand command = new OleDbCommand(strCommand, conn);
                    command.ExecuteNonQuery();

                    //Add empty 
                    var strCommand1 = "INSERT INTO SavedCategories "
                                                                 + "(PosNeg, categoryIDs) "
                                                                 + "VALUES (?, ?)";

                    var command1 = new OleDbCommand(strCommand1, conn);

                    command1.Parameters.Add("PosNeg", OleDbType.VarChar).Value = "Positive";
                    command1.Parameters.Add("categoryIDs", OleDbType.VarChar).Value = "";
                    command1.ExecuteNonQuery();

                    var command2 = new OleDbCommand(strCommand1, conn);
                    command2.Parameters.Add("PosNeg", OleDbType.VarChar).Value = "Negative";
                    command2.Parameters.Add("categoryIDs", OleDbType.VarChar).Value = "";

                    command2.ExecuteNonQuery();

                    conn.Close();
                }
                catch { }
            }

        }

        protected void InitRangeControl()
        {
            try
            {
                OleDbConnection conn = _views.MainForm.adapterDocuments.Connection;
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                OleDbCommand cmd = conn.CreateCommand();
                //SELECT * FROM Documents WHERE category IS NULL AND Score = 0;
                cmd.CommandText = "SELECT COUNT(*) FROM Documents WHERE category IS NULL AND Score = 0";

                var count = cmd.ExecuteScalar();

                numericNumberUncategorized.Minimum = 0;
                numericNumberUncategorized.Maximum = (int)count;


                lblRangeMax.Text = count.ToString();
            }
            catch { }
        }

        public void ClearAllControls()
        {
            lbPositiveChoosedSVM.Items.Clear();
            lbPositiveAllSVM.Items.Clear();
        }

        #endregion

        #region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
        {
            base.InitPane(views, ownerView, panel, tab);


            ClearAllControls();

            GetAllCategories();

            //Get saved categories from previous searches??
            GetSavedCategories();


            //Init range
            InitRangeControl();



            UpdateValidationValues();


            //Font and background color

            //this.FontHeight = 11;
            //this.Font = new System.Drawing.Font(this.Font, System.Drawing.FontStyle.Regular);
            //this.BackColor = Color.LightSlateGray;

        }

        protected override void InitPaneCommands(RibbonTab tab)
        {

            //RibbonButton btnUp = new RibbonButton("UPLOAD");

            //tab.Panels[0].Items.Add(btnUp);

            //btnUp.Image = Properties.Resources.GenerateTrainingSrong;
            //btnUp.SmallImage = Properties.Resources.GenerateTrainingSrong;
            //btnUp.Click += delegate
            //{
            //    UploadFileToServer(@"E:\POSAO\RegScoreCalc\Release10\RegScoreCalcSolution\Application\RegScoreCalc\bin\Debug\training.mdb", new UploadFileToServer.FileTransferServiceClient());
            //};
            //btnUp.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;



            RibbonButton btnGenerateTraning = new RibbonButton("Generate Training");

            tab.Panels[0].Items.Add(btnGenerateTraning);

            btnGenerateTraning.Image = Properties.Resources.GenerateTrainingSrong;
            btnGenerateTraning.SmallImage = Properties.Resources.GenerateTrainingSrong;
            btnGenerateTraning.Click += btnGenerateTraning_Click;
            btnGenerateTraning.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;



            RibbonPanel panelProcess1 = new RibbonPanel("Process 1");
            tab.Panels.Add(panelProcess1);

            runProc1 = new RibbonButton("Run");
            runProc1.Image = Properties.Resources.RunSVMStrong;
            runProc1.SmallImage = Properties.Resources.RunSVMStrong;
            runProc1.Click += btnUploadToServer_Click;
            runProc1.Tag = "1";
            runProc1.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

            resultsProc1 = new RibbonButton("Results");
            resultsProc1.Image = Properties.Resources.MaximizeNotes;
            resultsProc1.SmallImage = Properties.Resources.MaximizeNotes;
            resultsProc1.Click += resultsProc_Click;





            resultsProc1.Enabled = false;





            resultsProc1.Tag = "1";
            resultsProc1.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

            panelProcess1.Items.Add(runProc1);
            panelProcess1.Items.Add(resultsProc1);


            RibbonPanel panelProcess2 = new RibbonPanel("Process 2");
            tab.Panels.Add(panelProcess2);

            runProc2 = new RibbonButton("Run");
            runProc2.Image = Properties.Resources.RunSVMStrong;
            runProc2.SmallImage = Properties.Resources.RunSVMStrong;
            runProc2.Click += btnUploadToServer_Click;
            runProc2.Tag = "2";
            runProc2.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

            resultsProc2 = new RibbonButton("Results");
            resultsProc2.Image = Properties.Resources.MaximizeNotes;
            resultsProc2.SmallImage = Properties.Resources.MaximizeNotes;
            resultsProc2.Click += resultsProc_Click;





            resultsProc2.Enabled = false;





            resultsProc2.Tag = "2";
            resultsProc2.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

            panelProcess2.Items.Add(runProc2);
            panelProcess2.Items.Add(resultsProc2);


            RibbonPanel panelProcess3 = new RibbonPanel("Process 3");
            tab.Panels.Add(panelProcess3);

            runProc3 = new RibbonButton("Run");
            runProc3.Image = Properties.Resources.RunSVMStrong;
            runProc3.SmallImage = Properties.Resources.RunSVMStrong;
            runProc3.Click += btnUploadToServer_Click;
            runProc3.Tag = "3";
            runProc3.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

            resultsProc3 = new RibbonButton("Results");
            resultsProc3.Image = Properties.Resources.MaximizeNotes;
            resultsProc3.SmallImage = Properties.Resources.MaximizeNotes;
            resultsProc3.Click += resultsProc_Click;






            resultsProc3.Enabled = false;






            resultsProc3.Tag = "3";
            resultsProc3.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

            panelProcess3.Items.Add(runProc3);
            panelProcess3.Items.Add(resultsProc3);

        }

        void resultsProc_Click(object sender, EventArgs e)
        {
            RibbonButton btn = (RibbonButton)sender;

            if (_viewSVM != null)
            {
                _viewSVM.ReviewResults(btn.Tag.ToString());
            }

        }




        public override void UpdatePane()
        {
            try
            {
                //    ClearAllControls();

                //    GetAllCategories();

                //    //Get saved categories from previous searches??
                //    GetSavedCategories();


                //    //Init range
                //    InitRangeControl();

                //    //numericNegativeValidation.Value = 0;
                //    //numericPositiveValidation.Value = 0;

                //    UpdateValidationValues();


                //    numericPositiveValidationOldValue = (int)numericPositiveValidation.Value;
                //    numericNegativeValidationOldValue = (int)numericNegativeValidation.Value;
                //    numericNumberUncategorizedOldValue = (int)numericNumberUncategorized.Value;

            }
            catch { }
        }

        #endregion



        #region Implementation

        public void EnableAllStartProcessButtons()
        {
            runProc1.Enabled = true;
            runProc2.Enabled = true;
            runProc3.Enabled = true;
        }


        public void EnableReviewButton(string processName)
        {
            if (resultsProc1.Tag.ToString() == processName)
            {
                resultsProc1.Enabled = true;
            }
            if (resultsProc2.Tag.ToString() == processName)
            {
                resultsProc2.Enabled = true;
            }
            if (resultsProc3.Tag.ToString() == processName)
            {
                resultsProc3.Enabled = true;
            }
        }

        private void DisableRunButton(string processName)
        {
            runProc1.Enabled = false;
            runProc2.Enabled = false;
            runProc3.Enabled = false;

            //if (runProc1.Tag != processName)
            //{
            //    runProc1.Enabled = false;
            //}
            //if (runProc2.Tag != processName)
            //{
            //    runProc2.Enabled = false;
            //}
            //if (runProc3.Tag != processName)
            //{
            //    runProc3.Enabled = false;
            //}
        }


        #endregion


        private void numericNumberUncategorized_ValueChanged(object sender, EventArgs e)
        {
            UpdateValidationValues();
        }

        private void numericPositiveValidation_ValueChanged(object sender, EventArgs e)
        {
            UpdateValidationValues();
        }

        private void numericNegativeValidation_ValueChanged(object sender, EventArgs e)
        {
            UpdateValidationValues();
        }
    }
}
