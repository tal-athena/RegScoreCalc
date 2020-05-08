using System;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;


using System.Text;

using Excel = Microsoft.Office.Interop.Excel;

using FastColoredTextBoxNS;

using RegExpLib.Core;
using RegExpLib.Model;
using RegExpLib.Processing;

using RegScoreCalc.Code;
using RegScoreCalc.Forms;
using System.Data.OleDb;
using LocalDocumentsServiceTypes;
using DocumentsServiceInterfaceLib;
using System.Linq.Expressions;

namespace RegScoreCalc
{
	public partial class PaneRegTabNotes : Pane
	{
		#region Delegates

		public event EventHandler PaneTab_CalcScores;
		public event EventHandler PaneTab_RefreshHighlights;         

        #endregion

        #region Fields

        protected int _selectedColumnID;

        //protected List<PaneColumnNotes> _paneNotesList;

        //protected List<PaneColumnNotes> _extraPaneNotesList;

        //protected PaneRegTabNotesCommandsFast _commands;
        public RibbonButton btnWriteNotes;
        public RibbonPanel panel;

        //protected List<TabSetting> _tabSettings;
        protected List<TabPage> _tabPages;
        
        protected RibbonTab _tab;
        protected SplitterPanel _panel;
        #endregion

        #region Properties

        public int SelectedColumnID
        {
            get { return _selectedColumnID; }
            set
            {
                if (_selectedColumnID != value)
                {
                    _selectedColumnID = value;
                    /*
                    foreach(PaneColumnNotes _paneNote in _paneNotesList)
                    {
                        _paneNote.SelectedColumnID = _selectedColumnID;
                    }
                    */
                    foreach (TabPage tabPage in _tabPages)
                    {
                        if (((TabSetting)tabPage.Tag).TabType == DocumentViewType.Normal)
                        {
                            ((PaneColumnNotes)tabPage.Controls[0]).SelectedColumnID = _selectedColumnID;
                        }
                        if (((TabSetting)tabPage.Tag).TabType >= DocumentViewType.Html_ListView)
                        {
                            ((PaneNoteHtmlView)tabPage.Controls[0]).SelectedColumnID = _selectedColumnID;
                        }
                    }
                }
            }
        }
        #endregion

        #region Ctors

        public PaneRegTabNotes()
		{
            //_paneNotesList = new List<PaneColumnNotes>();
			//_tabSettings = new List<TabSetting>();
            //_extraPaneNotesList = new List<PaneColumnNotes>();
            _tabPages = new List<TabPage>();
            InitializeComponent();			
		}

        #endregion

        #region Events

