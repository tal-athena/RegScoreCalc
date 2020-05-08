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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using DocumentsServiceInterfaceLib;

using Helpers;

using RegExpLib.Core;
using RegExpLib.Model;
using RegExpLib.Processing;

using RegScoreCalc.Code;
using RegScoreCalc.Properties;

namespace RegScoreCalc.Forms
{
    public partial class FormRegularExpressionEditor : Form, IMessageFilter
    {
        #region Delegates

        public event EventHandler Modified;

        #endregion

        #region Fields

        protected BindingSource _source;

        protected RegExpBase _regExp;
        protected Func<DataRowView, RegExpBase> _funcCreateRegExp;

        protected string _connectionString;
        protected string text;
        protected int selectionStartPosition;
        protected int selectionLength;

        protected ViewsManager _views;
        protected int _nPrevPosition;
        protected int _nMatchIndex;
        protected PaneNotes _openedNotesPane;

        protected bool _preventUpdate;
        protected bool _isDirty;

        protected StatisticsWrapper _statistics;

        protected string _distanceProcessOutput;

        #endregion

        #region Properties

        public bool EnableNavigation
        {
            get { return panelNavButtons.Visible; }
            set { panelNavButtons.Visible = value; }
        }

        public bool IsColRegExp {
            get
            {
                if (_regExp is ColRegExp)
                    return true;
                return false;
            }
        }

		#endregion

		#region Ctors

		public FormRegularExpressionEditor(ViewsManager views, BindingSource source, Func<DataRowView, RegExpBase> funcCreateRegExp)
		{
			Application.AddMessageFilter(this);

			InitializeComponent();

			toolStripTop.Renderer = new CustomToolStripRenderer { RoundedEdges = false };

			_connectionString = views.MainForm.DocumentsDbPath;
			_views = views;
			_funcCreateRegExp = funcCreateRegExp;

			_source = source;

			gridStatistics.AutoGenerateColumns = false;

			lvQuickActions.SetBuddy(txtRegExp);

			///////////////////////////////////////////////////////////////////////////////

			tabControlEditRegEx.ShowIndicators = true;

			btnAddLookAhead.Tag = gridCriteriaLookahead;
			btnAddNegLookAhead.Tag = gridCriteriaNegativeLookahead;
			btnAddLookBehind.Tag = gridCriteriaLookbehind;
			btnAddNegLookBehind.Tag = gridCriteriaNegativeLookbehind;
			btnAddException.Tag = gridCriteriaExceptions;

			btnDeleteLookAhead.Tag = gridCriteriaLookahead;
			btnDeleteNegLookAhead.Tag = gridCriteriaNegativeLookahead;
			btnDeleteLookBehind.Tag = gridCriteriaLookbehind;
			btnDeleteNegLookBehind.Tag = gridCriteriaNegativeLookbehind;
			btnDeleteExceptions.Tag = gridCriteriaExceptions;

			///////////////////////////////////////////////////////////////////////////////

			_statistics = new StatisticsWrapper();

			this.BackColor = MainForm.ColorBackground;
			tabControlEditRegEx.BackColor = MainForm.ColorBackground;
			foreach (TabPage page in tabControlEditRegEx.TabPages)
			{
				page.BackColor = MainForm.ColorBackground;
			}
		}

		#endregion

		#region Events

		private void FormRegularExpressionEditor_Load(object sender, EventArgs e)
		{
			splitterMain.Panel2MinSize = splitterMain.Panel2.Width;

			splitterRegExpAndExamples.Panel2Collapsed = !Settings.Default.FormRegExpEditorShowExamples;
			if (Settings.Default.FormRegExpEditorExamplesPaneHeight > 0)
				splitterRegExpAndExamples.SplitterDistance = Settings.Default.FormRegExpEditorExamplesPaneHeight;

			splitterMain.Panel2Collapsed = !Settings.Default.FormRegExpEditorShowQuickActions;

			if (Settings.Default.FormRegExpEditorToolboxPaneWidth > 0)
				splitterMain.SplitterDistance = Settings.Default.FormRegExpEditorToolboxPaneWidth;

			///////////////////////////////////////////////////////////////////////////////

			UpdateButtonTitles();

			///////////////////////////////////////////////////////////////////////////////
            
			_source.CurrentChanged += OnCurrentRegExpChanged;
			_source.CurrentItemChanged += OnCurrentItemChanged;            

			FillExamplesGrid();

			UpdateRegExpView();
		}

