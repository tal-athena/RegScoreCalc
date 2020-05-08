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
using LocalDocumentsServiceTypes;
using DocumentsServiceInterfaceLib;
using System.Linq.Expressions;
using CefSharp.WinForms;
using System.Reflection;
using Newtonsoft.Json;

namespace RegScoreCalc
{
	public partial class PaneTabNotes : Pane
	{
		#region Delegates

		public event EventHandler PaneTab_CalcScores;
		public event EventHandler PaneTab_RefreshHighlights;

        #endregion

        #region Fields
                
        //protected List<PaneNotes> _paneNotesList;        

        //protected PaneTabNotesCommandsFast _commands;
        public RibbonButton btnWriteNotes;
        public RibbonPanel panel;

        //protected List<TabSetting> _tabSettings;

        protected RibbonTab _tab;
        protected SplitterPanel _panel;

        protected List<TabPage> _tabPages;

        protected List<PaneNotes> _paneViewNotesList;

        protected string _reviewNoteColumn;

        #endregion

        #region Properties

        public string ReviewNoteTextColumn
        {
            get { return _reviewNoteColumn; }
            set
            {
                _reviewNoteColumn = value;

                foreach(TabPage page in TabNotesControl.TabPages)
                {
                    if (((TabSetting)page.Tag).ColumnName == _reviewNoteColumn)
                    {
                        TabNotesControl.SelectedIndex = TabNotesControl.TabPages.IndexOf(page);
                        break;
                    }
                }
            }
        }

        #endregion

        #region Ctors

