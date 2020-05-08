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
using System.Reflection;
using System.Threading.Tasks;

namespace RegScoreCalc
{
    public partial class PaneNotes : Pane
    {
        #region Delegates

        public event EventHandler CalcScores;
        public event EventHandler RefreshHighlights;
        public event EventHandler SortTabs;
        public event EventHandler AddBrowserTab;
        public event EventHandler AddViewTab;
        #endregion

        #region Fields

        //protected PaneNotesCommandsFast _commands;

        private double _lastDocumentID;
        private string _lastDocument;
        protected string _currentNoteText;
        protected string _originalNoteText;

        protected bool _preventUpdate;

        protected int _nChanges;

        protected List<RegExpMatchResult> _listMatches;
        protected Dictionary<RegExpBase, TextStyle> _styleCache;
        protected TextStyle _defaultStyle;

        /*
		public RibbonButton btnWriteNotes;
        */
        public RibbonPanel panel;

        protected string _noteColumnName;

        protected bool _bIsShowFontPane;

        protected object myLocker = new object();

        #endregion

        #region Properties

        public bool ShowToolbar
        {
            get { return toolStripTop.Visible; }
            set { toolStripTop.Visible = value; }
        }

        public bool ShowFontPane
        {
            get { return _bIsShowFontPane; }
            set {
                _bIsShowFontPane = value;
            }
        }

        public string NoteColumnName {
            get { return _noteColumnName; }
        }

        #endregion

        #region Ctors

        public PaneNotes(string noteColumnName = "NOTE_TEXT")
		{

            _noteColumnName = noteColumnName;

			InitializeComponent();

			toolStripTop.Renderer = new CustomToolStripRenderer { RoundedEdges = false };

			_listMatches = new List<RegExpMatchResult>();
			_styleCache = new Dictionary<RegExpBase, TextStyle>();
			_defaultStyle = new TextStyle(Brushes.Black, Brushes.White, FontStyle.Regular);

			textBox.ShowLineNumbers = false;
			textBox.WordWrap = true;

            _bIsShowFontPane = false;

            _lastDocumentID = -1;
        }

        #endregion