		private void FormRegularExpressionEditor_Leave(object sender, EventArgs e)
		{
			Save(true);

			SaveExampleData();

			SaveSettings();
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			try
			{
				SaveRegExpToDatabase();

				_views.InvokeRefreshHighlights();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnShowExamples_Click(object sender, EventArgs e)
		{
			splitterRegExpAndExamples.Panel2Collapsed = !splitterRegExpAndExamples.Panel2Collapsed;
			UpdateButtonTitles();
		}

		private void btnShowToolbox_Click(object sender, EventArgs e)
		{
			splitterMain.Panel2Collapsed = !splitterMain.Panel2Collapsed;

			UpdateButtonTitles();
		}

		private void splitterMain_SplitterMoved(object sender, SplitterEventArgs e)
		{
			UpdateQuickActionsColumnWidth();
		}

		private void OnCurrentItemChanged(object sender, EventArgs e)
		{
			try
			{
				if (!_preventUpdate)
					UpdateRegExpView();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void OnCurrentRegExpChanged(object sender, EventArgs e)
		{
			try
			{
				//UpdateRegExpView();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void txtRegExp_TextChanged(object sender, EventArgs e)
		{
			_isDirty = true;

			SaveRegExp(false);
		}

		private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				this.Validate();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex < 0 || e.ColumnIndex < 0)
					return;

				///////////////////////////////////////////////////////////////////////////////

				var grid = (DataGridView) sender;

				var cell = grid[1, e.RowIndex];
				grid.CurrentCell = cell;

				///////////////////////////////////////////////////////////////////////////////

				var formEditLookaround = new FormEditLookaround
				                         {
					                         Value = (string) cell.Value
				                         };

				if (formEditLookaround.ShowDialog() == DialogResult.OK)
				{
					cell.Value = formEditLookaround.Value;
				}

				grid.CurrentCell = null;

				this.Validate();

				grid.Rows[e.RowIndex].Selected = true;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void grid_Validated(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (_preventUpdate)
					return;

				_isDirty = true;

				SaveCriteria((DataGridView) sender, false);

				_views.InvokeRefreshHighlights();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			try
			{
				var grid = (DataGridView) sender;
				if (grid.CurrentCell != null)
				{
					if (grid.CurrentCell.ColumnIndex == 0)
					{
						if (grid.IsCurrentCellDirty)
						{
							grid.CommitEdit(DataGridViewDataErrorContexts.Commit);

							SaveCriteria(grid, false);

							_views.InvokeRefreshHighlights();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void grid_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			try
			{
				e.Row.Cells[0].Value = true;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnAddCriteria_Click(object sender, EventArgs e)
		{
			try
			{
				var btn = (Button) sender;
				var grid = (DataGridView) btn.Tag;

				AddCriteria(grid);

				if (grid != gridCriteriaExceptions)
				{
					var cell = grid.CurrentCell;
					if (cell != null)
					{
						var formEditLookaround = new FormEditLookaround
						                         {
							                         Value = (string) cell.Value
						                         };

						if (formEditLookaround.ShowDialog() == DialogResult.OK)
						{
							cell.Value = formEditLookaround.Value;
						}

						grid.CurrentCell = null;

						this.Validate();

						cell.OwningRow.Selected = true;
					}
				}
				else
					grid.BeginEdit(true);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnDeleteCriteria_Click(object sender, EventArgs e)
		{
			try
			{
				var btn = (Button)sender;

				DeleteCriteria((DataGridView)btn.Tag);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void tabControlEditRegEx_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateCriteria(tabControlEditRegEx.SelectedTab);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void radioButtonLookbehind_CheckedChanged(object sender, EventArgs e)
		{
			_isDirty = false;

			SaveCriteria(gridCriteriaLookbehind, false);
		}

		private void radioButtonNegativeLookbehind_CheckedChanged(object sender, EventArgs e)
		{
			_isDirty = false;

			SaveCriteria(gridCriteriaNegativeLookbehind, false);
		}

		private void radioButtonLookahead_CheckedChanged(object sender, EventArgs e)
		{
			_isDirty = false;

			SaveCriteria(gridCriteriaLookahead, false);
		}

		private void radioButtonNegativeLookahead_CheckedChanged(object sender, EventArgs e)
		{
			_isDirty = false;

			SaveCriteria(gridCriteriaNegativeLookahead, false);
		}

		private void gridStatistics_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.ColumnIndex <= 0)
					return;

				var column = gridStatistics.Columns[e.ColumnIndex];

				var ascending = column.HeaderCell.SortGlyphDirection != SortOrder.Ascending;

				///////////////////////////////////////////////////////////////////////////////

				gridStatistics.Columns.Cast<DataGridViewColumn>()
				              .ToList()
				              .ForEach(x => x.HeaderCell.SortGlyphDirection = SortOrder.None);

				///////////////////////////////////////////////////////////////////////////////

				var propertyName = column.DataPropertyName;

				_statistics.SortAggregatedResults(propertyName, ascending);

				///////////////////////////////////////////////////////////////////////////////

				gridStatistics.DataSource = _statistics.Statistics;

				column.HeaderCell.SortGlyphDirection = ascending ? SortOrder.Ascending : SortOrder.Descending;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnSelectCategory_Click(object sender, EventArgs e)
		{
			try
			{
				var formSelectCategory = new FormSelectCategory(_views, FormSelectCategory.DisplayMode.Default, null, false);

				if (_regExp.Category != null)
					formSelectCategory.CategoryID = _regExp.Category.Value;

				if (formSelectCategory.ShowDialog() == DialogResult.OK)
				{
					_regExp.Category = formSelectCategory.CategoryID;
					UpdateCategoryText();
					Save(true);
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			try
			{
				_regExp.Category = null;
				Save(true);

				UpdateCategoryText();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

        private void btnSimilar_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(gridStatistics.SelectedRows[0].Cells[1].Value.ToString());
            var original = _views.MainForm.Cursor;

            _views.MainForm.Cursor = Cursors.WaitCursor;

            if (gridStatistics.SelectedRows.Count != 1)
            {
                MainForm.ShowErrorToolTip("Please select a row");
                return;
            }

            var wordsList = GetSimilarWordsList(gridStatistics.SelectedRows[0].Cells[1].Value.ToString());

            _views.MainForm.Cursor = original;

            if (wordsList.Count == 0)
            {
                MessageBox.Show("There are no similar words in dictionary");
                return;
            }

            FormSimilar similar = new FormSimilar(this, wordsList);

            similar.ShowDialog();

            /*
            {
                string similarWord = similar.GetWord();
                if (similarWord != "")
                {
                    if (_regExp is ColRegExp)
                        AddNewColRegExp(similarWord);
                    else 
                        AddNewRegExp(similarWord);
                  
                }
            }
            */
        }

        private void SimilarProcessOutputHandler(object sender, DataReceivedEventArgs e)
        {            
            _distanceProcessOutput += e.Data + "\n";
            //MainForm.ShowInfoToolTip("ass", e.Data);
        }
        private void SimilarProcessExited(object sender, EventArgs e)
        {

        }
        #endregion

        #region Events: Edit RegExp

        private void txtRegExp_Leave(object sender, EventArgs e)
		{
			//Save cursor position
			selectionStartPosition = txtRegExp.SelectionStart;
			selectionLength = txtRegExp.SelectionLength;
		}

		private void txtRegExp_KeyUp(object sender, KeyEventArgs e)
		{
			//CheckIfTextIsSelected();
		}

		private void txtRegExp_Enter(object sender, EventArgs e)
		{
			////Enable first 3
			//btnAnyCharacter.Enabled = true;
			//btnAnyDigit.Enabled = true;
			//btnAnyWhiteSpace.Enabled = true;
		}

		private void txtRegExp_MouseUp(object sender, MouseEventArgs e)
		{
			//CheckIfTextIsSelected();
		}

		private void btnEditDescription_Click(object sender, EventArgs e)
		{
			try
			{
				var formEditDescription = new FormEditDescription
				                          {
					                          Description = _regExp.Description
				                          };

				if (formEditDescription.ShowDialog() == DialogResult.OK)
				{
					_regExp.Description = formEditDescription.Description;
					lblDescription.Text = _regExp.Description.Replace(Environment.NewLine, " ");
					Save(true);
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Events: navigation

		private void btnPrevDocument_Click(object sender, EventArgs e)
		{
			InvokeNavigate(MatchNavigationMode.PrevDocument);
		}

		private void btnNextDocument_Click(object sender, EventArgs e)
		{
			InvokeNavigate(MatchNavigationMode.NextDocument);
		}

		private void btnPrevMatch_Click(object sender, EventArgs e)
		{
			InvokeNavigate(MatchNavigationMode.PrevMatch);
		}

		private void btnNextMatch_Click(object sender, EventArgs e)
		{
			InvokeNavigate(MatchNavigationMode.NextMatch);
		}

		private void btnPrevUniqueMatch_Click(object sender, EventArgs e)
		{
			InvokeNavigate(MatchNavigationMode.PrevUniqueMatch);
		}

		private void btnNextUniqueMatch_Click(object sender, EventArgs e)
		{
			InvokeNavigate(MatchNavigationMode.NextUniqueMatch);
		}

		#endregion

		#region Events: statistics

		private void btnCalculate_Click(object sender, EventArgs e)
		{
			try
			{
				CalculateStatistics();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnStatisticsPrevMatch_Click(object sender, EventArgs e)
		{
			try
			{
				NavigateStatistics(false);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnStatisticsNextMatch_Click(object sender, EventArgs e)
		{
			try
			{
				NavigateStatistics(true);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridStatistics_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			try
			{
				GetCriteria(gridCriteriaExceptions, _regExp.Exceptions);

				var item = (StatisticsModel) gridStatistics.Rows[e.RowIndex].DataBoundItem;

				e.Value = _statistics.GetExceptionValue(item.Word, _regExp.Exceptions);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

        private void GridStatistics_DataSourceChanged(object sender, System.EventArgs e)
        {
            if (gridStatistics.SelectedRows.Count == 0)
            {
                this.btnSimilar.Enabled = false;
            } else
            {
                this.btnSimilar.Enabled = true;
            }
        }

        private void GridStatistics_SelectionChanged(object sender, System.EventArgs e)
        {
            if (gridStatistics.SelectedRows.Count == 1 )
            {
                this.btnSimilar.Enabled = true;
            }
            else
            {
                this.btnSimilar.Enabled = false;
            }
        }

        private void gridStatistics_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{            
			try
			{
				var item = (StatisticsModel) gridStatistics.Rows[e.RowIndex].DataBoundItem;

				_statistics.SetExceptionValue(item.Word, _regExp.Exceptions, (bool) e.Value);

				FillCriteriaGrid(gridCriteriaExceptions, _regExp.Exceptions);

				Save(true);

				gridStatistics.Refresh();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridStatistics_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{   
			try
			{
				gridStatistics.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation

        public List<Tuple<string, string>> GetSimilarWordsList(string word)
        {
            _distanceProcessOutput = "";
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();

            try
            {
                var commandPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                                             .Location), "word2vec");
                Directory.SetCurrentDirectory(commandPath);

                var arguments = String.Format(@"vectors.bin");

                Process distanceProcess = new Process
                {
                    StartInfo = new ProcessStartInfo("distance.exe", arguments)
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                    },
                    EnableRaisingEvents = true
                };

                // Set our event handler to asynchronously read the sort output.
                distanceProcess.OutputDataReceived += new DataReceivedEventHandler(SimilarProcessOutputHandler);

                distanceProcess.Exited += SimilarProcessExited;


                // Redirect standard input as well.  This stream
                // is used synchronously.
                distanceProcess.StartInfo.RedirectStandardInput = true;
                distanceProcess.Start();

                // Use a stream writer to synchronously write the sort input.
                StreamWriter sortStreamWriter = distanceProcess.StandardInput;

                // Start the asynchronous read of the sort output stream.
                distanceProcess.BeginOutputReadLine();

                sortStreamWriter.WriteLine(word);

                sortStreamWriter.WriteLine("EXIT");

                distanceProcess.WaitForExit();

                bool startWord = false;
                string[] strLines = _distanceProcessOutput.Split('\n');
                foreach (string line in strLines)
                {
                    if (startWord)
                    {
                        string[] similar = line.Split(' ', '\t').Where(x => x != "").ToArray();
                        if (similar.Count() == 2)
                            result.Add(new Tuple<string, string>(similar[0], similar[1]));
                    }
                    if (line == "------------------------------------------------------------------------")
                        startWord = true;
                }
            } catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);                
            }            

            return result;
        }

        public void AddNewRegExp(string word)
        {
            try
            {
                _views.MainForm.sourceRegExp.RaiseListChangedEvents = false;

                ///////////////////////////////////////////////////////////////////////////////

                var guid = Guid.NewGuid()
                               .ToString();

                var count = _views.MainForm.adapterRegExp.Insert(word, null, null, null, null, null, null, null, null, null, null, guid, null, null);
                if (count != 1)
                    throw new Exception("Failed to insert RegExp");

                
                _views.MainForm.adapterRegExp.Update(_views.MainForm.datasetMain.RegExp);
                _views.MainForm.adapterRegExp.Fill(_views.MainForm.datasetMain.RegExp);

                ///////////////////////////////////////////////////////////////////////////////
                
                
             
                _views.MainForm.sourceRegExp.RaiseListChangedEvents = true;
                _views.MainForm.sourceRegExp.ResetBindings(false);
             
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrEmpty(ex.Message) && ex.Message.Contains("RegExp.RegExp"))
                {
                    var message = "Please check in RegExp->DesignView->Field RegExp->Allow zero length = YES"
                        + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    MessageBox.Show(message, MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                MainForm.ShowExceptionMessage(ex);
            }
        }

        public void AddNewColRegExp(string word)
        {
            var columnID = ((ColRegExp)_regExp).ColumnID;

            try
            {
                if (columnID == 0)
                {
                    MessageBox.Show("No tab selected", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                ///////////////////////////////////////////////////////////////////////////////

                
                var guid = Guid.NewGuid()
                               .ToString();

                var count = _views.MainForm.adapterColRegExp.Insert(guid, word, null, null, null, null, null, null, null, null, columnID, null);
                if (count != 1)
                    throw new Exception("Failed to insert Column RegExp");

                ///////////////////////////////////////////////////////////////////////////////

                _views.MainForm.adapterColRegExp.Update(_views.MainForm.datasetMain.ColRegExp);
                _views.MainForm.adapterColRegExp.Fill(_views.MainForm.datasetMain.ColRegExp);

                ///////////////////////////////////////////////////////////////////////////////
                _views.MainForm.sourceColRegExp.RaiseListChangedEvents = true;
                _views.MainForm.sourceColRegExp.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);
            }
        }

        public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == 0x20a)
			{
				// WM_MOUSEWHEEL, find the control at screen position m.LParam
				var pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);

				var hWnd = WindowFromPoint(pos);
				if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
				{
					SendMessage(hWnd, (uint) m.Msg, m.WParam, m.LParam);
					return true;
				}
			}

			return false;
		}

		protected void UpdateButtonTitles()
		{
			btnShowExamples.Text = splitterRegExpAndExamples.Panel2Collapsed ? "     Show Examples" : "     Hide Examples";
			btnShowToolbox.Text = splitterMain.Panel2Collapsed ? "     Show Toolbox" : "     Hide Toolbox";

			lvQuickActions.UpdateHeaderWidth();
		}

		protected void UpdateQuickActionsColumnWidth()
		{
			var width = lvQuickActions.GetQuickActionsMaximumTextWidth();

			splitterMain.SplitterDistance = splitterMain.ClientSize.Width - (width + splitterMain.SplitterWidth + 20);
		}

		protected void InvokeNavigate(MatchNavigationMode mode)
		{
			try
			{
				if (_isDirty)
				{
					if (!Save(true))
						return;
				}

				var formProgress = new FormGenericProgress("Navigating...", DoNavigate, mode, true)
				                   {
					                   CancellationEnabled = true
				                   };

				formProgress.ShowDialog();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected bool DoNavigate(BackgroundWorker worker, object objArgument)
		{
			try
			{
				var mode = (MatchNavigationMode) objArgument;

				_views.InvokeOnMatchNavigate(mode, _regExp, worker);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			return true;
		}

		protected void CalculateStatistics()
		{
			SaveRegExpToDatabase();

			///////////////////////////////////////////////////////////////////////////////

			var param = new RegExpStatisticsSingleProcessingParams
			            {
				            RegExpID = _regExp.ID,
				            ExcludeLookArounds = chkbExcludeLookArounds.Checked
			            };

			///////////////////////////////////////////////////////////////////////////////

			var formProgress = new FormGenericProgress("Calculating statistics...", DoCalculateStatistics, param, true);
			formProgress.ShowDialog();
			if (formProgress.Result)
				gridStatistics.DataSource = _statistics.Statistics;
			else
				gridStatistics.DataSource = null;

			gridStatistics.ResetBindings();
		}

		protected bool DoCalculateStatistics(BackgroundWorker worker, object objArgument)
		{
			try
			{
				var param = (RegExpStatisticsSingleProcessingParams) objArgument;

				using (var processor = new ExternalRegExpToolWrapper(worker, true))
				{
					var results = processor.RegExp_CalcStatisticsSingle(_views, _regExp is ColRegExp, param);

					///////////////////////////////////////////////////////////////////////////////

					_statistics.SetResults(results, _views.MainForm.datasetMain.Documents.Count);

					return true;
				}
			}
			catch (Exception ex)
			{
				_statistics.SetResults(null, 0);

				MainForm.ShowExceptionMessage(ex);
			}

			return false;
		}

		protected void NavigateStatistics(bool forward)
		{
			var item = _statistics.Navigate(forward);
			if (item == null)
				return;

			///////////////////////////////////////////////////////////////////////////////

			var position = _views.MainForm.sourceDocuments.Find("ED_ENC_NUM", item.DocumentID);
			if (position != -1)
				_views.InvokeOnMatchNavigate(new RegExpMatchResult(_regExp, position, item.Start, item.Length));
		}

		protected void RaiseModifiedEvent(RegExpBase regExp)
		{
			if (this.Modified != null)
				this.Modified(regExp, EventArgs.Empty);
		}

		#endregion

		#region Examples

		protected void FillExamplesGrid()
		{
			List<RegExpEditExample> examplesList = LoadExampleData();
			dataGridViewExamples.Rows.Clear();

			foreach (RegExpEditExample value in examplesList)
			{
				dataGridViewExamples.Rows.Add(value.Description, value.RegExp);
			}
		}

		public List<RegExpEditExample> GetExamplesValue()
		{
			List<RegExpEditExample> result = new List<RegExpEditExample>();

			foreach (DataGridViewRow row in dataGridViewExamples.Rows)
			{
				var description = (string) row.Cells[0].Value;
				var regExp = (string) row.Cells[1].Value;
				if (!String.IsNullOrEmpty(description))
				{
					result.Add(new RegExpEditExample()
					           {
						           Description = description,
						           RegExp = regExp
					           });
				}
			}

			return result;
		}

		private void dataGridViewExamples_SelectionChanged(object sender, EventArgs e)
		{
			if (dataGridViewExamples == null)
				return;
			if (dataGridViewExamples.CurrentRow.Selected)
			{
				var regExp = dataGridViewExamples.CurrentRow.Cells[1].Value.ToString();

				var text = txtRegExp.Text.Insert(selectionStartPosition, regExp);
				txtRegExp.Text = text;
			}
		}

		#endregion

		#region Methods

		public void UpdateRegExpView()
		{
			if (_preventUpdate)
				return;

			_preventUpdate = true;

			_isDirty = false;

			_regExp = null;

			///////////////////////////////////////////////////////////////////////////////

			try
			{
				var safeLoopCounter = 20;

				while (_regExp == null)
				{
					try
					{
						if (_source.Position == -1)
							break;

						var rowView = _source.Current as DataRowView;
						if (rowView != null)
						{
							_regExp = _funcCreateRegExp(rowView);
							if (_regExp == null)
								break;
						}
						else
							break;
					}
					catch
					{
						if (_source.Current != null)
						{
							var row = ((DataRowView) _source.Current).Row;

							var dlgres = MessageBox.Show(String.Format("Failed to build regular expression '{0}'{1}{1}Do you wish to manually edit the row?", RegExpBase.GetRegExpValue(row), Environment.NewLine), MainForm.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
							if (dlgres == DialogResult.Yes)
							{
								var manualEditForm = new FormManualEditRegExp(_views, row);

								_preventUpdate = true;
								dlgres = manualEditForm.ShowDialog();
								_preventUpdate = false;

								if (dlgres != DialogResult.OK)
									break;
							}
							else
								break;
						}
					}

					///////////////////////////////////////////////////////////////////////////////

					safeLoopCounter--;
					if (safeLoopCounter <= 0)
						break;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			///////////////////////////////////////////////////////////////////////////////

			if (_regExp == null)
			{
				_regExp = new RegExp();

				try
				{
					this.Enabled = false;
				}
				catch
				{
				}
			}
			else
				this.Enabled = true;

			///////////////////////////////////////////////////////////////////////////////

			

            if (txtRegExp.Text != _regExp.Expression)
            {
                _statistics.SetResults(null, 0);

                gridStatistics.DataSource = _statistics.Statistics;
            }
            txtRegExp.Text = _regExp.Expression;

            if (String.IsNullOrEmpty(_regExp.Description))
				lblDescription.Text = string.Empty;
			else
				lblDescription.Text = _regExp.Description.Replace(Environment.NewLine, " ");

			UpdateCategoryText();

			UpdateCriteriaIndicators();

			_preventUpdate = false;

			UpdateCriteria(tabControlEditRegEx.SelectedTab);
		}

		protected void UpdateCategoryText()
		{
			var categoryName = String.Empty;

			if (_regExp.Category != null)
			{
				var categoryRow = _views.MainForm.datasetMain.Categories.First(x => x.ID == _regExp.Category.Value);
				if (categoryRow != null)
					categoryName = categoryRow.Category;
			}

			txtCategory.Text = categoryName;
		}

		public bool Save(bool showError)
		{
			if (tabControlEditRegEx.SelectedTab == tabPageEditor)
				return SaveRegExp(showError);

			///////////////////////////////////////////////////////////////////////////////

			if (tabControlEditRegEx.SelectedTab == tabPageLookahead)
				return SaveCriteria(gridCriteriaLookahead, showError) && SaveCriteria(gridCriteriaNegativeLookahead, showError);

			///////////////////////////////////////////////////////////////////////////////

			if (tabControlEditRegEx.SelectedTab == tabPageLookbehind)
				return SaveCriteria(gridCriteriaLookbehind, showError) && SaveCriteria(gridCriteriaNegativeLookbehind, showError);

			///////////////////////////////////////////////////////////////////////////////

			return SaveCriteria(gridCriteriaExceptions, showError);
		}

		protected void UpdateCriteria(TabPage selectedTab)
		{
			if (_preventUpdate)
				return;

			_preventUpdate = true;

			try
			{
				if (tabControlEditRegEx.SelectedTab == tabPageLookahead)
				{
					FillCriteriaGrid(gridCriteriaLookahead, _regExp.LookAhead);
					FillCriteriaGrid(gridCriteriaNegativeLookahead, _regExp.NegLookAhead);

					radioButtonLookahead.Checked = _regExp.LookAhead.Enabled;
					radioButtonNegativeLookahead.Checked = _regExp.NegLookAhead.Enabled;
				}
				else if (tabControlEditRegEx.SelectedTab == tabPageLookbehind)
				{
					FillCriteriaGrid(gridCriteriaLookbehind, _regExp.LookBehind);
					FillCriteriaGrid(gridCriteriaNegativeLookbehind, _regExp.NegLookBehind);

					radioButtonLookbehind.Checked = _regExp.LookBehind.Enabled;
					radioButtonNegativeLookbehind.Checked = _regExp.NegLookBehind.Enabled;
				}
				else if (tabControlEditRegEx.SelectedTab == tabPageExceptions)
					FillCriteriaGrid(gridCriteriaExceptions, _regExp.Exceptions);
			}
			catch (Exception ex)
			{
				MainForm.ShowErrorToolTip(ex.Message);
			}
			finally
			{
				_preventUpdate = false;
			}
		}

		protected void SaveRegExpToDatabase()
		{
			SaveRegExp(true);

			///////////////////////////////////////////////////////////////////////////////

			if (_regExp is ColRegExp)
			{
				var row = _views.MainForm.datasetMain.ColRegExp.FindByID(_regExp.ID);

				_views.MainForm.adapterColRegExp.Update(row);
				row.AcceptChanges();
			}
			else
			{
				var row = _views.MainForm.datasetMain.RegExp.FindByID(_regExp.ID);

				_views.MainForm.adapterRegExp.Update(row);
				row.AcceptChanges();
			}
		}

		protected bool SaveRegExp(bool showError)
		{
			if (_regExp == null)
				return false;

			///////////////////////////////////////////////////////////////////////////////

			_preventUpdate = true;

			try
			{
				try
				{
					_regExp.SetExpression(txtRegExp.Text);
				}
				catch (Exception ex)
				{
					if (showError)
						MainForm.ShowErrorToolTip(ex.Message);
				}

				_source.RaiseListChangedEvents = false;

				if (!_regExp.SafeSave(_source.Current as DataRowView, showError))
					return false;

				_source.RaiseListChangedEvents = true;
				//_source.ResetBindings(false);

				RaiseModifiedEvent(_regExp);

				_isDirty = false;

				return true;
			}
			catch (Exception ex)
			{
				if (showError)
					MainForm.ShowErrorToolTip(ex.Message);
			}
			finally
			{
				_preventUpdate = false;
			}

			return false;
		}

		protected bool SaveCriteria(DataGridView grid, bool showError)
		{
			try
			{
				if (_preventUpdate)
					return false;

				if (_regExp == null)
					return false;

				///////////////////////////////////////////////////////////////////////////////

				if (grid == gridCriteriaLookahead)
				{
					GetCriteria(grid, _regExp.LookAhead);

					_regExp.LookAhead.Enabled = radioButtonLookahead.Checked;
					_regExp.NegLookAhead.Enabled = radioButtonNegativeLookahead.Checked;
				}
				else if (grid == gridCriteriaNegativeLookahead)
				{
					GetCriteria(grid, _regExp.NegLookAhead);

					_regExp.LookAhead.Enabled = radioButtonLookahead.Checked;
					_regExp.NegLookAhead.Enabled = radioButtonNegativeLookahead.Checked;
				}
				else if (grid == gridCriteriaLookbehind)
				{
					GetCriteria(grid, _regExp.LookBehind);

					_regExp.LookBehind.Enabled = radioButtonLookbehind.Checked;
					_regExp.NegLookBehind.Enabled = radioButtonNegativeLookbehind.Checked;
				}
				else if (grid == gridCriteriaNegativeLookbehind)
				{
					GetCriteria(grid, _regExp.NegLookBehind);

					_regExp.LookBehind.Enabled = radioButtonLookbehind.Checked;
					_regExp.NegLookBehind.Enabled = radioButtonNegativeLookbehind.Checked;
				}
				else
					GetCriteria(gridCriteriaExceptions, _regExp.Exceptions);

				///////////////////////////////////////////////////////////////////////////////

				_preventUpdate = true;

				_regExp.SafeSave(_source.Current as DataRowView, false);

				RaiseModifiedEvent(_regExp);

				_isDirty = false;

				return true;
			}
			catch (Exception ex)
			{
				if (showError)
					MainForm.ShowErrorToolTip(ex.Message);
			}
			finally
			{
				_preventUpdate = false;
			}

			return false;
		}

		protected void GetCriteria(DataGridView grid, RegExpCriteriaCollection criteria)
		{
			criteria.Items.Clear();

			foreach (DataGridViewRow row in grid.Rows)
			{
				try
				{
					if (row.IsNewRow)
						continue;

					var enabled = row.Cells[0].Value;

					criteria.Items.Add(new RegExpCriteria
					                   {
						                   Enabled = enabled as bool? ?? false,
						                   Expression = (string) row.Cells[1].Value
					                   });
				}
				catch
				{
				}
			}
		}

		protected void FillCriteriaGrid(DataGridView grid, RegExpCriteriaCollection criteria)
		{
			_preventUpdate = true;

			grid.Rows.Clear();

			criteria.Items.ForEach(x => grid.Rows.Add(x.Enabled, x.Expression));

			UpdateCriteriaIndicators();

			_preventUpdate = false;
		}

		protected void UpdateCriteriaIndicators()
		{
			tabPageLookahead.Tag = _regExp.LookAhead.Items.Any() || _regExp.NegLookAhead.Items.Any() ? tabPageLookahead : null;
			tabPageLookbehind.Tag = _regExp.LookBehind.Items.Any() || _regExp.NegLookBehind.Items.Any() ? tabPageLookbehind : null;
			tabPageExceptions.Tag = _regExp.Exceptions.Items.Any() ? tabPageExceptions : null;

			tabControlEditRegEx.Invalidate();
			tabControlEditRegEx.Update();
		}

		protected void AddCriteria(DataGridView grid)
		{
			var index = grid.Rows.Add(true);
			var row = grid.Rows[index];

			row.Selected = true;
			grid.CurrentCell = row.Cells[1];

			SaveCriteria(grid, false);

			UpdateCriteriaIndicators();

			_views.InvokeRefreshHighlights();
		}

		private void DeleteCriteria(DataGridView grid)
		{
			if (_regExp == null)
				return;

			foreach (DataGridViewRow row in grid.SelectedRows)
			{
				try
				{
					grid.Rows.Remove(row);
				}
				catch
				{
				}
			}

			SaveCriteria(grid, false);

			UpdateCriteriaIndicators();

			_views.InvokeRefreshHighlights();
		}

		private List<RegExpEditExample> LoadExampleData()
		{
			var connectionString = _views.MainForm.GetConnectionString(_connectionString);
			var queryString = "SELECT [Value] FROM DBSettings WHERE Key = @RegExpExamples";

			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{
				connection.Open();
				try
				{
					OleDbCommand command = new OleDbCommand(queryString, connection);
					command.Parameters.AddWithValue("RegExpExamples", "RegExpExamples");
					var result = command.ExecuteScalar();

					var returnResult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RegExpEditExample>>(result.ToString());
					if (returnResult != null)
						return returnResult;
					else
						return new List<RegExpEditExample>();
				}
				catch
				{
					try
					{
						OleDbCommand command = new OleDbCommand("INSERT INTO DBSettings ([Key], [Value]) VALUES (@RegExpExamples, @Value)", connection);
						command.Parameters.AddWithValue("RegExpExamples", "RegExpExamples");
						command.Parameters.AddWithValue("Value", "");
						command.ExecuteNonQuery();
					}
					catch
					{
					}

					return new List<RegExpEditExample>();
				}
			}
		}

		private void SaveExampleData()
		{
			var json = GetExamplesValue();
			var exampleJson = Newtonsoft.Json.JsonConvert.SerializeObject(json);

			var connectionString = _views.MainForm.GetConnectionString(_connectionString);
			var queryString = "UPDATE DBSettings SET [Value] = @exampleJson WHERE [Key] = @RegExpExamples";

			try
			{
				using (OleDbConnection connection = new OleDbConnection(connectionString))
				{
					connection.Open();
					OleDbCommand command = new OleDbCommand(queryString, connection);
					command.Parameters.AddWithValue("exampleJson", exampleJson);
					command.Parameters.AddWithValue("RegExpExamples", "RegExpExamples");
					var result = command.ExecuteNonQuery();
				}
			}
			catch
			{
			}
		}

		protected void SaveSettings()
		{
			Settings.Default.FormRegExpEditorShowExamples = !splitterRegExpAndExamples.Panel2Collapsed;
			Settings.Default.FormRegExpEditorExamplesPaneHeight = splitterRegExpAndExamples.SplitterDistance;

			Settings.Default.FormRegExpEditorShowQuickActions = !splitterMain.Panel2Collapsed;
			Settings.Default.FormRegExpEditorToolboxPaneWidth = splitterMain.SplitterDistance;

			Settings.Default.Save();
		}

		//private void CheckIfTextIsSelected()
		//{
		//    //Disable all buttons
		//    btnEitherOr.Enabled = false;
		//    btnOptionalOnceOrNone.Enabled = false;
		//    btnOneOrMoreTimes.Enabled = false;
		//    btnRange.Enabled = false;
		//    btnZeroOrMultipleTimes.Enabled = false;

		//    if (txtRegExp.SelectedText != "")
		//    {
		//        var count = txtRegExp.SelectionLength;

		//        if (count > 0)
		//        {
		//            //Enable Optional, One or more times, Zero or more times
		//            btnOptionalOnceOrNone.Enabled = true;
		//            btnOneOrMoreTimes.Enabled = true;
		//            btnZeroOrMultipleTimes.Enabled = true;
		//        }
		//        if (count == 2)
		//        {
		//            //Enable Range
		//            btnRange.Enabled = true;
		//        }
		//        if (count > 1)
		//        {
		//            //Enable Either or
		//            btnEitherOr.Enabled = true;
		//        }
		//    }
		//}

		#endregion

		#region Interop

		[DllImport("user32.dll")]
		private static extern IntPtr WindowFromPoint(Point pt);

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        #endregion

    }

    public class StatisticsWrapper
	{
		#region Fields

		protected List<RegExpStatisticsSingleProcessingResult> _rawResults;
		protected List<StatisticsModel> _aggregatedStatistics;

		protected BindingSource _source;

		protected int _currentItem;
		protected int _currentRawItem;

		#endregion

		#region Properties

		public BindingSource Statistics
		{
			get
			{
				if (_source == null)
				{
					if (_aggregatedStatistics == null)
						return null;

					_source = new BindingSource(_aggregatedStatistics, "");
				}

				return _source;
			}
		}

		#endregion

		#region Ctors

		public StatisticsWrapper()
		{
			_currentItem = -1;
		}

		#endregion

		#region Operations

		public void SetResults(RegExpProcessingResultsCollection<RegExpStatisticsSingleProcessingResult> rawResults, long totalDocsCount)
		{
			_source = null;

			if (rawResults != null)
			{
				_rawResults = rawResults.Items.OrderBy(x => x.DocumentID)
				                        .ThenBy(x => x.Start)
				                        .ToList();
				_aggregatedStatistics = AggregateStatistics(totalDocsCount);
			}
			else
			{
				_rawResults = null;
				_aggregatedStatistics = null;
			}

			_currentRawItem = 0;
			_currentItem = -1;
		}

		public void SortAggregatedResults(string propertyName, bool ascending)
		{
			if (_aggregatedStatistics == null)
				return;

			var type = typeof (StatisticsModel);
			var pi = type.GetProperty(propertyName);

			if (ascending)
			{
				_aggregatedStatistics = _aggregatedStatistics.OrderBy(x => pi.GetValue(x))
				                                             .ToList();
			}
			else
			{
				_aggregatedStatistics = _aggregatedStatistics.OrderByDescending(x => pi.GetValue(x))
				                                             .ToList();
			}

			_source = null;
		}

		public bool GetExceptionValue(string word, RegExpCriteriaCollection criteria)
		{
			if (String.IsNullOrEmpty(word))
				return false;

			return criteria.Items.Any(x => String.Compare(x.Expression, word, StringComparison.InvariantCultureIgnoreCase) == 0 && x.Enabled);
		}

		public void SetExceptionValue(string word, RegExpCriteriaCollection criteria, bool value)
		{
			if (value)
			{
				var exception = criteria.Items.FirstOrDefault(x => String.Compare(x.Expression, word, StringComparison.InvariantCultureIgnoreCase) == 0);
				if (exception == null)
				{
					criteria.Items.Add(new RegExpCriteria
					                   {
						                   Enabled = true,
						                   Expression = word
					                   });
				}
				else
					exception.Enabled = true;
			}
			else
			{
				var list = criteria.Items.Where(x => String.Compare(x.Expression, word, StringComparison.InvariantCultureIgnoreCase) == 0);
				foreach (var exception in list)
				{
					exception.Enabled = false;
				}
			}
		}

		public RegExpStatisticsSingleProcessingResult Navigate(bool forward)
		{
			if (_rawResults == null || _rawResults.Count == 0)
				return null;

			///////////////////////////////////////////////////////////////////////////////

			var source = this.Statistics;
			if (source == null)
				return null;

			///////////////////////////////////////////////////////////////////////////////

			if (_currentItem != source.Position)
			{
				if (forward)
					_currentRawItem = -1;
				else
					_currentRawItem = _rawResults.Count;

				_currentItem = source.Position;
			}

			var model = (StatisticsModel) source.Current;

			///////////////////////////////////////////////////////////////////////////////

			var index = _currentRawItem;

			var increment = forward ? 1 : -1;
			_currentRawItem += increment;

			///////////////////////////////////////////////////////////////////////////////

			while (_currentRawItem >= 0 && _currentRawItem < _rawResults.Count)
			{
				var item = _rawResults[_currentRawItem];
				if (String.Compare(item.Word, model.Word, StringComparison.InvariantCulture) == 0)
					return item;

				_currentRawItem += increment;
			}

			_currentRawItem = index;

			return null;
		}

		#endregion

		#region Implementation

		protected List<StatisticsModel> AggregateStatistics(float totalDocsCount)
		{
			var aggregatedStatistics = _rawResults.GroupBy(x => x.Word)
			                                      .Select(g => new StatisticsModel
			                                                   {
				                                                   Word = g.Key,
				                                                   MatchingDocsCount = g.GroupBy(x => x.DocumentID * 100 + x.ColumnID)
				                                                                        .Count(),
				                                                   MatchesCount = g.Count(),
                                                                   MatchingRecordsCount = g.GroupBy(x => x.DocumentID).Count(),
                                                  })
			                                      .ToList();

			///////////////////////////////////////////////////////////////////////////////

			float totalMatchesCount = 0;
			float totalMatchingDocumentsCount = 0;
            float totalMatchingRecordsCount = 0;

			aggregatedStatistics.ForEach(x =>
			                             {
				                             totalMatchesCount += x.MatchesCount;
				                             totalMatchingDocumentsCount += x.MatchingDocsCount;
                                             totalMatchingRecordsCount += x.MatchingRecordsCount;
			                             });

			foreach (var item in aggregatedStatistics)
			{
				item.MatchesCountPercentage = item.MatchesCount / totalMatchesCount * 100f;
				item.MatchingDocsPercentage = item.MatchingDocsCount / totalMatchingDocumentsCount * 100f;
                //item.AllDocsPercentage = item.MatchingDocsCount / totalDocsCount * 100f;                
                item.AllDocsPercentage = item.MatchingRecordsCount / totalDocsCount * 100f;
            }

			return aggregatedStatistics;
		}

		#endregion
	}

	public class StatisticsModel
	{
		#region Fields

		public string Word { get; set; }
		public int MatchesCount { get; set; }
		public float MatchesCountPercentage { get; set; }
		public int MatchingDocsCount { get; set; }
		public float MatchingDocsPercentage { get; set; }
		public float AllDocsPercentage { get; set; }
        public int MatchingRecordsCount { get; set; }

        #endregion
    }

	public class RegExpEditExample
	{
		#region Fields

		public string Description { get; set; }
		public string RegExp { get; set; }

		#endregion
	}
}