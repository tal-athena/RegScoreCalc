using System;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using DocumentsServiceInterfaceLib;

using FastColoredTextBoxNS;

using Helpers;

using RegExpLib.Core;
using RegExpLib.Model;
using RegExpLib.Processing;

using RegScoreCalc.Code;

namespace RegScoreCalc
{
	public partial class PaneAdvNotes : Pane
	{
		#region Delegates

		public event EventHandler CalcScores;
		public event EventHandler RefreshHighlights;

		#endregion

		#region Fields

		protected PaneNotesCommandsFast _commands;

		protected string _currentNoteText;
		protected string _originalNoteText;

		protected bool _bIgnoreCurrentDocumentChange;

		protected int _nChanges;

		protected List<RegExpMatchResult> _listMatches;
		protected Dictionary<int, TextStyle> _styleCache;
		protected TextStyle _defaultStyle;

		#endregion

		#region Ctors

		public PaneAdvNotes()
		{
			InitializeComponent();

			toolStripTop.Renderer = new CustomToolStripRenderer { RoundedEdges = false };

			_listMatches = new List<RegExpMatchResult>();
			_styleCache = new Dictionary<int, TextStyle>();
			_defaultStyle = new TextStyle(Brushes.Black, Brushes.White, FontStyle.Regular);

			textBox.ShowLineNumbers = false;
			textBox.WordWrap = true;
		}

		#endregion

		#region Events

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

		private void OnWriteNotes_Clicked(object sender, EventArgs e)
		{
			WriteNotes();
		}

		private void OnCurrentDocumentChanged(object sender, EventArgs e)
		{
			if (!_bIgnoreCurrentDocumentChange)
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

		#endregion

		#region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			base.InitPane(views, ownerView, panel, tab);

			_views.MainForm.sourceDocuments.CurrentChanged += OnCurrentDocumentChanged;
			_views.MainForm.sourceRegExp.CurrentChanged += OnCurrentRegExpChanged;
			_views.MainForm.sourceRegExp.CurrentItemChanged += OnCurrentItemChanged;

			_views.OnMatchNavigate += OnMatchNavigate;
		}

		protected override void InitPaneCommands(RibbonTab tab)
		{
			RibbonPanel panel = new RibbonPanel("Notes");
			tab.Panels.Add(panel);

			RibbonButton btnWriteNotes = new RibbonButton("Write Modified Notes");
			panel.Items.Add(btnWriteNotes);

			btnWriteNotes.Image = Properties.Resources.WriteNotes;
			btnWriteNotes.SmallImage = Properties.Resources.WriteNotes;
			btnWriteNotes.Click += new EventHandler(OnWriteNotes_Clicked);

			_commands = new PaneNotesCommandsFast(_views, panel, textBox);
			_commands._eventDataModified += new EventHandler(OnDataModified);
		}

		public override void UpdatePane()
		{
			UpdateText();
		}

		public override void DestroyPane()
		{
			_views.MainForm.sourceDocuments.CurrentChanged -= OnCurrentDocumentChanged;
			_views.MainForm.sourceRegExp.CurrentChanged -= OnCurrentRegExpChanged;
			_views.MainForm.sourceRegExp.CurrentItemChanged -= OnCurrentItemChanged;

			_views.OnMatchNavigate -= OnMatchNavigate;

			_commands._eventDataModified -= OnDataModified;

			base.DestroyPane();
		}

		#endregion

		#region Implementation

