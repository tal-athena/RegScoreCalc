using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data.OleDb;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Helpers;

using RegScoreCalc.Code;
using RegScoreCalc.Views;

using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;

namespace RegScoreCalc
{
	public partial class MainForm : RibbonForm
	{
		#region Fields

		protected bool MainDataSetLoaded = false;
		protected bool BillingDataSetLoaded = false;

		protected DatabaseVersion databaseVersion;
		protected string databaseFilePath;

		protected static string _strAppName = "Score Calculator";
		protected static Color _clrBackground = Color.FromArgb(191, 219, 255);

		protected static ViewsManager _views;

		protected FormSplashScreen _formSplashScreen;

		protected List<string> _listRecentFiles;
		protected string _strHistoryFilePath;

		protected string _strDocumentsDbPath;
		protected string _strRegExpDbPath;

		public string DatabaseName;

		public ToolTip tip;
		private static ToolTip _globalToolTip;

		protected bool _reviewOpenedFromStart;

		protected bool _preventUpdate;

		protected System.Windows.Forms.Timer _timer;
		protected NotebooksWatcher _watcher;

		public MainDataSet.ColRegExpRow _lastSelectedColRegExpRow;

		public string DbPassword { get; set; }

		#endregion

		#region Properties

		public bool ReviewMLOpenedFromStart
		{
			get { return _reviewOpenedFromStart; }
			set { _reviewOpenedFromStart = value; }
		}

		public static string AppName
		{
			get { return _strAppName; }
		}

		public static ViewsManager ViewsManager
		{
			get { return _views; }
		}

		public static Color ColorBackground
		{
			get { return _clrBackground; }
			set { _clrBackground = value; }
		}

		public bool IsDocumentsDbLoaded
		{
			get { return !String.IsNullOrEmpty(_strDocumentsDbPath); }
		}

		public bool IsRegExpDbLoaded
		{
			get { return !String.IsNullOrEmpty(_strRegExpDbPath); }
		}

		public string DocumentsDbPath
		{
			get { return _strDocumentsDbPath; }
		}

		public string RegExpsDbPath
		{
			get { return _strRegExpDbPath; }
		}

        public string DataBaseFilePath
        {
            get { return databaseFilePath; }
        }

		#endregion

		#region Ctors

		public MainForm()
		{
			InitializeComponent();

			_strDocumentsDbPath = "";

			tip = new ToolTip();
			_globalToolTip = new ToolTip();

			this.KeyPreview = true;
			this.KeyDown += MainForm_OnKeyDown;

			_timer = new System.Windows.Forms.Timer();
			_timer.Tick += timer_Tick;
			_timer.Interval = 100;
		}

		#endregion

		#region Events

		private void MainForm_Load(object sender, EventArgs e)
		{
#if !DEBUG
			ShowSplashScreen();
#endif

			DateTime dt = DateTime.Now;
			_views = new ViewsManager(this, ribbon);
			_views.BeforeViewCreate += ViewsManager_BeforeViewCreate;

			//Add views to dropdown list
			_views.AddViewType("Default", typeof(ViewDefault), false, true, true);
			_views.AddViewType("Advanced", typeof(ViewAdvanced), true, true, true);
			_views.AddViewType("Template", typeof(ViewTemplate), true, false, true);
			_views.AddViewType("Statistics", typeof(ViewStatistics), false, false, true);
			_views.AddViewType("Categories Export", typeof(ViewExport), false, true, false);
			_views.AddViewType("SVM", typeof(ViewSVM), false, false, false);

			_views.AddViewType("Accuracy", typeof(ViewBILLING_1), false, false, false);

			_views.AddViewType("Columns RegEx", typeof(ViewColumnsRegEx), false, false, false);

			_views.AddViewType("HTML", typeof(ViewHtmlBase), false, false, false, true, true);

			_views.AddViewType("ReviewML", typeof(ViewReviewML), false, false, false, false);

			_views.LoadFormState();

			this.BackColor = MainForm.ColorBackground;

			Control.CheckForIllegalCrossThreadCalls = false;

			UpdateControls(DatabaseType.Documents, false);
			UpdateControls(DatabaseType.RegExps, false);

			sourceRegExp.CurrentChanged += sourceRegExp_CurrentChanged;
			sourceColRegExp.CurrentChanged += sourceColRegExp_CurrentChanged;

			//////////////////////////////////////////////////////////////////////////

			_strHistoryFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
																			 .Location), "History.txt");
			_listRecentFiles = new List<string>();

			UpdateTitle();

			LoadHistory();

#if !DEBUG
			TimeSpan span = DateTime.Now - dt;
			while (span.TotalSeconds < 3)
			{
				Application.DoEvents();

				System.Threading.Thread.Sleep(50);

				span = DateTime.Now - dt;
			}

			HideSplashScreen();
#endif

			if (_listRecentFiles.Count > 0)
			{
				FormRecentDatasets formRecentDatasets = new FormRecentDatasets(_listRecentFiles);
				var dlgres = formRecentDatasets.ShowDialog(this);

				SaveHistory();

				if (dlgres == DialogResult.OK)
				{
					string strFullPath = formRecentDatasets.SelectedDataset;

					OpenDatabaseFile(strFullPath);
				}
			}

			FillHistoryButtons();
			FillNotebooksMenu();


            InsertLineSpacingItem("1", 0);
            InsertLineSpacingItem("1,25", 1);
            InsertLineSpacingItem("1,5", 2);
            InsertLineSpacingItem("2", 3);

            string strText = "";

            switch (_views.LineSpacing)
            {
                case 0:
                    strText = "1";
                    break;
                case 1:
                    strText = "1,25";
                    break;
                case 2:
                    strText = "1,5";
                    break;
                case 3:
                    strText = "2";
                    break;
                default :
                    strText = "1";
                    break;
            }

