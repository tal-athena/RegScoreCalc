using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.OleDb;
using System.Linq;

using RegExpLib.Core;

using RegScoreCalc.Code;
using RegScoreCalc.Forms;
using System.IO;
using RegExpLib.Processing;

namespace RegScoreCalc
{
	public partial class PaneRegExp : Pane
	{
		#region Fields

		private DataGridViewTextBoxColumn regExpDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn scoreDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn arithmeticFactorDataGridViewTextBoxColumn;
		private MatchColumn matchDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn regExpColorDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn colTotalMatches;
		private DataGridViewTextBoxColumn colTotalDocuments;
        private DataGridViewTextBoxColumn colTotalRecords;
        private DataGridViewTextBoxColumn colTotalCategorized;
        private DataGridViewTextBoxColumn colCategorizedRecords;

        private DataGridViewTextBoxColumn entityDataGridViewTextBoxColumn;              
        
        private DataGridViewTextBoxColumn entityColorDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn colEntityTotalMatches;
        private DataGridViewTextBoxColumn colEntityTotalDocuments;
        private DataGridViewTextBoxColumn colEntityTotalRecords;
        private DataGridViewTextBoxColumn colEntityTotalCategorized;
        

        protected RibbonComboBox _cmbCalcScores;

		protected FormRegularExpressionEditor _editor;
        protected FormEntityStatistics _entity_editor;

        protected ColumnSizeContextMenuWrapper _columnsContextMenu;

		protected int _nRegExpRow;
		protected int _nMatchIndex;

        protected string _strEntitiesSqliteFile;

        protected string _currentOutputFolder;
		#endregion

		#region Ctors

		public PaneRegExp(ViewsManager views)
		{
			InitializeComponent();

			_views = views;

			toolStripTop.Renderer = new CustomToolStripRenderer { RoundedEdges = false };
            toolStripAutoSizeEntityGrid.Renderer = new CustomToolStripRenderer { RoundedEdges = false };


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
		}

		#endregion

		#region Properties

		public bool EnableNavigation
		{
			get { return _editor.EnableNavigation; }
			set { _editor.EnableNavigation = value; }
		}

        #endregion

        #region Events
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {
                splitter.Panel2.Controls[0].Visible = true;
                splitter.Panel2.Controls[1].Visible = false;

            } else
            {
                splitter.Panel2.Controls[0].Visible = false;
                splitter.Panel2.Controls[1].Visible = true;
            }
        }


        protected void OnCalcScores_Click(object sender, EventArgs e)
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

