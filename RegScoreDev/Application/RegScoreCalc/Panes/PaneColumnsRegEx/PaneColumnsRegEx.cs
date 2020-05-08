using System;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using RegScoreCalc.Forms;

using Helpers;

using RegExpLib.Core;


using RegScoreCalc.Code;

using Cursor = System.Windows.Forms.Cursor;
using ScriptFunctionLibrary;
using System.Collections.Generic;
using DocumentsServiceInterfaceLib;

namespace RegScoreCalc
{
	public partial class PaneColumnsRegEx : Pane
	{
		#region Delegates

		public event Action<int> ExtractFinished;

		#endregion

		#region Fields

		CSScriptManager manager = new CSScriptManager();

		private DataGridViewTextBoxColumn regExpDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn extractExpDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn numberExpDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn docsDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn recordsDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn posDocsDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn percentagePosDocsDataGridViewTextBoxColumn;
		private MatchColumn matchDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn regExpColorDataGridViewTextBoxColumn;

		protected DataRowView _currentRow;

		protected int _nRegExpRow;
		protected int _nMatchIndex;

		protected FormRegularExpressionEditor _editor;

		public int _previousSelectedColumnID;

		private RibbonButton btnPositiveScore;

		protected bool _preventUpdate;

		protected ColumnSizeContextMenuWrapper _columnsContextMenu;

        //protected List<TabSetting> _tabSettings;

		#endregion

		#region Properties

		public int CurrentSelectedColumnID
		{
			get
			{
				if (flowLayoutPanel.Tag == null)
					return 0;

				var columnButton = (RibbonStyleButton) flowLayoutPanel.Tag;
				if (columnButton.Tag == null)
					return 0;

				///////////////////////////////////////////////////////////////////////////////

				var columnID = (int) columnButton.Tag;
				return columnID;
			}
			set
			{
				var buttons = flowLayoutPanel.Controls.Cast<RibbonStyleButton>().ToList();

				var currentButton = buttons.FirstOrDefault(x => (int) x.Tag == value);
				if (currentButton != null)
				{
					currentButton.IsHighlighted = true;
					currentButton.Refresh();
				}

				foreach (var btn in buttons.Where(x => x != currentButton))
				{
					btn.IsHighlighted = false;
					btn.Refresh();
				}

				flowLayoutPanel.Tag = currentButton;
			}
		}

		#endregion

		#region Ctors

		public PaneColumnsRegEx(ViewsManager views)
		{
			InitializeComponent();

			toolStripTop.Renderer = new CustomToolStripRenderer { RoundedEdges = false };

			_views = views;
			
			panelButtons.BackColor = MainForm.ColorBackground;

			_previousSelectedColumnID = 0;

			menuitemEditRegExp.Click += new EventHandler(menuitemEditRegExp_Clicked);
			menuitemDeleteRows.Click += new EventHandler(menuitemDeleteRows_Clicked);

			if (_views.MainForm.RegexpToGroupsTableExist())
			{
				//Separator
				ToolStripSeparator separator = new ToolStripSeparator();
				menuRegExp.Items.Add(separator);

				//Add menu Item to "assign to group"
				ToolStripMenuItem menuitemAssignToGroup = new ToolStripMenuItem("Assign to Group");
				menuitemAssignToGroup.Click += menuitemAssignToGroup_Click;
				menuRegExp.Items.Add(menuitemAssignToGroup);
			}

			///////////////////////////////////////////////////////////////////////////////

			_columnsContextMenu = new ColumnSizeContextMenuWrapper(_views, _ownerView, regExpDataGridView);

			flowLayoutPanel.SizeChanged += flowLayoutPanel_SizeChanged;
		}

        #endregion

        #region Events