        private void TabNotesControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int ix = 0; ix < TabNotesControl.TabCount; ++ix)
                {
                    if (((TabSetting)TabNotesControl.TabPages[ix].Tag).Dynamic && TabNotesControl.GetTabRect(ix).Contains(e.Location))
                    {
                        TabNotesControl.SelectedIndex = ix;
                        ContextMenu m = new ContextMenu();

                        MenuItem bntOpenColumnRegEx = new MenuItem("Column specific RegEx");
                        bntOpenColumnRegEx.Click += bntOpenColumnRegEx_Click;
                        bntOpenColumnRegEx.Tag = TabNotesControl.TabPages[ix].Tag;
                        m.MenuItems.Add(bntOpenColumnRegEx);

                        MenuItem deleteValues = new MenuItem("Delete All Values");
                        deleteValues.Click += menuDeleteAllValues_Click;
                        deleteValues.Tag = TabNotesControl.TabPages[ix].Tag;
                        m.MenuItems.Add(deleteValues);

                        m.Show(this, new Point(e.X, e.Y));
                        break;
                    }
                    else if (!((TabSetting)TabNotesControl.TabPages[ix].Tag).ColumnName.StartsWith("NOTE_TEXT") && TabNotesControl.GetTabRect(ix).Contains(e.Location))
                    {
                        TabNotesControl.SelectedIndex = ix;

                        ContextMenu m = new ContextMenu();

                        MenuItem menuCloseItem = new MenuItem("Close Tab");
                        menuCloseItem.Click += onMenuCloseItem;
                        menuCloseItem.Tag = TabNotesControl.TabPages[ix];
                        m.MenuItems.Add(menuCloseItem);

                        m.Show(this, new Point(e.X, e.Y));
                        break;
                    }
                }
            }
        }
		
		private void menuDeleteAllValues_Click(object sender, EventArgs e)
        {
            try
            {
                var menuItem = (MenuItem)sender;
                var dynamicColumnID = _views.DocumentsService.GetDynamicColumnID(((TabSetting)menuItem.Tag).ColumnName);

                _views.MainForm.sourceDocuments.RaiseListChangedEvents = false;

                var classRow = _views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.ID == dynamicColumnID);
                var formProgress = new FormGenericProgress("Deleting values...", new LengthyOperation(DoDeleteColumnValues), classRow, false);
                formProgress.ShowDialog();

                _views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
                _views.MainForm.sourceDocuments.ResetBindings(false);

                UpdatePane();
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);
            }
        }
        private void bntOpenColumnRegEx_Click(object sender, EventArgs e)
        {
            try
            {
                var menuItem = (MenuItem)sender;
                int ID = _views.DocumentsService.GetDynamicColumnID(((TabSetting)menuItem.Tag).ColumnName);

                ActivateColumnsRegExTab(ID.ToString(), true);
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);
            }
        }

        private void onMenuCloseItem(object sender, EventArgs e)
        {
            try
            {
                var menuItem = (MenuItem)sender;
                TabPage browserTab = (TabPage)menuItem.Tag;
                _views.TabSettings.Remove((TabSetting)browserTab.Tag);

                _tabPages.Remove(browserTab);                

                _views.DocumentsService.SetDocumentColumnSettings(_views.TabSettings);

                _views.InvokeRefreshTabSettings();
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);
            }
        }

        private void paneTabs_CalcScores(object sender, EventArgs e)
        {
            if (this.PaneTab_CalcScores != null)
            {
                this.PaneTab_CalcScores(this, EventArgs.Empty);
            }
        }
		private void onTabs_Clicked(object sender, EventArgs e)
        {
            try
            {
                var form = new FormTabs(_views, this, 1);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    //SortColumns(false);
                    RaiseDataModifiedEvent();
                }
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);
            }
        }

        private void onAddBrowserTab_Clicked(object sender, EventArgs e)
        {
            try
            {
                var columnList = _views.MainForm.adapterDocuments.GetActualColumnsList();
                List<string> columnNames = new List<string>();
                foreach (var colInfo in columnList)
                {
                    columnNames.Add(colInfo.Name);
                }

                var form = new FormAddBrowserTab(this, columnNames);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string newTabName = form.GetTabName();
                    string columnId = form.GetColumnName();

                    if (newTabName == "" || columnId == "")
                    {
                        MessageBox.Show("Can't add brower tab\nPlease input correct information");
                        return;
                    }

                    AddBrowserTab(newTabName, columnId);
                }
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);
            }
        }

        private void paneTabs_RefreshHighlights(object sender, EventArgs e)
        {
            if (this.PaneTab_RefreshHighlights != null)
            {
                this.PaneTab_RefreshHighlights(this, EventArgs.Empty);
            }
        }
        
        private void OnDataModified(object sender, EventArgs e)
        {
            UpdatePane();
        }

        private void OnMatchNavigate(RegExpMatchResult result)
        {
            foreach (TabPage page in TabNotesControl.TabPages)
            {
                if (((TabSetting)page.Tag).Index == result.ColumnIndex)
                {
                    if (TabNotesControl.TabPages.IndexOf(page) != -1)
                        TabNotesControl.SelectedIndex = TabNotesControl.TabPages.IndexOf(page);
                }
            }
        }

        private void OnWriteNotes_Clicked(object sender, EventArgs e)
        {
            WriteNotes();
        }       

        private void onAddViewTab_Clicked(object sender, EventArgs e)
        {
            var dlgAddViewTab = new FormAddViewTab(this);
            if (dlgAddViewTab.ShowDialog() == DialogResult.OK)
            {
                var tabName = dlgAddViewTab.GetTabName();
                var viewType = dlgAddViewTab.GetViewType();

                AddViewTab(tabName, viewType);
            }
        }

        #endregion

        #region Operations
        public string GetSelectedText()
        {
            //return _paneNotesList[TabNotesControl.SelectedIndex].GetSelectedText();

            var tabSetting = (TabSetting)TabNotesControl.SelectedTab.Tag;
            if (tabSetting.TabType == DocumentViewType.Normal)
            {
                return ((PaneColumnNotes)TabNotesControl.SelectedTab.Controls[0]).GetSelectedText();
            }
            return "";

        }
                

        protected void InitPaneCommands2(RibbonTab tab)
        {
            panel = new RibbonPanel("Notes");
            tab.Panels.Add(panel);

            //_commands = new PaneRegTabNotesCommandsFast(_views, panel, _paneNotesList, _extraPaneNotesList);
            //_commands._eventDataModified += new EventHandler(OnDataModified);

            btnWriteNotes = new RibbonButton("Write Modified Notes");

            panel.Items.Add(btnWriteNotes);

            btnWriteNotes.Image = Properties.Resources.WriteNotes;
            btnWriteNotes.SmallImage = Properties.Resources.WriteNotes;
            btnWriteNotes.Click += new EventHandler(OnWriteNotes_Clicked);
            btnWriteNotes.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

            /*
            panel = new RibbonPanel("Notes");

            tab.Panels.Add(panel);

            //_commands = new PaneTabNotesCommandsFast(_views, panel, _paneNotesList);
            //_commands._eventDataModified += new EventHandler(OnDataModified);

            btnWriteNotes = new RibbonButton("Export Modified Notes");

            panel.Items.Add(btnWriteNotes);

            btnWriteNotes.Image = Properties.Resources.ExportToExcel;
            btnWriteNotes.SmallImage = Properties.Resources.ExportToExcel;
            btnWriteNotes.Click += new EventHandler(OnExportNotes_Clicked);
            btnWriteNotes.MouseEnter += _views.MainForm.RibbonButton_MouseEnter; 
            */
        }

        #endregion

        #region Overrides

        public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{

            base.InitPane(views, ownerView, panel, tab);

            _tab = tab;
            _panel = panel;

            //_views.DocumentsService.GetDocumentCount();
            //_tabSettings = _views.DocumentsService.GetDocumentColumnSettings();

            foreach (var tabSetting in _views.TabSettings)
            {
                //if (tabSetting.Visible == false) continue;

                TabPage tabPage = AddNewTab(tabSetting);                
                                
                _tabPages.Add(tabPage);
            }

            RefreshTabs();

            _views.OnMatchNavigate += OnMatchNavigate;
            _views.OnNoteColumnsChanged += OnTabSettingsChanged;

            //Ribbon
            InitPaneCommands2(tab);
           
        }

        private void OnTabSettingsChanged()
        {
            //_views.MainForm.adapterDynamicColumns.Update(_views.MainForm.datasetMain.DynamicColumns);
            //_views.MainForm.adapterDynamicColumns.Fill(_views.MainForm.datasetMain.DynamicColumns);
            ResetAllTabs();            
        }

        protected override void InitPaneCommands(RibbonTab tab)
		{
            
        }

        public override void UpdatePane()
		{
            /*
            foreach (PaneColumnNotes paneNotes in _paneNotesList)
            {
                if (paneNotes != null)
                    paneNotes.UpdatePane();
            } 
            */
        }

		public override void DestroyPane()
		{
            foreach (var page in _tabPages)
            {
                if (((TabSetting)page.Tag).TabType == 0)
                {
                    ((PaneColumnNotes)page.Controls[0]).DestroyPane();
                }
                else if (((TabSetting)page.Tag).TabType == DocumentViewType.Browser)
                {
                    ((PaneBrowser)page.Controls[0]).DestroyPane();
                }
                else
                {
                    ((PaneNoteHtmlView)page.Controls[0]).DestroyPane();
                }
                
            }
            /*
            foreach (var page in _tabPages)
            {
                if (((TabSettings)page.Tag).TabType >= 2)
                {
                    ((PaneNoteHtmlView)page.Controls[0]).DestroyPane();
                }
            }

            foreach (PaneColumnNotes paneNotes in _paneNotesList)
            {
                paneNotes.DestroyPane();
            }
            */
            _views.OnNoteColumnsChanged += OnTabSettingsChanged;
            base.DestroyPane();
		}

        #endregion

        #region Implementation

        protected bool DoDeleteColumnValues(BackgroundWorker worker, object objArgument)
        {
            var dynamicColumnRow = (MainDataSet.DynamicColumnsRow)objArgument;

            ///////////////////////////////////////////////////////////////////////////////

            _views.MainForm.adapterDocuments.SqlClearColumnValues(dynamicColumnRow.Title, null, null, ExpressionType.Default);

            ///////////////////////////////////////////////////////////////////////////////

            UpdatePane();
            return true;
        }

        protected void WriteNotes()
        {
            try
            {
                //TTrace(System.Reflection.MethodBase.GetCurrentMethod(), " Start writing notes...");

                string newDatabaseName = GetNewDatabaseName();

                RegScoreCalc.Forms.frmModifiedNotesPopUp frmChooseWhereToSave = new Forms.frmModifiedNotesPopUp(newDatabaseName);
                frmChooseWhereToSave.ShowDialog();

                if (frmChooseWhereToSave.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    OleDbConnection connection;
                    string tableName = "";

                    //get selected option
                    int option = frmChooseWhereToSave.GetSelectedOption();
                    if (option == 3) //write to the new database
                    {
                        tableName = "ModifiedNotes";

                        var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
                        var newName = frmChooseWhereToSave.GetNewName();
                        var destinationDatabase = Path.Combine(dataPath, newName);
                        //Copy template database
                        File.Copy(Path.Combine(dataPath, "Template.mdb"), destinationDatabase, true);

                        //Create new connection
                        string connectionString = _views.MainForm.GetConnectionString(destinationDatabase);

                        //Make a new connection
                        //
                        connection = new OleDbConnection();
                        connection.ConnectionString = connectionString;
                        connection.Open();

                        OleDbCommand command;

                        string strCommand = "CREATE TABLE " + tableName + " (ED_ENC_NUM double, NoteText ntext, Score int, Category ntext, CategoryID int)";
                        command = new OleDbCommand(strCommand, connection);
                        command.ExecuteNonQuery();

                        //Start writing data 

                        WriteModifiedNotesToTable(connection, tableName);

                        connection.Close();
                        connection.Dispose();
                        connection = null;
                        GC.Collect();
                    }
                    else
                    {
                        connection = _views.MainForm.adapterDocuments.Connection;
                        if (connection != null)
                        {
                            if (connection.State != ConnectionState.Open)
                                connection.Open();

                            bool bTableExist = false;

                            OleDbCommand command;

                            if (option == 1) //save in new table
                            {
                                List<string> listOfTableNames = new List<string>();
                                DataTable dt = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                foreach (DataRow row in dt.Rows)
                                {
                                    string strSheetTableName = row["TABLE_NAME"].ToString();
                                    if (row["TABLE_TYPE"].ToString() == "TABLE")
                                    {
                                        if (strSheetTableName.StartsWith("ModifiedNotes"))
                                            listOfTableNames.Add(strSheetTableName);
                                    }
                                }

                                if (listOfTableNames.Count > 0)
                                {
                                    listOfTableNames.Sort();

                                    int newIndex = 1;
                                    var lastIndex = listOfTableNames[listOfTableNames.Count - 1].Substring(13);

                                    if (lastIndex != "")
                                        newIndex = Int32.Parse(lastIndex) + 1;
                                    tableName = "ModifiedNotes" + newIndex.ToString();
                                }
                                else
                                    tableName = "ModifiedNotes";

                                //Create new table

                                string strCommand = "CREATE TABLE " + tableName + " (ED_ENC_NUM double, NoteText ntext, Score int, Category ntext, CategoryID int)";
                                //TTrace(System.Reflection.MethodBase.GetCurrentMethod(), strCommand);
                                command = new OleDbCommand(strCommand, connection);
                                command.ExecuteNonQuery();

                                bTableExist = true;
                            }
                            else //delete data from ModifiedNotes and save there
                            {
                                tableName = "ModifiedNotes";
                                try
                                {
                                    string strCommand = "SELECT * FROM ModifiedNotes";

                                    command = new OleDbCommand(strCommand, connection);
                                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);

                                    DataSet ds = new DataSet();

                                    adapter.Fill(ds, "ModifiedNotes");

                                    bTableExist = true;

                                    strCommand = "DELETE * FROM ModifiedNotes";
                                    command = new OleDbCommand(strCommand, connection);

                                    command.ExecuteNonQuery();
                                }
                                catch
                                {
                                    string strCommand = "CREATE TABLE ModifiedNotes (ED_ENC_NUM double, NoteText ntext, Score int, Category ntext, CategoryID int)";
                                    //TTrace(System.Reflection.MethodBase.GetCurrentMethod(), strCommand);
                                    command = new OleDbCommand(strCommand, connection);
                                    command.ExecuteNonQuery();

                                    bTableExist = true;
                                }
                            }

                            //save new data to table
                            if (bTableExist)
                                WriteModifiedNotesToTable(connection, tableName);

                            connection.Close();

                            MessageBox.Show("Operation finished", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                frmChooseWhereToSave.Dispose();
            }
            catch (System.Exception ex)
            {
                //TTrace(System.Reflection.MethodBase.GetCurrentMethod(), ex);
                MessageBox.Show(ex.Message, MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private string GetNewDatabaseName()
        {
            int newIndex = 0;

            var databaseName = Path.GetFileNameWithoutExtension(_views.MainForm.DocumentsDbPath);
            var startOfFileName = databaseName + "_ModifiedNotes_";

            string[] filePaths = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Data"), "*.mdb");
            List<string> filePathsList = filePaths.ToList<string>()
                                                  .Where(p => p.Contains(startOfFileName))
                                                  .ToList<string>();
            filePathsList.Sort();

            if (filePathsList.Count == 0)
                return databaseName + "_ModifiedNotes_0.mdb";

            var lastItem = filePathsList[filePathsList.Count - 1];

            var currentPosition = Path.GetFileNameWithoutExtension(lastItem)
                                      .Substring(startOfFileName.Length);

            newIndex = Int32.Parse(currentPosition) + 1;

            string newDatabaseName = databaseName + "_ModifiedNotes_" + newIndex + ".mdb";

            //Copy template to the new database

            return newDatabaseName;
        }

        private void WriteModifiedNotesToTable(OleDbConnection connection, string tableName)
        {
            int nCategory;

            MainDataSet.DocumentsRow dataRow;
            foreach (DataRowView row in _views.MainForm.sourceDocuments)
            {
                //nNotesProccessed++;
                dataRow = row.Row as MainDataSet.DocumentsRow;

                if (!dataRow.IsCategoryNull())
                    nCategory = dataRow.Category;
                else
                    nCategory = -1;

                if (dataRow.Score <= 0) // write only notes that have positive score
                    continue;
                bool retVal = true;

                foreach (var setting in _views.TabSettings)
                {
                    if (setting.ColumnName.StartsWith("NOTE_TEXT"))
                    retVal &= WriteNote(connection, dataRow.ED_ENC_NUM, _views.DocumentsService.GetDocumentText(dataRow.ED_ENC_NUM, setting.ColumnName), dataRow.Score, nCategory, tableName, setting.Index);
                }
                /*
                for (int i = 0; i < _paneNotesList.Count(); i ++)
                    retVal &= WriteNote(connection, dataRow.ED_ENC_NUM, _views.DocumentsService.GetDocumentText(dataRow.ED_ENC_NUM, _paneNotesList[i].NoteColumnName), dataRow.Score, nCategory, tableName, i);
                    */
                //if (retVal)
                //nNotesWritten++;
            }
        }

        protected bool WriteNote(OleDbConnection connection, double ED_ENC_NUM, string strOriginalText, int nScore, int nCategoryID, string tableName, int nNoteColumnIndex = 0)
        {
            bool bResult = false;

            try
            {
                if (!String.IsNullOrEmpty(strOriginalText))
                {
                    var strModifiedText = strOriginalText;

                    var procesor = CreateRegExpProcessor();
                    var matches = procesor.GetAllMatches(strModifiedText);

                    for (var i = matches.Count - 1; i >= 0; i--)
                    {
                        var match = matches[i];

                        strModifiedText = strModifiedText.Remove(match.Match.Index, match.Match.Length);
                        strModifiedText = strModifiedText.Insert(match.Match.Index, match.Match.Value);
                    }

                    ///////////////////////////////////////////////////////////////////////////////

                    if (strModifiedText != strOriginalText)
                    {
                        string strCommand;
                        OleDbCommand command;

                        string strCategory = "";

                        if (nCategoryID != -1)
                        {
                            try
                            {
                                strCommand = "SELECT Category FROM Categories WHERE ID = " + nCategoryID.ToString();
                                command = new OleDbCommand(strCommand, connection);

                                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                                DataSet ds = new DataSet();

                                adapter.Fill(ds, "Categories");

                                if (ds.Tables[0].Rows.Count == 1)
                                    strCategory = (string)ds.Tables[0].Rows[0][0];
                            }
                            catch
                            {
                                //TTrace(System.Reflection.MethodBase.GetCurrentMethod(), ex);
                            }
                        }

                        if (nNoteColumnIndex == 0)
                            strCommand = "INSERT INTO  " + tableName
                                     + "  (ED_ENC_NUM, NoteText, Score, Category, CategoryID) "
                                     + "VALUES (?, ?, ?, ?, ?)";
                        else
                            strCommand = "INSERT INTO  " + tableName
                                     + "  (ED_ENC_NUM, NoteText" + nNoteColumnIndex.ToString() + ", Score, Category, CategoryID) "
                                     + "VALUES (?, ?, ?, ?, ?)";

                        command = new OleDbCommand(strCommand, connection);

                        command.Parameters.Add("ED_ENC_NUM", OleDbType.Double)
                               .Value = ED_ENC_NUM;

                        if (nNoteColumnIndex == 0)
                            command.Parameters.Add("NoteText", OleDbType.Char)
                               .Value = strModifiedText;
                        else
                            command.Parameters.Add("NoteText" + nNoteColumnIndex.ToString(), OleDbType.Char)
                               .Value = strModifiedText;

                        command.Parameters.Add("Score", OleDbType.Integer)
                               .Value = nScore;
                        command.Parameters.Add("Category", OleDbType.Char)
                               .Value = strCategory;

                        if (nCategoryID != -1)
                        {
                            command.Parameters.Add("CategoryID", OleDbType.Integer)
                                   .Value = nCategoryID;
                        }
                        else
                        {
                            command.Parameters.Add("CategoryID", OleDbType.Integer)
                                   .Value = DBNull.Value;
                        }

                        if (command.ExecuteNonQuery() == 1)
                            bResult = true;
                    }
                }
            }
            catch
            {
                //TTrace(System.Reflection.MethodBase.GetCurrentMethod(), ex);
            }

            return bResult;
        }

        protected ColRegExpProcessor CreateRegExpProcessor()
        {
            var highlightColumnsList = new List<int>();

            var viewModel = ColumnsViewModel.FromJSON(BrowserManager.GetViewData(_views, "Columns RegEx"));
            var settings = viewModel.ColumnsSettingsList.FirstOrDefault(x => x.ColumnID == _selectedColumnID);

            if (settings != null)
                highlightColumnsList.AddRange(settings.HighlightColumnsList.Where(x => x.IsSelected).Select(x => x.ColumnID));
            else
                highlightColumnsList.Add(_selectedColumnID);

            return new ColRegExpProcessor(_views.MainForm.datasetMain.ColRegExp.Rows.Cast<DataRow>(), highlightColumnsList);
        }


        private void ActivateColumnsRegExTab(string dynamicColumnID, bool openNew)
        {

            ViewColumnsRegEx colView;

            var openedViews = _views.GetAllOpenedViews();
            for (int i = 0; i < openedViews.Count; i++)
            {
                if (openedViews[i].GetType() == typeof(ViewColumnsRegEx))
                {
                    //Open this view and display only this selected column
                    colView = (ViewColumnsRegEx)openedViews[i];

                    //Filter by this column id
                    colView.GetPaneColumnsRegEx()
                           .FilterByColumnID(dynamicColumnID, false);

                    if (openNew)
                        _views.ActivateView(i);

                    return;
                }
            }

            if (!openNew)
                return;

            //Open new view
            if (_views.CreateView("Columns RegEx", true, false))
            {
                //Get new view
                openedViews = _views.GetAllOpenedViews();
                colView = (ViewColumnsRegEx)openedViews[openedViews.Count - 1];

                //Filter by this column id
                colView.GetPaneColumnsRegEx()
                       .FilterByColumnID(dynamicColumnID, false);
            }
        }

        public void SetTabSettings(List<TabSetting> tabSettings)
        {            
            if (_views.TabSettings.Count < tabSettings.Count)
            {
                foreach (var setting in tabSettings)
                {
                    if (setting.TabType == DocumentViewType.Normal && _views.TabSettings.Find(x => x.ColumnName == setting.ColumnName) == null)
                    {
                        AddNewColumn(setting);
                        _tabPages.Add(AddNewTab(setting));
                    }
                }
            }

            _views.TabSettings = tabSettings;
            _views.DocumentsService.SetDocumentColumnSettings(tabSettings);

            ////////////////////////      Score column add         /////////////////////////
            _views.BeforeDocumentsTableLoad(true);

            _views.MainForm.adapterDocuments.StartBatchQuery();
            foreach (var setting in tabSettings)
            {
                try
                {
                    if (!setting.ColumnName.StartsWith("NOTE_TEXT"))
                        continue;

                    var columnName = "Score";
                    if (setting.Index > 0)
                        columnName += setting.Index;

                    if (setting.Score && !_views.MainForm.adapterDocuments.SqlIsColumnExist(columnName))
                    {
                        _views.MainForm.adapterDocuments.AddExtraColumn(columnName, "INTEGER", true);
                        _views.MainForm.adapterReviewMLDocumentsNew.AddExtraColumn(columnName, "INTEGER", false);
                    }
                    else if (!setting.Score && _views.MainForm.adapterDocuments.SqlIsColumnExist(columnName))
                    {
                        _views.MainForm.adapterDocuments.DeleteColumn(columnName, true);
                        _views.MainForm.adapterReviewMLDocumentsNew.AddExtraColumn(columnName, "INTEGER", false);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("SetColumnSettings (Score ): " + ex.ToString());
                }
            }
            _views.MainForm.adapterDocuments.EndBatchQuery();

            _views.AfterDocumentsTableLoad(true);

            RefreshTabs();
        }
        public TabPage AddNewTab(TabSetting tabSetting)
        {
            TabPage tabPage = new TabPage();
            tabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            tabPage.Location = new System.Drawing.Point(4, 25);
            tabPage.Margin = new System.Windows.Forms.Padding(0);
            tabPage.Name = tabSetting.ColumnName;
            tabPage.Size = new System.Drawing.Size(940, 820);
            //tabPage.TabIndex = tabSetting.Index;

            tabPage.Text = tabSetting.DisplayName;

            tabPage.Tag = tabSetting;

            if (tabSetting.TabType == DocumentViewType.Normal)
            {
                PaneColumnNotes paneNotes = new PaneColumnNotes(tabSetting.ColumnName);

                paneNotes.InitPane(_views, _ownerView, _panel, _tab);
                paneNotes._eventDataModified += new EventHandler(OnDataModified);
                paneNotes.CalcScores += new EventHandler(paneTabs_CalcScores);
                paneNotes.RefreshHighlights += new EventHandler(paneTabs_RefreshHighlights);

                paneNotes.SortTabs += new EventHandler(onTabs_Clicked);
                paneNotes.AddBrowserTab += new EventHandler(onAddBrowserTab_Clicked);
                paneNotes.AddViewTab += new EventHandler(onAddViewTab_Clicked);

                //_splitter.Panel2.Controls.Add(_paneNotes);                
                paneNotes.ShowPane();

                //_paneNotesList.Add(paneNotes);

                tabPage.Controls.Add(paneNotes);

            }
            else if (tabSetting.TabType == DocumentViewType.Browser)
            {

                PaneBrowser paneBrowser = new PaneBrowser(tabSetting.ColumnName);
                paneBrowser.InitPane(_views, _ownerView, _panel, _tab);

                paneBrowser.SortTabs += new EventHandler(this.onTabs_Clicked);
                paneBrowser.AddBrowserTab += new EventHandler(this.onAddBrowserTab_Clicked);
                paneBrowser.AddViewTab += new EventHandler(this.onAddViewTab_Clicked);

                paneBrowser.ShowPane();

                tabPage.Controls.Add(paneBrowser);
            }
            else
            {
                PaneNoteHtmlView paneHtmlView = new PaneNoteHtmlView(tabSetting.ColumnName, tabSetting.TabType);

                paneHtmlView.InitPane(_views, _ownerView, _panel, _tab);
                paneHtmlView._eventDataModified += new EventHandler(OnDataModified);


                paneHtmlView.SortTabs += new EventHandler(onTabs_Clicked);
                paneHtmlView.AddBrowserTab += new EventHandler(onAddBrowserTab_Clicked);
                paneHtmlView.AddViewTab += new EventHandler(onAddViewTab_Clicked);

                //_splitter.Panel2.Controls.Add(_paneNotes);                
                paneHtmlView.ShowPane();

                //_extraPaneNotesList.Add(paneHtmlView.GetDebugPaneColumnNotes());

                tabPage.Controls.Add(paneHtmlView);
            }
            return tabPage;
        }
        protected void AddNewColumn(TabSetting setting)
        {
            var newColumn = _views.MainForm.adapterDocuments.AddDynamicColumn(setting.ColumnName, DynamicColumnType.FreeText, true);
            var newColumnReviewML = _views.MainForm.adapterReviewMLDocumentsNew.AddDynamicColumn(setting.ColumnName, DynamicColumnType.FreeText, false);

            
            var order = _views.MainForm.datasetMain.DynamicColumns.Count(x => x.RowState != DataRowState.Deleted) + 1;

            _views.MainForm.adapterDynamicColumns.Insert(setting.ColumnName, (int)DynamicColumnType.FreeText, order, String.Empty);
            _views.MainForm.adapterDynamicColumns.Fill(_views.MainForm.datasetMain.DynamicColumns);
            
            
        }

        public void ResetAllTabs()
        {
            foreach (var page in _tabPages)
            {
                ((Pane)page.Controls[0]).DestroyPane();
            }
            _tabPages.Clear();            

            //_views.TabSettings = _views.DocumentsService.GetDocumentColumnSettings();

            foreach (var tabSetting in _views.TabSettings)
            {
                //if (tabSetting.Visible == false) continue;

                TabPage tabPage = AddNewTab(tabSetting);

                _tabPages.Add(tabPage);
            }

            RefreshTabs();
        }

        public void RefreshTabs()
        {
            TabNotesControl.TabPages.Clear();
            //_paneNotesList.Clear();
            
            foreach (var tabSetting in _views.TabSettings)
            {
                if (tabSetting.Visible)
                {
                    TabPage page = _tabPages.Find(x => ((TabSetting)x.Tag).ColumnName == tabSetting.ColumnName);
                    if (page != null)
                    {
                        page.Tag = tabSetting;
                        page.Text = tabSetting.DisplayName;
                        TabNotesControl.TabPages.Add(page);
                    }                    
                }
                    
            }            
        }

        protected void AddBrowserTab(string tabName, string columnName)
        {   
            TabSetting tabSetting = new TabSetting();
            tabSetting.DisplayName = tabName;
            tabSetting.ColumnName = columnName;
            tabSetting.Dynamic = false;
            tabSetting.Order = _views.TabSettings.Count + 1;
            tabSetting.Index = -1;
            tabSetting.Visible = true;
            tabSetting.TabType = DocumentViewType.Browser;

            _views.TabSettings.Add(tabSetting);

            _views.DocumentsService.SetDocumentColumnSettings(_views.TabSettings);

            _tabPages.Add(AddNewTab(tabSetting));

            RefreshTabs();
        }


        protected void AddViewTab(string tabName, DocumentViewType viewType)
        {
            int lastIndex = 0;
            foreach (var setting in _views.TabSettings)
                if (lastIndex < setting.Index) lastIndex = setting.Index;
            lastIndex++;

            TabSetting tabSetting = new TabSetting();
            tabSetting.DisplayName = tabName;
            tabSetting.ColumnName = "NOTE_TEXT" + lastIndex.ToString();
            tabSetting.Dynamic = true;
            tabSetting.Order = _views.TabSettings.Count + 1;
            tabSetting.Index = lastIndex;
            tabSetting.Visible = true;
            tabSetting.TabType = viewType;

            _views.TabSettings.Add(tabSetting);

            AddNewColumn(tabSetting);
            _views.DocumentsService.SetDocumentColumnSettings(_views.TabSettings);
            
            _tabPages.Add(AddNewTab(tabSetting));

            RefreshTabs();
        }
        #endregion

        #region Implementation: context menu

        #endregion

        private void TabNotesControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }        
    }

}