		protected void OnAddRegExp_Click(object sender, EventArgs e)
		{
			try
			{
				_views.MainForm.sourceRegExp.RaiseListChangedEvents = false;

				regExpDataGridView.EndEdit(DataGridViewDataErrorContexts.Commit);
				regExpDataGridView.CurrentCell = null;
				this.Validate();

				///////////////////////////////////////////////////////////////////////////////

				var guid = Guid.NewGuid()
							   .ToString();

				var count = _views.MainForm.adapterRegExp.Insert(string.Empty, null, null, null, null, null, null, null, null, null, null, guid, null, null);
				if (count != 1)
					throw new Exception("Failed to insert RegExp");

				var id = MainForm.GetLastInsertedID(_views.MainForm.adapterRegExp.Connection);

				///////////////////////////////////////////////////////////////////////////////

				_views.MainForm.adapterRegExp.Update(_views.MainForm.datasetMain.RegExp);
				_views.MainForm.adapterRegExp.Fill(_views.MainForm.datasetMain.RegExp);

				///////////////////////////////////////////////////////////////////////////////

				_views.MainForm.sourceRegExp.RaiseListChangedEvents = true;
				_views.MainForm.sourceRegExp.ResetBindings(false);

				///////////////////////////////////////////////////////////////////////////////

				var position = _views.MainForm.sourceRegExp.Find("ID", id);
				if (position != -1)
				{
					_views.MainForm.sourceRegExp.Position = position;

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
				if (ex != null && !string.IsNullOrEmpty(ex.Message) && ex.Message.Contains("RegExp.RegExp"))
				{
					var message = "Please check in RegExp->DesignView->Field RegExp->Allow zero length = YES" 
						+ Environment.NewLine + Environment.NewLine + Environment.NewLine;
					MessageBox.Show(message, MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnAutoCalc_Click(object sender, EventArgs e)
		{
			if (sender is RibbonButton)
			{
				RibbonButton btn = (RibbonButton)sender;
				if (btn.Tag is int)
				{
					int nTimes = (int)btn.Tag;
					_views.AutoCalc = nTimes;

					_cmbCalcScores.TextBoxText = btn.Text;
				}
			}
		}

		private void regExpDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
            if (e.RowIndex < 0)
                return;
            try
            {               
                if (e.ColumnIndex == colTotalCategorized.Index)
                {
                    FormCategorizedRecordsCount dialog = new FormCategorizedRecordsCount(_views, regExpDataGridView.Rows[e.RowIndex].Cells[this.colCategorizedRecords.Index].Value);
                    dialog.ShowDialog();
                }
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
				if (e.ColumnIndex == regExpColorDataGridViewTextBoxColumn.Index) // index of column with color
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
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}
        private void entityDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                if (e.ColumnIndex == entityColorDataGridViewTextBoxColumn.Index) // index of column with color
                {
                    DataGridViewCell c = entityDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
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

                    _views.MainForm.adapterEntities.Update(_views.MainForm.datasetMain.Entities);                    
                    
                }
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);
            }
        }
        private void regExpDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
			//RaiseDataModifiedEvent();
		}

		private void regExpDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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
						if (c.Value is int)
							argbColor = (int) c.Value;
						else if (c.Value is string)
							argbColor = Convert.ToInt32(c.Value);
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
		}
        private void entityDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == entityColorDataGridViewTextBoxColumn.Index && e.RowIndex >= 0) // index of column with color
            {
                DataGridViewCell c = entityDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (c.Value != null)
                {
                    var color = RegExpBase.DefaultHighlightColor;
                    int argbColor = 0;
                    try
                    {
                        if (c.Value is int)
                            argbColor = (int)c.Value;
                        else if (c.Value is string)
                            argbColor = Convert.ToInt32(c.Value);
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
        }
        private void regExpDataGridView_MouseClick(object sender, MouseEventArgs e)
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

						var rowView = (DataRowView) cell.OwningRow.DataBoundItem;
						var row = (MainDataSet.RegExpRow) rowView.Row;

						FormAssignRegExpToGroup dialog = new FormAssignRegExpToGroup(_views, row.ID);
						dialog.ShowDialog();
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
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

							var regExp = RegExpFactory.Create_RegExp(rowview, true);

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
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					if (e.RowIndex >= 0)
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
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void regExpDataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				BindingSource source = (BindingSource)regExpDataGridView.DataSource;
				source.EndEdit();
			}
			catch
			{
			}
		}

		private void regExpDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
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

		private void btnSynergies_Click(object sender, EventArgs e)
		{
			try
			{
				var form = new FormSynergies(_views);
				form.ShowDialog();
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
			regExpDataGridView.DataSource = _views.MainForm.sourceRegExp;

            entityDataGridView.AutoGenerateColumns = false;
            entityDataGridView.DataSource = _views.MainForm.sourceEntities;
			///////////////////////////////////////////////////////////////////////////////

			_columnsContextMenu = new ColumnSizeContextMenuWrapper(_views, _ownerView, regExpDataGridView);

			///////////////////////////////////////////////////////////////////////////////

			splitter.BorderStyle = BorderStyle.Fixed3D;

			InitializeGridColumns();

            InitializeEntityGridColumns();

            var entityTab = _views.TabSettings.Find(x => x.ColumnName == "NOTE_ENTITIES");

            if (entityTab == null || entityTab.Visible == false)
                this.tabControl.Controls.Remove(this.tabEntities);

            _views.OnNoteColumnsChanged += OnTabSettingsChanged;

            InitializeRegExpEditor(true);
		}
        public override void DestroyPane()
        {
            _views.OnNoteColumnsChanged += OnTabSettingsChanged;
            base.DestroyPane();
        }
        private void OnTabSettingsChanged()
        {
            foreach (var tab in _views.TabSettings)
            {
                if (tab.TabType == DocumentsServiceInterfaceLib.DocumentViewType.Html_SpaCyEntityView)
                {
                    if (tab.Visible == true)
                    {
                        if (!this.tabControl.Controls.Contains(this.tabEntities))
                            this.tabControl.Controls.Add(this.tabEntities);
                    } else
                    {
                        if (this.tabControl.Controls.Contains(this.tabEntities))
                            this.tabControl.Controls.Remove(this.tabEntities);
                    }
                }
            }
        }

        protected override void InitPaneCommands(RibbonTab tab)
		{
			RibbonPanel panel = new RibbonPanel("Regular Expressions");
			tab.Panels.Add(panel);

			///////////////////////////////////////////////////////////////////////////////

			RibbonButton btnAddRegExp = new RibbonButton("Add RegExp");
			panel.Items.Add(btnAddRegExp);

			btnAddRegExp.Image = Properties.Resources.add_large;
			btnAddRegExp.SmallImage = Properties.Resources.add_large;
			btnAddRegExp.Click += new EventHandler(OnAddRegExp_Click);
			btnAddRegExp.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			///////////////////////////////////////////////////////////////////////////////

			RibbonButton btnCalcScores = new RibbonButton("Calculate Scores");
			btnCalcScores.Image = Properties.Resources.CalcScores;
			btnCalcScores.SmallImage = Properties.Resources.CalcScores;
			btnCalcScores.Click += new EventHandler(OnCalcScores_Click);
			btnCalcScores.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			panel.Items.Add(btnCalcScores);

			//////////////////////////////////////////////////////////////////////////

			//CreateAutoCalcScoresComboBox();

			//////////////////////////////////////////////////////////////////////////

			panel.Items.Add(new RibbonSeparator());

            RibbonButton btnCalcSettings = new RibbonButton("Settings");

            btnCalcSettings.Click += new EventHandler(OnCalcSettings);
            btnCalcSettings.Image = Properties.Resources.hotkeys;
            btnCalcSettings.SmallImage = Properties.Resources.hotkeys;
            btnCalcSettings.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

            panel.Items.Add(btnCalcSettings);

            RibbonPanel entitiesPanel = new RibbonPanel("Entities");
            tab.Panels.Add(entitiesPanel);

            RibbonButton btnEntityCalculate = new RibbonButton("Calculate");
            btnEntityCalculate.Click += new EventHandler(OnEntityCalculate);
            btnEntityCalculate.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

            entitiesPanel.Items.Add(btnEntityCalculate);

            RibbonButton btnEntitySettings = new RibbonButton("Settings");
            btnEntitySettings.Click += new EventHandler(OnEntitySettings);
            btnEntitySettings.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

            entitiesPanel.Items.Add(btnEntitySettings);
		}

        private void OnEntitySettings(object sender, EventArgs e)
        {
            FormEntitySettings entitySettings = new FormEntitySettings(_strEntitiesSqliteFile, _views.MainForm.DataBaseFilePath);

            var result = entitySettings.ShowDialog();

            if (result != DialogResult.Cancel)
            {
                _strEntitiesSqliteFile = entitySettings.GetSqliteFilePath();

                if (result == DialogResult.Yes)
                {
                    EntitiesSqliteGenerate();
                }
            }
        }


        private void OnEntityCalculate(object sender, EventArgs e)
        {
            EntitiesCalculate();
        }



        private void OnCalcSettings(object sender, EventArgs e)
        {
            FormRegExpSettings regExpSettings = new FormRegExpSettings(_views.AutoCalc);
            if (regExpSettings.ShowDialog() == DialogResult.OK)
            {
                _views.AutoCalc = regExpSettings.GetAutoCalc();
            }
        }

        protected void CreateAutoCalcScoresComboBox()
		{
            
			_cmbCalcScores = new RibbonComboBox();
			//_cmbCalcScores.MinSizeMode = RibbonElementSizeMode.DropDown;
			_cmbCalcScores.AllowTextEdit = false;
			_cmbCalcScores.Text = "Auto-calc Scores: ";

			//////////////////////////////////////////////////////////////////////////

			RibbonButton btn;

			btn = new RibbonButton("never");
			btn.Tag = 0;
			btn.Click += btnAutoCalc_Click;
			_cmbCalcScores.DropDownItems.Add(btn);

			btn = new RibbonButton("1 change");
			btn.Tag = 1;
			btn.Click += btnAutoCalc_Click;
			_cmbCalcScores.DropDownItems.Add(btn);

			btn = new RibbonButton("5 changes");
			btn.Tag = 5;
			btn.Click += btnAutoCalc_Click;
			_cmbCalcScores.DropDownItems.Add(btn);

			btn = new RibbonButton("10 changes");
			btn.Tag = 10;
			btn.Click += btnAutoCalc_Click;
			_cmbCalcScores.DropDownItems.Add(btn);

			//////////////////////////////////////////////////////////////////////////

			switch (_views.AutoCalc)
			{
				case 0:
					_cmbCalcScores.TextBoxText = "never";
					break;

				case 1:
					_cmbCalcScores.TextBoxText = "1 change";
					break;

				case 5:
					_cmbCalcScores.TextBoxText = "5 changes";
					break;

				case 10:
					_cmbCalcScores.TextBoxText = "10 changes";
					break;
			}
            
		}

		#endregion

		#region Implementation

		protected void InitializeGridColumns()
		{
			this.regExpDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.scoreDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.arithmeticFactorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.matchDataGridViewTextBoxColumn = new MatchColumn();
			this.regExpColorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTotalMatches = new DataGridViewTextBoxColumn();
			this.colTotalDocuments = new DataGridViewTextBoxColumn();
            this.colTotalRecords = new DataGridViewTextBoxColumn();
            this.colTotalCategorized = new DataGridViewTextBoxColumn();
            this.colCategorizedRecords = new DataGridViewTextBoxColumn();

            // 
            // regExpDataGridViewTextBoxColumn
            // 
            this.regExpDataGridViewTextBoxColumn.DataPropertyName = "RegExp";
			this.regExpDataGridViewTextBoxColumn.HeaderText = "RegExp";
			this.regExpDataGridViewTextBoxColumn.Name = "regExpDataGridViewTextBoxColumn";
			this.regExpDataGridViewTextBoxColumn.Width = 230;
			this.regExpDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			// 
			// scoreDataGridViewTextBoxColumn
			// 
			this.scoreDataGridViewTextBoxColumn.DataPropertyName = "score";
			this.scoreDataGridViewTextBoxColumn.HeaderText = "Score";
			this.scoreDataGridViewTextBoxColumn.Name = "scoreDataGridViewTextBoxColumn";
			scoreDataGridViewTextBoxColumn.Width = 60;
			scoreDataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			// 
			// arithmeticFactorDataGridViewTextBoxColumn
			// 
			this.arithmeticFactorDataGridViewTextBoxColumn.DataPropertyName = "Arithmetic factor";
			this.arithmeticFactorDataGridViewTextBoxColumn.HeaderText = "Arithmetic Factor";
			this.arithmeticFactorDataGridViewTextBoxColumn.Name = "arithmeticFactorDataGridViewTextBoxColumn";
			arithmeticFactorDataGridViewTextBoxColumn.Width = 60;
			arithmeticFactorDataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			// 
			// matchDataGridViewTextBoxColumn
			// 
			this.matchDataGridViewTextBoxColumn.HeaderText = "Match";
			this.matchDataGridViewTextBoxColumn.Name = "matchDataGridViewTextBoxColumn";
			this.matchDataGridViewTextBoxColumn.Width = 40;
			this.matchDataGridViewTextBoxColumn.MinimumWidth = 40;
			this.matchDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			// 
			// regExpColorDataGridViewTextBoxColumn
			// 
			this.regExpColorDataGridViewTextBoxColumn.DataPropertyName = "RegExpColor";
			this.regExpColorDataGridViewTextBoxColumn.HeaderText = "Color";
			this.regExpColorDataGridViewTextBoxColumn.Name = "regExpColorDataGridViewTextBoxColumn";
			this.regExpColorDataGridViewTextBoxColumn.ReadOnly = true;
			regExpColorDataGridViewTextBoxColumn.Width = 50;
			// 
			// colTotalMatches
			// 
			this.colTotalMatches.DataPropertyName = "TotalMatches";
			this.colTotalMatches.HeaderText = "#";
			this.colTotalMatches.Name = "colTotalMatches";
			this.colTotalMatches.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

			// 
			// colTotalDocuments
			// 
			this.colTotalDocuments.DataPropertyName = "TotalDocuments";
			this.colTotalDocuments.HeaderText = "docs";
			this.colTotalDocuments.Name = "colTotalDocuments";
			this.colTotalDocuments.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            // 
            // colTotalRecords
            // 
            this.colTotalRecords.DataPropertyName = "TotalRecords";
            this.colTotalRecords.HeaderText = "records";
            this.colTotalRecords.Name = "colTotalRecords";
            this.colTotalRecords.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            //
            // colTotalCategorized
            //
            this.colTotalCategorized.DataPropertyName = "TotalCategorized";
            this.colTotalCategorized.HeaderText = "# categorized";
            this.colTotalCategorized.Name = "colTotalCategorized";
            this.colTotalCategorized.ReadOnly = true;
            this.colTotalCategorized.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;


            //
            // colCategorizedRecords
            //
            this.colCategorizedRecords.DataPropertyName = "CategorizedRecords";
            this.colCategorizedRecords.HeaderText = "categorized records";
            this.colCategorizedRecords.Name = "colCategorizedRecords";
            this.colCategorizedRecords.ReadOnly = true;
            this.colCategorizedRecords.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.colCategorizedRecords.Visible = false;


            //////////////////////////////////////////////////////////////////////////

            this.regExpDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
													 {
														 this.regExpDataGridViewTextBoxColumn,                                                         
														 this.colTotalMatches,
														 this.colTotalDocuments,
                                                         this.colTotalRecords,                                                         
                                                         this.scoreDataGridViewTextBoxColumn,
                                                         this.colTotalCategorized,
                                                         this.arithmeticFactorDataGridViewTextBoxColumn,
														 this.matchDataGridViewTextBoxColumn,
														 this.regExpColorDataGridViewTextBoxColumn,
                                                         this.colCategorizedRecords
                                                     });

			foreach (DataGridViewColumn column in this.regExpDataGridView.Columns)
			{
				column.HeaderCell.Style.Font = new Font("Arial", 11);
				column.DefaultCellStyle.Font = new System.Drawing.Font(FontFamily.GenericMonospace, 10);
			}
                        
            ///////////////////////////////////////////////////////////////////////////////

            regExpDataGridView.AllowUserToAddRows = false;
			regExpDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			regExpDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
		}
        protected void InitializeEntityGridColumns()
        {
        
            this.entityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entityColorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEntityTotalMatches = new DataGridViewTextBoxColumn();
            this.colEntityTotalDocuments = new DataGridViewTextBoxColumn();
            this.colEntityTotalRecords = new DataGridViewTextBoxColumn();
            this.colEntityTotalCategorized = new DataGridViewTextBoxColumn();


            // 
            // entityDataGridViewTextBoxColumn
            // 
            this.entityDataGridViewTextBoxColumn.DataPropertyName = "Entity";
            this.entityDataGridViewTextBoxColumn.HeaderText = "Entity";
            this.entityDataGridViewTextBoxColumn.Name = "entityDataGridViewTextBoxColumn";
            this.entityDataGridViewTextBoxColumn.Width = 230;
            this.entityDataGridViewTextBoxColumn.ReadOnly = true;
            this.entityDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // 
            // entityColorDataGridViewTextBoxColumn
            // 
            this.entityColorDataGridViewTextBoxColumn.DataPropertyName = "EntityColor";
            this.entityColorDataGridViewTextBoxColumn.HeaderText = "Color";
            this.entityColorDataGridViewTextBoxColumn.Name = "entityColorDataGridViewTextBoxColumn";
            this.entityColorDataGridViewTextBoxColumn.ReadOnly = true;
            this.entityColorDataGridViewTextBoxColumn.Width = 50;
            // 
            // colEntityTotalMatches
            // 
            this.colEntityTotalMatches.DataPropertyName = "TotalMatches";
            this.colEntityTotalMatches.HeaderText = "#";
            this.colEntityTotalMatches.Name = "colEntityTotalMatches";
            this.colEntityTotalMatches.ReadOnly = true;
            this.colEntityTotalMatches.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.colEntityTotalMatches.Width = 60;

            // 
            // colEntityTotalDocuments
            // 
            this.colEntityTotalDocuments.DataPropertyName = "TotalDocuments";
            this.colEntityTotalDocuments.HeaderText = "docs";
            this.colEntityTotalDocuments.Name = "colEntityTotalDocuments";
            this.colEntityTotalDocuments.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.colEntityTotalDocuments.Width = 60;

            // 
            // colEntityTotalRecords
            // 
            this.colEntityTotalRecords.DataPropertyName = "TotalRecords";
            this.colEntityTotalRecords.HeaderText = "records";
            this.colEntityTotalRecords.Name = "colEntityTotalRecords";
            this.colEntityTotalRecords.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.colEntityTotalRecords.Width = 60;

            //
            // colEntityTotalCategorized
            //
            this.colEntityTotalCategorized.DataPropertyName = "TotalCategorized";
            this.colEntityTotalCategorized.HeaderText = "# categorized";
            this.colEntityTotalCategorized.Name = "colEntityTotalCategorized";
            this.colEntityTotalCategorized.ReadOnly = true;
            this.colEntityTotalCategorized.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.colEntityTotalCategorized.Width = 100;

            //////////////////////////////////////////////////////////////////////////

            this.entityDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
                                                     {
                                                         this.entityDataGridViewTextBoxColumn,
                                                         this.colEntityTotalMatches,
                                                         this.colEntityTotalDocuments,
                                                         this.colEntityTotalRecords,
                                                         this.colEntityTotalCategorized,                                                         
                                                         this.entityColorDataGridViewTextBoxColumn                                                         
                                                     });

            foreach (DataGridViewColumn column in this.entityDataGridView.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Arial", 11);
                column.DefaultCellStyle.Font = new System.Drawing.Font(FontFamily.GenericMonospace, 10);
            }

            ///////////////////////////////////////////////////////////////////////////////

            entityDataGridView.AllowUserToAddRows = false;
            entityDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            entityDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }

        protected void InitializeRegExpEditor(bool enableNavigation)
		{
			_editor = new FormRegularExpressionEditor(_views, _views.MainForm.sourceRegExp, x => RegExpFactory.Create_RegExp(x, false))
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


            _entity_editor = new FormEntityStatistics(_views, _views.MainForm.sourceEntities)
            {
                Dock = DockStyle.Fill,
                TopLevel = false,                
                Parent = splitter.Panel2
            };

            _entity_editor.Modified += (sender, args) => RaiseDataModifiedEvent();

            ///////////////////////////////////////////////////////////////////////////////

            var entity_panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Visible = false
            };

            entity_panel.Controls.Add(_entity_editor);

            splitter.Panel2.Controls.Add(entity_panel);

            _entity_editor.Show();

        }

		public void CalcScores()
		{
			regExpDataGridView.EndEdit(DataGridViewDataErrorContexts.Commit);

			_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;
			_views.MainForm.sourceRegExp.RaiseListChangedEvents = false;

			///////////////////////////////////////////////////////////////////////////////

			var formGenericProgress = new FormGenericProgress("Calculating scores, please wait...", DoCalcScores, null, true);
			formGenericProgress.ShowDialog();

			if (formGenericProgress.Result)
				MessageBox.Show("Calculation finished successfully", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

			///////////////////////////////////////////////////////////////////////////////

			_views.MainForm.sourceRegExp.RaiseListChangedEvents = true;
			_views.MainForm.sourceRegExp.ResetBindings(false);

			_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
			_views.MainForm.sourceDocuments.ResetBindings(false);

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

						_views.MainForm.sourceRegExp.Remove(rowView);

						var rowRegExp = (MainDataSet.RegExpRow) rowView.Row;

						_views.MainForm.adapterRegExp.Update(rowRegExp);
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

		#endregion

		#region Implementation: score calculation

		protected bool DoCalcScores(BackgroundWorker worker, object objArgument)
		{
			try
			{
				using (var processor = new ExternalRegExpToolWrapper(worker))
				{
					return processor.RegExp_CalcScores(_views);
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);

				return false;
			}
		}

        protected bool DoEntityGenerateSqlite(BackgroundWorker worker, object objArgument)
        {
            try
            {
                using (var processor = new ExternalEntitiesToolWrapper(worker, (EntitiesProcessingParam)objArgument))
                {
                    return processor.RunAction(_views);
                }
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);

                return false;
            }
        }
        protected bool DoEntityCalculate(BackgroundWorker worker, object objArgument)
        {
            try
            {
                using (var processor = new ExternalEntitiesToolWrapper(worker, (EntitiesProcessingParam)objArgument))
                {
                    return processor.RunAction(_views);
                }                
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);

                return false;
            }
        }

        private void EntitiesSqliteGenerate()
        {
            if (string.IsNullOrEmpty(_strEntitiesSqliteFile))
            {
                MessageBox.Show("No Sqlite DB file selected.", MainForm.AppName);
                return;
            }

            EntitiesProcessingParam parm = new EntitiesProcessingParam
            {
                Command = "Generate",
                SqliteFilePath = _strEntitiesSqliteFile,                
                AccessFilePath = _views.MainForm.DataBaseFilePath,
                AnacondaPath = _views.AnacondaPath,
                VirtualEnv = _views.PythonEnv
            };
            

            var formGenericProgress = new FormGenericProgress("Generating sqlite DB, please wait...", DoEntityGenerateSqlite, parm, true);
            formGenericProgress.ShowDialog();

            if (formGenericProgress.Result)
                MessageBox.Show("Generation finished successfully", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        private void EntitiesCalculate()
        {
            if (string.IsNullOrEmpty(_strEntitiesSqliteFile) || !File.Exists(_strEntitiesSqliteFile))
            {
                MessageBox.Show("No Sqlite DB file selected.", MainForm.AppName);
                return;
            }

            EntitiesProcessingParam parm = new EntitiesProcessingParam
            {
                Command = "Calculate",
                SqliteFilePath = _strEntitiesSqliteFile,                
                AnacondaPath = _views.AnacondaPath,
                VirtualEnv = _views.PythonEnv
            };
            
            var formGenericProgress = new FormGenericProgress("Calculating entities, please wait...", DoEntityCalculate, parm, true);
            formGenericProgress.ShowDialog();

            _views.MainForm.adapterEntities.Update(_views.MainForm.datasetMain.Entities);
            _views.MainForm.adapterEntities.Fill(_views.MainForm.datasetMain.Entities);

            if (formGenericProgress.Result)
                MessageBox.Show("Calculation finished successfully", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        #endregion

        private void toolAutosizeEntity_Click(object sender, EventArgs e)
        {
            try
            {
                entityDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);
            }
        }

        private void toolStripRefreshEntity_Click(object sender, EventArgs e)
        {
            EntitiesProcessingParam parm = new EntitiesProcessingParam
            {
                Command = "GetEntityNames",
                SqliteFilePath = "",
                AnacondaPath = _views.AnacondaPath,
                VirtualEnv = _views.PythonEnv
            };

            var formGenericProgress = new FormGenericProgress("Getting entity names, please wait...", DoEntityCalculate, parm, true);
            formGenericProgress.ShowDialog();

            _views.MainForm.adapterEntities.Update(_views.MainForm.datasetMain.Entities);
            _views.MainForm.adapterEntities.Fill(_views.MainForm.datasetMain.Entities);

            if (formGenericProgress.Result)
                MessageBox.Show("Finished successfully", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