		protected string GetDocumentText(DataRowView rowView)
		{
			var documentText = String.Empty;

			if (rowView != null && rowView.Row != null)
			{
				var documentID = (double)rowView.Row["ED_ENC_NUM"];
				documentText = _views.DocumentsService.GetDocumentText(documentID);
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

		protected TextStyle GetCachedStyle(int rgb)
		{
			TextStyle style;
			if (_styleCache.TryGetValue(rgb, out style))
				return style;

			style = new TextStyle(Brushes.Black, new SolidBrush(Color.FromArgb(rgb)), FontStyle.Bold | FontStyle.Underline);

			_styleCache[rgb] = style;

			return style;
		}

		protected void HighlightMatch(RegExpMatchResult matchResult)
		{
			var arrColors = PaneAdvRegExp.GetRegExpColors(matchResult.RegExp.ID, _views, true);
			if (arrColors.Count == 0)
				arrColors.Add(new ColorInfo("", -1, RegExpBase.DefaultHighlightColor.ToArgb(), true));

			///////////////////////////////////////////////////////////////////////////////

			int nBlockLen = matchResult.Match.Length / arrColors.Count;
			int nBlockStart = matchResult.Match.Index;

			ColorInfo ci;
			for (int i = 0; i < arrColors.Count; i++)
			{
				ci = (ColorInfo)arrColors[i];

				if (i == arrColors.Count - 1)
					nBlockLen = (matchResult.Match.Index + matchResult.Match.Length) - nBlockStart;

				var style = GetCachedStyle(ci.RGB);
				textBox.GetRange(nBlockStart, nBlockStart + nBlockLen).SetStyle(style);

				nBlockStart += nBlockLen;
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

		protected void UpdateText()
		{
			try
			{
				var documentText = GetDocumentText(_views.MainForm.sourceDocuments.Current as DataRowView);
				InvokeUpdateTextOnMainThread(documentText, !_views.IsNavigating);
			}
			catch (Exception ex)
			{
				MainForm.ShowErrorToolTip(ex.Message);
			}
		}

		public void InvokeUpdateTextOnMainThread(string text, bool highlight)
		{
			if (textBox.InvokeRequired)
				textBox.Invoke((MethodInvoker)delegate { UpdateText(text, highlight); });
			else
				UpdateText(text, highlight);
		}

		public void UpdateText(string text, bool highlight)
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

			if (!highlight)
			{
				_commands.SetLineSpacing(_views.LineSpacing);
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

			_commands.SetLineSpacing(_views.LineSpacing);

			///////////////////////////////////////////////////////////////////////////////

			if (scrollToFirstMatch)
				ScrollToMatch(_listMatches.First());

			textBox.Refresh();
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
						connection = new OleDbConnection(connectionString);
						connection.Open();

						OleDbCommand command;

						string strCommand = "CREATE TABLE " + tableName + " (ED_ENC_NUM double, NoteText ntext, Score int, Category ntext, CategoryID int)";
						command = new OleDbCommand(strCommand, connection);
						command.ExecuteNonQuery();

						//Start writing data 

						WriteModifiedNotesToTable(connection, tableName);

						connection.Close();
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
				bool retVal = WriteNote(connection, dataRow.ED_ENC_NUM, _views.DocumentsService.GetDocumentText(dataRow.ED_ENC_NUM), dataRow.Score, nCategory, tableName);
				//if (retVal)
				//nNotesWritten++;
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

		protected bool WriteNote(OleDbConnection connection, double ED_ENC_NUM, string strOriginalText, int nScore, int nCategoryID, string tableName)
		{
			bool bResult = false;

			try
			{
				if (!String.IsNullOrEmpty(strOriginalText))
				{
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
									strCategory = (string) ds.Tables[0].Rows[0][0];
							}
							catch
							{
								//TTrace(System.Reflection.MethodBase.GetCurrentMethod(), ex);
							}
						}

						strCommand = "INSERT INTO  " + tableName
						             + "  (ED_ENC_NUM, NoteText, Score, Category, CategoryID) "
						             + "VALUES (?, ?, ?, ?, ?)";

						command = new OleDbCommand(strCommand, connection);

						command.Parameters.Add("ED_ENC_NUM", OleDbType.Double)
						       .Value = ED_ENC_NUM;
						command.Parameters.Add("NoteText", OleDbType.Char)
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

		private void AddSelectionToExceptions()
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
}