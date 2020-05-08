using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

using RegScoreCalc.Code;
using RegScoreCalc.Views.Models;

using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace RegScoreCalc
{
	public partial class PaneDocumentsReviewMLOld : Pane
	{
		#region Fields

		protected BinaryClassificationOldViewModel _binaryClassificationOldViewModel;
		protected Pen _gridPen;

		protected int _currentMouseOverRow;

		public RibbonButton _btnSortScores;
		public RibbonPanel _panel;

		protected bool _isCtrlDown;

		#endregion

		#region Ctors

		public PaneDocumentsReviewMLOld()
		{
			InitializeComponent();

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
							BeginEditStaticCategoryColumn(currentCell, true);

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
				if (e.RowIndex >= 0 && e.ColumnIndex == columnCategory.Index)
				{
					var cell = documentsDataGridView[e.ColumnIndex, e.RowIndex];
					BeginEditStaticCategoryColumn(cell, false);
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

				Brush backgroundBrush;
				Brush textBrush;

				if (cell.Selected)
				{
					backgroundBrush = SystemBrushes.Highlight;
					textBrush = Brushes.White;
				}
				else if (value != DBNull.Value && toolStripButtonHighlightCategories.Checked && _binaryClassificationOldViewModel != null)
				{
					var cellColumnID = (int) value;
					if (_binaryClassificationOldViewModel.positiveCategories.Contains(cellColumnID))
						backgroundBrush = _binaryClassificationOldViewModel.positiveColor;
					else if (_binaryClassificationOldViewModel.excludedCategories.Contains(cellColumnID))
						backgroundBrush = _binaryClassificationOldViewModel.excludedColor;
					else
						backgroundBrush = _binaryClassificationOldViewModel.negativeColor;

					textBrush = Brushes.Black;
				}
				else
				{
					backgroundBrush = Brushes.White;
					textBrush = Brushes.Black;
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
				var cell = documentsDataGridView.Rows[_currentMouseOverRow].Cells[columnCategory.Index];
				FormSelectCategory formSelectCategory = new FormSelectCategory(_views, FormSelectCategory.DisplayMode.Default, cell, false);

				//////////////////////////////////////////////////////////////////////////

				if (formSelectCategory.ShowDialog() == DialogResult.OK)
				{
					foreach (DataGridViewRow row in documentsDataGridView.SelectedRows)
					{
						DataRowView rowview = (DataRowView) row.DataBoundItem;

						MainDataSet.ReviewMLDocumentsRow rowReviewDocuments = (MainDataSet.ReviewMLDocumentsRow) rowview.Row;
						MainDataSet.DocumentsRow rowDocuments = _views.MainForm.datasetMain.Documents.FindByPrimaryKey(rowReviewDocuments.ED_ENC_NUM);

						if (formSelectCategory.CategoryID != -1)
							rowDocuments.Category = formSelectCategory.CategoryID;
						else
							rowDocuments.SetCategoryNull();

						UpdateDocumentsRow(rowDocuments);
					}
				}
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
			//_views.MainForm.sourceReviewMLDocuments.DataSource = _views.MainForm.adapterReviewMLDocuments;
			documentsDataGridView.DataSource = _views.MainForm.sourceReviewMLDocuments;

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

			this.columnProc1SVM.DataPropertyName = "Proc1SVM";
			this.columnProc1SVM.HeaderText = "SVM Score";
			this.columnProc1SVM.MaxInputLength = 100;
			this.columnProc1SVM.Name = "columnProc1SVM";
			this.columnProc1SVM.ReadOnly = true;
			this.columnProc1SVM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.columnProc1SVM.Width = 110;

			this.columnCategory.DataPropertyName = "Category";
			this.columnCategory.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
			this.columnCategory.DataSource = _views.MainForm.sourceCategories;
			this.columnCategory.DisplayMember = "Category";
			this.columnCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.columnCategory.HeaderText = "Category";
			this.columnCategory.MaxDropDownItems = 18;
			this.columnCategory.Name = "Category";
			this.columnCategory.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.columnCategory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.columnCategory.ValueMember = "ID";
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
				BeginEditStaticCategoryColumn(documentsDataGridView.CurrentCell, false);

				return true;
			}

			///////////////////////////////////////////////////////////////////////////////

			return base.OnHotkey(code);
		}

		#endregion

		#region Implementation

		public DataGridView GetDocumentsGrid()
		{
			return this.documentsDataGridView;
		}

		protected void LoadCategoryColors()
		{
			try
			{
				var json = BrowserManager.GetViewData(_views, "Binary Classification");
				_binaryClassificationOldViewModel = BinaryClassificationOldViewModel.FromJSON(json);
				if (_binaryClassificationOldViewModel != null)
				{
					if (_binaryClassificationOldViewModel.positiveColor == null)
						_binaryClassificationOldViewModel.positiveColor = new SolidBrush(Color.FromArgb(150, 0, 255, 0));

					if (_binaryClassificationOldViewModel.negativeColor == null)
						_binaryClassificationOldViewModel.negativeColor = new SolidBrush(Color.FromArgb(150, 255, 0, 0));

					if (_binaryClassificationOldViewModel.excludedColor == null)
						_binaryClassificationOldViewModel.excludedColor = new SolidBrush(Color.FromArgb(150, 255, 255, 0));

					///////////////////////////////////////////////////////////////////////////////

					if (_binaryClassificationOldViewModel.positiveCategories == null)
						_binaryClassificationOldViewModel.positiveCategories = new List<int>();

					if (_binaryClassificationOldViewModel.excludedCategories == null)
						_binaryClassificationOldViewModel.excludedCategories = new List<int>();
				}
			}
			catch
			{
			}
		}

		public void UpdateDocumentsRow(MainDataSet.DocumentsRow rowDocument)
		{
			OleDbConnection conn = _views.MainForm.adapterDocuments.Connection;
			if (conn.State != ConnectionState.Open)
				conn.Open();

			OleDbCommand cmd = conn.CreateCommand();
			cmd.CommandText = "UPDATE Documents SET Category = @Category WHERE ED_ENC_NUM = @ED_ENC_NUM";

			if (!rowDocument.IsCategoryNull())
				cmd.Parameters.Add(new OleDbParameter("@Category", rowDocument.Category));
			else
				cmd.Parameters.Add(new OleDbParameter("@Category", DBNull.Value));

			cmd.Parameters.Add(new OleDbParameter("@ED_ENC_NUM", rowDocument.ED_ENC_NUM));

			cmd.ExecuteNonQuery();

			rowDocument.AcceptChanges();
		}

		protected void BeginEditStaticCategoryColumn(DataGridViewCell cell, bool triggeredByCtrl)
		{
			var rowview = (DataRowView) cell.OwningRow.DataBoundItem;

			var rowReviewDocuments = (MainDataSet.ReviewMLDocumentsRow)rowview.Row;

			var rowDocuments = _views.MainForm.datasetMain.Documents.FindByPrimaryKey(rowReviewDocuments.ED_ENC_NUM);

			var formSelectCategory = new FormSelectCategory(_views, FormSelectCategory.DisplayMode.Default, cell, triggeredByCtrl);

			if (!rowDocuments.IsCategoryNull())
				formSelectCategory.CategoryID = rowDocuments.Category;

			//////////////////////////////////////////////////////////////////////////

			if (formSelectCategory.ShowDialog() == DialogResult.OK)
			{
				if (formSelectCategory.CategoryID != -1)
				{
					rowDocuments.Category = formSelectCategory.CategoryID;
					rowReviewDocuments.Category = formSelectCategory.CategoryID;
				}
				else
				{
					rowDocuments.SetCategoryNull();
					rowReviewDocuments.SetCategoryNull();
				}

				UpdateDocumentsRow(rowDocuments);

				rowReviewDocuments.AcceptChanges();

				//documentsDataGridView.Refresh();
			}
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