        public PaneTabNotes()
		{
            //_paneNotesList = new List<PaneNotes>();
            //_tabSettings = new List<TabSetting>();
            _tabPages = new List<TabPage>();

            _paneViewNotesList = new List<PaneNotes>();

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
                RefreshTabs();
                
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
                var form = new FormTabs(_views, this);
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
                foreach(var colInfo in columnList)
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

        private void OnExportNotes_Clicked(object sender, EventArgs e)
        {
            ExportNotes();
        }

        protected void ExportNotes()
        {
            try
            {
                var newCsvName = GetNewXMLName();

                var formExportNotes = new ExportModifiedNotesPopUp(_views, newCsvName);
                if (formExportNotes.ShowDialog() == DialogResult.OK)
                {
                    var options = formExportNotes.Options;

                    EnumerableRowCollection<MainDataSet.DocumentsRow> documents = null;

                    switch (options.ExportCriteria)
                    {
                        case ExportCriteria.AllDocuments:
                            //Get all documents
                            documents = _views.MainForm.datasetMain.Documents.AsEnumerable();
                            break;
                        case ExportCriteria.Positive:
                            //Get only documents that have positive score
                            documents = _views.MainForm.datasetMain.Documents.Where(p => p.Score > 0);
                            break;
                        case ExportCriteria.PositiveNegative:
                            //Get only documents that have positive or negative score
                            documents = _views.MainForm.datasetMain.Documents.Where(p => p.Score != 0);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (options.ExportWithCategory)
                    {
                        //Get only documents that have category
                        documents = documents.Where(p => !p.IsCategoryNull() && options.Categories.Contains(p.Category));
                    }

                    ///////////////////////////////////////////////////////////////////////////////

                    var outputFilePath = Path.Combine(options.OutputFolder, options.OutputFileName);

                    if (options.Mode == ExportMode.Xml)
                    {
                        ProcessExtractingXMLFile(outputFilePath, documents, options.AddPrefixSuffix);

                        MessageBox.Show("Exporting documents finished!", "Export finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (options.Mode == ExportMode.Excel)
                    {
                        if (ExportToExcel(outputFilePath, documents, options))
                        {
                            var dlgres = MessageBox.Show("Exporting documents finished!" + Environment.NewLine + Environment.NewLine + "Do you wish to open file in Excel?", "Export finished", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dlgres == DialogResult.Yes)
                                Process.Start(outputFilePath);
                        }
                    } else
                    {
                        ProcessExtractingJsonFile(outputFilePath, documents, options);

                        MessageBox.Show("Exporting documents finished!", "Export finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        protected bool ExportToExcel(string outputFilePath, EnumerableRowCollection<MainDataSet.DocumentsRow> documents, ExportNotesOptions options)
        {
            var args = new ExcelExportArgs
            {
                OutputFilePath = outputFilePath,
                Options = options,
                Documents = documents,
            };

            var formProgress = new FormGenericProgress("Exporting to Excel...", DoExportToExcel, args, true);
            formProgress.ShowDialog();

            return formProgress.Result;
        }

        private bool DoExportToExcel(BackgroundWorker worker, object args)
        {
            Excel.Application excelApp = null;

            try
            {
                var exportArgs = (ExcelExportArgs)args;

                excelApp = new Excel.Application();
                excelApp.Visible = false;

                var book = excelApp.Workbooks.Add();
                var sheet = (Excel.Worksheet)book.Worksheets[1];
                var cells = sheet.Cells;

                ///////////////////////////////////////////////////////////////////////////////

                var rowIndex = 1;

                var columns = _views.MainForm.datasetMain.Documents.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

                for (var i = 0; i < _views.TabSettings.Count; i ++)
                {
                    if (_views.TabSettings[i].ColumnName.Contains("NOTE_TEXT"))
                        columns.Add(_views.TabSettings[i].ColumnName);
                }
                /*
                columns.Add("NOTE_TEXT");
                for (var i = 1; i < _paneNotesList.Count(); i++)
                {
                    columns.Add("NOTE_TEXT" + i.ToString());
                }
                */

                var columnIndex = 1;
                foreach (var columnName in columns)
                {
                    var range = (Excel.Range)cells[rowIndex, columnIndex];
                    range.Value2 = columnName;
                    range.Font.Bold = true;

                    if (columnName.StartsWith("NOTE_TEXT"))
                    {
                        range.EntireColumn.ColumnWidth = 100;
                        range.EntireColumn.Interior.Color = ColorTranslator.ToOle(Color.LightGray);
                    }

                    columnIndex++;
                }

                ///////////////////////////////////////////////////////////////////////////////

                var totalDocuments = (double)exportArgs.Documents.Count();

                var categories = _views.MainForm.datasetMain.Categories.Cast<MainDataSet.CategoriesRow>()
                                       .Select(x => new
                                       {
                                           Key = x.ID,
                                           Value = x.Category
                                       })
                                       .ToDictionary(x => x.Key, x => x.Value);

                var processor = new RegExpProcessor(_views.MainForm.datasetMain.RegExp);
                List<RegExpMatchResult> highlightMatches;

                ///////////////////////////////////////////////////////////////////////////////

                rowIndex++;
                foreach (var doc in exportArgs.Documents)
                {
                    columnIndex = 1;

                    foreach (var columnName in columns)
                    {
                        var value = String.Empty;
                        var isNoteTextColumn = false;
                        highlightMatches = null;

                        if (columnName.StartsWith("NOTE_TEXT"))
                        {
                            isNoteTextColumn = true;

                            var documentID = (double)doc["ED_ENC_NUM"];                           

                            value = _views.DocumentsService.GetDocumentText(documentID, columnName);

                            if (exportArgs.Options.AddPrefixSuffix || exportArgs.Options.ColorMatches)
                            {
                                var matches = processor.GetAllMatches(value);

                                if (exportArgs.Options.AddPrefixSuffix)
                                    value = GetModifiedNoteTextForExcel(value, matches, exportArgs.Options.ColorMatches);

                                if (exportArgs.Options.ColorMatches)
                                    highlightMatches = matches;
                            }
                        }
                        else if (!doc.IsNull(columnName))
                        {
                            if (columnName == "Category")
                            {
                                var categoryID = doc[columnIndex - 1];
                                if (categoryID != DBNull.Value && categoryID is int)
                                {
                                    string categoryName;
                                    if (categories.TryGetValue((int)categoryID, out categoryName))
                                        value = categoryID + " - " + categoryName;
                                }
                            }
                            else
                                value = Convert.ToString(doc[columnIndex - 1]);
                        }

                        if (!String.IsNullOrEmpty(value))
                        {
                            var range = (Excel.Range)cells[rowIndex, columnIndex];
                            range.Value2 = value;

                            if (isNoteTextColumn)
                            {
                                range.WrapText = true;

                                range.EntireRow.RowHeight = exportArgs.Options.ExcelNumberOfLines * 15;

                                if (highlightMatches != null && highlightMatches.Any())
                                {
                                    foreach (var match in highlightMatches)
                                    {
                                        var font = range.Characters[match.Start + 1, match.Length].Font;
                                        font.Color = ColorTranslator.ToOle(match.RegExp.Color);
                                        font.Bold = true;
                                        //font.Underline = true;

                                        if (worker.CancellationPending)
                                            break;
                                    }

                                    if (worker.CancellationPending)
                                        break;
                                }
                            }
                        }

                        columnIndex++;
                    }

                    var row = (Excel.Range)cells[rowIndex, 1];
                    row.EntireRow.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                    if (worker.CancellationPending)
                    {
                        break;
                    }

                    var progress = (int)(((rowIndex - 1) / totalDocuments) * 100d);
                    worker.ReportProgress(progress);

                    rowIndex++;
                }

                if (!worker.CancellationPending)
                {
                    var region = cells.CurrentRegion;
                    var start = region[1, 1];
                    var end = region[rowIndex - 1, region.Columns.Count - 1];
                    sheet.Range[start, end].Columns.AutoFit();
                }

                book.SaveAs(exportArgs.OutputFilePath);
            }
            catch (Exception ex)
            {
                if (excelApp == null)
                    MessageBox.Show("Failed to start Microsoft Excel. Please make sure it is installed on your system", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MainForm.ShowExceptionMessage(ex);

                return false;
            }
            finally
            {
                if (excelApp != null)
                    excelApp.Quit();
            }

            return true;
        }
        protected string GetModifiedNoteTextForExcel(string documentText, List<RegExpMatchResult> matches, bool updateMatches)
        {
            var startOffset = 0;

            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                var value = match.Match.Value;

                var replacementValue = match.RegExp.PrefixMatch + value + match.RegExp.SuffixMatch;

                var start = match.Start + startOffset;

                documentText = documentText.Remove(start, match.Length);
                documentText = documentText.Insert(start, replacementValue);

                startOffset += match.RegExp.PrefixMatch.Length + match.RegExp.SuffixMatch.Length;

                if (updateMatches)
                    matches[i] = new RegExpMatchResult(match.RegExp, match.Position, start + match.RegExp.PrefixMatch.Length, match.Length);
            }

            return documentText;
        }

        private void ProcessExtractingXMLFile(string xmlName, EnumerableRowCollection<MainDataSet.DocumentsRow> documents, bool addPrefixAndSuffix)
        {
            List<KeyValuePair<string, string>> more = new List<KeyValuePair<string, string>>();

            //Remove not needed columns
            List<string> selectColumns = new List<string>();
            foreach (DataColumn column in documents.AsDataView()
                                                   .ToTable()
                                                   .Columns)
            {
                if (column.ColumnName.StartsWith("Class") || (column.ColumnName == "ED_ENC_NUM"
                                                              || column.ColumnName.StartsWith("NOTE_TEXT") || column.ColumnName == "Category"
                                                              || column.ColumnName.StartsWith("Score")))
                    selectColumns.Add(column.ColumnName);
            }

            var processedDocumentsDataTable = documents.AsDataView()
                                                       .ToTable(false, selectColumns.ToArray());

            var columns = processedDocumentsDataTable.Columns.Cast<DataColumn>().ToList();

            for (var i = 0; i < _views.TabSettings.Count; i ++)
            {
                if (_views.TabSettings[i].ColumnName.Contains("NOTE_TEXT")) 
                    columns.Add(new DataColumn(_views.TabSettings[i].ColumnName) { Caption = _views.TabSettings[i].ColumnName });
            }
            /*
            columns.Add(new DataColumn("NOTE_TEXT") { Caption = "NOTE_TEXT" });
            for (var i = 1; i < _paneNotesList.Count(); i++)
            {
                columns.Add(new DataColumn("NOTE_TEXT" + i.ToString()) { Caption = "NOTE_TEXT" + i.ToString() });
            }
            */

            //Write XML
            using (var sw = new StreamWriter(xmlName, false, new UTF8Encoding()))
            {
                //Open XML file
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>");
                sw.WriteLine("<DocumentElement>");

                //Write schema
                WriteXMLSchema(sw);

                //Write data
                foreach (DataRow item in processedDocumentsDataTable.Rows)
                {
                    WriteDocumentToXML(sw, item, columns, more, addPrefixAndSuffix);
                }

                //Close XML file
                sw.WriteLine("</DocumentElement>");

                sw.Flush();
            }
        }
        private void ProcessExtractingJsonFile(string jsonFile, EnumerableRowCollection<MainDataSet.DocumentsRow> documents, ExportNotesOptions options)
        {
            var fs = File.Create(jsonFile);
            StreamWriter sw = new StreamWriter(fs, new UTF8Encoding());

            sw.WriteLine("{");
            sw.Write(@"""Fields"": ");
            
            var selectedColumns = _views.MainForm.adapterDocuments.GetAllColumnsList().Where(x => options.Columns.Exists(y => x.Name == y));

            List<object> Fields = new List<object>();

            foreach (var sc in selectedColumns)
            {
                Fields.Add(new
                {
                    Name = sc.Name,
                    Type = GetUseableType(sc.Name, sc.Type)
                });
            }
            sw.WriteLine(JsonConvert.SerializeObject(Fields));
            sw.WriteLine(",");

            sw.WriteLine(@"""Documents"":[");

            bool first = true;

            foreach (var row in documents)
            {
                Dictionary<string, object> rowData = new Dictionary<string, object>();
                foreach (var columnName in options.Columns)
                {
                    object value;
                    if (columnName.StartsWith("NOTE_"))
                    {  
                        var documentID = (double)row["ED_ENC_NUM"];

                        value = _views.DocumentsService.GetDocumentText(documentID, columnName);                        
                    }
                    else if (columnName == "Category")
                    {
                        //Get category by ID
                        if (String.IsNullOrEmpty(row[columnName].ToString()))
                        {
                            value = "";
                        }
                        else
                        {
                            var category = _views.MainForm.datasetMain.Categories.First(x => x.ID == (int)row[columnName]);
                            value = category.Category;                            
                        }
                    }                    
                    else
                    {
                        value = row[columnName];
                    }

                    rowData.Add(columnName, value);
                }
                if (first)
                {
                    sw.WriteLine(JsonConvert.SerializeObject(rowData));
                    first = false;
                } else
                {
                    sw.WriteLine("," + JsonConvert.SerializeObject(rowData));
                }                
            }
            sw.WriteLine("]}");
            sw.Close();
            fs.Close();
        }

        private string GetUseableType(string name, Type type)
        {
            if (name.ToLower() == "category")
                return "selection";

            if (type == typeof(string)) return "ntext";
            else if (type == typeof(double)) return "double";
            else if (type == typeof(double)) return "integer";
            else if (type == typeof(DateTime)) return "datetime";
            
            return type.Name; 
            
        }

        private void WriteXMLSchema(StreamWriter sw)
        {
            StringBuilder sb = new StringBuilder();

            //Add dynamic columns
            foreach (var categoryClassRow in _views.MainForm.datasetMain.DynamicColumns)
            {
                string type = "";
                var classType = (DynamicColumnType)categoryClassRow.Type;
                if (classType == DynamicColumnType.Numeric)
                    type = "xs:double";
                else if (classType == DynamicColumnType.DateTime)
                    type = "xs:dateTime";
                else
                    type = "xs:string";

                var name = RemoveInvalidXmlChars(categoryClassRow.Title.Replace(" ", "_"));
                //var name = "Class" + categoryClassRow.CategoryClassID;
                var schema = "<xs:element name=\"" + name + "\" type=\"" + type + "\" minOccurs=\"0\" />";
                sb.AppendLine(schema);
            }

            //Add schema definition for other columns

            //Replace <!-- ADD HERE DYNAMIC --> with generated dynamic columns schema
            var schemaTemplate = Properties.Settings.Default.XMLSchemaTemplate;
            schemaTemplate = schemaTemplate.Replace("<!-- ADD HERE DYNAMIC -->", sb.ToString());

            //Write to XML document
            sw.WriteLine(schemaTemplate);
        }

        public static string CleanInvalidXmlChars(string text)
        {
            // From xml spec valid chars: 
            // #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]     
            // any Unicode character, excluding the surrogate blocks, FFFE, and FFFF. 
            string re = @"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-\u10FFFF]";
            return Regex.Replace(text, re, "");
        }

        private void WriteDocumentToXML(StreamWriter fileStream, DataRow row, List<DataColumn> columns, List<KeyValuePair<string, string>> more, bool addPrefixAndSuffix)
        {
            fileStream.WriteLine("<Documents>");

            foreach (DataColumn dc in columns)
            {
                if (dc.ColumnName.StartsWith("NOTE_TEXT"))
                {
                    fileStream.Write("\t<" + dc.ColumnName + ">");

                    var documentID = (double)row["ED_ENC_NUM"];

                    var noteText = _views.DocumentsService.GetDocumentText(documentID, dc.ColumnName);

                    if (addPrefixAndSuffix)
                        noteText = GetModifiedNoteText(noteText);

                    fileStream.Write("<![CDATA[" + CleanInvalidXmlChars(noteText) + "]]>");

                    fileStream.WriteLine("</" + dc.ColumnName + ">");
                }
                else if (dc.ColumnName == "Category")
                {
                    //Get category by ID
                    if (String.IsNullOrEmpty(row[dc].ToString()))
                    {
                        fileStream.Write("\t<" + dc.ColumnName + ">");
                        //Do not write anything
                        fileStream.WriteLine("</" + dc.ColumnName + ">");
                    }
                    else
                    {
                        var category = _views.MainForm.datasetMain.Categories.First(x => x.ID == (int)row[dc]);

                        fileStream.Write("\t<" + dc.ColumnName + ">");
                        fileStream.Write(category.Category);
                        fileStream.WriteLine("</" + dc.ColumnName + ">");
                    }
                }
                else if (dc.ColumnName.StartsWith("Class"))
                {
                    //White space is not allowed, () are not allowed...
                    var caption = dc.Caption;
                    if (String.IsNullOrEmpty(row[dc].ToString()))
                    {
                        fileStream.Write("\t<" + caption + ">");
                        fileStream.WriteLine("</" + caption + ">");
                    }
                    else
                    {
                        //System.Xml.Linq.XElement element = new System.Xml.Linq.XElement(caption, row[dc]);
                        //fileStream.WriteLine("\t" + element);

                        System.Xml.Linq.XElement element = new System.Xml.Linq.XElement("test", row[dc]);

                        fileStream.Write("\t<" + caption + ">");
                        fileStream.Write(XmlEscape(element.Value));
                        fileStream.WriteLine("</" + caption + ">");
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(row[dc].ToString()))
                    {
                        fileStream.Write("\t<" + dc.ColumnName + ">");
                        fileStream.WriteLine("</" + dc.ColumnName + ">");
                    }
                    else
                    {
                        System.Xml.Linq.XElement element = new System.Xml.Linq.XElement(dc.ColumnName, row[dc]);
                        fileStream.WriteLine("\t" + element);
                    }
                }
            }

            fileStream.WriteLine("</Documents>");
        }

        public string XmlEscape(string unescaped)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlNode node = doc.CreateElement("root");
            node.InnerText = unescaped;
            return node.InnerXml;
        }

        public string GetModifiedNoteText(string strOriginalText)
        {
            if (String.IsNullOrEmpty(strOriginalText))
                return strOriginalText;

            ///////////////////////////////////////////////////////////////////////////////

            var strModifiedText = strOriginalText;

            var processor = new RegExpProcessor(_views.MainForm.datasetMain.RegExp.Rows.Cast<DataRow>());

            var matches = processor.GetAllMatches(strModifiedText);
            for (var i = matches.Count - 1; i >= 0; i--)
            {
                var match = matches[i];

                var value = match.RegExp.PrefixMatch + match.Match.Value + match.RegExp.SuffixMatch;

                strModifiedText = strModifiedText.Remove(match.Match.Index, match.Match.Length);
                strModifiedText = strModifiedText.Insert(match.Match.Index, value);
            }

            return strModifiedText;
        }

        private string GetNewXMLName()
        {
            int newIndex = 0;

            var databaseName = Path.GetFileNameWithoutExtension(_views.MainForm.DocumentsDbPath);
            var startOfFileName = databaseName + "_ModifiedNotes_";

            string[] filePaths = Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Data"), "*.xml");
            List<string> filePathsList = filePaths.ToList<string>()
                                                  .Where(p => p.Contains(startOfFileName))
                                                  .ToList<string>();
            filePathsList.Sort();

            if (filePathsList.Count == 0)
                return databaseName + "_ModifiedNotes_0.xml";

            var lastItem = filePathsList[filePathsList.Count - 1];

            var currentPosition = Path.GetFileNameWithoutExtension(lastItem)
                                      .Substring(startOfFileName.Length);

            newIndex = Int32.Parse(currentPosition) + 1;

            string newDatabaseName = databaseName + "_ModifiedNotes_" + newIndex + ".xml";

            //Copy template to the new database

            return newDatabaseName;
        }

        static string RemoveInvalidXmlChars(string text)
        {
            var validXmlChars = text.Where(ch => System.Xml.XmlConvert.IsStartNCNameChar(ch))
                                    .ToArray();
            return new string(validXmlChars);
        }
        #endregion

        #region Operations
        public string GetSelectedText()
        {
            //return _paneNotesList[((TabSettings)TabNotesControl.SelectedTab.Tag).Index].GetSelectedText();
            TabSetting setting = (TabSetting)TabNotesControl.SelectedTab.Tag;
            if (setting.TabType == DocumentViewType.Normal)
                ((PaneNotes)TabNotesControl.SelectedTab.Controls[0]).GetSelectedText();

            return "";
        }
                

        protected void InitPaneCommands2(RibbonTab tab)
        {
            
            panel = new RibbonPanel("Notes");

            tab.Panels.Add(panel);

            //_commands = new PaneTabNotesCommandsFast(_views, panel, _paneNotesList, _paneViewNotesList);
            //_commands._eventDataModified += new EventHandler(OnDataModified);

            btnWriteNotes = new RibbonButton("Export Modified Notes");

            panel.Items.Add(btnWriteNotes);

            btnWriteNotes.Image = Properties.Resources.ExportToExcel;
            btnWriteNotes.SmallImage = Properties.Resources.ExportToExcel;
            btnWriteNotes.Click += new EventHandler(OnExportNotes_Clicked);
            btnWriteNotes.MouseEnter += _views.MainForm.RibbonButton_MouseEnter; 
        }

        #endregion

        #region Overrides

        public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			base.InitPane(views, ownerView, panel, tab);

            _tab = tab;
            _panel = panel;

            //_tabSettings = _views.DocumentsService.GetDocumentColumnSettings();

            _views.OnMatchNavigate += OnMatchNavigate;
            _views.OnNoteColumnsChanged += OnTabSettingsChanged;

            foreach (var tabSetting in _views.TabSettings)
            {
                //if (tabSetting.Visible == false) continue;

                TabPage tabPage = AddNewTab(tabSetting);

                _tabPages.Add(tabPage);
            }

            RefreshTabs();

            

            //Ribbon
            InitPaneCommands2(tab);
        }

        private void OnTabSettingsChanged()
        {
            ResetAllTabs();
        }

        protected override void InitPaneCommands(RibbonTab tab)
		{
            
        }

        public override void UpdatePane()
		{
            
            /*
            foreach (PaneNotes paneNotes in _paneNotesList)
            {
                if (paneNotes != null)
                    paneNotes.UpdatePane();
            } 
            */
        }

		public override void DestroyPane()
		{   
            /*foreach (PaneNotes paneNotes in _paneNotesList)
            {
                paneNotes.DestroyPane();
            }
            */

            foreach (var page in _tabPages)
            {
                if (((TabSetting)page.Tag).TabType == DocumentViewType.Normal)
                {
                    ((PaneNotes)page.Controls[0]).DestroyPane();
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
                    if (setting.TabType == 0 && _views.TabSettings.Find(x => x.ColumnName == setting.ColumnName) == null)
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

            //RefreshTabs();
        }
        public void ResetAllTabs()
        {
            
            foreach (var page in _tabPages)
            {
                ((Pane)page.Controls[0]).DestroyPane();
            }
            _tabPages.Clear();

            //_tabSettings = _views.DocumentsService.GetDocumentColumnSettings();

            foreach (var tabSetting in _views.TabSettings)
            {
                //if (tabSetting.Visible == false) continue;

                TabPage tabPage = AddNewTab(tabSetting);

                _tabPages.Add(tabPage);
            }

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
                PaneNotes paneNotes = new PaneNotes(tabSetting.ColumnName);

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
            else    // HtmlViewer
            {
                PaneNoteHtmlView paneHtmlView = new PaneNoteHtmlView(tabSetting.ColumnName, tabSetting.TabType);

                paneHtmlView.InitPane(_views, _ownerView, _panel, _tab);
                paneHtmlView._eventDataModified += new EventHandler(OnDataModified);


                paneHtmlView.SortTabs += new EventHandler(onTabs_Clicked);
                paneHtmlView.AddBrowserTab += new EventHandler(onAddBrowserTab_Clicked);
                paneHtmlView.AddViewTab += new EventHandler(onAddViewTab_Clicked);

                //_splitter.Panel2.Controls.Add(_paneNotes);                
                paneHtmlView.ShowPane();

                _paneViewNotesList.Add(paneHtmlView.GetDebugPaneNotes());

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

        public void InvokeUpdateTextOnMainThread(double ED_ENC_NUM, bool v)
        {
            
            foreach (TabPage page in TabNotesControl.TabPages )
            {
                if (((TabSetting)page.Tag).TabType == DocumentViewType.Normal)
                {
                    var pane = (PaneNotes)page.Controls[0];
                    var text = _views.DocumentsService.GetDocumentText(ED_ENC_NUM, pane.NoteColumnName);
                    pane.InvokeUpdateTextOnMainThread(text, true);

                } else if (((TabSetting)page.Tag).TabType != DocumentViewType.Browser)
                {
                    var pane = (PaneNoteHtmlView)page.Controls[0];
                    var text = _views.DocumentsService.GetDocumentText(ED_ENC_NUM, pane.NoteColumnName);
                    pane.InvokeUpdateTextOnMainThread(text, true);
                }
            }
            /*
            foreach (PaneNotes pane in _paneNotesList)
            {
                var text = _views.DocumentsService.GetDocumentText(ED_ENC_NUM, pane.NoteColumnName);
                pane.InvokeUpdateTextOnMainThread(text, true);
            }
            foreach(var viewPane in _paneViewNotesList)
            {   
                var text = _views.DocumentsService.GetDocumentText(ED_ENC_NUM, viewPane.NoteColumnName);
                viewPane.InvokeUpdateTextOnMainThread(text, true);
            }
            */
        }

        #endregion

        #region Implementation: context menu

        #endregion
    }

}
