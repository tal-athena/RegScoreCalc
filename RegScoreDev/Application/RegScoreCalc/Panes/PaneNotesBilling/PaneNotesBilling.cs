using System;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

using DocumentsServiceInterfaceLib;

using FastColoredTextBoxNS;

using RegExpLib.Core;
using RegExpLib.Model;
using RegExpLib.Processing;

using RegScoreCalc.Code;
using RegScoreCalc.Data;

namespace RegScoreCalc
{
	public enum NoteType
	{
		TEXTNOTE,
		DISCNOTE
	}

	public partial class PaneNotesBilling : Pane
	{
		#region Delegates

		public event EventHandler CalcScores;
		public event EventHandler RefreshHighlights;

		#endregion

		#region Fields

		//Turn off regular expression highlighting at start

		private readonly NoteType _noteType;

		protected string _currentNoteText;
		protected string _originalNoteText;

		protected bool _bIgnoreCurrentDocumentChange;

		protected int _nChanges;

		protected List<RegExpMatchResult> _listMatches;
		protected Dictionary<RegExpBase, TextStyle> _styleCache;
		protected TextStyle _defaultStyle;

		private DataGridView _billingGridView;
		ViewBILLING_1 _viewBilling;

		#endregion

		#region Ctors

		public PaneNotesBilling(NoteType type, ViewBILLING_1 viewBilling)
		{
			InitializeComponent();

			_viewBilling = viewBilling;
			_billingGridView = _viewBilling.GetDocumentsGrid();
			_billingGridView.SelectionChanged += billingGridView_SelectionChanged;
			_noteType = type;

			_listMatches = new List<RegExpMatchResult>();

			toolStripTop.Renderer = new CustomToolStripRenderer { RoundedEdges = false };

			_styleCache = new Dictionary<RegExpBase, TextStyle>();
			_defaultStyle = new TextStyle(Brushes.Black, Brushes.White, FontStyle.Regular);

			textBox.ShowLineNumbers = false;
			textBox.WordWrap = true;
		}

		#endregion

		#region Events

		private void billingGridView_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateText();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
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
				UpdateText(text, false);

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

		public string GetSelectedText()
		{
			return textBox.SelectedText.Trim();
		}

		#endregion

		#region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			base.InitPane(views, ownerView, panel, tab);

			_views.MainForm.sourceDocumentsBilling.CurrentChanged += OnCurrentDocumentChanged;
			_views.MainForm.sourceRegExp.CurrentChanged += OnCurrentRegExpChanged;
			_views.MainForm.sourceRegExp.CurrentItemChanged += OnCurrentItemChanged;

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
		}

		public override void UpdatePane()
		{
			UpdateText();
		}

		public override void DestroyPane()
		{
			_views.MainForm.sourceDocumentsBilling.CurrentChanged -= OnCurrentDocumentChanged;
			_views.MainForm.sourceRegExp.CurrentChanged -= OnCurrentRegExpChanged;
			_views.MainForm.sourceRegExp.CurrentItemChanged -= OnCurrentItemChanged;

			_views.OnMatchNavigate -= OnMatchNavigate;

            _views.OnFontChanged -= this.OnTextBoxFontChanged;
            _views.OnLineSpacingChanged -= this.OnLineSpacingChanged;

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
			if (_billingGridView.SelectedRows.Count > 0)
			{
				var gridRow = _billingGridView.SelectedRows[0];
				var edEnc = (double)gridRow.Cells[3].Value;
				var row1 = _views.MainForm.datasetBilling.Documents.FindByPrimaryKey(edEnc);
				//  DataRowView row = _views.MainForm.sourceDocumentsBilling.Single as DataRowView;
				if (row1 != null)
				{
					//BillingDataSet.DocumentsRow dataRow = row.Row as BillingDataSet.DocumentsRow;

					try
					{
						if (_noteType == NoteType.TEXTNOTE)
						{
                            //UpdateText(dataRow.NOTE_TEXT);

                            //UpdateText(row1.NOTE_TEXT, !_views.IsNavigating);
                            UpdateText(row1.NOTE_TEXT, _views.HighlightBillingMatches);
						}
						if (_noteType == NoteType.DISCNOTE)
						{
                            //UpdateText(row1.DISC_TEXT, !_views.IsNavigating);
                            UpdateText(row1.DISC_TEXT, _views.HighlightBillingMatches);
                            //UpdateText(dataRow.DISC_TEXT);
                        }
					}
					catch (Exception ex)
					{
						MainForm.ShowErrorToolTip(ex.Message);
					}
				}
			}
			else
			{
				UpdateText(String.Empty, false);
			}
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

			List<MainDataSet.RegExpRow> regExps;

			if (_viewBilling._filterId > -1)
			{
				regExps = new List<MainDataSet.RegExpRow>();

				//Find this filter all groups
				var groupsIDsString = _views.MainForm.datasetBilling.ICDFilters.FindByFilterID(_viewBilling._filterId).GroupIDs;
				string[] groupsIDs = groupsIDsString.Split(',');

				foreach (var group in groupsIDs)
				{
					foreach (BillingDataSet.RegexpToGroupsRow item in _views.MainForm.datasetBilling.RegexpToGroups.Select("GroupID = " + group))
					{
						var dataRowRegExp = _views.MainForm.datasetMain.RegExp.FindByID(item.ID);
						if (dataRowRegExp != null && dataRowRegExp.RowState != DataRowState.Deleted)
						{
							regExps.Add(dataRowRegExp);
						}
					}
				}
			}
			else
			{
				regExps = _views.MainForm.datasetMain.RegExp.Rows.Cast<MainDataSet.RegExpRow>()
				                .Where(x => x.RowState != DataRowState.Deleted)
				                .ToList();
			}

			var processor = new RegExpProcessor(regExps);
			if (processor.HasEmptyItems)
				MainForm.ShowErrorToolTip("Found empty regular expressions, skipping");

			_listMatches = processor.GetAllMatches(text);

			///////////////////////////////////////////////////////////////////////////////

			if (!_listMatches.Any())
				return;

			if (_listMatches.Count > 1000)
				return;

            if (highlight)
            {

                foreach (var match in _listMatches)
                {
                    HighlightMatch(match);
                }
            }

			///////////////////////////////////////////////////////////////////////////////

			if (scrollToFirstMatch)
				ScrollToMatch(_listMatches.First());

			textBox.Refresh();
		}

		public List<RegExpMatchResult> GetRegExpMatchResults(string text)
		{
			IEnumerable<DataRow> listRegExps = null;

			//Then for every regExp highlight
			if (_viewBilling._filterId > -1)
			{
				//Find this filter all groups
				var filter = _views.MainForm.datasetBilling.ICDFilters.FindByFilterID(_viewBilling._filterId);
				if (filter != null && !filter.IsGroupIDsNull())
				{
					var groupsIDsString = filter.GroupIDs;

					var groupsIDs = groupsIDsString.Split(',');

					listRegExps = from @group in groupsIDs
								  from BillingDataSet.RegexpToGroupsRow item in _views.MainForm.datasetBilling.RegexpToGroups.Select("GroupID = " + @group)
								  select _views.MainForm.datasetMain.RegExp.FindByID(item.ID /* RegExpID */);
				}
			}
			
			if (listRegExps == null)
				listRegExps = _views.MainForm.datasetMain.RegExp.Rows.Cast<DataRow>();

			///////////////////////////////////////////////////////////////////////////////

			var processor = new RegExpProcessor(listRegExps);
			var matches = processor.GetAllMatches(text);

			return matches;
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