        #region Events

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.AddBrowserTab != null)
            {
                this.AddBrowserTab(this, EventArgs.Empty);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.SortTabs != null)
            {
                this.SortTabs(this, EventArgs.Empty);
            }
        }

        private void btnAddViewTab_Click(object sender, EventArgs e)
        {
            if (this.AddViewTab != null)
            {
                this.AddViewTab(this, EventArgs.Empty);
            }
        }

        private void toolStripButtonAddToExceptions_Click(object sender, EventArgs e)
		{
			try
			{
				AddSelectionToExceptions();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void OnExportNotes_Clicked(object sender, EventArgs e)
		{
			ExportNotes();
		}

		private void OnCurrentDocumentChanged(object sender, EventArgs e)
		{
			if (_ownerView.Visible)
				UpdateText();
		}

		private void OnCurrentRegExpChanged(object sender, EventArgs e)
		{
			
		}

		private void OnCurrentItemChanged(object sender, EventArgs e)
		{
			
		}

		private void OnMatchNavigate(RegExpMatchResult result)
		{
            /*
            Task.Run(() =>
            {
                lock (myLocker)
                {
                */
                    if (!_noteColumnName.StartsWith("NOTE_TEXT"))
                        return;

                    if (result.ColumnIndex != Int32.Parse(_noteColumnName.Replace("NOTE_TEXT", "0")))
                        return;

                    if (textBox.IsDisposed)
                        return;

                    ClearHighlights();
                    ClearStyleCache();

                    if (result.RefreshAll)
                    {
                        _listMatches = GetAllMatches();
                        if (!_listMatches.Any())
                            return;

                        ///////////////////////////////////////////////////////////////////////////////

                        foreach (var match in _listMatches)
                        {
                            HighlightMatch(match);
                        }

                        textBox.Refresh();

                        return;
                    }

                    ///////////////////////////////////////////////////////////////////////////////

                    if (_views.MainForm.sourceDocuments.Position != result.Position)
                        _views.MainForm.sourceDocuments.Position = result.Position;

                    var text = GetDocumentText(_views.MainForm.sourceDocuments.Current as DataRowView);
                    var currentText = textBox.Text;
                    if (text != currentText)
                        InvokeUpdateTextOnMainThread(text, false);

                    ///////////////////////////////////////////////////////////////////////////////

                    HighlightMatch(result);

                    ScrollToMatch(result);

                    textBox.Refresh();
            /*
                }
            });            
            */
		}

		private void btnScrollToFirstMatch_Click(object sender, EventArgs e)
		{
			try
			{
				OnMatchNavigate(RegExpMatchResult.RefreshResult());

				_listMatches = GetAllMatches();
				if (!_listMatches.Any())
				{
					MessageBox.Show("No matches found in current document", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				///////////////////////////////////////////////////////////////////////////////

				ScrollToMatch(_listMatches.First());
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnScrollToLastMatch_Click(object sender, EventArgs e)
		{
			try
			{
				OnMatchNavigate(RegExpMatchResult.RefreshResult());

				_listMatches = GetAllMatches();
				if (!_listMatches.Any())
				{
					MessageBox.Show("No matches found in current document", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				///////////////////////////////////////////////////////////////////////////////

				ScrollToMatch(_listMatches.Last());
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Operations

		public string GetSelectedText()
		{
			return textBox.SelectedText.Trim();
		}

        public Font GetTextFont()
        {
            return textBox.Font;
        }
        public void SetTextFont(Font fnt)
        {
            textBox.Font = fnt;
        }

        public int GetLineInterval()
        {
            return textBox.LineInterval;
        }

        public void SetLineInterval( int interval )
        {
            textBox.LineInterval = interval;
        }

		#endregion

		#region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			base.InitPane(views, ownerView, panel, tab);

			_views.MainForm.sourceDocuments.CurrentChanged += OnCurrentDocumentChanged;
			//_views.MainForm.sourceRegExp.CurrentChanged += OnCurrentRegExpChanged;
			//_views.MainForm.sourceRegExp.CurrentItemChanged += OnCurrentItemChanged;

			_views.OnMatchNavigate += OnMatchNavigate;

            _views.OnFontChanged += this.OnTextBoxFontChanged;
            _views.OnLineSpacingChanged += this.OnLineSpacingChanged;

            this.OnTextBoxFontChanged();
            this.OnLineSpacingChanged();
		}

        private void OnLineSpacingChanged()
        {
            int interval = 5;

            switch (_views.LineSpacing)
            {
                case 0:
                    interval = 1;
                    break;

                case 1:
                    interval = 5;
                    break;

                case 2:
                    interval = 10;
                    break;

                case 3:
                    interval = 15;
                    break;               
            }
            textBox.LineInterval = interval;
            textBox.Refresh();
        }

        private void OnTextBoxFontChanged()
        {
            textBox.Font = new Font(_views.FontFamily, _views.FontSize, _views.FontStyle);
            textBox.Refresh();
        }

        protected override void InitPaneCommands(RibbonTab tab)
		{

            if (this.ShowFontPane == true)
            {
                panel = new RibbonPanel("Notes");

                tab.Panels.Add(panel);

                //_commands = new PaneNotesCommandsFast(_views, panel, textBox);
                //_commands._eventDataModified += new EventHandler(OnDataModified);
                /*
                btnWriteNotes = new RibbonButton("Export Modified Notes");

                panel.Items.Add(btnWriteNotes);

                btnWriteNotes.Image = Properties.Resources.ExportToExcel;
                btnWriteNotes.SmallImage = Properties.Resources.ExportToExcel;
                btnWriteNotes.Click += new EventHandler(OnExportNotes_Clicked);
                btnWriteNotes.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
                */
            }
		}

		public override void UpdatePane()
		{
			UpdateText();
		}

		public override void DestroyPane()
		{
			_views.MainForm.sourceDocuments.CurrentChanged -= OnCurrentDocumentChanged;
			//_views.MainForm.sourceRegExp.CurrentChanged -= OnCurrentRegExpChanged;
			//_views.MainForm.sourceRegExp.CurrentItemChanged -= OnCurrentItemChanged;

			_views.OnMatchNavigate -= OnMatchNavigate;
            _views.OnFontChanged -= this.OnTextBoxFontChanged;
            _views.OnLineSpacingChanged -= this.OnLineSpacingChanged;
            /*
            if (this.ShowFontPane)
			    _commands._eventDataModified -= OnDataModified;
                */
            base.DestroyPane();
		}

        #endregion

        #region Implementation

        protected string GetDocumentText(DataRowView rowView)
        {
            var documentText = String.Empty;

            if (rowView != null && rowView.Row != null)
            {
				double documentID = Convert.ToDouble(rowView.Row["ED_ENC_NUM"]);


                if (documentID == _lastDocumentID)
                    documentText = _lastDocument;
                else
                {
                    documentText = _views.DocumentsService.GetDocumentText(documentID, _noteColumnName);
                    _lastDocumentID = documentID;
                    _lastDocument = documentText;
                }
			}

			return documentText;
		}

		protected List<RegExpMatchResult> GetAllMatches()
		{
			var processor = new RegExpProcessor(_views.MainForm.datasetMain.RegExp);
			return processor.GetAllMatches(textBox.Text);
		}

		protected void ClearStyleCache()
		{
			if (_styleCache.Any())
				_styleCache.Clear();

			textBox.ClearStylesBuffer();
		}

		protected TextStyle GetMatchStyle(RegExpMatchResult result)
		{
			TextStyle style;
			if (_styleCache.TryGetValue(result.RegExp, out style))
				return style;

			style = new TextStyle(Brushes.Black, new SolidBrush(result.RegExp.Color), FontStyle.Bold | FontStyle.Underline);

			_styleCache[result.RegExp] = style;

			return style;
		}

		protected void HighlightMatch(RegExpMatchResult match)
		{
			try
			{
				var range = textBox.GetRange(match.Start, match.End);
				range.SetStyle(GetMatchStyle(match));
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected void ScrollToMatch(RegExpMatchResult match)
		{
			var color = textBox.SelectionColor;

			try
			{
				var range = textBox.GetRange(match.Start, textBox.Text.Length - match.Start);

				textBox.SelectionColor = Color.Transparent;

				textBox.Selection = range;
				textBox.DoSelectionVisible();
				textBox.Selection = textBox.GetRange(match.Start, match.Start);

				//textBox.OnScroll(new ScrollEventArgs(ScrollEventType.SmallIncrement, textBox.VerticalScroll.Value, ScrollOrientation.VerticalScroll), true);
			}
			catch
			{
			}
			finally
			{
				textBox.SelectionColor = color;
			}
		}

		public void UpdateText()
		{
            /*
            Task.Run(() =>
            {
                lock (myLocker)
                { */
                    try
                    {
                        _preventUpdate = true;

                        var documentText = GetDocumentText(_views.MainForm.sourceDocuments.Current as DataRowView);
                        InvokeUpdateTextOnMainThread(documentText, !_views.IsNavigating);
                    }
                    catch (Exception ex)
                    {
                        MainForm.ShowErrorToolTip(ex.Message);
                    }
                    finally
                    {
                        _preventUpdate = false;
                    }
            /*
                }
            });			
            */
		}

		public void InvokeUpdateTextOnMainThread(string text, bool highlight)
		{
			if (textBox.InvokeRequired)
				textBox.Invoke((MethodInvoker) delegate { UpdateText(text, highlight); });
			else
				UpdateText(text, highlight);
		}

		protected void UpdateText(string text, bool highlight)
		{
			if (textBox.IsDisposed)
				return;

			ClearStyleCache();

			///////////////////////////////////////////////////////////////////////////////

			bool scrollToFirstMatch;

			if (text != _originalNoteText)
			{
				_originalNoteText = text;

				textBox.Text = text;

				_currentNoteText = textBox.Text;

				scrollToFirstMatch = true;
			}
			else
			{
				ClearHighlights();

				scrollToFirstMatch = false;
			}

            ///////////////////////////////////////////////////////////////////////////////
            textBox.Refresh();
            if (!highlight)
			{
                /*
                if (this.ShowFontPane)
				    _commands.SetLineSpacing(_views.LineSpacing);
                    */
				return;
			}

			///////////////////////////////////////////////////////////////////////////////

			var processor = new RegExpProcessor(_views.MainForm.datasetMain.RegExp.Rows.Cast<DataRow>());
			if (processor.HasEmptyItems)
				MainForm.ShowErrorToolTip("Found empty regular expressions, skipping");

			_listMatches = processor.GetAllMatches(text);

			///////////////////////////////////////////////////////////////////////////////

			if (!_listMatches.Any())
				return;

			if (_listMatches.Count > 1000)
				return;

			foreach (var match in _listMatches)
			{
				HighlightMatch(match);
			}
            /*
            if (this.ShowFontPane)
			    _commands.SetLineSpacing(_views.LineSpacing);
                */
			///////////////////////////////////////////////////////////////////////////////

			if (scrollToFirstMatch)
				ScrollToMatch(_listMatches.First());

			textBox.Refresh();
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
					else
					{
						if (ExportToExcel(outputFilePath, documents, options))
						{
							var dlgres = MessageBox.Show("Exporting documents finished!" + Environment.NewLine + Environment.NewLine + "Do you wish to open file in Excel?", "Export finished", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
							if (dlgres == DialogResult.Yes)
								Process.Start(outputFilePath);
						}
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
				var exportArgs = (ExcelExportArgs) args;

				excelApp = new Excel.Application();
				excelApp.Visible = false;

				var book = excelApp.Workbooks.Add();
				var sheet = (Excel.Worksheet) book.Worksheets[1];
				var cells = sheet.Cells;

				///////////////////////////////////////////////////////////////////////////////

				var rowIndex = 1;

				var columns = _views.MainForm.datasetMain.Documents.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
				columns.Add("NOTE_TEXT");

				var columnIndex = 1;
				foreach (var columnName in columns)
				{
					var range = (Excel.Range) cells[rowIndex, columnIndex];
					range.Value2 = columnName;
					range.Font.Bold = true;

					if (columnName == "NOTE_TEXT")
					{
						range.EntireColumn.ColumnWidth = 100;
						range.EntireColumn.Interior.Color = ColorTranslator.ToOle(Color.LightGray);
					}

					columnIndex++;
				}

				///////////////////////////////////////////////////////////////////////////////

				var totalDocuments = (double) exportArgs.Documents.Count();

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

							var documentID = (double) doc["ED_ENC_NUM"];
							value = _views.DocumentsService.GetDocumentText(documentID, _noteColumnName);

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
									if (categories.TryGetValue((int) categoryID, out categoryName))
										value = categoryID + " - " + categoryName;
								}
							}
							else
								value = Convert.ToString(doc[columnIndex - 1]);
						}

						if (!String.IsNullOrEmpty(value))
						{
							var range = (Excel.Range) cells[rowIndex, columnIndex];
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

					var row = (Excel.Range) cells[rowIndex, 1];
					row.EntireRow.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

					if (worker.CancellationPending)
					{
						break;
					}

					var progress = (int) (((rowIndex - 1) / totalDocuments) * 100d);
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
															  || column.ColumnName == "NOTE_TEXT" || column.ColumnName == "Category"
															  || column.ColumnName == "Score"))
					selectColumns.Add(column.ColumnName);
			}

			var processedDocumentsDataTable = documents.AsDataView()
													   .ToTable(false, selectColumns.ToArray());

			var columns = processedDocumentsDataTable.Columns.Cast<DataColumn>().ToList();
			columns.Add(new DataColumn("NOTE_TEXT") { Caption = "NOTE_TEXT" });

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

		private void WriteXMLSchema(StreamWriter sw)
		{
			StringBuilder sb = new StringBuilder();

			//Add dynamic columns
			foreach (var categoryClassRow in _views.MainForm.datasetMain.DynamicColumns)
			{
				string type = "";
				var classType = (DynamicColumnType) categoryClassRow.Type;
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

					var documentID = (double) row["ED_ENC_NUM"];
					var noteText = _views.DocumentsService.GetDocumentText(documentID, _noteColumnName);

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
						var category = _views.MainForm.datasetMain.Categories.First(x => x.ID == (int) row[dc]);

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

		protected void ClearHighlights()
		{
			try
			{
				textBox.ClearStyle(StyleIndex.All);
			}
			catch
			{
			}
		}

		static string RemoveInvalidXmlChars(string text)
		{
			var validXmlChars = text.Where(ch => System.Xml.XmlConvert.IsStartNCNameChar(ch))
									.ToArray();
			return new string(validXmlChars);
		}

		protected void AddSelectionToExceptions()
		{
			if (textBox.SelectionLength == 0)
				return;

			var text = textBox.SelectedText.Trim();
			if (!String.IsNullOrEmpty(text))
			{
				var rowView = _views.MainForm.sourceRegExp.Current as DataRowView;

				var regExp = RegExpFactory.Create_RegExp(rowView, false);
				if (regExp != null)
				{
					regExp.Exceptions.Items.Add(new RegExpCriteria
												{
													Enabled = true,
													Expression = text
												});

					regExp.SafeSave(rowView, true);

					_views.UpdateViews();
				}
			}
		}

		#endregion

		#region Implementation: context menu

		private void textBox_SelectionChanged(object sender, EventArgs e)
		{
			if (_preventUpdate)
				return;

			textBox.ContextMenuStrip = null;
			menuPopup.Tag = null;

			menuitemAddToLookAhead.Visible = false;

			menuitemAddToLookAhead.Visible = false;
			menuitemAddToLookBehind.Visible = false;
			menuitemAddToNegLookAhead.Visible = false;
			menuitemAddToNegLookBehind.Visible = false;
			menuitemAddToExceptions.Visible = false;

			//////////////////////////////////////////////////////////////////////////

			bool bSeparatorVisible = false;

			int nSelStart = textBox.SelectionStart;
			int nSelEnd = nSelStart + textBox.SelectionLength;

			string strSelectedText = textBox.SelectedText.Trim();
			if (!String.IsNullOrEmpty(strSelectedText))
			{
				textBox.ContextMenuStrip = menuPopup;
				textBox.Tag = strSelectedText;
			}

			//////////////////////////////////////////////////////////////////////////

			if (_listMatches != null && _listMatches.Any())
			{
				var varMatchesException = _listMatches.Where(match => nSelStart >= match.Start && nSelEnd <= match.End).ToList();

				if (varMatchesException.Any())
				{
					menuitemAddToExceptions.Visible = true;

					var match = varMatchesException.First();

					textBox.Tag = _currentNoteText.Substring(match.Start, match.Length);
					textBox.ContextMenuStrip = menuPopup;

					menuPopup.Tag = match;

					bSeparatorVisible = true;
				}
				else
				{
					if (!String.IsNullOrEmpty(strSelectedText))
					{
						const int nMaxDistance = 5;

						var varMatchesBehind = _listMatches.Where(match =>
						{
							int nDistance = match.Start - nSelEnd;
							if (nDistance > 0)
							{
								if (nDistance <= nMaxDistance)
								{
									string strText = _currentNoteText.Substring(nSelEnd, nDistance);
									if (String.IsNullOrEmpty(strText.Trim()))
										return true;
								}
							}
							else if (nDistance == 0)
								return true;

							return false;
						})
														   .ToList();

						if (varMatchesBehind.Any())
						{
							menuitemAddToLookBehind.Visible = true;
							menuitemAddToNegLookBehind.Visible = true;

							textBox.ContextMenuStrip = menuPopup;

							menuPopup.Tag = varMatchesBehind.First();

							bSeparatorVisible = true;
						}
						else
						{
							var varMatchesAhead = _listMatches.Where(match =>
							{
								int nDistance = nSelStart - match.End;
								if (nDistance > 0)
								{
									if (nDistance <= nMaxDistance)
									{
										string strText = _currentNoteText.Substring(match.End, nDistance);
										if (String.IsNullOrEmpty(strText.Trim()))
											return true;
									}
								}
								else if (nDistance == 0)
									return true;

								return false;
							})
															  .ToList();

							if (varMatchesAhead.Any())
							{
								menuitemAddToLookAhead.Visible = true;
								menuitemAddToNegLookAhead.Visible = true;

								textBox.ContextMenuStrip = menuPopup;

								menuPopup.Tag = varMatchesAhead.First();

								bSeparatorVisible = true;
							}
						}
					}
				}
			}

			//////////////////////////////////////////////////////////////////////////

			menuitemSeparator.Visible = bSeparatorVisible;
		}

		private void menuitemCopy_Click(object sender, EventArgs e)
		{
			try
			{
				if (menuPopup.Tag is string)
				{
					var match = (RegExpMatchResult)menuPopup.Tag;
					Clipboard.SetText(_currentNoteText.Substring(match.Start, match.Length));
				}
				else
					Clipboard.SetText(textBox.SelectedText);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void menuitemAddToLookBehind_Click(object sender, EventArgs e)
		{
			AppendRegExpCriteria((RegExpMatchResult)menuPopup.Tag, RegExpCriteriaType.LookBehind, "", @"\s*");
		}

		private void menuitemAddToNegLookBehind_Click(object sender, EventArgs e)
		{
			AppendRegExpCriteria((RegExpMatchResult)menuPopup.Tag, RegExpCriteriaType.NegLookBehind, "", @"\s*");
		}

		private void menuitemAddToLookAhead_Click(object sender, EventArgs e)
		{
			AppendRegExpCriteria((RegExpMatchResult)menuPopup.Tag, RegExpCriteriaType.LookAhead, @"\s*", "");
		}

		private void menuitemAddToNegLookAhead_Click(object sender, EventArgs e)
		{
			AppendRegExpCriteria((RegExpMatchResult)menuPopup.Tag, RegExpCriteriaType.NegLookAhead, @"\s*", "");
		}

		private void menuitemAddToExceptions_Click(object sender, EventArgs e)
		{
			AppendRegExpCriteria((RegExpMatchResult)menuPopup.Tag, RegExpCriteriaType.Exception, "", "");
		}

		protected void AppendRegExpCriteria(RegExpMatchResult match, RegExpCriteriaType criteriaType, string strPrefix, string strSuffix)
		{
			try
			{
				string strSelectedText = (string)textBox.Tag;

				if (criteriaType != RegExpCriteriaType.Exception)
				{
					if (strSelectedText.IndexOf(" ", StringComparison.InvariantCulture) == -1 && strSelectedText.Trim() == strSelectedText)
						strSelectedText = strPrefix + @"\b" + Regex.Escape(strSelectedText) + @"\b" + strSuffix;
					else
						strSelectedText = strPrefix + Regex.Escape(strSelectedText) + strSuffix;
				}
				else
					strSelectedText = strPrefix + strSelectedText + strSuffix;

				var rowRegExp = _views.MainForm.datasetMain.RegExp.FirstOrDefault(x => x.ID == match.RegExp.ID);
				if (rowRegExp != null)
				{
					var regExp = RegExpFactory.Create_RegExp(rowRegExp, false);
					regExp.AddCriteria(criteriaType, strSelectedText);

					regExp.SafeSave(rowRegExp, true);

					//////////////////////////////////////////////////////////////////////////

					if (this.RefreshHighlights != null)
						this.RefreshHighlights(this, EventArgs.Empty);

					//////////////////////////////////////////////////////////////////////////

					if (this.CalcScores != null)
					{
						if (_views.AutoCalc > 0)
						{
							_nChanges++;
							if (_nChanges >= _views.AutoCalc)
							{
								_nChanges = 0;

								DialogResult dlgres = MessageBox.Show("Do you wish to calculate scores?", MainForm.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
								if (dlgres == DialogResult.Yes)
									this.CalcScores(this, EventArgs.Empty);
							}
						}
					}

					//ClearHighlights();
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}



        #endregion
    }

    public class ExcelExportArgs
	{
		public ExportNotesOptions Options { get; set; }
		public EnumerableRowCollection<MainDataSet.DocumentsRow> Documents { get; set; }
		public string OutputFilePath { get; set; }
	}
}