            if (!String.IsNullOrEmpty(strText))
                cmbLineSpacing.TextBoxText = strText;
            
        }

		private bool ViewsManager_BeforeViewCreate(ViewType viewType)
		{
			try
			{
				bool loaded = true;

				if (viewType.Name == "Accuracy")
				{
					if (!BillingDataSetLoaded)
					{
						var connectionString = GetConnectionString(databaseFilePath);
						if (String.IsNullOrEmpty(connectionString))
						{
							MessageBox.Show("Cannot open database.");
							return false;
						}

						OleDbConnection connection = new OleDbConnection(connectionString);
						//Load billingDataSet
						if (GetDatabaseVersion(connection) == DatabaseVersion.Billing_1)
						{
							databaseVersion = DatabaseVersion.Billing_1;
							loaded = this.LoadDocumentsDatabase(databaseFilePath, false);
							this.LoadRegExpDatabase(databaseFilePath, false);

							BillingDataSetLoaded = loaded;
						}
						else
						{
							BillingDataSetLoaded = false;
							loaded = false;
							MessageBox.Show(this, "Couldn't load view for this database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}
				else
				{
					if (!MainDataSetLoaded)
					{
						//Load MainDataSet
						databaseVersion = DatabaseVersion.Default;
						LoadDocumentsDatabase(databaseFilePath, false);
						this.LoadRegExpDatabase(databaseFilePath, false);

						MainDataSetLoaded = true;
					}

					if (viewType.Name == "ReviewML" || viewType.Name == "ReviewML NEW")
						ReviewMLOpenedFromStart = true;
				}
				if (!loaded)
					return false;
			}
			catch (Exception ex)
			{
				ShowExceptionMessage(ex);
			}

			return true;
		}
		protected void UpdateTitle()
		{
			var text = "Document Review Tool v" + Assembly.GetExecutingAssembly()
			                                              .GetName()
			                                              .Version.ToString(3);

			if (!String.IsNullOrEmpty(databaseFilePath))
				text += " - " + Path.GetFileName(databaseFilePath);

			this.Text = text;
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				adapterRegExp.Update(datasetMain.RegExp);
				datasetMain.RegExp.AcceptChanges();

				adapterColRegExp.Update(datasetMain.ColRegExp);
				datasetMain.ColRegExp.AcceptChanges();

				if (datasetMain.HasChanges())
				{
					/*DataSet ds = datasetMain.GetChanges();
					foreach (DataTable table in ds.Tables)
					{
						foreach (DataRow row in table.Rows)
						{

						}
					}*/

					//If the change is because of the ReviewML ignore it
					//if (datasetMain.Documents.Columns.IndexOf("RankDataProperty") < 0)
					//{
					DialogResult result = MessageBox.Show("Do you wish to save changes?", _strAppName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					if (result == DialogResult.Yes)
					{
						SaveAll();

						_views.DestroyAllViews();
					}
					else if (result == DialogResult.Cancel)
						e.Cancel = true;
					//}
				}
				else
					_views.DestroyAllViews();

				_views.SaveConfig();
			}
			catch (Exception ex)
			{
				ShowExceptionMessage(ex);
			}
		}

		private void MainForm_OnKeyDown(object sender, KeyEventArgs keyEventArgs)
		{
			try
			{
				_views.ProcessHotkey(keyEventArgs);
			}
			catch (Exception ex)
			{
				ShowExceptionMessage(ex);
			}
		}

		private void sourceRegExp_CurrentChanged(object sender, EventArgs e)
		{
			try
			{
				if (_preventUpdate)
					return;

				_preventUpdate = true;

				_views.MainForm.sourceRegExp.RaiseListChangedEvents = false;

				var table = datasetMain.RegExp.GetChanges();
				if (table != null && table.Rows.Count > 0)
				{
					var rowview = (DataRowView) sourceRegExp.Current;
					if (rowview != null && rowview.Row != null)
					{
						var rowRegExp = (MainDataSet.RegExpRow) rowview.Row;
						if (!rowRegExp.IsRegExpNull())
						{
							if (!String.IsNullOrEmpty(rowRegExp.RegExp))
							{
								CleanupEmptyRegExp();

								FillRegExpGUIDs();

								SaveRegExps();

								///////////////////////////////////////////////////////////////////////////////

								table = datasetMain.ColorCodes.GetChanges();
								if (table != null && table.Rows.Count > 0)
								{
									adapterColorCodes.Update(datasetMain.ColorCodes);
									adapterColorCodes.Fill(datasetMain.ColorCodes);
								}

								table = datasetMain._Relations.GetChanges();
								if (table != null && table.Rows.Count > 0)
								{
									adapterRelations.Update(datasetMain._Relations);
									adapterRelations.Fill(datasetMain._Relations);
								}
							}
						}
					}
				}

				_views.MainForm.sourceRegExp.RaiseListChangedEvents = true;

				_views.InvokeRefreshHighlights();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, _strAppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			_preventUpdate = false;
		}

		public void sourceColRegExp_CurrentChanged(object sender, EventArgs e)
		{
			try
			{
				if (_preventUpdate)
					return;

				_preventUpdate = true;

				var bindingSource = sender as BindingSource;
				if (bindingSource != null)
				{
					var rowview = (DataRowView)bindingSource.Current;
					if (rowview != null && rowview.Row != null)
					{
						var rowColRegExp = (MainDataSet.ColRegExpRow)rowview.Row;
						if (!rowColRegExp.IsRegExpNull() && !String.IsNullOrEmpty(rowColRegExp.RegExp))
						{
							if (!Equals(rowColRegExp, _lastSelectedColRegExpRow) || rowColRegExp.RowState == DataRowState.Modified || rowColRegExp.RowState == DataRowState.Added)
							{
								CleanupEmptyColRegExp();

								FillColRegExpGUIDs();

								SaveColRegExps();

								_views.UpdateViews();

								_lastSelectedColRegExpRow = rowColRegExp;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, _strAppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			_preventUpdate = false;
		}

		protected void SaveRegExps()
		{
			sourceRegExp.EndEdit();

			adapterRegExp.Update(datasetMain.RegExp);
			datasetMain.RegExp.AcceptChanges();
		}

		protected void SaveColRegExps()
		{
			foreach (var view in _views.GetOpenedViewsByType<ViewColumnsRegEx>())
			{
				view.EndEdit();
			}

			adapterColRegExp.Update(datasetMain.ColRegExp);
			datasetMain.ColRegExp.AcceptChanges();
		}

		public void RibbonButton_MouseEnter(object sender, MouseEventArgs e)
		{
			var btn = (RibbonButton)sender;
			var toolTip = btn.ToolTip;
			if (String.IsNullOrEmpty(toolTip))
				toolTip = btn.Text;

			tip.Show(toolTip, this, btn.Bounds.Right + 10, e.Location.Y + 10, 1000);
		}

		private void OnRecentFile_Click(object sender, EventArgs e)
		{
			if (sender is RibbonButton)
			{
				RibbonButton btnRecent = (RibbonButton)sender;
				string strFilePath = (string)btnRecent.Tag;
				if (!String.IsNullOrEmpty(strFilePath))
				{
					OpenDatabaseFile(strFilePath);
				}
			}
		}

		private void btnOpenDocuments_Click(object sender, EventArgs e)
		{
			OpenDatabase(DatabaseType.Documents);
		}

		private void btnSaveDocuments_Click(object sender, EventArgs e)
		{
			SaveDocumentsDatabase();
			MessageBox.Show("Operation finished", _strAppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btnOpenDatabase_Click(object sender, EventArgs e)
		{
            /*
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "MS Access database (*.accdb;*.mdb)|*.accdb;*.mdb|All files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = openFileDialog.FileName;
				OpenDatabaseFile(fileName);
			}
            */

            FormRecentDatasets formRecentDatasets = new FormRecentDatasets(_listRecentFiles);
            var dlgres = formRecentDatasets.ShowDialog(this);

            SaveHistory();

            if (dlgres == DialogResult.OK)
            {
                string strFullPath = formRecentDatasets.SelectedDataset;

                OpenDatabaseFile(strFullPath);
            }

        }

		public void OpenDatabaseFile(string fileName)
		{   

            databaseFilePath = fileName;

			DatabaseName = Path.GetFileName(databaseFilePath);

			var connectionString = GetConnectionString(databaseFilePath);
			if (String.IsNullOrEmpty(connectionString))
			{
				MessageBox.Show("Cannot open database.");
				return;
			}

			OleDbConnection connection = new OleDbConnection(connectionString);

			var databaseType = GetDatabaseVersion(connection);

			BillingDataSetLoaded = false;
			MainDataSetLoaded = false;

			UpdateTitle();

			//destroy all opened views
            
			var openedViews = _views.GetAllOpenedViews();
			while (openedViews.Count > 0)
			{
                var view = openedViews[0];

                _views.SaveLayout(view);

                view.DestroyView();

                openedViews.Remove(view);
                
                GC.Collect();
                GC.WaitForPendingFinalizers();                                
            }
            
			//Check for autoOpen views
			var autoOpenViews = _views.GetAutoOpenViews();
			if (autoOpenViews.Contains("Accuracy"))
			{
				if (!BillingDataSetLoaded)
				{
					//Load MainDataSet
					databaseVersion = DatabaseVersion.Billing_1;
					var loaded = LoadDocumentsDatabase(databaseFilePath, false);
					if (loaded)
					{
						this.LoadRegExpDatabase(databaseFilePath, false);
						BillingDataSetLoaded = true;


                        this.sourceRegExp.RaiseListChangedEvents = true;
                        this.sourceRegExp.ResetBindings(false);

                        this.sourceColRegExp.RaiseListChangedEvents = true;
                        this.sourceColRegExp.ResetBindings(false);

                        _views.HandleDatabaseLoad(true, true, true);
					}
					else
						MessageBox.Show("This database doesn't support auto start view.", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			else
			{
				//Init dataset for view that is AutoStart
				if (!MainDataSetLoaded)
				{
					//Load MainDataSet
					databaseVersion = DatabaseVersion.Default;
					LoadDocumentsDatabase(databaseFilePath, false);
					this.LoadRegExpDatabase(databaseFilePath, false);

					MainDataSetLoaded = true;

                    this.sourceRegExp.RaiseListChangedEvents = true;
                    this.sourceRegExp.ResetBindings(false);

                    this.sourceColRegExp.RaiseListChangedEvents = true;
                    this.sourceColRegExp.ResetBindings(false);

                    _views.HandleDatabaseLoad(true, true, true);
				}
			}
        }
            

		private void btnSaveDatabase_Click(object sender, EventArgs e)
		{
			this.SaveAll();

			MessageBox.Show("Operation finished", MainForm._strAppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void btnSaveAsDocuments_Click(object sender, EventArgs e)
		{
			SaveDatabaseAs(DatabaseType.Documents);
			MessageBox.Show("Operation finished", _strAppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btnOpenRegExps_Click(object sender, EventArgs e)
		{
			OpenDatabase(DatabaseType.RegExps);
		}

		private void btnSaveRegExps_Click(object sender, EventArgs e)
		{
			SaveRegExpsDatabase();
			MessageBox.Show("Operation finished", _strAppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btnSaveAsRegExps_Click(object sender, EventArgs e)
		{
			SaveDatabaseAs(DatabaseType.RegExps);
			MessageBox.Show("Operation finished", _strAppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btnOpenView_Click(object sender, EventArgs e)
		{
			string viewName = _views.GetSelectedViewName();
			if (!String.IsNullOrEmpty(viewName))
			{
				try
				{
					_views.CreateSelectedView();
				}
				catch
				{
					MessageBox.Show(this, "Couldn't load view for this database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void orbitemOpenDocuments_Click(object sender, EventArgs e)
		{
			OpenDatabase(DatabaseType.Documents);
		}

		private void orbitemSaveDocuments_Click(object sender, EventArgs e)
		{
			SaveDocumentsDatabase();
		}

		private void orbitemSaveAsDocuments_Click(object sender, EventArgs e)
		{
			SaveDatabaseAs(DatabaseType.Documents);
		}

		private void orbitemOpenRegExps_Click(object sender, EventArgs e)
		{
			OpenDatabase(DatabaseType.RegExps);
		}

		private void orbitemSaveRegExps_Click(object sender, EventArgs e)
		{
			SaveRegExpsDatabase();
		}

		private void orbitemSaveAsRegExps_Click(object sender, EventArgs e)
		{
			SaveRegExpsDatabase();
		}

		private void btnSaveAll_Click(object sender, EventArgs e)
		{
			SaveAll();
		}

        private void btnPythonSettings_Click(object sender, EventArgs e)
        {
            PythonSettings();
        }

        private void orbitemSaveAll_Click(object sender, EventArgs e)
		{
			SaveAll();
		}

		private void orbitemClearHistory_Click(object sender, EventArgs e)
		{
			_listRecentFiles.Clear();
			SaveHistory();
			FillHistoryButtons();
		}

		private void orbitemExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void notebookItem_Click(object sender, EventArgs e)
		{
			try
			{
				var item = (RibbonItem) sender;
				var filePath = (string) item.Tag;
				if (File.Exists(filePath))
				{
					var viewTitle = item.Text;

					_views.CreateView("Notebooks", true, false, filePath, viewTitle);
				}
				else if (filePath != null)
					Process.Start("explorer.exe", filePath);
				else if (!_timer.Enabled)
					_timer.Start();
			}
			catch (Exception ex)
			{
				ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation

		protected void SaveDataTable(DataTable table, bool bRegExp, bool bColRegExp, BackgroundWorker worker)
		{
			var varModifiedRows = table.AsEnumerable()
									   .Where(x => x.RowState == DataRowState.Modified);

			double dModifiedRowsCount = varModifiedRows.Count();

			if (dModifiedRowsCount > 0)
			{
				int nCounter = 1;

				int nProgress;
				string strProgress;

				OleDbConnection conn = this.adapterDocuments.Connection;
				if (conn.State != ConnectionState.Open)
					conn.Open();

				OleDbCommand cmd = conn.CreateCommand();
				cmd.CommandText = "UPDATE Documents SET Score = @Score, Category = @Category, Rank = @Rank WHERE ED_ENC_NUM = @ED_ENC_NUM";

				foreach (DataRow row in varModifiedRows)
				{
					if (bRegExp)
					{
						if (bColRegExp)
							this.adapterColRegExp.Update(row);
						else
							this.adapterRegExp.Update(row);
					}
					else
					{
						MainDataSet.DocumentsRow rowDocument = (MainDataSet.DocumentsRow)row;

						cmd.Parameters.Clear();
						cmd.Parameters.Add(new OleDbParameter("@Score", rowDocument.Score));

						if (!rowDocument.IsCategoryNull())
							cmd.Parameters.Add(new OleDbParameter("@Category", rowDocument.Category));
						else
							cmd.Parameters.Add(new OleDbParameter("@Category", DBNull.Value));

						if (!rowDocument.IsRankNull())
							cmd.Parameters.Add(new OleDbParameter("@Rank", rowDocument.Rank));
						else
							cmd.Parameters.Add(new OleDbParameter("@Rank", DBNull.Value));

						cmd.Parameters.Add(new OleDbParameter("@ED_ENC_NUM", rowDocument.ED_ENC_NUM));

						cmd.ExecuteNonQuery();

						rowDocument.AcceptChanges();

						//this.adapterDocuments.Update(row);
					}

					//////////////////////////////////////////////////////////////////////////

					if (worker != null)
					{
						nCounter++;

						nProgress = (int)(((double)nCounter / dModifiedRowsCount) * (double)100);
						strProgress = "updating row " + nCounter.ToString() + " of " + dModifiedRowsCount.ToString("0");

						worker.ReportProgress(nProgress, strProgress);

						//////////////////////////////////////////////////////////////////////////

						if (worker.CancellationPending)
							break;
					}
				}
			}
		}

		public string GetConnectionString(string strFilePath)
		{
			bool invalidPasword;
			var connectionString = ConnectionStringHelper.GetConnectionString(strFilePath, this.DbPassword, out invalidPasword);

			while (String.IsNullOrEmpty(connectionString) && invalidPasword)
			{
				var formPassword = new FormDatabasePassword();
				var dlgres = formPassword.ShowDialog();
				if (dlgres != DialogResult.OK)
					break;

				connectionString = ConnectionStringHelper.GetConnectionString(strFilePath, formPassword.Password, out invalidPasword);
				if (!String.IsNullOrEmpty(connectionString))
					this.DbPassword = formPassword.Password;
				else
					MessageBox.Show("Invalid password", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			return connectionString;
		}

		public void SaveAll()
		{
			SaveDocumentsDatabase();
			SaveRegExpsDatabase();
		}

        private void OnSelectFont_Clicked(object sender, EventArgs e)
        {
            FontDialog dlgFont = new FontDialog();
            dlgFont.FixedPitchOnly = true;
            dlgFont.Font = new Font(_views.FontFamily, _views.FontSize, _views.FontStyle);           


            if (dlgFont.ShowDialog() == DialogResult.OK)
            {

                _views.FontFamily = dlgFont.Font.FontFamily.Name;
                _views.FontSize = dlgFont.Font.Size;
                _views.FontStyle = dlgFont.Font.Style;

                _views.InvokeRefreshFont();
            }
        }

        protected void InsertLineSpacingItem(string strItem, int nItem)
        {
            RibbonButton btnItem = new RibbonButton(strItem);
            this.cmbLineSpacing.DropDownItems.Add(btnItem);
            btnItem.Tag = nItem;
            btnItem.Click += new EventHandler(OnLineSpacingItem_Clicked);
        }

        private void OnLineSpacingItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is RibbonButton)
                {
                    RibbonButton btn = (RibbonButton)sender;
                    int nIndex = (int)btn.Tag;

                   
                    string strText = "";
                    
                    switch (nIndex)
                    {
                        case 0:
                            strText = "1";
                            break;

                        case 1:
                            strText = "1,25";
                            break;

                        case 2:
                            strText = "1,5";
                            break;

                        case 3:
                            strText = "2";
                            break;
                    }

                    if (!String.IsNullOrEmpty(strText))
                        cmbLineSpacing.TextBoxText = strText;

                    _views.LineSpacing = nIndex;

                    _views.InvokeRefreshLineSpacing();                    
                }
               
            }
            catch
            {
            }
        }

        public void PythonSettings()
        {
            FormPythonSettings formPythonSettings = new FormPythonSettings(_views.AnacondaPath, _views.PythonEnv);

            if (formPythonSettings.ShowDialog() == DialogResult.OK)
            {
                _views.AnacondaPath = formPythonSettings.AnacondaPath;
                _views.PythonEnv = formPythonSettings.VirtualEnv;
                _views.PythonVersion = formPythonSettings.PythonVersion;
                _views.SaveConfig();
            }
        }
        protected void SaveDocumentsDatabase()
		{
			try
			{
				if (this.IsDocumentsDbLoaded)
				{
					this.sourceDocuments.RaiseListChangedEvents = false;
					this.sourceCategories.RaiseListChangedEvents = false;

					FormGenericProgress formGenericProgress = new FormGenericProgress("Saving documents data, please wait...", new LengthyOperation(DoSaveDocumentsDatabase), null, true);
					formGenericProgress.ShowDialog();

					this.sourceDocuments.RaiseListChangedEvents = true;
					this.sourceDocuments.ResetBindings(false);

					this.sourceCategories.RaiseListChangedEvents = true;
					this.sourceCategories.ResetBindings(false);
				}
			}
			catch
			{
			}
		}

		public bool DoSaveDocumentsDatabase(BackgroundWorker worker, object objArgument)
		{
			SaveDataTable(this.datasetMain.Documents, false, false, worker);

			return true;
		}

		protected void SaveRegExpsDatabase()
		{
			try
			{
				if (this.IsRegExpDbLoaded)
				{
					this.sourceRegExp.RaiseListChangedEvents = false;
					this.sourceColRegExp.RaiseListChangedEvents = false;

					FormGenericProgress formGenericProgress = new FormGenericProgress("Saving regular expressions data, please wait...", new LengthyOperation(DoSaveRegExpDatabase), null, true);
					formGenericProgress.ShowDialog();

					this.sourceRegExp.RaiseListChangedEvents = true;
					this.sourceRegExp.ResetBindings(false);

					this.sourceColRegExp.RaiseListChangedEvents = true;
					this.sourceColRegExp.ResetBindings(false);
				}
			}
			catch
			{
			}
		}

		public bool DoSaveRegExpDatabase(BackgroundWorker worker, object objArgument)
		{
			sourceRegExp.EndEdit();

			foreach (var view in _views.GetOpenedViewsByType<ViewColumnsRegEx>())
			{
				view.EndEdit();
			}

			sourceRegExp.RaiseListChangedEvents = false;

			///////////////////////////////////////////////////////////////////////////////

			CleanupEmptyRegExp();
			CleanupEmptyColRegExp();

			FillRegExpGUIDs();
			FillColRegExpGUIDs();

			///////////////////////////////////////////////////////////////////////////////

			SaveDataTable(this.datasetMain.RegExp, true, false, worker);
			adapterRegExp.Update(this.datasetMain.RegExp);
			datasetMain.RegExp.AcceptChanges();

			///////////////////////////////////////////////////////////////////////////////

			SaveDataTable(this.datasetMain.ColRegExp, true, true, worker);
			adapterColRegExp.Update(this.datasetMain.ColRegExp);
			datasetMain.ColRegExp.AcceptChanges();

			///////////////////////////////////////////////////////////////////////////////

			sourceRegExp.RaiseListChangedEvents = true;

			return true;
		}

		protected bool LoadDocumentsDatabase(string strFilePath, bool bSilent)
		{
			UpdateControls(DatabaseType.Documents, false);

			try
			{
				_views.BeforeDocumentsTableLoad(false);

				var formLoadingProgress = new FormLoadingProgress(DoLoadDocumentsDatabase, strFilePath);
				formLoadingProgress.ShowDialog(this);

				_views.AfterDocumentsTableLoad(false);

				UpdateControls(DatabaseType.Documents, true);

				_strDocumentsDbPath = strFilePath;
				AddFileToHistory(_strDocumentsDbPath);

				UpdateTitle();

				return true;
			}
			catch
			{
				if (!bSilent)
					MessageBox.Show("Selected database file is not valid", _strAppName, MessageBoxButtons.OK, MessageBoxIcon.Error);

				return false;
			}
		}

		public bool RegexpToGroupsTableExist()
		{
			var connectionString = GetConnectionString(databaseFilePath);
			if (String.IsNullOrEmpty(connectionString))
			{
				MessageBox.Show("Cannot open database.");
				return false;
			}

			OleDbConnection connection = new OleDbConnection(connectionString);

			//Check if the database have the column Ranks, and if it doesnt have alter table to add that column
			try
			{
				connection.Open();
				string strCommand = "SELECT * FROM RegexpToGroups";
				OleDbCommand command = new OleDbCommand(strCommand, connection);
				command.ExecuteNonQuery();

				return true;
			}
			catch
			{
				return false;
			}
		}

		protected bool DoLoadDocumentsDatabase(BackgroundWorker worker, object objArgument)
		{
			worker.ReportProgress(0, "Documents");

			if (adapterDocuments.Connection != null && adapterDocuments.Connection.State == ConnectionState.Open)
				adapterDocuments.Connection.Close();

			string strFilePath = (string)objArgument;

			var connectionString = GetConnectionString(strFilePath);
			if (String.IsNullOrEmpty(connectionString))
			{
				MessageBox.Show("Cannot open database.");
				return false;
			}

			OleDbConnection connection = new OleDbConnection(connectionString);

			//Check if the database have the column Ranks, and if it doesnt have alter table to add that column
			try
			{
				connection.Open();

				///////////////////////////////////////////////////////////////////////////////

				try
				{
					var indexName = GetIndexName(connection);
					if (!String.IsNullOrEmpty(indexName))
						DeleteIndex(connection, indexName);

					CreateIndex(connection);
				}
				catch (Exception ex)
				{
					ShowExceptionMessage(ex);
				}

				///////////////////////////////////////////////////////////////////////////////

				string strCommand = "SELECT Rank, Proc1SVM, Proc3SVM FROM Documents";
				OleDbCommand command = new OleDbCommand(strCommand, connection);
				command.ExecuteNonQuery();
			}
			catch
			{
				MessageBox.Show(this, "You are using old version of database. Now it will be updated.", "Old database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				try
				{
					try
					{
						string strCommand1 = "ALTER TABLE Documents ADD COLUMN Rank Integer";
						OleDbCommand command1 = new OleDbCommand(strCommand1, connection);
						command1.ExecuteNonQuery();
					}
					catch
					{
					}
					try
					{
						string strCommand2 = "ALTER TABLE Documents ADD COLUMN Proc1SVM Integer";
						OleDbCommand command2 = new OleDbCommand(strCommand2, connection);
						command2.ExecuteNonQuery();

						string strCommand3 = "ALTER TABLE Documents ADD COLUMN Proc3SVM Integer";
						OleDbCommand command3 = new OleDbCommand(strCommand3, connection);
						command3.ExecuteNonQuery();
					}
					catch
					{
					}
				}
				catch
				{
				}
			}

			if (databaseVersion == DatabaseVersion.Billing_1)
			{
				if (GetDatabaseVersion(connection) == DatabaseVersion.Billing_1)
				{
					try
					{
						if (connection.State != ConnectionState.Open)
							connection.Open();

						string strCommand = "SELECT * FROM ICD9Groups, DocsToFilters";
						OleDbCommand command = new OleDbCommand(strCommand, connection);
						command.ExecuteNonQuery();
					}
					catch
					{
						MessageBox.Show(this, "There is no ICD9Groups table in database, view will not be opened.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					//Load data
					LoadDataInBillingDataSet(connection, worker);
				}
				else
					return false;
			}
			else if (databaseVersion == DatabaseVersion.Default)
			{
				//Load data
				LoadDataInMainDataSet(connection, worker);
			}

			if (connection.State == ConnectionState.Open)
				connection.Close();

			return true;
		}

		public void LoadReviewMLDocuments()
		{
			if (this.datasetMain.ReviewMLDocuments.Rows.Count == 0)
			{
				var formGenericProgress = new FormGenericProgress("Loading documents", DoLoadReviewMLDocuments, null, false);
				formGenericProgress.ShowDialog();
			}
		}

		public void LoadReviewMLDocumentsNew()
		{
			if (this.adapterReviewMLDocumentsNew.Table.Rows.Count == 0)
			{
				var formGenericProgress = new FormGenericProgress("Loading documents", DoLoadReviewMLDocumentsNew, null, false);
				formGenericProgress.ShowDialog();
			}
		}

		private bool DoLoadReviewMLDocuments(BackgroundWorker worker, object objArgument)
		{
			ReportLoadingProgress(worker, datasetMain.ReviewMLDocuments);
			this.adapterReviewMLDocuments.Fill(datasetMain.ReviewMLDocuments);

			return true;
		}

		private bool DoLoadReviewMLDocumentsNew(BackgroundWorker worker, object objArgument)
		{
			ReportLoadingProgress(worker, this.adapterReviewMLDocumentsNew.Table);
			this.adapterReviewMLDocumentsNew.Fill();

			return true;
		}

		private void LoadDataInMainDataSet(OleDbConnection connection, BackgroundWorker worker)
		{
			this.adapterReviewMLDocuments.Connection = connection;
			this.adapterCategories.Connection = connection;
			this.adapterColorCodes.Connection = connection;
			this.adapterColumns.Connection = connection;
			this.adapterColScript.Connection = connection;
            this.adapterColPython.Connection = connection;
            this.adapterEntities.Connection = connection;

            _views.DocumentsService.OpenConnection(connection.ConnectionString, _views.DocumentsServiceDebug);

            _views.TabSettings = _views.DocumentsService.GetDocumentColumnSettings();

            ReportLoadingProgress(worker, datasetMain.Categories);
			this.adapterCategories.Fill(this.datasetMain.Categories);

			ReportLoadingProgress(worker, datasetMain.ColorCodes);
			this.adapterColorCodes.Fill(this.datasetMain.ColorCodes);

			ReportLoadingProgress(worker, datasetMain.ColScript);
			this.adapterColScript.Fill(this.datasetMain.ColScript);

            ReportLoadingProgress(worker, datasetMain.ColPython);
            this.adapterColPython.Fill(this.datasetMain.ColPython);

            ReportLoadingProgress(worker, datasetMain.Entities);
            this.adapterEntities.Fill(this.datasetMain.Entities);

            ///////////////////////////////////////////////////////////////////////////////

            this.adapterDynamicColumns.Connection = connection;
			this.adapterDynamicColumnCategories.Connection = connection;

			ReportLoadingProgress(worker, datasetMain.DynamicColumns);
			this.adapterDynamicColumns.Fill(datasetMain.DynamicColumns);

			ReportLoadingProgress(worker, datasetMain.DynamicColumnCategories);
			this.adapterDynamicColumnCategories.Fill(datasetMain.DynamicColumnCategories);

			ReportLoadingProgress(worker, datasetMain.Columns);
			this.adapterColumns.Fill(datasetMain.Columns);
			

			///////////////////////////////////////////////////////////////////////////////

			/*var cmd = connection.CreateCommand();
			cmd.CommandText = "DELETE FROM Documents";
			cmd.ExecuteNonQuery();

			cmd = connection.CreateCommand();
			cmd.CommandText = "INSERT INTO Documents (ED_ENC_NUM, NOTE_TEXT) VALUES (@ID, @Text)";
			cmd.Parameters.AddWithValue("@ID", 0d);
			cmd.Parameters.AddWithValue("@Text", "");

			var text = @"Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

			for (var i = 0; i < 60000; i++)
			{
				cmd.Parameters[0].Value = i + 1;
				cmd.Parameters[1].Value = text;

				cmd.ExecuteNonQuery();
			}*/

			///////////////////////////////////////////////////////////////////////////////
            
			var dynamicColumnsList = GetDynamicColumnsList(datasetMain.DynamicColumns);

			var missingDynamicColumnsList = adapterDocuments.Initialize(connection, datasetMain.Documents, "Documents", "ED_ENC_NUM", dynamicColumnsList);
			if (missingDynamicColumnsList.Any())
			{
				var join = String.Join(", ", missingDynamicColumnsList.Select(x => x.Name));

				var message = String.Format("Following dynamic column(s) not found in Documents table:{0}{1}{0}{0}They will be loaded as normal columns.", Environment.NewLine, @join);

				MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			///////////////////////////////////////////////////////////////////////////////
            
			var table = new MainDataSet.ReviewMLDocumentsDataTable();

			var dataSet = new DataSet();
			dataSet.Tables.Add(table);

			this.adapterReviewMLDocumentsNew.Initialize(connection, table, "Documents", "ED_ENC_NUM", dynamicColumnsList);

			this.sourceReviewMLDocumentsNew.DataMember = "ReviewMLDocuments";
			this.sourceReviewMLDocumentsNew.DataSource = dataSet;
            
			///////////////////////////////////////////////////////////////////////////////
            
			ReportLoadingProgress(worker, datasetMain.Documents);

			this.adapterDocuments.Fill();
		}

		public static List<ColumnInfo> GetDynamicColumnsList(MainDataSet.DynamicColumnsDataTable columnsTable)
		{

            return columnsTable.Where(x => !x.Title.StartsWith("NOTE_TEXT"))
                                .Select(x => new ColumnInfo
                                        {
                                            Name = x.Title,
                                            Type = CustomOleDbDataAdapter.ColumnTypeToClrType((DynamicColumnType)x.Type),
                                            IsDynamic = true,
                                            DynamicColumnID = x.ID
                                        })
                                .ToList();
		}

		private List<ColumnInfo> _addedColumns = new List<ColumnInfo>();

		public List<ColumnInfo> GetAddedColumns()
		{
			return _addedColumns;
		}

		private void LoadDataInBillingDataSet(OleDbConnection connection, BackgroundWorker worker)
		{
			this.adapterBillingBilling.Connection = connection;
			this.adapterDocumentsBilling.Connection = connection;
			this.adapterCategoriesBilling.Connection = connection;

			this.adapterICDFiltersBilling.Connection = connection;
			this.adapterICDCodesBilling.Connection = connection;
			this.adapterICD9GroupsBilling.Connection = connection;

			this.adapterDocsToFiltersBilling.Connection = connection;
			//this.adapterVisibleDocumentsBilling.Connection = connection;
			this.adapterCategoryToFilterExclusionBilling.Connection = connection;
			this.adapterRegexpToGroupsBilling.Connection = connection;

			this.adapterColScriptBilling.Connection = connection;

			this.adapterColorCodesBilling.Connection = connection;

			ReportLoadingProgress(worker, datasetBilling.Billing);
			this.adapterBillingBilling.Fill(this.datasetBilling.Billing);

			ReportLoadingProgress(worker, datasetBilling.Documents);
			this.adapterDocumentsBilling.Fill(this.datasetBilling.Documents);

			ReportLoadingProgress(worker, datasetBilling.Categories);
			this.adapterCategoriesBilling.Fill(this.datasetBilling.Categories);

			ReportLoadingProgress(worker, datasetBilling.ICD9Groups);
			this.adapterICD9GroupsBilling.Fill(this.datasetBilling.ICD9Groups);

			ReportLoadingProgress(worker, datasetBilling.ICDCodes);
			this.adapterICDCodesBilling.Fill(this.datasetBilling.ICDCodes);

			ReportLoadingProgress(worker, datasetBilling.ICDFilters);
			this.adapterICDFiltersBilling.Fill(this.datasetBilling.ICDFilters);

			ReportLoadingProgress(worker, datasetBilling.DocsToFilters);
			this.adapterDocsToFiltersBilling.Fill(this.datasetBilling.DocsToFilters);

			ReportLoadingProgress(worker, datasetBilling.CategoryToFilterExclusion);
			this.adapterCategoryToFilterExclusionBilling.Fill(this.datasetBilling.CategoryToFilterExclusion);

			ReportLoadingProgress(worker, datasetBilling.RegexpToGroups);
			this.adapterRegexpToGroupsBilling.Fill(this.datasetBilling.RegexpToGroups);

			ReportLoadingProgress(worker, datasetBilling.ColScript);
			this.adapterColScriptBilling.Fill(this.datasetBilling.ColScript);

			ReportLoadingProgress(worker, datasetBilling.ColorCodes);
			this.adapterColorCodesBilling.Fill(this.datasetBilling.ColorCodes);
		}

		protected bool LoadRegExpDatabase(string strFilePath, bool bSilent)
		{
			bool bNewVersion = true;

			UpdateControls(DatabaseType.RegExps, false);

			try
			{
				var formLoadingProgress = new FormLoadingProgress(DoLoadRegExpDatabase, strFilePath);
				formLoadingProgress.ShowDialog(this);

				UpdateControls(DatabaseType.RegExps, true);

				_strRegExpDbPath = strFilePath;
				AddFileToHistory(_strRegExpDbPath);
			}
			catch
			{
				if (!bSilent)
					MessageBox.Show("Selected database file is not valid", _strAppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return bNewVersion;
		}

		protected bool DoLoadRegExpDatabase(BackgroundWorker worker, object objArgument)
		{
			worker.ReportProgress(0, "Regular Expressions");

			bool bContinueLoad = false;
			string strFilePath = (string)objArgument;

			var connectionString = GetConnectionString(strFilePath);
			if (String.IsNullOrEmpty(connectionString))
			{
				MessageBox.Show("Cannot open database.");
				return false;
			}

			OleDbConnection newConnection = new OleDbConnection(connectionString);

			//Check if RegExp is already loaded
			//if (datasetMain.RegExp.Rows.Count == 0)
			{
				this.adapterRegExp.Connection.Close();
				this.adapterColRegExp.Connection.Close();

				newConnection.Open();

				if (!IsGUIDColumnsExist(newConnection))
				{
					MessageBox.Show("Regular expression tables don't have required columns - you are using old database version.", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					return false;
				}

				this.adapterRegExp.Connection = newConnection;

				ReportLoadingProgress(worker, datasetMain.RegExp);
				this.adapterRegExp.Fill(datasetMain.RegExp);

				try
				{
					this.adapterColRegExp.Connection = newConnection;

					ReportLoadingProgress(worker, datasetMain.ColRegExp);
					this.adapterColRegExp.Fill(datasetMain.ColRegExp);
				}
				catch
				{
				}

				this.adapterColorCodes.Connection = newConnection;
				this.adapterRelations.Connection = newConnection;

				if (!IsRelationsTableExist() || !IsColorCodesTableExist())
				{
					DialogResult res = MessageBox.Show("You selected older version of database. Do you wish to upgrade to newer version?", MainForm.AppName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					if (res == DialogResult.Yes)
					{
						if (CreateRelationsTable(newConnection) && CreateColorCodesTable(newConnection))
							bContinueLoad = true;
						else
							MessageBox.Show("Upgrade failed but you still can work with older version of database", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else if (res == DialogResult.No)
						bContinueLoad = true;
				}
				else
				{
					ReportLoadingProgress(worker, datasetMain.ColorCodes);
					this.adapterColorCodes.Fill(this.datasetMain.ColorCodes);

					ReportLoadingProgress(worker, datasetMain._Relations);
					this.adapterRelations.Fill(this.datasetMain._Relations);

					if (!this.datasetMain.RegExp.Columns.Contains("TotalMatches"))
						this.datasetMain.RegExp.Columns.Add("TotalMatches", typeof(int));
					if (!this.datasetMain.RegExp.Columns.Contains("TotalDocuments"))
						this.datasetMain.RegExp.Columns.Add("TotalDocuments", typeof(int));

                    if (!this.datasetMain.RegExp.Columns.Contains("TotalRecords"))
                        this.datasetMain.RegExp.Columns.Add("TotalRecords", typeof(int));
                    if (!this.datasetMain.RegExp.Columns.Contains("TotalCategorized"))
                        this.datasetMain.RegExp.Columns.Add("TotalCategorized", typeof(int));
                    if (!this.datasetMain.RegExp.Columns.Contains("CategorizedRecords"))
                        this.datasetMain.RegExp.Columns.Add("CategorizedRecords", typeof(Dictionary<int, int>));

                    if (!this.datasetMain.ColRegExp.Columns.Contains("TotalMatches"))
						this.datasetMain.ColRegExp.Columns.Add("TotalMatches", typeof(int));

					if (!this.datasetMain.ColRegExp.Columns.Contains("TotalDocuments"))
						this.datasetMain.ColRegExp.Columns.Add("TotalDocuments", typeof(int));

                    if (!this.datasetMain.ColRegExp.Columns.Contains("TotalRecords"))
                        this.datasetMain.ColRegExp.Columns.Add("TotalRecords", typeof(int));

                    if (!this.datasetMain.ColRegExp.Columns.Contains("PosDocuments"))
						this.datasetMain.ColRegExp.Columns.Add("PosDocuments", typeof(int));
					if (!this.datasetMain.ColRegExp.Columns.Contains("PercentPosDocuments"))
						this.datasetMain.ColRegExp.Columns.Add("PercentPosDocuments", typeof(int));

					bContinueLoad = true;
				}
			}           

            return bContinueLoad;
		}

		protected void ReportLoadingProgress(BackgroundWorker worker, DataTable table)
		{
			worker.ReportProgress(1, table.TableName);
		}

		protected void SaveDatabaseAs(DatabaseType dbtype)
		{
			SaveFileDialog dlgSaveFile = new SaveFileDialog();
			dlgSaveFile.Filter = "MS Access database (*.accdb;*.mdb)|*.accdb;*.mdb|All files (*.*)|*.*";
			if (dlgSaveFile.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				if (dbtype == DatabaseType.Documents)
				{
					if (this.IsDocumentsDbLoaded)
					{
						SaveDocumentsDatabase();
						System.IO.File.Copy(_strDocumentsDbPath, dlgSaveFile.FileName, true);
					}
				}
				else if (dbtype == DatabaseType.RegExps)
				{
					if (this.IsRegExpDbLoaded)
					{
						SaveRegExpsDatabase();
						System.IO.File.Copy(_strRegExpDbPath, dlgSaveFile.FileName, true);
					}
				}
			}

			MessageBox.Show("Operation finished", _strAppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		protected void OpenDatabase(DatabaseType dbtype)
		{
			OpenFileDialog dlgOpenFile = new OpenFileDialog();

			dlgOpenFile.Filter = "MS Access database (*.accdb;*.mdb)|*.accdb;*.mdb|All files (*.*)|*.*";
			if (dlgOpenFile.ShowDialog(this) == DialogResult.OK)
			{
				string strFilePath = dlgOpenFile.FileName;

				//bool bNewVersion = true;

				//if (dbtype == DatabaseType.Documents)
				//{
				//    databaseFilePath = strFilePath;

				//    //bNewVersion = LoadRegExpDatabase(strFilePath, true);
				//    //LoadDocumentsDatabase(strFilePath, false);

				//}
				//else if (dbtype == DatabaseType.RegExps)
				//{
				//    databaseFilePath = strFilePath;

				//    //bNewVersion = LoadRegExpDatabase(strFilePath, false);
				//    //LoadDocumentsDatabase(strFilePath, true);
				//}

				OpenDatabaseFile(strFilePath);

				//_views.HandleDatabaseLoad(bNewVersion, this.IsDocumentsDbLoaded, this.IsRegExpDbLoaded);
			}
		}

		protected void SaveHistory()
		{
			try
			{
				string strRecentFiles = "";

				foreach (string strFile in _listRecentFiles)
				{
					strRecentFiles += strFile + Environment.NewLine;
				}

				File.WriteAllText(_strHistoryFilePath, strRecentFiles);
			}
			catch
			{
			}
		}

		protected void LoadHistory()
		{
			_listRecentFiles.Clear();

			try
			{
				if (File.Exists(_strHistoryFilePath))
				{
					_listRecentFiles.AddRange(File.ReadAllLines(_strHistoryFilePath));

					CleanupHistory();

					SaveHistory();
				}
			}
			catch
			{
			}
		}

		protected void CleanupHistory()
		{
			bool bRemove;

			string strFullPath;
			for (int i = 0; i < _listRecentFiles.Count; i++)
			{
				strFullPath = _listRecentFiles[i];
				bRemove = false;

				if (String.IsNullOrEmpty(strFullPath))
					bRemove = true;

				if (!bRemove)
					bRemove = !File.Exists(strFullPath);

				//////////////////////////////////////////////////////////////////////////

				if (bRemove)
				{
					_listRecentFiles.RemoveAt(i);
					i--;
				}
			}
		}

		protected void AddFileToHistory(string strFilePath)
		{
			try
			{
				string strFile;
				for (int i = 0; i < _listRecentFiles.Count; i++)
				{
					strFile = (string)_listRecentFiles[i];

					if (String.Compare(strFile, strFilePath, true) == 0)
					{
						_listRecentFiles.RemoveAt(i);
						break;
					}
				}

				_listRecentFiles.Insert(0, strFilePath);

				if (_listRecentFiles.Count > 10)
					_listRecentFiles.RemoveAt(_listRecentFiles.Count - 1);

				SaveHistory();
				FillHistoryButtons();
			}
			catch
			{
			}
		}

		protected void FillHistoryButtons()
		{
			ribbon.OrbDropDown.RecentItems.Clear();

			RibbonOrbRecentItem orbitemRecent;

			foreach (string strFile in _listRecentFiles)
			{
				orbitemRecent = new RibbonOrbRecentItem(GetCompactPath(strFile, 55));
				orbitemRecent.Tag = strFile;
				orbitemRecent.Click += new EventHandler(OnRecentFile_Click);

				ribbon.OrbDropDown.RecentItems.Add(orbitemRecent);
			}
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			try
			{
				_timer.Stop();

				FillNotebooksMenu();
			}
			catch (Exception ex)
			{
				ShowExceptionMessage(ex);
			}
		}

		protected void FillNotebooksMenu()
		{
			if (!_views.IsViewTypeAvailable("Notebooks", true))
				return;

			var notebooksFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Notebooks");
			if (Directory.Exists(notebooksFolder))
			{
				if (_watcher == null)
				{
					_watcher = new NotebooksWatcher(notebooksFolder);
					_watcher.FolderChanged += (sender, args) => { if (!_timer.Enabled) _timer.Start(); };
				}

				var orbitemNotebooks = (RibbonOrbMenuItem)ribbon.OrbDropDown.MenuItems.FirstOrDefault(x => x.Text == "Notebooks");
				if (orbitemNotebooks == null)
				{
					orbitemNotebooks = new RibbonOrbMenuItem
					                   {
						                   AltKey = null,
						                   DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left,
						                   DropDownArrowSize = new System.Drawing.Size(5, 3),
						                   Image = Properties.Resources.orb_notebooks,
						                   Style = System.Windows.Forms.RibbonButtonStyle.DropDown,
						                   Tag = null,
						                   Text = "Notebooks",
						                   ToolTip = null,
						                   ToolTipImage = null,
						                   ToolTipTitle = null
					                   };

					ribbon.OrbDropDown.MenuItems.Insert(1, orbitemNotebooks);

					///////////////////////////////////////////////////////////////////////////////

					orbitemNotebooks.DropDownItems.Add(new RibbonSeparator());

					var menuItem = new RibbonOrbMenuItem()
					               {
						               Image = Properties.Resources.explore_notebooks,
						               Style = RibbonButtonStyle.Normal,
						               Text = "Explore notebooks folder",
						               Tag = notebooksFolder
					               };
					menuItem.Click += notebookItem_Click;
					orbitemNotebooks.DropDownItems.Add(menuItem);

					///////////////////////////////////////////////////////////////////////////////

					menuItem = new RibbonOrbMenuItem
					           {
						           Image = Properties.Resources.refresh,
						           Style = RibbonButtonStyle.Normal,
						           Text = "Refresh list",
						           KeepOpen = true
					           };

					menuItem.Click += notebookItem_Click;
					orbitemNotebooks.DropDownItems.Add(menuItem);
				}
				else
				{
					orbitemNotebooks.DropDownItems.RemoveRange(0, orbitemNotebooks.DropDownItems.Count - 3);
				}

				///////////////////////////////////////////////////////////////////////////////

				var di = new DirectoryInfo(notebooksFolder);
				var files = di.GetFiles("*.ipynb");
				for (var i = 0; i < files.Length; i++)
				{
					var file = files[i];
					var recentItem = new RibbonOrbMenuItem
					{
						                 Image = Properties.Resources.python,
						                 Style = RibbonButtonStyle.Normal,
						                 Text = Path.GetFileNameWithoutExtension(file.Name),
						                 Tag = file.FullName
					                 };

					recentItem.Click += notebookItem_Click;

					orbitemNotebooks.DropDownItems.Insert(i, recentItem);
				}

				ribbon.OrbDropDown.OnRegionsChanged();
				orbitemNotebooks.UpdateDropdown();
			}
		}

		public static string GetCompactPath(string strPath, int nLength)
		{
			StringBuilder sb = new StringBuilder();

			try
			{
				sb.Append(strPath);

				//PathCompactPathEx(sb, strPath, nLength, 0);
			}
			catch
			{
			}

			return sb.ToString();
		}

		protected bool IsRelationsTableExist()
		{
			bool bResult = false;

			try
			{
				this.adapterRelations.Fill(this.datasetMain._Relations);

				bResult = true;
			}
			catch
			{
			}

			return bResult;
		}

		protected bool IsRelationsTableExistBilling()
		{
			bool bResult = false;

			try
			{
				this.adapterRelationsBilling.Fill(this.datasetBilling._Relations);

				bResult = true;
			}
			catch
			{
			}

			return bResult;
		}

		protected bool CreateRelationsTable(OleDbConnection connection)
		{
			bool bResult = false;

			try
			{
				connection.Open();

				string strCommand = "CREATE TABLE Relations (ID AUTOINCREMENT, RegExpID ntext, ColorCodeID int, Primary Key(ID))";

				OleDbCommand command = new OleDbCommand(strCommand, connection);
				command.ExecuteNonQuery();

				connection.Close();

				this.adapterRelations.Fill(this.datasetMain._Relations);

				bResult = true;
			}
			catch
			{
			}

			return bResult;
		}

		protected bool CreateRelationsTableBilling(OleDbConnection connection)
		{
			bool bResult = false;

			try
			{
				connection.Open();

				string strCommand = "CREATE TABLE Relations (ID AUTOINCREMENT, RegExpID ntext, ColorCodeID int, Primary Key(ID))";

				OleDbCommand command = new OleDbCommand(strCommand, connection);
				command.ExecuteNonQuery();

				connection.Close();

				this.adapterRelationsBilling.Fill(this.datasetBilling._Relations);

				bResult = true;
			}
			catch
			{
			}

			return bResult;
		}

		protected bool IsColorCodesTableExist()
		{
			bool bResult = false;

			try
			{
				this.adapterColorCodes.Fill(this.datasetMain.ColorCodes);

				bResult = true;
			}
			catch
			{
			}

			return bResult;
		}

		protected bool IsColorCodesTableExistBilling()
		{
			bool bResult = false;

			try
			{
				this.adapterColorCodesBilling.Fill(this.datasetBilling.ColorCodes);

				bResult = true;
			}
			catch
			{
			}

			return bResult;
		}

		protected bool CreateColorCodesTable(OleDbConnection connection)
		{
			bool bResult = false;

			try
			{
				connection.Open();

				string strCommand = "CREATE TABLE ColorCodes (ID AUTOINCREMENT, Description ntext, Color int, Primary Key(ID))";

				OleDbCommand command = new OleDbCommand(strCommand, connection);
				command.ExecuteNonQuery();

				connection.Close();

				this.adapterColorCodes.Fill(this.datasetMain.ColorCodes);

				bResult = true;
			}
			catch
			{
			}

			return bResult;
		}

		protected bool CreateColorCodesTableBilling(OleDbConnection connection)
		{
			bool bResult = false;

			try
			{
				connection.Open();

				string strCommand = "CREATE TABLE ColorCodes (ID AUTOINCREMENT, Description ntext, Color int, Primary Key(ID))";

				OleDbCommand command = new OleDbCommand(strCommand, connection);
				command.ExecuteNonQuery();

				connection.Close();

				this.adapterColorCodesBilling.Fill(this.datasetBilling.ColorCodes);

				bResult = true;
			}
			catch
			{
			}

			return bResult;
		}

		protected void UpdateControls(DatabaseType dbtype, bool bEnable)
		{
			if (dbtype == DatabaseType.Documents)
			{
				btnSaveDocuments.Enabled = bEnable;
				btnSaveAsDocuments.Enabled = bEnable;

				orbitemSaveDocuments.Enabled = bEnable;
				orbitemSaveAsDocuments.Enabled = bEnable;
			}
			else if (dbtype == DatabaseType.RegExps)
			{
				btnSaveRegExps.Enabled = bEnable;
				btnSaveAsRegExps.Enabled = bEnable;

				orbitemSaveRegExps.Enabled = bEnable;
				orbitemSaveAsRegExps.Enabled = bEnable;
			}

			btnSaveDatabase.Enabled = bEnable;
			btnSaveAll.Enabled = bEnable;
			orbitemSaveDatabase.Enabled = bEnable;
			orbitemSaveAll.Enabled = bEnable;

			//panelViews.Enabled = bEnable;
			panelViews.Enabled = true;
		}

		public static void ScrollToCaret(RichTextBox txt)
		{
			try
			{
				txt.ScrollToCaret();
			}
			catch
			{
			}
		}

		public MainDataSet GetDataSet()
		{
			return this.datasetMain;
		}

		//Used for storing classTypes based on columnId
		public static Dictionary<int, DynamicColumnType> _classTypeById = new Dictionary<int, DynamicColumnType>();

		public static DynamicColumnType GetDynamicColumnTypeByID(MainDataSet.DynamicColumnsDataTable dynamicColumnsRow, int columnId)
		{
			//Try to find category from memory
			if (_classTypeById.ContainsKey(columnId))
				return _classTypeById[columnId];

			///////////////////////////////////////////////////////////////////////////////


			MainDataSet.DynamicColumnsRow colRegExp = dynamicColumnsRow.FirstOrDefault(p => p.ID == columnId);
			if (colRegExp != null)
			{
				var id = colRegExp.Type;
				if (id == 0)
				{
					_classTypeById.Add(columnId, DynamicColumnType.Category);
					return DynamicColumnType.Category;
				}
				else if (id == 1)
				{
					_classTypeById.Add(columnId, DynamicColumnType.FreeText);
					return DynamicColumnType.FreeText;
				}
				else if (id == 2)
				{
					_classTypeById.Add(columnId, DynamicColumnType.Numeric);
					return DynamicColumnType.Numeric;
				}
				else if (id == 3)
				{
					_classTypeById.Add(columnId, DynamicColumnType.DateTime);
					return DynamicColumnType.DateTime;
				}
			}
			_classTypeById.Add(columnId, DynamicColumnType.None);

			return DynamicColumnType.None;
		}

		protected void ShowSplashScreen()
		{
			_formSplashScreen = new FormSplashScreen();
			_formSplashScreen.Show(this);

			this.Enabled = false;
		}

		protected void HideSplashScreen()
		{
			_formSplashScreen.Hide();

			this.Enabled = true;
		}

		protected DatabaseVersion GetDatabaseVersion(OleDbConnection connection)
		{
			if (connection.State != ConnectionState.Open)
				connection.Open();
			var schema = connection.GetSchema("COLUMNS");
			var col = schema.Select("TABLE_NAME='Documents' AND COLUMN_NAME='DISC_TEXT'");
			if (col.Length > 0)
				return DatabaseVersion.Billing_1;
			else
				return DatabaseVersion.Default;
		}

		protected bool IsGUIDColumnsExist(OleDbConnection connection)
		{
			var schema = connection.GetSchema("COLUMNS");
			var col = schema.Select("(TABLE_NAME='RegExp' OR TABLE_NAME='ColRegExp') AND COLUMN_NAME='GUID'");

			return col.Length == 2;
		}

		protected string GetIndexName(OleDbConnection connection)
		{
			var table = connection.GetSchema("Indexes");
			var indexRow = table.Rows
								.Cast<DataRow>()
								.FirstOrDefault(x => x.Field<string>("TABLE_NAME") == "Documents" && x.Field<string>("COLUMN_NAME") == "ED_ENC_NUM");

			if (indexRow != null)
				return (string)indexRow["INDEX_NAME"];

			return string.Empty;
		}

		protected void DeleteIndex(OleDbConnection connection, string indexName)
		{
			var query = string.Format("DROP INDEX {0} ON Documents", indexName);

			var cmd = new OleDbCommand(query, connection);
			cmd.ExecuteNonQuery();
		}

		protected void CreateIndex(OleDbConnection connection)
		{
			var query = "CREATE INDEX INDEX_DOCUMENTS ON Documents (ED_ENC_NUM)";

			var cmd = new OleDbCommand(query, connection);
			cmd.ExecuteNonQuery();
		}

		protected void CleanupEmptyRegExp()
		{
			datasetMain.RegExp.Where(x => (x.RowState == DataRowState.Added) && (x.IsRegExpNull() || String.IsNullOrEmpty(x.RegExp)))
					   .ToList()
					   .ForEach(x => datasetMain.RegExp.Rows.Remove(x));
		}

		protected void CleanupEmptyColRegExp()
		{
			datasetMain.ColRegExp.Where(x => (x.RowState == DataRowState.Added) && (x.IsRegExpNull() || String.IsNullOrEmpty(x.RegExp)))
					   .ToList()
					   .ForEach(x => datasetMain.ColRegExp.Rows.Remove(x));
		}

		private void FillRegExpGUIDs()
		{
			datasetMain.RegExp.Where(x => (x.RowState != DataRowState.Deleted) && (x.IsGUIDNull() || String.IsNullOrEmpty(x.GUID)))
					   .ToList()
					   .ForEach(x => x.GUID = Guid.NewGuid()
												  .ToString());
		}

		private void FillColRegExpGUIDs()
		{
			datasetMain.ColRegExp.Where(x => (x.RowState != DataRowState.Deleted) && (x.IsGUIDNull() || String.IsNullOrEmpty(x.GUID)))
					   .ToList()
					   .ForEach(x => x.GUID = Guid.NewGuid()
												  .ToString());
		}

		public static void ShowErrorToolTip(string message)
		{
			try
			{
				var screen = Screen.FromControl(ViewsManager.MainForm);
				var pt = new Point(screen.Bounds.Left + 30, screen.Bounds.Bottom - 130);
				pt = ViewsManager.MainForm.PointToClient(pt);

				_globalToolTip.ToolTipIcon = ToolTipIcon.Error;
				_globalToolTip.ToolTipTitle = "Failed to build regular expression";
				_globalToolTip.Show(message, ViewsManager.MainForm, pt, 2000);
			}
			catch
			{
			}
		}

		public static void ShowInfoToolTip(string title, string message)
		{
			try
			{
				var screen = Screen.FromControl(ViewsManager.MainForm);
				var pt = new Point(screen.Bounds.Left + 30, screen.Bounds.Bottom - 130);
				pt = ViewsManager.MainForm.PointToClient(pt);

				_globalToolTip.ToolTipIcon = ToolTipIcon.Info;
				_globalToolTip.ToolTipTitle = title;
				_globalToolTip.Show(message, ViewsManager.MainForm, pt, 2000);
			}
			catch
			{
			}
		}

		public static void ShowExceptionMessage(System.Exception ex)
		{
			var message = ex.Message;
			if (ex.InnerException != null)
				message += Environment.NewLine + Environment.NewLine + ex.InnerException.Message;

			MessageBox.Show(message, MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public static int GetLastInsertedID(OleDbConnection connection)
		{
			var closeConnection = false;

			if (connection.State != ConnectionState.Open)
			{
				connection.Open();

				closeConnection = true;
			}

			///////////////////////////////////////////////////////////////////////////////

			var cmd = new OleDbCommand("SELECT @@IDENTITY", connection);
			var id = (int)cmd.ExecuteScalar();

			///////////////////////////////////////////////////////////////////////////////

			if (closeConnection)
				connection.Close();

			return id;
		}

		#endregion

		#region WinAPI

		[DllImport("shlwapi.dll")]
		public static extern bool PathCompactPathEx([Out] StringBuilder pszOut, string szPath, int cchMax, int dwFlags);

		#endregion
	}

	#region Helper types

	public enum DatabaseType
	{
		Documents,
		RegExps
	}

	public enum DatabaseVersion
	{
		Default,
		Billing_1
	}

	#endregion
}