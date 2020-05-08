using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

using RegScoreCalc.Code;
using RegScoreCalc.Views.Models;

using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace RegScoreCalc
{
	public partial class PaneDocumentsReviewML : Pane
	{
		#region Fields

		protected BinaryClassificationViewModel _binaryClassificationViewModel;
		protected Pen _gridPen;

		protected int _currentMouseOverRow;

		public RibbonButton _btnSortScores;
		public RibbonPanel _panel;

		protected int _dynamicCategoryColumnID;
		protected string _svmColumnName;

		protected bool _isCtrlDown;

        protected int _noteColumnIndex;

        #region property
        public int NoteColumnIndex
        {
            get { return _noteColumnIndex; }
            set {
                _noteColumnIndex = value;
                if (_noteColumnIndex == 0)
                {
                    this.columnScore.HeaderText = "Score";
                    this.columnScore.DataPropertyName = "Score";
                }
                else
                {
                    this.columnScore.HeaderText = "Score" + _noteColumnIndex;
                    this.columnScore.DataPropertyName = "Score" + _noteColumnIndex;
                }
                
            }
        }
        #endregion

        #endregion

        #region Ctors

        public PaneDocumentsReviewML(int dynamicCategoryColumnID, string svmColumnName)
		{
			InitializeComponent();

			_dynamicCategoryColumnID = dynamicCategoryColumnID;
			_svmColumnName = svmColumnName;

			_gridPen = new Pen(documentsDataGridView.GridColor);

			toolStripTop.Renderer = new CustomToolStripRenderer { RoundedEdges = false };
		}

		#endregion

		#region Events

		private void documentsDataGridView_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.ControlKey)
				{
					if (_isCtrlDown)
						return;

					var currentCell = documentsDataGridView.CurrentCell;
					if (currentCell != null)
					{
						if (currentCell.ColumnIndex == columnCategory.Index)
						{
							if (_dynamicCategoryColumnID > 0)
								BeginEditDynamicCategoryColumn(new List<DataGridViewRow> { currentCell.OwningRow }, true);
							else
								BeginEditStaticCategoryColumn(new List<DataGridViewRow> { currentCell.OwningRow }, true);

							UpdateCtrlDownState();
							e.Handled = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void documentsDataGridView_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.ControlKey)
					_isCtrlDown = false;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void documentsDataGridView_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateCtrlDownState();

				RaiseDataModifiedEvent();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void documentsDataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			if (documentsDataGridView.Rows.Count > 0)
			{
				if (e.RowIndex >= 0)
				{
					if (e.ColumnIndex == this.columnNo.Index)
						e.Value = e.RowIndex + 1;
				}
			}
		}

		private void documentsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex < 0)
					return;

				if (e.ColumnIndex == columnCategory.Index)
				{
					var cell = documentsDataGridView[e.ColumnIndex, e.RowIndex];
					if (_dynamicCategoryColumnID > 0)
						BeginEditDynamicCategoryColumn(new List<DataGridViewRow> { cell.OwningRow }, false);
					else
						BeginEditStaticCategoryColumn(new List<DataGridViewRow> { cell.OwningRow }, false);
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void documentsDataGridView_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				ContextMenu m = new ContextMenu();

				_currentMouseOverRow = documentsDataGridView.HitTest(e.X, e.Y)
				                                            .RowIndex;

				if (_currentMouseOverRow >= 0)
				{
					MenuItem selectCategory = new MenuItem("Select Category");
					selectCategory.Click += btnSelectCategory_Click;
					m.MenuItems.Add(selectCategory);
				}

				m.Show(documentsDataGridView, new Point(e.X, e.Y));
			}
		}

		private void documentsDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			try
			{
				if (e.RowIndex < 0)
					return;

				if (e.ColumnIndex != columnCategory.Index)
					return;

				///////////////////////////////////////////////////////////////////////////////

				var cell = documentsDataGridView[e.ColumnIndex, e.RowIndex];
				var value = cell.Value;

				var backgroundBrush = Brushes.White;
				var textBrush = Brushes.Black;

				if (cell.Selected)
				{
					backgroundBrush = SystemBrushes.Highlight;
					textBrush = Brushes.White;
				}
				else if (value != DBNull.Value && toolStripButtonHighlightCategories.Checked && _binaryClassificationViewModel != null)
				{
					var cellColumnID = (int) value;

					var columnInfo = _binaryClassificationViewModel.columns.FirstOrDefault(x => x.ID == _dynamicCategoryColumnID);
					if (columnInfo != null)
					{
						if (columnInfo.positiveCategories.Any(x => x.ID == cellColumnID))
							backgroundBrush = _binaryClassificationViewModel.positiveColor;
						else if (columnInfo.excludedCategories.Any(x => x.ID == cellColumnID))
							backgroundBrush = _binaryClassificationViewModel.excludedColor;
						else
							backgroundBrush = _binaryClassificationViewModel.negativeColor;
					}
				}

				///////////////////////////////////////////////////////////////////////////////

				var rc = new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1);
				e.Graphics.FillRectangle(Brushes.White, rc);
				e.Graphics.FillRectangle(backgroundBrush, rc);

				///////////////////////////////////////////////////////////////////////////////

				var formattedValue = e.FormattedValue as string;
				if (!String.IsNullOrEmpty(formattedValue))
				{
					var sf = new StringFormat
					         {
						         Alignment = StringAlignment.Near,
						         LineAlignment = StringAlignment.Center
					         };

					rc = new Rectangle(e.CellBounds.X + 2, e.CellBounds.Y + 2, e.CellBounds.Width - 4, e.CellBounds.Height - 2);

					e.Graphics.DrawString(formattedValue, new Font("Arial", 10.5f), textBrush, rc, sf);
				}

				///////////////////////////////////////////////////////////////////////////////

				e.Graphics.DrawLine(_gridPen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
				e.Graphics.DrawLine(_gridPen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

				///////////////////////////////////////////////////////////////////////////////

				e.Handled = true;
			}
			catch
			{
			}
		}

		private void documentsDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs args)
		{
			if (args.Exception != null)
				MainForm.ShowErrorToolTip(args.Exception.Message);
		}

		private void btnAddCategory_Clicked(object sender, EventArgs e)
		{
			AddCategory addCategoryForm = new AddCategory(_views);
			if (addCategoryForm.ShowDialog() == DialogResult.OK)
				RaiseDataModifiedEvent();
		}

		private void btnSelectCategory_Click(object sender, EventArgs e)
		{
			if (documentsDataGridView.SelectedRows.Count > 0)
			{
				var rows = documentsDataGridView.SelectedRows.Cast<DataGridViewRow>().ToList();

				if (_dynamicCategoryColumnID > 0)
					BeginEditDynamicCategoryColumn(rows, false);
				else
					BeginEditStaticCategoryColumn(rows, false);
			}
		}

		private void toolStripButtonHighlightCategories_Click(object sender, EventArgs e)
		{
			if (toolStripButtonHighlightCategories.Checked)
				LoadCategoryColors();

			documentsDataGridView.Refresh();
		}

		#endregion

		#region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			base.InitPane(views, ownerView, panel, tab);

			documentsDataGridView.AutoGenerateColumns = false;
			documentsDataGridView.DataSource = _views.MainForm.sourceReviewMLDocumentsNew;

			this.columnID.DataPropertyName = "ED_ENC_NUM";
			this.columnID.HeaderText = "ED_ENC_NUM";
			this.columnID.Name = "dataGridViewTextBoxColumn7";
			this.columnID.ReadOnly = true;
			this.columnID.Width = 110;

			this.columnRank.DataPropertyName = "Rank";
			this.columnRank.HeaderText = "Rank";
			this.columnRank.MaxInputLength = 100;
			this.columnRank.Name = "columnRank";
			this.columnRank.ReadOnly = true;
			this.columnRank.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.columnRank.Width = 110;

			this.columnProc3SVM.DataPropertyName = "Proc3SVM";
			this.columnProc3SVM.HeaderText = "SVM Score";
			this.columnProc3SVM.MaxInputLength = 100;
			this.columnProc3SVM.Name = "columnProc3SVM";
			this.columnProc3SVM.ReadOnly = true;
			this.columnProc3SVM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.columnProc3SVM.Width = 110;

			this.columnProc1SVM.DataPropertyName = _svmColumnName;
			this.columnProc1SVM.HeaderText = "SVM Score";
			this.columnProc1SVM.MaxInputLength = 100;
			this.columnProc1SVM.Name = "columnProc1SVM";
			this.columnProc1SVM.ReadOnly = true;
			this.columnProc1SVM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.columnProc1SVM.Width = 110;

			this.columnCategory.DataPropertyName = "Category";
			this.columnCategory.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
			this.columnCategory.DataSource = GetCategories();
			this.columnCategory.DisplayMember = "Title";
			this.columnCategory.ValueMember = "ID";
			this.columnCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.columnCategory.HeaderText = GetCategoryColumnHeader();
			this.columnCategory.MaxDropDownItems = 18;
			this.columnCategory.Name = "Category";
			this.columnCategory.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.columnCategory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.columnCategory.Width = 130;

			this.columnScore.DataPropertyName = "Score";
			this.columnScore.HeaderText = "Score";
			this.columnScore.MaxInputLength = 100;
			this.columnScore.Name = "columnScore";
			this.columnScore.ReadOnly = true;
			this.columnScore.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.columnScore.Width = 110;

			this.documentsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
			                                            {
				                                            this.columnNo,
				                                            this.columnID,
				                                            this.columnCategory,
				                                            this.columnProc1SVM,
				                                            this.columnRank,
				                                            this.columnProc3SVM,
				                                            this.columnScore,
			                                            });

			foreach (DataGridViewColumn column in this.documentsDataGridView.Columns)
			{
				column.HeaderCell.Style.Font = new Font("Arial", 11);
				column.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 10);
			}
		}

		protected override void InitPaneCommands(RibbonTab tab)
		{
			_panel = new RibbonPanel("Documents");

			RibbonButton btnAddCategory = new RibbonButton("Add Category");

			tab.Panels.Add(_panel);

			_panel.Items.Add(btnAddCategory);

			btnAddCategory.Image = Properties.Resources.AddCategory;
			btnAddCategory.SmallImage = Properties.Resources.AddCategory;
			btnAddCategory.Click += new EventHandler(btnAddCategory_Clicked);
			btnAddCategory.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
		}

		public override void UpdatePane()
		{
			base.UpdatePane();

			documentsDataGridView.Refresh();
		}

		public override bool OnHotkey(string code)
		{
			var currentCell = documentsDataGridView.CurrentCell;
			if (currentCell == null)
				return base.OnHotkey(code);

			///////////////////////////////////////////////////////////////////////////////

			if (code == PaneDocuments.HotkeyCode_SelectCategory)
			{
				BeginEditStaticCategoryColumn(new List<DataGridViewRow> { documentsDataGridView.CurrentCell.OwningRow }, false);

				return true;
			}

			///////////////////////////////////////////////////////////////////////////////

			return base.OnHotkey(code);
		}

		#endregion

		#region	Operations

		public void DisableDynamicColumns()
		{
			documentsDataGridView.DataSource = null;
		}

		public void SetDynamicCategoryColumn(int dynamicCategoryColumnID, string svmColumnName)
		{
			_dynamicCategoryColumnID = dynamicCategoryColumnID;
			_svmColumnName = svmColumnName;

			if (_dynamicCategoryColumnID > 0)
				columnScore.Visible = false;
			else
				columnScore.Visible = true;

			this.columnCategory.DataSource = GetCategories();
			this.columnCategory.DataPropertyName = "Category";
			this.columnCategory.DisplayMember = "Title";
			this.columnCategory.HeaderText = GetCategoryColumnHeader();
			this.columnCategory.ValueMember = "ID";

			this.columnProc1SVM.HeaderText = dynamicCategoryColumnID > 0 ? _svmColumnName : "SVM Score";
			this.columnProc1SVM.DataPropertyName = _svmColumnName;

			documentsDataGridView.DataSource = _views.MainForm.sourceReviewMLDocumentsNew;

			documentsDataGridView.AutoResizeColumnHeadersHeight();
			documentsDataGridView.AutoResizeColumn(this.columnProc1SVM.Index, DataGridViewAutoSizeColumnMode.ColumnHeader);

			LoadCategoryColors();
		}

		public DataGridView GetDocumentsGrid()
		{
			return this.documentsDataGridView;
		}

		#endregion

		#region Implementation

		protected void LoadCategoryColors()
		{
			try
			{
				var json = BrowserManager.GetViewData(_views, "Binary Classification");
				_binaryClassificationViewModel = BinaryClassificationViewModel.FromJSON(json);
				if (_binaryClassificationViewModel != null)
				{
					if (_binaryClassificationViewModel.positiveColor == null)
						_binaryClassificationViewModel.positiveColor = new SolidBrush(Color.FromArgb(150, 0, 255, 0));

					if (_binaryClassificationViewModel.negativeColor == null)
						_binaryClassificationViewModel.negativeColor = new SolidBrush(Color.FromArgb(150, 255, 0, 0));

					if (_binaryClassificationViewModel.excludedColor == null)
						_binaryClassificationViewModel.excludedColor = new SolidBrush(Color.FromArgb(150, 255, 255, 0));

					///////////////////////////////////////////////////////////////////////////////

					if (_binaryClassificationViewModel.columns == null)
						_binaryClassificationViewModel.columns = new List<BinaryClassificationColumnInfo>();
				}
			}
			catch
			{
			}
		}
		protected MainDataSet.ReviewMLDocumentsRow GetReviewDocumentRow(DataGridViewRow row)
		{
			var rowView = (DataRowView) row.DataBoundItem;

			return (MainDataSet.ReviewMLDocumentsRow) rowView.Row;
		}
		protected void BeginEditStaticCategoryColumn(List<DataGridViewRow> reviewRows, bool triggeredByCtrl)
		{
			if (!reviewRows.Any())
				return;

			var cellRowIndex = documentsDataGridView.CurrentCell.RowIndex;
			var cellColumnIndex = documentsDataGridView.CurrentCell.ColumnIndex;

			var formSelectCategory = new FormSelectCategory(_views, FormSelectCategory.DisplayMode.Default, reviewRows.First().Cells[columnCategory.Index], triggeredByCtrl);

			if (reviewRows.Count == 1)
			{
				var reviewRow = GetReviewDocumentRow(reviewRows.Single());
				formSelectCategory.CategoryID = !reviewRow.IsCategoryNull() ? reviewRow.Category : -1;
			}

			//////////////////////////////////////////////////////////////////////////

			if (formSelectCategory.ShowDialog() == DialogResult.OK)
				SetStaticCategoryValue(reviewRows, formSelectCategory.CategoryID);

			if (cellRowIndex < documentsDataGridView.Rows.Count)
				documentsDataGridView.CurrentCell = documentsDataGridView[cellColumnIndex, cellRowIndex];
		}

		protected void BeginEditDynamicCategoryColumn(List<DataGridViewRow> reviewRows, bool triggeredByCtrl)
		{
			if (!reviewRows.Any())
				return;

			var dynamicCategoryColumn = _views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.ID == _dynamicCategoryColumnID);
			if (dynamicCategoryColumn != null)
			{
				var selectedCategoryTitle = String.Empty;

				var cellRowIndex = documentsDataGridView.CurrentCell.RowIndex;
				var cellColumnIndex = documentsDataGridView.CurrentCell.ColumnIndex;

				var cell = reviewRows.First().Cells[columnCategory.Index];

				if (reviewRows.Count == 1)
				{
					var reviewDocumentRow = GetReviewDocumentRow(reviewRows.Single());
					var documentRow = _views.MainForm.datasetMain.Documents.FindByPrimaryKey(reviewDocumentRow.ED_ENC_NUM);

					selectedCategoryTitle = documentRow.Field<string>(dynamicCategoryColumn.Title);
				}

				var formSelectDynamicColumnCategory = new FormSelectDynamicColumnCategory(_views, cell, triggeredByCtrl)
				                                      {
					                                      SelectedCategoryTitle = selectedCategoryTitle,
					                                      DynamicColumnID = _dynamicCategoryColumnID
				                                      };

				///////////////////////////////////////////////////////////////////////////////

				var dlgres = formSelectDynamicColumnCategory.ShowDialog();
				if (dlgres == DialogResult.OK)
				{
					SetDynamicCategoryValue(reviewRows, formSelectDynamicColumnCategory.SelectedCategoryID, formSelectDynamicColumnCategory.SelectedCategoryTitle, dynamicCategoryColumn);
					UpdatePane();
				}
				else if (dlgres == DialogResult.Abort)
				{
					SetDynamicCategoryValue(reviewRows, null, null, dynamicCategoryColumn);
					UpdatePane();
				}

				if (cellRowIndex < documentsDataGridView.Rows.Count)
					documentsDataGridView.CurrentCell = documentsDataGridView[cellColumnIndex, cellRowIndex];
			}
		}

		protected void SetStaticCategoryValue(List<DataGridViewRow> reviewRows, int categoryID)
		{
			var join = from reviewDocRow in reviewRows.Select(x => x.DataBoundItem)
													  .Cast<DataRowView>().Select(x => x.Row).Cast<MainDataSet.ReviewMLDocumentsRow>()
					   join docRow in _views.MainForm.datasetMain.Documents
						   on reviewDocRow.ED_ENC_NUM equals docRow.ED_ENC_NUM
					   select new { docRow, reviewDocRow };

			foreach (var j in join)
			{
				if (categoryID != -1)
				{
					j.reviewDocRow.Category = categoryID;
					j.docRow.Category = categoryID;

					_views.MainForm.adapterDocuments.SqlSetColumnValueByPrimaryKey("Category", categoryID, j.docRow.ED_ENC_NUM);
				}
				else
				{
					j.reviewDocRow.SetCategoryNull();
					j.docRow.SetCategoryNull();

					_views.MainForm.adapterDocuments.SqlSetColumnValueByPrimaryKey("Category", null, j.docRow.ED_ENC_NUM);
				}
			}

			_views.MainForm.datasetMain.Documents.AcceptChanges();
			_views.MainForm.adapterReviewMLDocumentsNew.Table.AcceptChanges();
		}

		protected void SetDynamicCategoryValue(List<DataGridViewRow> reviewRows, int? categoryID, string categoryTitle, MainDataSet.DynamicColumnsRow dynamicCategoryColumn)
		{
			var join = from reviewDocRow in reviewRows.Select(x => x.DataBoundItem)
			                                          .Cast<DataRowView>().Select(x => x.Row).Cast<MainDataSet.ReviewMLDocumentsRow>()
					   join docRow in _views.MainForm.datasetMain.Documents
						   on reviewDocRow.ED_ENC_NUM equals docRow.ED_ENC_NUM
					   select new { docRow, reviewDocRow };

			foreach (var j in join)
			{
				if (categoryID != null && categoryTitle != null)
				{
					j.reviewDocRow.Category = categoryID.Value;
					j.docRow[dynamicCategoryColumn.Title] = categoryTitle;

					_views.MainForm.adapterDocuments.SqlSetColumnValueByPrimaryKey(dynamicCategoryColumn.Title, categoryTitle, j.docRow.ED_ENC_NUM);
				}
				else
				{
					j.reviewDocRow.SetCategoryNull();
					j.docRow[dynamicCategoryColumn.Title] = DBNull.Value;

					_views.MainForm.adapterDocuments.SqlSetColumnValueByPrimaryKey(dynamicCategoryColumn.Title, null, j.docRow.ED_ENC_NUM);
				}
			}

			_views.MainForm.datasetMain.Documents.AcceptChanges();
			_views.MainForm.adapterReviewMLDocumentsNew.Table.AcceptChanges();
		}

		protected string GetCategoryColumnHeader()
		{
			if (_dynamicCategoryColumnID > 0)
			{
				var dynamicCategory = _views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.ID == _dynamicCategoryColumnID);
				if (dynamicCategory != null)
					return dynamicCategory.Title;
			}

			return "Category";
		}

		protected List<CategoryInfo> GetCategories()
		{
			var categories = _dynamicCategoryColumnID > 0
				? _views.MainForm.datasetMain.DynamicColumnCategories.Where(x => !x.IsDynamicColumnIDNull() && x.DynamicColumnID == _dynamicCategoryColumnID)
						.Select(x => new CategoryInfo { ID = x.ID, Title = x.Title })
						.ToList()
				: _views.MainForm.datasetMain.Categories.Select(x => new CategoryInfo { ID = x.ID, Title = x.Category })
						.ToList();

			return categories;
		}

		protected void UpdateCtrlDownState()
		{
			try
			{
				if (!documentsDataGridView.InvokeRequired)
					_isCtrlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
			}
			catch { }
		}

		#endregion
	}
}