        private void CheckBoxExtractValues_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxExtractValues.Checked == true)
                checkBoxExtractPython.Checked = false;
        }
        private void CheckBoxExtractPython_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxExtractPython.Checked == true)
                checkBoxExtractValues.Checked = false;
        }

        private void OnCalculate_Click(object sender, EventArgs e)
		{
			try
			{
				CalcScores();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void OnExtract_Click(object sender, EventArgs e)
		{
			try
			{
				Extract();

				InvokeEvent_ExtractFinished();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void OnExtractAll_Click(object sender, EventArgs e)
		{
			try
			{
				ExtractAll();

				InvokeEvent_ExtractFinished();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void OnAddRegExp_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.CurrentSelectedColumnID == 0)
				{
					MessageBox.Show("No tab selected", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

				///////////////////////////////////////////////////////////////////////////////

				var bindingSource = GetBindingSource();
				bindingSource.RaiseListChangedEvents = false;

				regExpDataGridView.EndEdit(DataGridViewDataErrorContexts.Commit);
				regExpDataGridView.CurrentCell = null;
				this.Validate();

				///////////////////////////////////////////////////////////////////////////////

				var guid = Guid.NewGuid()
							   .ToString();

				var columnID = this.CurrentSelectedColumnID;

				var count = _views.MainForm.adapterColRegExp.Insert(guid, "", null, null, null, null, null, null, null, null, columnID, null);
				if (count != 1)
					throw new Exception("Failed to insert RegExp");

				var id = MainForm.GetLastInsertedID(_views.MainForm.adapterColRegExp.Connection);

				///////////////////////////////////////////////////////////////////////////////

				_views.MainForm.adapterColRegExp.Update(_views.MainForm.datasetMain.ColRegExp);
				_views.MainForm.adapterColRegExp.Fill(_views.MainForm.datasetMain.ColRegExp);

				///////////////////////////////////////////////////////////////////////////////

				bindingSource.RaiseListChangedEvents = true;
				bindingSource.ResetBindings(false);

				///////////////////////////////////////////////////////////////////////////////

				var position = bindingSource.Find("ID", id);
				if (position != -1)
				{
					bindingSource.Position = position;

					if (regExpDataGridView.CurrentCell != null)
					{
						var newRow = regExpDataGridView.CurrentCell.OwningRow;

						regExpDataGridView.ClearSelection();

						newRow.Selected = true;
						regExpDataGridView.BeginEdit(true);
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void OnPositiveScoreOnClick(object sender, EventArgs eventArgs)
		{
			try
			{
				var ribbonButton = (RibbonButton)sender;
				_views.OnlyPositiveScores = ribbonButton.Checked;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void regExpDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
				return;

			try
			{
				if (regExpDataGridView.Rows.Count > 0)
				{
					if (e.ColumnIndex == regExpColorDataGridViewTextBoxColumn.Index && e.RowIndex >= 0) // index of column with color
					{
						DataGridViewCell c = regExpDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
						ColorDialog cd = new ColorDialog();

						cd.Color = c.Style.BackColor;
						if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
						{
							c.Style.BackColor = cd.Color;
							c.Style.ForeColor = cd.Color;
							c.Style.SelectionBackColor = cd.Color;
							c.Style.SelectionForeColor = cd.Color;

							c.Value = cd.Color.ToArgb();

							RaiseDataModifiedEvent();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void regExpDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (regExpDataGridView.Rows.Count > 0)
			{
				
			}
		}

		private void regExpDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				//if (regExpDataGridView.Rows.Count > 0)
				//	RaiseDataModifiedEvent();
			}
			catch { }
		}

		private void regExpDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (regExpDataGridView.Rows.Count > 0)
			{
				if (e.ColumnIndex == regExpColorDataGridViewTextBoxColumn.Index && e.RowIndex >= 0) // index of column with color
				{
					DataGridViewCell c = regExpDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

					if (c.Value != null)
					{
						var color = RegExpBase.DefaultHighlightColor;
						int argbColor = 0;
						try
						{
							Int32.TryParse(c.Value.ToString(), out argbColor);
						}
						catch (Exception)
						{
							// do nothing e.g. DBNull
						}

						if (argbColor != 0)
							color = Color.FromArgb(argbColor);

						c.Style.BackColor = color;
						c.Style.ForeColor = color;
						c.Style.SelectionBackColor = color;
						c.Style.SelectionForeColor = color;
					}
				}
				else if (e.ColumnIndex == 0)
				{
					try
					{
						DataGridViewCell cell = regExpDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
						if (cell.Value != null && cell.Value != DBNull.Value)
						{
							string strValue = (string)cell.Value;
							cell.ToolTipText = strValue;
						}
						else
							cell.ToolTipText = "";
					}
					catch
					{
					}
				}
				else if (e.ColumnIndex == extractExpDataGridViewTextBoxColumn.Index)
				{
					try
					{
						DataGridViewCell cell = regExpDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

						if (cell.Tag != null && cell.Tag.ToString() == "True")
						{
							cell.Style.ForeColor = Color.Green;
							cell.Style.SelectionForeColor = Color.Green;
							cell.Style.BackColor = Color.Green;
							cell.Style.SelectionBackColor = Color.Green;
						}
						else
						{
							cell.Style.ForeColor = Color.White;
							cell.Style.SelectionForeColor = Color.White;
							cell.Style.BackColor = Color.White;
							cell.Style.SelectionBackColor = Color.White;
						}
					}
					catch
					{
					}
				}
			}
		}

		private void regExpDataGridView_MouseClick(object sender, MouseEventArgs e)
		{
			if (regExpDataGridView.Rows.Count > 0)
			{
				if (menuOperations.Visible)
					menuOperations.Hide();

				if (menuRegExp.Visible)
					menuRegExp.Hide();

				if (e.Button == MouseButtons.Right && !regExpDataGridView.IsCurrentCellInEditMode)
				{
					menuitemEditRegExp.Visible = false;
					menuitemCopyRegExp.Visible = false;
					menuitemDeleteRows.Visible = false;

					bool bShow = false;

					if (regExpDataGridView.CurrentCell != null)
					{
						if (IsStringColumn(regExpDataGridView.CurrentCell.ColumnIndex))
						{
							menuitemEditRegExp.Visible = true;
							menuitemCopyRegExp.Visible = true;

							bShow = true;
						}
					}

					if (regExpDataGridView.SelectedCells.Count > 0)
					{
						menuitemDeleteRows.Visible = true;
						bShow = true;
					}

					if (bShow)
						menuOperations.Show(Cursor.Position);
				}
			}
		}

		private void regExpDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			if (regExpDataGridView.Rows.Count > 0)
			{
				foreach (DataGridViewRow item in regExpDataGridView.Rows)
				{
					var cell = item.Cells["extractExpDataGridViewTextBoxColumn"];
					if (cell.Value != null)
					{
						var value = cell.Value.ToString();
						if (!String.IsNullOrEmpty(value))
						{
							//Check if value has Extract = true (faster than parsing from JSON to object)
							if (value.Contains("\"Extract\":true"))
								cell.Tag = "True";
							else
								cell.Tag = "False";
						}
					}
				}
			}
		}

		private void menuitemDeleteRows_Clicked(object sender, EventArgs e)
		{
			DeleteRows();
		}

		private void menuitemEditRegExp_Clicked(object sender, EventArgs e)
		{
			EditRegExp();
		}

		private void menuitemAssignToGroup_Click(object sender, EventArgs e)
		{
			try
			{
				if (regExpDataGridView.CurrentCell != null)
				{
					DataGridViewCell cell = regExpDataGridView.CurrentCell;
					if (IsStringColumn(cell.ColumnIndex))
					{
						regExpDataGridView.EndEdit(DataGridViewDataErrorContexts.Commit);

						var rowView = (DataRowView)cell.OwningRow.DataBoundItem;
						var row = (MainDataSet.RegExpRow)rowView.Row;

						FormAssignRegExpToGroup dialog = new FormAssignRegExpToGroup(_views, row.ID);
						dialog.ShowDialog();
					}
				}
			}
			catch
			{
			}
		}

		private void menuitemCopyRegExp_Click(object sender, EventArgs e)
		{
			try
			{
				if (regExpDataGridView.CurrentCell != null)
				{
					DataGridViewCell cell = regExpDataGridView.CurrentCell;
					if (cell.ColumnIndex == regExpDataGridViewTextBoxColumn.Index)
					{
						regExpDataGridView.EndEdit(DataGridViewDataErrorContexts.Commit);

						if (cell.Value != DBNull.Value)
						{
							DataRowView rowview = (DataRowView)cell.OwningRow.DataBoundItem;

							var regExp = RegExpFactory.Create_ColRegExp(rowview, null, true);

							Clipboard.SetText(regExp.BuiltExpression);
						}
					}
				}
			}
			catch
			{
			}
		}

		private void regExpDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Left)
				{

					if (Control.ModifierKeys == Keys.Control)
						EditRegExp();
					else
						regExpDataGridView.BeginEdit(true);
				}
			}
			catch { }
		}

		private void regExpDataGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
		{
			if (regExpDataGridView.Rows.Count > 0)
				RaiseDataModifiedEvent();
		}

		private void menuitemCopy_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewCell cell = (DataGridViewCell)menuRegExp.Tag;
				if (cell.Value != null)
				{
					string strValue = (string)cell.Value;

					Clipboard.SetText(strValue);
				}
			}
			catch
			{
			}
		}

		private void menuitemCut_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewCell cell = (DataGridViewCell)menuRegExp.Tag;
				if (cell.Value != null)
				{
					string strValue = (string)cell.Value;

					Clipboard.SetText(strValue);

					cell.Value = "";
				}
			}
			catch
			{
			}
		}

		private void menuitemPaste_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewCell cell = (DataGridViewCell)menuRegExp.Tag;
				if (cell.Value != null)
				{
					string strValue = Clipboard.GetText();

					cell.Value = strValue;
				}
			}
			catch
			{
			}
		}

		private void menuitemWordBoundaries_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewCell cell = (DataGridViewCell)menuRegExp.Tag;
				if (cell.Value != null)
				{
					string strValue = (string)cell.Value;

					cell.Value = @"\b" + strValue + @"\b";
				}
			}
			catch
			{
			}
		}

		private void regExpDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (regExpDataGridView.Rows.Count > 0 && e.RowIndex >= 0)
			{
				try
				{
					if (e.Button == MouseButtons.Right)
					{
						if (e.ColumnIndex == regExpDataGridViewTextBoxColumn.Index)
						{
							if (menuOperations.Visible)
								menuOperations.Hide();

							if (menuRegExp.Visible)
								menuRegExp.Hide();

							DataGridViewCell cell = regExpDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
							regExpDataGridView.CurrentCell = cell;

							menuRegExp.Tag = cell;

							menuRegExp.Show(Cursor.Position);
						}
					}
					else
					{
						if (e.ColumnIndex == extractExpDataGridViewTextBoxColumn.Index)
						{
							var dynamicColumnType = MainForm.GetDynamicColumnTypeByID(_views.MainForm.datasetMain.DynamicColumns, _previousSelectedColumnID);

							//Check if this is categorical cell and if yes don't open the FormExtractColRegExp
							if (dynamicColumnType != DynamicColumnType.Category)
							{
								DataGridViewCell c = regExpDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

								_previousSelectedColumnID = this.CurrentSelectedColumnID;

								var json = c.Value != null ? c.Value.ToString() : "";

                                List<TabSetting> columnSettings = _views.DocumentsService.GetDocumentColumnSettings();
                                Dictionary<string, int> noteColumn = new Dictionary<string, int>();
                                foreach (var tab in columnSettings)
                                {
                                    if (tab.ColumnName.StartsWith("NOTE_TEXT"))
                                    {
                                        noteColumn.Add(tab.DisplayName, tab.Index);                                        
                                    }
                                }

								FormExtractColRegExp formExtractColRegExp = new FormExtractColRegExp(dynamicColumnType, json, noteColumn);
								if (formExtractColRegExp.ShowDialog() == DialogResult.OK)
								{
									var extract = formExtractColRegExp.extract;
									var jsonResult = formExtractColRegExp.json;

									//Get values to store in extract column as JSON
									c.Value = jsonResult;
									//Set tag to True or False (for cell color)
									c.Tag = extract.ToString();
								}
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void regExpDataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
		{
			if (regExpDataGridView.Rows.Count > 0)
			{
				try
				{
					GetBindingSource().EndEdit();
				}
				catch
				{
				}
			}
		}

		private void regExpDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
		}

		private void columnButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (_preventUpdate)
					return;

				_previousSelectedColumnID = this.CurrentSelectedColumnID;

				var button = (RibbonStyleButton) sender;
				var columnID = (int) button.Tag;

				//Update dataGridView
				FilterByColumnID(columnID.ToString(), true);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void sourceColRegExp_CurrentChanged(object sender, EventArgs e)
		{
			try
			{
				var bindingSource = sender as BindingSource;
				if (bindingSource != null)
				{
					if (!Equals(bindingSource.Current, _currentRow))
					{
						_currentRow = (DataRowView) bindingSource.Current;
						_views.InvokeRefreshHighlights();
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void flowLayoutPanel_SizeChanged(object o, EventArgs eventArgs)
		{
			try
			{
				Task.Delay(1)
				    .ContinueWith(x => RedrawButtons());
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void toolStripAutoSizeColumns_Click(object sender, EventArgs e)
		{
			try
			{
				regExpDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			base.InitPane(views, ownerView, panel, tab);

			regExpDataGridView.AutoGenerateColumns = false;

			///////////////////////////////////////////////////////////////////////////////

			_columnsContextMenu = new ColumnSizeContextMenuWrapper(_views, _ownerView, regExpDataGridView);

			///////////////////////////////////////////////////////////////////////////////

			splitter.BorderStyle = BorderStyle.Fixed3D;

			InitializeGridColumns();
			InitializeTabs(true);

			InitializeRegExpEditor(true);

            FillNoteTextColumnCombo();

			RaiseDataModifiedEvent();

            _views.OnNoteColumnsChanged += OnNoteColumnChanged;
		}
        public override void DestroyPane()
        {
            _views.OnNoteColumnsChanged -= OnNoteColumnChanged;
            base.DestroyPane();
        }
        private void OnNoteColumnChanged()
        {
            FillNoteTextColumnCombo();
        }

        private void FillNoteTextColumnCombo(int columnIndex = 0)
        {
            comboNoteTextColumn.Items.Clear();

            //_tabSettings = _views.DocumentsService.GetDocumentColumnSettings();

            foreach ( TabSetting tab in _views.TabSettings)
            {
                if (tab.ColumnName.StartsWith("NOTE_TEXT"))
                {
                    comboNoteTextColumn.Items.Add(tab.DisplayName);

                    if (tab.Index == columnIndex)
                    {
                        comboNoteTextColumn.Text = tab.DisplayName;
                    }

                }
            }     
        }

        public bool IsTabAdded(int dynamicColumnID)
		{
			return flowLayoutPanel.Controls.Cast<RibbonStyleButton>().Any(x => (int) x.Tag == dynamicColumnID);
		}

		private void InitializeTabs(bool filterResults)
		{
			if (_views.IsDocumentsTableLoading)
				return;

			var rows = (from dynCol in _views.MainForm.datasetMain.DynamicColumns.Where(x => x.RowState != DataRowState.Deleted)
			            join col in _views.MainForm.datasetMain.Columns on dynCol.Title equals col.Name
						into tmp
						from t in tmp.DefaultIfEmpty()
			            orderby t != null ? t.Order : 100
			            select dynCol).ToList();

			var buttons = flowLayoutPanel.Controls.Cast<RibbonStyleButton>().ToList();

			if (buttons.Count > rows.Count())
			{
				//FInd what rows are deleted and remove buttons
				for (var i = 0; i < buttons.Count; i++)
				{
					var button = buttons[i];
					var columnID = (int) button.Tag;

					bool found = rows.Any(x => x.ID == columnID);
					if (!found)
					{
						//Delete this button from list
						flowLayoutPanel.Controls.RemoveAt(i);

						if (this.CurrentSelectedColumnID == columnID)
							this.CurrentSelectedColumnID = (int) buttons[0].Tag;
					}
				}
			}

			foreach (Control ctrl in flowLayoutPanel.Controls)
			{
				var index = rows.FindIndex(x => x.ID == (int) ctrl.Tag);
				if (index != -1)
					flowLayoutPanel.Controls.SetChildIndex(ctrl, index);
			}

			///////////////////////////////////////////////////////////////////////////////

			foreach (var row in rows)
			{
				if (!IsTabAdded(row.ID))
				{
					var button = new RibbonStyleButton
					             {
						             Text = row.Title,
						             Tag = row.ID,
						             AutoSize = true,
									 Margin = new Padding(0),
									 DrawNormalBorder = true
					             };
                    if (button.Text.StartsWith("NOTE_TEXT"))
                    {
                        button.Text = _views.DocumentsService.GetDynamicColumnDisplayName(button.Text);
                    }
					button.Click += columnButton_Click;

					flowLayoutPanel.Controls.Add(button);
				}
			}

			if (flowLayoutPanel.Controls.Count > 0 && filterResults)
			{
				var button = (RibbonStyleButton) flowLayoutPanel.Controls[0];
				//Load data for this category

				FilterByColumnID(((int) button.Tag).ToString(), false);
			}
		}

		public void FilterByColumnID(string strColumnID, bool savePreviousScriptCode)
		{
			if (_views.IsDocumentsTableLoading)
				return;

			//Get selected dynamic column type
			var columnID = Int32.Parse(strColumnID);

			if (savePreviousScriptCode)
			{
				//Save previous C# script to DB
				SaveCSharpScriptToDb(_previousSelectedColumnID);
                SavePythonToDb(_previousSelectedColumnID);
			}

			_preventUpdate = true;

			this.CurrentSelectedColumnID = columnID;

			GetBindingSource().Filter = "ColumnID = " + strColumnID;

			_preventUpdate = false;

			//Load C# script from DB
			var row = _views.MainForm.datasetMain.ColScript.FirstOrDefault(p => p.ColumnID == columnID);
			if (row != null)
			{
				//Parse row.Data from JSON to object
				var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CSScriptData>(row.Data);
				fastColoredTextBox.Text = obj.Script;
				checkBoxExtractValues.Checked = obj.ExtractValuesWithScript;
			}
			else
			{
				fastColoredTextBox.Text = "";
				checkBoxExtractValues.Checked = false;
			}

            var pythonRow = _views.MainForm.datasetMain.ColPython.FirstOrDefault(p => p.ColumnID == columnID);
            if (pythonRow != null)
            {
                //Parse row.Data from JSON to object
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CSPythonData>(row.Data);
                textBoxPythonFile.Text = obj.PythonFile;

                FillNoteTextColumnCombo(obj.NoteColumnIndex);

                checkBoxExtractPython.Checked = obj.ExtractValuesWithPython;

                if (checkBoxExtractPython.Checked == true)
                    checkBoxExtractValues.Checked = false;

            } else
            {
                textBoxPythonFile.Text = "";
                checkBoxExtractPython.Checked = false;
            }

			RaiseDataModifiedEvent();
		}

		protected override void InitPaneCommands(RibbonTab tab)
		{
			RibbonPanel panel = new RibbonPanel("Regular Expressions");
			tab.Panels.Add(panel);

			panel.Items.Add(new RibbonSeparator());

			//Missing positive score checkbox
			btnPositiveScore = new RibbonButton("Positive score");
			panel.Items.Add(btnPositiveScore);
			btnPositiveScore.Image = Properties.Resources.up_icon;
			btnPositiveScore.SmallImage = Properties.Resources.up_icon;
			btnPositiveScore.CheckOnClick = true;
			btnPositiveScore.Text = "Positive score";
			btnPositiveScore.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
			btnPositiveScore.Click += OnPositiveScoreOnClick;
			btnPositiveScore.Checked = _views.OnlyPositiveScores;

			//////////////////////////////////////////////////////////////////////////

			RibbonButton btnCalculate = new RibbonButton("Calculate");
			panel.Items.Add(btnCalculate);

			btnCalculate.Image = Properties.Resources.CalcScores;
			btnCalculate.SmallImage = Properties.Resources.CalcScores;
			btnCalculate.Click += new EventHandler(OnCalculate_Click);
			btnCalculate.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			//////////////////////////////////////////////////////////////////////////

			RibbonButton btnExtract = new RibbonButton("Extract");
			panel.Items.Add(btnExtract);

			btnExtract.Image = Properties.Resources.Export;
			btnExtract.SmallImage = Properties.Resources.Export;
			btnExtract.Click += new EventHandler(OnExtract_Click);
			btnExtract.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			//////////////////////////////////////////////////////////////////////////

			RibbonButton btnExtractAll = new RibbonButton("Extract All");
			panel.Items.Add(btnExtractAll);

			btnExtractAll.Image = Properties.Resources.Export;
			btnExtractAll.SmallImage = Properties.Resources.Export;
			btnExtractAll.Click += new EventHandler(OnExtractAll_Click);
			btnExtractAll.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			//////////////////////////////////////////////////////////////////////////

			RibbonButton btnAddRegExp = new RibbonButton("Add RegExp");
			panel.Items.Add(btnAddRegExp);

			btnAddRegExp.Image = Properties.Resources.add_large;
			btnAddRegExp.SmallImage = Properties.Resources.add_large;
			btnAddRegExp.Click += new EventHandler(OnAddRegExp_Click);
			btnAddRegExp.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
		}

		public override void UpdatePane()
		{
			var bindingSource = GetBindingSource();
			if (bindingSource != null)
				bindingSource.CurrencyManager.Refresh();

			InitializeTabs(false);
		}

		#endregion

		#region Operations

		public void EndEdit()
		{
			var bindingSource = GetBindingSource();
			if (bindingSource != null)
				bindingSource.EndEdit();
		}

		#endregion

		#region Implementation

		private void SaveCSharpScriptToDb(int columnID)
		{
			if (columnID == -1)
				return;

			var isNewRow = false;

			var existingRow = _views.MainForm.datasetMain.ColScript.FirstOrDefault(p => p.ColumnID == columnID);
			if (existingRow == null)
			{
				//Create new row
				existingRow = _views.MainForm.datasetMain.ColScript.NewColScriptRow();
				existingRow.ColumnID = columnID;

				_views.MainForm.datasetMain.ColScript.AddColScriptRow(existingRow);

				isNewRow = true;
			}
			//Save script
			var obj = new CSScriptData(checkBoxExtractValues.Checked, fastColoredTextBox.Text);

			var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

			if (!isNewRow)
			{
				if (existingRow.Data == json && existingRow.RowState == DataRowState.Unchanged)
					return;
			}

			existingRow.Data = json;

			_views.MainForm.adapterColScript.Update(existingRow);

			if (isNewRow)
				_views.MainForm.adapterColScript.Fill(_views.MainForm.datasetMain.ColScript);
			else
				existingRow.AcceptChanges();
        }

        private void SavePythonToDb(int columnID)
        {
            if (columnID == -1)
                return;

            var isNewRowPython = false;

            var existingRowPython = _views.MainForm.datasetMain.ColPython.FirstOrDefault(p => p.ColumnID == columnID);
            if (existingRowPython == null)
            {
                //Create new row
                existingRowPython = _views.MainForm.datasetMain.ColPython.NewColPythonRow();
                existingRowPython.ColumnID = columnID;

                _views.MainForm.datasetMain.ColPython.AddColPythonRow(existingRowPython);

                isNewRowPython = true;
            }

            var columnIndex = 0;
            foreach (TabSetting tab in _views.TabSettings)
            {
                if (tab.DisplayName == comboNoteTextColumn.Text)
                    columnIndex = tab.Index;
            }

            //Save Python
            var pythonObj = new CSPythonData(checkBoxExtractPython.Checked, textBoxPythonFile.Text, columnIndex);

            var pythonJson = Newtonsoft.Json.JsonConvert.SerializeObject(pythonObj);

            if (!isNewRowPython)
            {
                if (existingRowPython.Data == pythonJson && existingRowPython.RowState == DataRowState.Unchanged)
                    return;
            }

            existingRowPython.Data = pythonJson;

            _views.MainForm.adapterColPython.Update(existingRowPython);

            if (isNewRowPython)
                _views.MainForm.adapterColPython.Fill(_views.MainForm.datasetMain.ColPython);
            else
                existingRowPython.AcceptChanges();


        }

        protected void InitializeGridColumns()
		{
			this.regExpDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.extractExpDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.numberExpDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.docsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //this.recordsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.posDocsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.percentagePosDocsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.matchDataGridViewTextBoxColumn = new MatchColumn();
			this.regExpColorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();

			// 
			// regExpDataGridViewTextBoxColumn
			// 
			this.regExpDataGridViewTextBoxColumn.DataPropertyName = "RegExp";
			this.regExpDataGridViewTextBoxColumn.HeaderText = "RegExp";
			this.regExpDataGridViewTextBoxColumn.Name = "regExpDataGridViewTextBoxColumn";
			this.regExpDataGridViewTextBoxColumn.Width = 200;
			// 
			// extractExpDataGridViewTextBoxColumn
			// 
			this.extractExpDataGridViewTextBoxColumn.DataPropertyName = "Extract";
			this.extractExpDataGridViewTextBoxColumn.HeaderText = "Extract";
			this.extractExpDataGridViewTextBoxColumn.Name = "extractExpDataGridViewTextBoxColumn";
			this.extractExpDataGridViewTextBoxColumn.ReadOnly = true;
			this.extractExpDataGridViewTextBoxColumn.Width = 100;
			this.extractExpDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			// 
			// numberExpDataGridViewTextBoxColumn
			// 
			this.numberExpDataGridViewTextBoxColumn.DataPropertyName = "TotalMatches";
			this.numberExpDataGridViewTextBoxColumn.HeaderText = "#";
			this.numberExpDataGridViewTextBoxColumn.Name = "numberExpDataGridViewTextBoxColumn";
			numberExpDataGridViewTextBoxColumn.Width = 80;
			numberExpDataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			// 
			// docsDataGridViewTextBoxColumn
			// 
			this.docsDataGridViewTextBoxColumn.DataPropertyName = "TotalDocuments";
			this.docsDataGridViewTextBoxColumn.HeaderText = "Docs";
			this.docsDataGridViewTextBoxColumn.Name = "docsDataGridViewTextBoxColumn";
			docsDataGridViewTextBoxColumn.Width = 50;
			docsDataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            /*
            // 
            // docsDataGridViewTextBoxColumn
            // 
            this.recordsDataGridViewTextBoxColumn.DataPropertyName = "TotalRecords";
            this.recordsDataGridViewTextBoxColumn.HeaderText = "Records";
            this.recordsDataGridViewTextBoxColumn.Name = "recordsDataGridViewTextBoxColumn";
            recordsDataGridViewTextBoxColumn.Width = 50;
            recordsDataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            */

            // 
            // posDocsDataGridViewTextBoxColumn
            // 
            this.posDocsDataGridViewTextBoxColumn.DataPropertyName = "PosDocuments";
			this.posDocsDataGridViewTextBoxColumn.HeaderText = "Pos Docs";
			this.posDocsDataGridViewTextBoxColumn.Name = "posDocsDataGridViewTextBoxColumn";
			posDocsDataGridViewTextBoxColumn.Width = 100;
			posDocsDataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			// 
			// percentagePosDocsDataGridViewTextBoxColumn
			// 
			this.percentagePosDocsDataGridViewTextBoxColumn.DataPropertyName = "PercentPosDocuments";
			this.percentagePosDocsDataGridViewTextBoxColumn.HeaderText = "% Pos Docs";
			this.percentagePosDocsDataGridViewTextBoxColumn.Name = "percentagePosDocsDataGridViewTextBoxColumn";
			this.percentagePosDocsDataGridViewTextBoxColumn.Width = 160;
			this.percentagePosDocsDataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			// 
			// matchDataGridViewTextBoxColumn
			// 
			this.matchDataGridViewTextBoxColumn.HeaderText = "Match";
			this.matchDataGridViewTextBoxColumn.Name = "matchDataGridViewTextBoxColumn";
			this.matchDataGridViewTextBoxColumn.Width = 40;
			this.matchDataGridViewTextBoxColumn.MinimumWidth = 40;
			this.matchDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			this.matchDataGridViewTextBoxColumn.Visible = false;
			// 
			// regExpColorDataGridViewTextBoxColumn
			// 
			this.regExpColorDataGridViewTextBoxColumn.DataPropertyName = "RegExpColor";
			this.regExpColorDataGridViewTextBoxColumn.HeaderText = "Color";
			this.regExpColorDataGridViewTextBoxColumn.Name = "regExpColorDataGridViewTextBoxColumn";
			this.regExpColorDataGridViewTextBoxColumn.ReadOnly = true;
			regExpColorDataGridViewTextBoxColumn.Width = 80;

			//////////////////////////////////////////////////////////////////////////

			this.regExpDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
			                                         {
				                                         this.regExpDataGridViewTextBoxColumn,
				                                         this.extractExpDataGridViewTextBoxColumn,
				                                         this.numberExpDataGridViewTextBoxColumn,
				                                         this.docsDataGridViewTextBoxColumn,
                                                         //this.recordsDataGridViewTextBoxColumn,
				                                         this.posDocsDataGridViewTextBoxColumn,
				                                         this.percentagePosDocsDataGridViewTextBoxColumn,
				                                         this.matchDataGridViewTextBoxColumn,
				                                         this.regExpColorDataGridViewTextBoxColumn
			                                         });

			foreach (DataGridViewColumn column in this.regExpDataGridView.Columns)
			{
				column.HeaderCell.Style.Font = new Font("Arial", 11);
				column.DefaultCellStyle.Font = new System.Drawing.Font(FontFamily.GenericMonospace, 10);
			}

			///////////////////////////////////////////////////////////////////////////////

			regExpDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			regExpDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
		}

		protected void InitializeRegExpEditor(bool enableNavigation)
		{
			_editor = new FormRegularExpressionEditor(_views, GetBindingSource(), x =>
			                                                                      {
				                                                                      var columnID = this.CurrentSelectedColumnID;
																					  if (columnID != 0)
																						return RegExpFactory.Create_ColRegExp(x, new[] { columnID }, false);

				                                                                      return null;
			                                                                      })
			          {
				          Dock = DockStyle.Fill,
				          TopLevel = false,
				          Parent = splitter.Panel2
			          };

			_editor.Modified += (sender, args) => RaiseDataModifiedEvent();

			///////////////////////////////////////////////////////////////////////////////

			var panel = new Panel
			{
				Dock = DockStyle.Fill,
				BorderStyle = BorderStyle.None,
				Visible = true
			};

			panel.Controls.Add(_editor);

			splitter.Panel2.Controls.Add(panel);

			_editor.Show();
		}

		public void CalcScores()
		{
			_views.MainForm.datasetMain.Documents.DefaultView.Sort = String.Empty;

			regExpDataGridView.EndEdit(DataGridViewDataErrorContexts.Commit);

			//_editor.Save(true);

			_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;
			_views.MainForm.sourceColRegExp.RaiseListChangedEvents = false;

			///////////////////////////////////////////////////////////////////////////////

			try
			{
				FormGenericProgress formGenericProgress = new FormGenericProgress("Calculating scores, please wait...", DoCalcScores, this.CurrentSelectedColumnID, true);
				formGenericProgress.ShowDialog();

				if (formGenericProgress.Result)
					MessageBox.Show("Calculation finished", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			///////////////////////////////////////////////////////////////////////////////

			_views.MainForm.sourceColRegExp.RaiseListChangedEvents = true;
			_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;

			_views.MainForm.sourceColRegExp.ResetBindings(false);
			_views.MainForm.sourceDocuments.ResetBindings(false);

			RaiseDataModifiedEvent();
		}

		protected void Extract()
		{
			_editor.Save(true);

			_views.BeforeDocumentsTableLoad(false);

			BindingSource source = (BindingSource)regExpDataGridView.DataSource;
			source.SuspendBinding();

			regExpDataGridView.EndEdit(DataGridViewDataErrorContexts.Commit);

			_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;
			_views.MainForm.sourceColRegExp.RaiseListChangedEvents = false;

			///////////////////////////////////////////////////////////////////////////////

			try
			{
                SaveCSharpScriptToDb(this.CurrentSelectedColumnID);
                SavePythonToDb(this.CurrentSelectedColumnID);

				FormGenericProgress formGenericProgress = new FormGenericProgress("Extracting, please wait...", new LengthyOperation(DoExtract), this.CurrentSelectedColumnID, true);
				formGenericProgress.ShowDialog();

				if (formGenericProgress.Result)
					MessageBox.Show("Extracting finished", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			///////////////////////////////////////////////////////////////////////////////

			source.ResumeBinding();

			_views.MainForm.sourceColRegExp.RaiseListChangedEvents = true;
			_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;

			_views.MainForm.sourceColRegExp.ResetBindings(false);
			_views.MainForm.sourceDocuments.ResetBindings(false);

			_views.AfterDocumentsTableLoad(false);

			RaiseDataModifiedEvent();
		}

		protected void ExtractAll()
		{
			_editor.Save(true);

			_views.BeforeDocumentsTableLoad(false);

			BindingSource source = (BindingSource)regExpDataGridView.DataSource;
			source.SuspendBinding();

			regExpDataGridView.EndEdit(DataGridViewDataErrorContexts.Commit);

			_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;
			_views.MainForm.sourceColRegExp.RaiseListChangedEvents = false;

			///////////////////////////////////////////////////////////////////////////////

			try
			{
				var previousColumnId = this.CurrentSelectedColumnID;

				SaveCSharpScriptToDb(previousColumnId);
                SavePythonToDb(previousColumnId);

				FormGenericProgress formGenericProgress = new FormGenericProgress("Extracting all, please wait...", new LengthyOperation(DoExtractAll), null, true);
				formGenericProgress.ShowDialog();

				if (formGenericProgress.Result)
					MessageBox.Show("Extracting finished", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			///////////////////////////////////////////////////////////////////////////////

			source.ResumeBinding();

			_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
			_views.MainForm.sourceColRegExp.RaiseListChangedEvents = true;

			_views.MainForm.sourceColRegExp.ResetBindings(false);
			_views.MainForm.sourceDocuments.ResetBindings(false);

			_views.AfterDocumentsTableLoad(false);

			RaiseDataModifiedEvent();
		}

		protected void DeleteRows()
		{
			try
			{
				foreach (var row in regExpDataGridView.SelectedRows.Cast<DataGridViewRow>().ToList())
				{
					try
					{
						var rowView = (DataRowView) row.DataBoundItem;

						_views.MainForm.sourceColRegExp.Remove(rowView);

						var rowRegExp = (MainDataSet.ColRegExpRow) rowView.Row;

						_views.MainForm.adapterColRegExp.Update(rowRegExp);
					}
					catch
					{
					}
				}
			}
			catch
			{
			}

			RaiseDataModifiedEvent();

			_views.InvokeRefreshHighlights();
		}

		protected void EditRegExp()
		{
			try
			{
				if (regExpDataGridView.CurrentCell != null)
				{
					DataGridViewCell cell = regExpDataGridView.CurrentCell;
					if (IsStringColumn(cell.ColumnIndex))
					{
						regExpDataGridView.EndEdit(DataGridViewDataErrorContexts.Commit);

						FormEditRegExp formEditRegExp = new FormEditRegExp();

						if (cell.Value == DBNull.Value)
							formEditRegExp.RegExpValue = "";
						else
							formEditRegExp.RegExpValue = (string)cell.Value;

						if (formEditRegExp.ShowDialog() == DialogResult.OK)
						{
							cell.Value = formEditRegExp.RegExpValue;
							regExpDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
							regExpDataGridView.Refresh();

							RaiseDataModifiedEvent();
						}
					}
				}
			}
			catch
			{
			}
		}

		protected bool IsStringColumn(int nColumn)
		{
			if (nColumn == 0 || nColumn == 3 || nColumn == 5 || nColumn == 6 || nColumn == 7 || nColumn == 8 || nColumn == 9)
				return true;

			return false;
		}

		public BindingSource GetBindingSource()
		{
			if (regExpDataGridView.DataSource == null)
			{
				var bindingSource = new BindingSource(_views.MainForm.sourceColRegExp, null);
				bindingSource.CurrentChanged += new EventHandler(_views.MainForm.sourceColRegExp_CurrentChanged);
				bindingSource.CurrentChanged += new EventHandler(sourceColRegExp_CurrentChanged);

				regExpDataGridView.DataSource = bindingSource;
			}

			return regExpDataGridView.DataSource as BindingSource;
		}

		protected void InvokeEvent_ExtractFinished()
		{
			var handler = ExtractFinished;
			if (handler != null)
			{
				if (_previousSelectedColumnID != -1)
					handler(this.CurrentSelectedColumnID);
			}
		}

		protected void RedrawButtons()
		{
			foreach (var ctrl in flowLayoutPanel.Controls.Cast<Control>().ToList())
			{
				ctrl.Invalidate();
				ctrl.Update();
			}
		}

		#endregion

		#region Implementation: matches

		public void RefreshHighlights()
		{
			try
			{
				_views.InvokeRefreshHighlights();
			}
			catch
			{
			}
		}

		#endregion

		#region Implementation: ExtractOptions and Calculation

		protected bool DoCalcScores(BackgroundWorker worker, object objArgument)
		{
			bool result;

			var columnID = (int) objArgument;

			using (var processor = new ExternalRegExpToolWrapper(worker))
			{
				result = processor.ColRegExp_CalcStatistics(_views, columnID, btnPositiveScore.Checked);
			}

			return result;
		}

		protected bool DoExtract(BackgroundWorker worker, object objArgument)
		{
			var columnID = (int) objArgument;

			using (var processor = new ExternalRegExpToolWrapper(worker))
			{
				if (checkBoxExtractValues.Checked)
				{
					var compileResult = String.Empty;

					if (!manager.IsCodeCompiled() || manager.IsCodeChanged(fastColoredTextBox.Text))
                    {
                        compileResult = manager.Compile(GetAvailableCompileCode(fastColoredTextBox.Text));
                    }
						

					if (!String.IsNullOrEmpty(compileResult))
					{
						MessageBox.Show(compileResult, "Compile error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}

					///////////////////////////////////////////////////////////////////////////////

					processor.ColRegExp_Extract(_views, columnID, true, fastColoredTextBox.Text, btnPositiveScore.Checked);
				}
                else if (checkBoxExtractPython.Checked)
                {
                    if (String.IsNullOrEmpty(textBoxPythonFile.Text))
                    {
                        MessageBox.Show("Please select python file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    ///////////////////////////////////////////////////////////////////////////////

                    processor.ColPython_Extract(_views, columnID, textBoxPythonFile.Text, 0, btnPositiveScore.Checked);
                }
                else
				{
					processor.ColRegExp_Extract(_views, columnID, false, null, btnPositiveScore.Checked);

				}
			}

			return true;
		}

		protected bool DoExtractAll(BackgroundWorker worker, object objArgument)
		{
			var result = false;

			using (var processor = new ExternalRegExpToolWrapper(worker))
			{
				var extractUsingRegExp = (from colScriptRow in _views.MainForm.datasetMain.ColScript.Rows.Cast<MainDataSet.ColScriptRow>()
				                          join dynamicColumnsRow in _views.MainForm.datasetMain.DynamicColumns.Cast<MainDataSet.DynamicColumnsRow>() on colScriptRow.ColumnID equals dynamicColumnsRow.ID
				                          select Newtonsoft.Json.JsonConvert.DeserializeObject<CSScriptData>(colScriptRow.Data)).All(obj => !obj.ExtractValuesWithScript);

				if (!extractUsingRegExp)
				{
					foreach (RegScoreCalc.MainDataSet.ColScriptRow row in _views.MainForm.datasetMain.ColScript.Rows)
					{
						//If it is not category column
						if (row.ColumnID != 0)
						{
							var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CSScriptData>(row.Data);
							if (obj.ExtractValuesWithScript)
							{
								//Compile script
								var compileResult = manager.Compile(GetAvailableCompileCode(obj.Script));
								if (!String.IsNullOrEmpty(compileResult))
								{
									worker.ReportProgress(-1, compileResult);
									result = false;

									break;
								}

								///////////////////////////////////////////////////////////////////////////////

								result = true;
							}
						}
					}

					if (result)
						processor.ColRegExp_Extract(_views, -1, true, null, btnPositiveScore.Checked);
				}
				else
				{
					processor.ColRegExp_Extract(_views, -1, false, null, btnPositiveScore.Checked);

					result = true;
				}
			}

			return result;
		}

        private string GetNoteColumnIndexFromCode(string codeText)
        {            
            string columnName = "NOTE_TEXT";
            int st = codeText.IndexOf("NOTE_TEXT") + 9;
            int i;
            for (i = st; st != -1 && i < codeText.Length; i++)
            {
                if ('0' > codeText[i] || codeText[i] > '9')
                    break;
                columnName += codeText[i];
            }

            return columnName;
        }

        private string GetAvailableCompileCode(string codeText)
        {
            string columnName = GetNoteColumnIndexFromCode(codeText);

            codeText = codeText.Replace(columnName, "NOTE_TEXT");

            return codeText;
        }

		#endregion

		#region "ExtractOptions using C# code"

		private void btnRunOnCurrentDoc_Click(object sender, EventArgs e)
		{
			RunScript();
		}

		private void btnRunOnNextDoc_Click(object sender, EventArgs e)
		{
			//Select next document and then run this
			_views.MainForm.sourceDocuments.MoveNext();

			RunScript();
		}

        private void RunScript()
        {
            string columnName = GetNoteColumnIndexFromCode(fastColoredTextBox.Text);

            string compileResult = "";

			//Get currently selected row
			DataRowView rowview = (DataRowView)_views.MainForm.sourceDocuments.Current;
			MainDataSet.DocumentsRow rowDocument = (MainDataSet.DocumentsRow)rowview.Row;

            ////Only compile if code is not compiled or code has changed from the one that is compile at the moment
            //if (manager.IsCodeChanged(fastColoredTextBox.Text))
            //{

            //Allways compile script
            
            compileResult = manager.Compile(GetAvailableCompileCode(fastColoredTextBox.Text));

			//Save to DB now
			var previousColumnId = this.CurrentSelectedColumnID;
			//Save previous C# script to DB
			SaveCSharpScriptToDb(previousColumnId);

			//}

			//Check result of compiling, show error if exists, otherwise run code
			if (String.IsNullOrEmpty(compileResult))
			{
                List<string> regexp = new List<string>();

                foreach (DataGridViewRow row in regExpDataGridView.Rows)
                {
                    if (row.Cells[this.extractExpDataGridViewTextBoxColumn.Index].Tag != null && row.Cells[this.extractExpDataGridViewTextBoxColumn.Index].Tag.ToString() == "True")
                        regexp.Add(row.Cells[0].Value.ToString());
                }

                var result = manager.Run(_views.DocumentsService.GetDocumentText(rowDocument.ED_ENC_NUM, columnName), regexp);
				MessageBox.Show(this, result, "Extract result", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
				MessageBox.Show(compileResult, "Compile error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

        #endregion

        private void btnHelp_Click(object sender, EventArgs e)
        {
            var helpDialog = new FormScriptHelp(ScriptFunctions.Help());
            if (helpDialog.ShowDialog() == DialogResult.OK)
            {
                fastColoredTextBox.Text =  fastColoredTextBox.Text.Insert(fastColoredTextBox.SelectionStart, helpDialog.GetExampleCode());                
            }
        }

        private void btnOpenPythonFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Python File (*.py)|*.py";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxPythonFile.Text = openFileDialog.FileName;
            }
        }
    }
    public class CSPythonData
    {
        public CSPythonData(bool extractValuesWithPython, string filepath, int columnIndex)
        {
            ExtractValuesWithPython = extractValuesWithPython;
            PythonFile = filepath;
            NoteColumnIndex = columnIndex;
        }

        public bool ExtractValuesWithPython { get; set; }
        public string PythonFile { get; set; }
        public int NoteColumnIndex { get; set; }
    }
}