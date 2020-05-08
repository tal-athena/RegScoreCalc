using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;

using System.Drawing;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

using Helpers;

using Newtonsoft.Json;
using RegScoreCalc.Code;
using RegScoreCalc.Forms;
using RegScoreCalc.Views.Models;

using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Timer = System.Windows.Forms.Timer;

namespace RegScoreCalc
{
	public partial class PaneDocuments : Pane
	{
		#region Hotkey codes

		public static string HotkeyCode_SelectCategory
		{
			get { return "PaneDocuments_SelectCategory"; }
		}

		public static string HotkeyCode_EditValue
		{
			get { return "PaneDocuments_EditValue"; }
		}

		#endregion

		#region Fields

		protected Timer _timer;

		protected RibbonButton _btnSortScores;
		protected RibbonButton _btnFreezeSort;
		protected RibbonButton _btnFilterByCategory;
		protected RibbonPanel _panel;

		protected BinaryClassificationViewModel _binaryClassificationViewModel;
		protected Pen _gridPen;

		protected Dictionary<int, Brush> _brushes;

		protected int _currentMouseOverRow;

		protected int _lastSortColumnIndex = -1;
		protected ListSortDirection _lastSortDirection;
		protected bool _sortedByCategoryValue;

		protected Font _cellFontRegular;
		protected Font _cellFontBold;
		protected Font _headerSortIndicatorFont;

		protected int _currentRowIndex;
		protected int _currentColumnIndex;
		protected int _scrollRowIndex;
		protected int _scrollColumnIndex;

		protected bool _isCtrlDown;

		protected bool _isSortingGroupsInProgress;
		protected bool _documentsTableReloaded;
		protected ExternalSortOptions _sortOptions;
		protected string _sortGroupsByColumn;
		protected SortOrder _sortGroupsByColumnDirection;

		protected Dictionary<object, Brush> _groupBrushes;

		protected string _categoryFilter;
		protected string _groupsFilter;

		#endregion

		#region Ctors

		public PaneDocuments()
		{
			InitializeComponent();

			toolStripTop.Renderer = new CustomToolStripRenderer { RoundedEdges = false };

			_gridPen = new Pen(documentsDataGridView.GridColor);

			_timer = new Timer();
			_timer.Interval = 500;
			_timer.Tick += timer_Tick;

			_cellFontRegular = new Font("Arial", 10.5f);
			_cellFontBold = new Font("Arial", 10.5f, FontStyle.Bold);
			_headerSortIndicatorFont = new Font("Arial", 7.0f, FontStyle.Bold);

			_scrollRowIndex = -1;
			_scrollColumnIndex = -1;
			_currentRowIndex = -1;
			_currentColumnIndex = -1;

			_brushes = new Dictionary<int, Brush>();
			_groupBrushes = new Dictionary<object, Brush>();
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
						if (currentCell.ColumnIndex != columnCategory.Index)
						{
							if (currentCell.OwningColumn.Tag != null)
							{
								var dynamicColumnID = (int)currentCell.OwningColumn.Tag;
								var dynamicColumnRow = _views.MainForm.datasetMain.DynamicColumns.First(x => x.ID == dynamicColumnID);
								var columnType = (DynamicColumnType)dynamicColumnRow.Type;

								var documentRow = GetDocumentRow(currentCell);

								///////////////////////////////////////////////////////////////////////////////

								if (columnType == DynamicColumnType.Category)
								{
									BeginEditDynamicCategoryColumn(documentRow, dynamicColumnRow, dynamicColumnID, currentCell, true);

									UpdateCtrlDownState();

									e.Handled = true;
									return;
								}
							}
						}
						else
						{
							BeginEditStaticCategoryColumn(currentCell, true);

							UpdateCtrlDownState();

							e.Handled = true;
							return;
						}
					}
				}

				if (e.KeyCode == Keys.Return || e.KeyCode == Keys.F2)
				{
					if (!e.Control && !e.Alt && !e.Shift)
					{
						e.Handled = true;

						BeginEditColumn(documentsDataGridView.CurrentCell);
					}
				}
				else if (e.KeyCode == Keys.Home)
				{
					if (e.Control && !e.Alt && !e.Shift)
					{
						e.Handled = true;

						if (documentsDataGridView.RowCount > 0)
						{
							var columnIndex = documentsDataGridView.CurrentCell != null ? documentsDataGridView.CurrentCell.ColumnIndex : 0;

							documentsDataGridView.CurrentCell = documentsDataGridView[columnIndex, 0];

							ScrollDataGridView(true);
						}
					}
				}
				else if (e.KeyCode == Keys.End)
				{
					if (!e.Alt && !e.Shift)
					{
						e.Handled = true;

						if (!e.Control)
						{
							var currentCell = documentsDataGridView.CurrentCell;
							if (currentCell != null)
							{
								documentsDataGridView.CurrentCell = documentsDataGridView[documentsDataGridView.ColumnCount - 1, currentCell.RowIndex];

								EnsureCurrentCellVisibleHorizontally();
							}
						}
						else
						{
							if (documentsDataGridView.RowCount > 0)
							{
								var columnIndex = documentsDataGridView.CurrentCell != null ? documentsDataGridView.CurrentCell.ColumnIndex : 0;

								documentsDataGridView.CurrentCell = documentsDataGridView[columnIndex, documentsDataGridView.RowCount - 1];

								ScrollDataGridView(false);
							}
						}
					}
				}
				else if (e.KeyCode == Keys.Tab)
				{
					e.Handled = true;

					var currentCell = documentsDataGridView.CurrentCell;
					if (currentCell != null)
						GotoNextEditableCell(currentCell, false);
				}
				else if (e.KeyCode == Keys.Delete)
				{
					e.Handled = true;

					var currentCell = documentsDataGridView.CurrentCell;
					if (currentCell != null)
					{
						if (currentCell.ColumnIndex == columnCategory.Index)
						{
							var rowView = (DataRowView)currentCell.OwningRow.DataBoundItem;
							var row = (MainDataSet.DocumentsRow)rowView.Row;

							SqlSetColumnValueByPrimaryKey("Category", null, row.ED_ENC_NUM, false, true);

							row.SetCategoryNull();

							row.AcceptChanges();
							UpdateGroupingAppearance();
						}
						else if (currentCell.OwningColumn.Tag != null)
						{
							var rowView = (DataRowView)currentCell.OwningRow.DataBoundItem;
							var row = (MainDataSet.DocumentsRow)rowView.Row;

							var columnName = currentCell.OwningColumn.DataPropertyName;

							SqlSetColumnValueByPrimaryKey(columnName, null, row.ED_ENC_NUM, false, true);

							row[columnName] = DBNull.Value;
							row.AcceptChanges();
							UpdateGroupingAppearance();
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

		private void documentsDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Left)
				{
					if (_views.FreezeSort)
						return;

					///////////////////////////////////////////////////////////////////////////////

					if (e.ColumnIndex != columnNo.Index)
					{
						var gridColumn = documentsDataGridView.Columns[e.ColumnIndex];

						if (_sortOptions == null)
						{
							ClearMultiSort();

							///////////////////////////////////////////////////////////////////////////////

							if (_lastSortColumnIndex == -1)
								_lastSortDirection = ListSortDirection.Ascending;
							else if (_lastSortColumnIndex != e.ColumnIndex)
								_lastSortDirection = ListSortDirection.Ascending;

							var newSortDirection = _lastSortDirection != ListSortDirection.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;

							///////////////////////////////////////////////////////////////////////////////

							_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;

							///////////////////////////////////////////////////////////////////////////////

							documentsDataGridView.Sort(gridColumn, newSortDirection);

							///////////////////////////////////////////////////////////////////////////////

							_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
							_views.MainForm.sourceDocuments.ResetBindings(false);
						}
						else
						{
							if (gridColumn.HeaderCell.Tag is int)
							{
								var index = (int)gridColumn.HeaderCell.Tag - 1;
								if (index >= 0)
								{
									if (index < _sortOptions.SortByColumns.Count)
									{
										var opt = _sortOptions.SortByColumns[index];
										opt.SortOrder = opt.SortOrder != SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending;

										var headerCell = documentsDataGridView.Columns.Cast<DataGridViewColumn>().Select(x => x.HeaderCell).FirstOrDefault(x => x.Tag is int && (int)x.Tag == -1);

										var newSortDirection = headerCell != null && headerCell.SortGlyphDirection != SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending;
										InvokeExternalSort(_sortOptions, headerCell?.OwningColumn.DataPropertyName, newSortDirection);
									}

									return;
								}
							}

							if (_sortOptions.SortGroupsByCriteria != SortGroupsBy.None)
							{
								if (_sortOptions.SortGroupsByCriteria == SortGroupsBy.Sum)
								{
									var dataType = gridColumn.ValueType;
									if (dataType != typeof(int) && dataType != typeof(double) && dataType != typeof(decimal))
										throw new Exception("Cannot apply SUM operation to a non-numeric type");
								}

								var newSortDirection = gridColumn.HeaderCell.SortGlyphDirection != SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending;
								if (InvokeExternalSort(_sortOptions, gridColumn.DataPropertyName, newSortDirection))
								{
									gridColumn.HeaderCell.Tag = -1;
									gridColumn.HeaderCell.SortGlyphDirection = newSortDirection;
								}
							}
						}
					}
				}
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
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void documentsDataGridView_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					var hitTestInfo = documentsDataGridView.HitTest(e.X, e.Y);
					_currentMouseOverRow = hitTestInfo.RowIndex;

					ContextMenu m = new ContextMenu();

					if (_currentMouseOverRow >= 0)
					{
						MenuItem selectCategory = new MenuItem("Select Category");
						selectCategory.Click += menuSelectCategory_Click;
						m.MenuItems.Add(selectCategory);
					}

					if (hitTestInfo.RowIndex == -1 && hitTestInfo.ColumnIndex >= 0)
					{
						var column = documentsDataGridView.Columns[hitTestInfo.ColumnIndex];
						if (column.Tag != null)
						{
							MenuItem bntOpenColumnRegEx = new MenuItem("Column specific RegEx");
							bntOpenColumnRegEx.Click += bntOpenColumnRegEx_Click;
							bntOpenColumnRegEx.Tag = column.Tag;
							m.MenuItems.Add(bntOpenColumnRegEx);

							MenuItem deleteValues = new MenuItem("Delete All Values");
							deleteValues.Click += menuDeleteAllValues_Click;
							deleteValues.Tag = column.Tag;
							m.MenuItems.Add(deleteValues);
						}
					}

					if (hitTestInfo.ColumnIndex == -1 && documentsDataGridView.SelectedRows.Count > 0)
					{
						m.MenuItems.Add("Delete selected row(s)", (s, ev) => DeleteRows());
					}

					m.Show(documentsDataGridView, new Point(e.X, e.Y));
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void documentsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (documentsDataGridView.Rows.Count > 0)
				{
					if (e.RowIndex >= 0)
					{
						var cell = documentsDataGridView[e.ColumnIndex, e.RowIndex];

						if (e.ColumnIndex != columnCategory.Index)
							BeginEditDynamicColumn(cell, false);
						else
							BeginEditStaticCategoryColumn(cell, false);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void documentsDataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			if (documentsDataGridView.Rows.Count > 0)
			{
				if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
				{
					if (e.ColumnIndex == this.columnNo.Index)
						e.Value = e.RowIndex + 1;
				}
			}
		}

		private void documentsDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
		}

		private void documentsDataGridView_Sorted(object sender, EventArgs e)
		{
			
		}

		private void documentsDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			try
			{
				if (e.ColumnIndex < 0)
					return;

				if (e.RowIndex < 0)
				{
					PaintColumnHeader(e);
					return;
				}

				///////////////////////////////////////////////////////////////////////////////

				var highlightSortGroup = false;
				var highlightSortGroupAlternateColor = false;

				if (_sortOptions != null)
				{
					var headerCell = documentsDataGridView.Columns[e.ColumnIndex].HeaderCell;
					var isMultiSorted = headerCell.Tag is int;
					if (isMultiSorted)
					{
						var index = (int)headerCell.Tag - 1;
						if (index >= 0 && index < _sortOptions.SortByColumns.Count)
						{
							var opt = _sortOptions.SortByColumns[index];
							if (opt.GroupSimilar)
							{
								highlightSortGroup = true;
								highlightSortGroupAlternateColor = opt.AlternateColor;
							}
						}
					}
				}

				///////////////////////////////////////////////////////////////////////////////

				var cell = documentsDataGridView[e.ColumnIndex, e.RowIndex];
				var value = cell.Value;

				///////////////////////////////////////////////////////////////////////////////

				Brush backgroundBrush;
				Brush textBrush;

				///////////////////////////////////////////////////////////////////////////////

				if (!highlightSortGroup)
				{
					if (cell.Selected)
					{
						if (documentsDataGridView.CurrentCell == cell && documentsDataGridView.SelectedRows.Count == 1)
						{
							backgroundBrush = Brushes.FloralWhite;
							textBrush = Brushes.Black;
						}
						else
						{
							backgroundBrush = SystemBrushes.Highlight;
							textBrush = Brushes.White;
						}
					}
					else if (e.ColumnIndex == columnCategory.Index && value != DBNull.Value)
					{
						textBrush = Brushes.Black;
						backgroundBrush = Brushes.White;

						if (toolStripButtonHighlightCategories.Checked && _binaryClassificationViewModel != null)
						{
							var column = _binaryClassificationViewModel.columns.FirstOrDefault(x => x.ID == 0);
							if (column != null)
							{
								var cellColumnID = (int)value;
								if (column.positiveCategories.Any(x => x.ID == cellColumnID))
									backgroundBrush = _binaryClassificationViewModel.positiveColor;
								else if (column.excludedCategories.Any(x => x.ID == cellColumnID))
									backgroundBrush = _binaryClassificationViewModel.excludedColor;
								else
									backgroundBrush = _binaryClassificationViewModel.negativeColor;
							}
						}
						else
						{
							if (value != null && value != DBNull.Value)
							{
								var category = _views.MainForm.datasetMain.Categories.FirstOrDefault(x => x.ID == (int) value);
								if (category != null && !category.IsColorNull())
								{
									if (!category.IsColorNull())
										backgroundBrush = GetCachedBrush(category.Color);
								}
							}
						}
					}
					else
					{
						textBrush = Brushes.Black;
						backgroundBrush = Brushes.White;

						var valueStr = value as string;

						var gridColumn = cell.OwningColumn;
						if (gridColumn.Tag != null && valueStr != null)
						{
							var dynamicColumnID = (int)gridColumn.Tag;
							var dynamicColumnRow = _views.MainForm.datasetMain.DynamicColumns.First(x => x.ID == dynamicColumnID);
							var columnType = (DynamicColumnType)dynamicColumnRow.Type;

							///////////////////////////////////////////////////////////////////////////////

							if (columnType == DynamicColumnType.Category)
							{
								var dynamicCategoryRow = _views.MainForm.datasetMain.DynamicColumnCategories.FirstOrDefault(x => x.DynamicColumnID == dynamicColumnID && x.Title == valueStr);
								if (dynamicCategoryRow != null && !dynamicCategoryRow.IsPropertiesNull())
								{
									var props = DynamicCategoryProperties.FromJSON(dynamicCategoryRow.Properties);
									backgroundBrush = GetCachedBrush(props.Background.ToArgb());
								}
							}
						}
					}
				}
				else
				{
					if (cell.Selected)
					{
						if (documentsDataGridView.CurrentCell == cell && documentsDataGridView.SelectedRows.Count == 1)
						{
							backgroundBrush = Brushes.FloralWhite;
							textBrush = Brushes.Black;
						}
						else
						{
							backgroundBrush = SystemBrushes.Highlight;
							textBrush = Brushes.White;
						}
					}
					else
					{
						textBrush = Brushes.Black;

						if (highlightSortGroupAlternateColor)
						{
							Brush brush;

							var lookupValue = value;
							if (lookupValue == null)
								lookupValue = DBNull.Value;

							if (_groupBrushes.TryGetValue(lookupValue, out brush))
								backgroundBrush = brush;
							else
								backgroundBrush = Brushes.LightGray;
						}
						else
							backgroundBrush = Brushes.LightGray;
					}
				}

				///////////////////////////////////////////////////////////////////////////////

				var rc = new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1);
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

					if (cell == documentsDataGridView.CurrentCell)
						e.Graphics.DrawString(formattedValue, _cellFontBold, textBrush, rc, sf);
					else
						e.Graphics.DrawString(formattedValue, _cellFontRegular, textBrush, rc, sf);
				}

				///////////////////////////////////////////////////////////////////////////////

				if (documentsDataGridView.SelectedRows.Count == 1)
				{
					if (cell == documentsDataGridView.CurrentCell && (cell.OwningColumn.Tag != null || cell.ColumnIndex == columnCategory.Index))
					{
						var image = Properties.Resources.editable;
						var width = image.Width + 6;

						var imagePoint = new Point(e.CellBounds.Right - width, e.CellBounds.Top);

						e.Graphics.DrawImage(image, imagePoint);
					}
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

		private void documentsDataGridView_CurrentCellChanged(object sender, EventArgs e)
		{            
			try
			{
				var cell = documentsDataGridView.CurrentCell;
				if (cell != null)
				{
					if (cell.OwningColumn.Tag is int)
						ActivateColumnsRegExTab(cell.OwningColumn.Tag.ToString(), false);


					documentsDataGridView.Focus();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnFilterByCategory_Clicked(object sender, EventArgs e)
		{
			try
			{
				var form = new FormFilterByCategory(_views);
				if (form.ShowDialog() == DialogResult.OK)
					FilterDocumentsByCategory(true);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected void FilterDocumentsByCategory(bool resetBindings)
		{
			var isFiltered = !String.IsNullOrEmpty(_categoryFilter);

			var filter = CategoryFilterViewModel.GetCategoryFilter(_views);

			var selectedCellRow = -1;
			var selectedCellColumn = -1;

			if (documentsDataGridView.CurrentCell != null)
			{
				selectedCellRow = documentsDataGridView.CurrentCell.RowIndex;
				selectedCellColumn = documentsDataGridView.CurrentCell.ColumnIndex;
			}

			if (resetBindings)
				_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;

			var selectedCategoriesList = GetSelectedCategories();
			if (!filter.ShowAllDocuments && (filter.ShowUncategorizedDocuments || selectedCategoriesList.Any() || isFiltered))
			{
				var filterString = String.Empty;

				if (selectedCategoriesList.Any())
				{
					var categoryIDs = selectedCategoriesList.Select(x => "Category = " + x.ID);
					filterString = String.Join(" OR ", categoryIDs);
				}

				if (filter.ShowUncategorizedDocuments)
				{
					if (!String.IsNullOrEmpty(filterString))
						filterString += " OR ";

					filterString += "Category is NULL";
				}

				ApplyCategoryFilter(filterString);
			}
			else
				ApplyCategoryFilter("");

			if (resetBindings)
			{
				_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
				_views.MainForm.sourceDocuments.ResetBindings(false);
			}

			if (selectedCellRow >= 0 && selectedCellRow < documentsDataGridView.RowCount && selectedCellColumn >= 0 && selectedCellColumn < documentsDataGridView.ColumnCount)
				documentsDataGridView.CurrentCell = documentsDataGridView[selectedCellColumn, selectedCellRow];

			///////////////////////////////////////////////////////////////////////////////

			isFiltered = !String.IsNullOrEmpty(_categoryFilter);

			_btnFilterByCategory.Checked = isFiltered;
		}

		private void btnCategoryClasses_Clicked(object sender, EventArgs e)
		{
			//Clear sort
			var bs = (BindingSource)documentsDataGridView.DataSource;
			bs.RemoveSort();

			///////////////////////////////////////////////////////////////////////////////

			var formDynamicColumns = new FormDynamicColumns(_views);
			formDynamicColumns.ShowDialog();

			///////////////////////////////////////////////////////////////////////////////

			_views.BeforeDocumentsTableLoad(true);

			///////////////////////////////////////////////////////////////////////////////

			if (formDynamicColumns.NeedNeedReloadDocuments)
			{
				var formProgress = new FormGenericProgress("Reloading documents...", ReloadDocuments, null, false);
				formProgress.ShowDialog();
			}

			///////////////////////////////////////////////////////////////////////////////

			_views.AfterDocumentsTableLoad(true);

			///////////////////////////////////////////////////////////////////////////////

			RaiseDataModifiedEvent();
		}

		private void bntOpenColumnRegEx_Click(object sender, EventArgs e)
		{
			try
			{
				var menuItem = (MenuItem)sender;

				ActivateColumnsRegExTab(menuItem.Tag.ToString(), true);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnSortScores_Clicked(object sender, EventArgs e)
		{
			try
			{
				var btn = (RibbonButton)sender;
				_views.SortByDocumentScore = btn.Checked;

				if (_views.SortByDocumentScore && !_views.FreezeSort)
					InvokeSortByScore();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnFreezeSort_Click(object sender, EventArgs e)
		{
			try
			{
				var btn = (RibbonButton)sender;

				///////////////////////////////////////////////////////////////////////////////

				if (!_views.FreezeSort)
				{
					if (_lastSortColumnIndex != -1)
					{
						ClearMultiSort();

						var gridColumn = documentsDataGridView.Columns[_lastSortColumnIndex];
						var columnName = gridColumn.DataPropertyName;

						///////////////////////////////////////////////////////////////////////////////

						var formProgress = new FormGenericProgress("Applying sort lock...", FreezeSortOperation, columnName, true);
						formProgress.ShowDialog();

						_views.FreezeSort = true;
					}
					else
					{
						_views.FreezeSort = false;
						MessageBox.Show("Please sort the documents first", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				else
				{
					if (_views.MainForm.adapterDocuments.IsSorted)
					{
						ClearMultiSort();

						var columnName = _views.MainForm.adapterDocuments.SortColumn;
						var gridColumn = documentsDataGridView.Columns.Cast<DataGridViewColumn>()
															  .First(x => String.Equals(x.DataPropertyName, columnName, StringComparison.InvariantCultureIgnoreCase));

						///////////////////////////////////////////////////////////////////////////////

						_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;

						///////////////////////////////////////////////////////////////////////////////

						documentsDataGridView.Sort(gridColumn, _lastSortDirection);

						///////////////////////////////////////////////////////////////////////////////

						_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
						_views.MainForm.sourceDocuments.ResetBindings(false);
					}
					else
					{
						_lastSortColumnIndex = -1;
					}

					_views.FreezeSort = false;
				}

				btn.Checked = _views.FreezeSort;

				RaiseDataModifiedEvent();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnHotkeys_Click(object sender, EventArgs e)
		{
			try
			{
				var form = new FormHotkeys(_views);
				form.ShowDialog();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void menuSelectCategory_Click(object sender, EventArgs e)
		{
			if (documentsDataGridView.SelectedRows.Count > 0)
			{
				var cell = documentsDataGridView.Rows[_currentMouseOverRow]
												.Cells[columnCategory.Index];

				var formSelectCategory = new FormSelectCategory(_views, FormSelectCategory.DisplayMode.Default, cell, false);

				//////////////////////////////////////////////////////////////////////////

				if (formSelectCategory.ShowDialog() == DialogResult.OK)
				{
					try
					{
						_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;

						SaveScrollPosition();

						foreach (DataGridViewRow row in documentsDataGridView.SelectedRows)
						{
							DataRowView rowview = (DataRowView)row.DataBoundItem;
							MainDataSet.DocumentsRow rowDocuments = (MainDataSet.DocumentsRow)rowview.Row;

							var categoryID = formSelectCategory.CategoryID != -1 ? (object)formSelectCategory.CategoryID : null;

							SqlSetColumnValueByPrimaryKey("Category", categoryID, rowDocuments.ED_ENC_NUM, true, false);

							if (formSelectCategory.CategoryID != -1)
								rowDocuments.Category = formSelectCategory.CategoryID;
							else
								rowDocuments.SetCategoryNull();

							rowDocuments.AcceptChanges();
						}

						FilterDocumentsByCategory(false);
					}
					finally
					{
						_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
						_views.MainForm.sourceDocuments.ResetBindings(false);

						RestoreScrollPosition();
					}
				}
			}
		}

		private void menuDeleteAllValues_Click(object sender, EventArgs e)
		{
			try
			{
				var menuItem = (MenuItem)sender;
				var dynamicColumnID = (int)menuItem.Tag;

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

		private void toolStripButtonAutoAssignCategory_Click(object sender, EventArgs e)
		{
			try
			{
				var formAssignCategories = new Forms.FormAssignCategory(_views);
				formAssignCategories.ShowDialog(_views.MainForm);

				//documentsDataGridView.AutoResizeColumn(columnCategory.Index, DataGridViewAutoSizeColumnMode.AllCells);

				documentsDataGridView.Columns.Cast<DataGridViewColumn>()
									 .Where(x => x.Name.StartsWith("Class"))
									 .ToList()
									 .ForEach(x => { documentsDataGridView.AutoResizeColumn(x.Index, DataGridViewAutoSizeColumnMode.AllCells); });
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void toolStripButtonShowMLScores_Click(object sender, EventArgs e)
		{
			try
			{
				ShowMLScoreColumns(toolStripButtonShowMLScores.Checked);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void toolStripButtonHighlightCategories_Click(object sender, EventArgs e)
		{
			if (toolStripButtonHighlightCategories.Checked)
				LoadCategoryColors();

			documentsDataGridView.Refresh();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			try
			{
				_timer.Stop();

				InvokeFilterByCategories();

				///////////////////////////////////////////////////////////////////////////////

				if (_views.SortByDocumentScore && !_views.FreezeSort)
					InvokeSortByScore();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void sourceDocuments_ListChanged(object sender, ListChangedEventArgs e)
		{
			try
			{
				_btnFilterByCategory.Checked = !String.IsNullOrEmpty(_categoryFilter);

				///////////////////////////////////////////////////////////////////////////////

				if (_views.FreezeSort)
				{
					if (_lastSortColumnIndex != -1)
					{
						var gridColumn = documentsDataGridView.Columns[_lastSortColumnIndex];
						if (gridColumn.HeaderCell.SortGlyphDirection != SortOrder.None)
							gridColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
					}
					else if (documentsDataGridView.SortedColumn != null)
					{
						if (documentsDataGridView.SortedColumn.HeaderCell.SortGlyphDirection != SortOrder.None)
							documentsDataGridView.SortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
					}

					///////////////////////////////////////////////////////////////////////////////

					return;
				}

				///////////////////////////////////////////////////////////////////////////////

				DataGridViewColumn sortedColumn;

				///////////////////////////////////////////////////////////////////////////////

				if (_views.MainForm.sourceDocuments.IsSorted && _sortOptions == null)
				{
					var columnName = _views.MainForm.sourceDocuments.SortProperty?.Name;

					///////////////////////////////////////////////////////////////////////////////

					sortedColumn = documentsDataGridView.Columns.Cast<DataGridViewColumn>()
														.FirstOrDefault(x => String.Equals(x.DataPropertyName, columnName, StringComparison.InvariantCultureIgnoreCase));

					///////////////////////////////////////////////////////////////////////////////

					if (sortedColumn != null)
					{
						_lastSortColumnIndex = sortedColumn.Index;
						_lastSortDirection = _views.MainForm.sourceDocuments.SortDirection;

						_sortedByCategoryValue = false;
					}
					else
					{
						_lastSortColumnIndex = -1;
						_lastSortDirection = _views.MainForm.sourceDocuments.SortDirection;
					}
				}
				else if (_views.MainForm.adapterDocuments.IsSorted)
				{
					var columnName = _views.MainForm.adapterDocuments.SortColumn;

					///////////////////////////////////////////////////////////////////////////////

					sortedColumn = documentsDataGridView.Columns.Cast<DataGridViewColumn>()
														.FirstOrDefault(x => String.Equals(x.DataPropertyName, columnName, StringComparison.InvariantCultureIgnoreCase));

					///////////////////////////////////////////////////////////////////////////////

					if (sortedColumn != null)
					{
						_lastSortColumnIndex = sortedColumn.Index;
						_lastSortDirection = _views.MainForm.adapterDocuments.SortDirection;

						_sortedByCategoryValue = false;
					}
				}
				else
				{
					_lastSortColumnIndex = -1;
					_lastSortDirection = ListSortDirection.Ascending;

					sortedColumn = null;

					_sortedByCategoryValue = false;
				}

				///////////////////////////////////////////////////////////////////////////////

				UpdateSortByCategoryValueButton(_sortedByCategoryValue ? (ListSortDirection?)_lastSortDirection : null);

				if (sortedColumn != null)
				{
					var gridColumnGlyphOrder = _lastSortDirection == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;

					///////////////////////////////////////////////////////////////////////////////

					if (documentsDataGridView.SortedColumn != sortedColumn)
					{
						if (documentsDataGridView.SortedColumn != null)
							documentsDataGridView.SortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;

						sortedColumn.HeaderCell.SortGlyphDirection = gridColumnGlyphOrder;
					}
					else if (documentsDataGridView.SortedColumn.HeaderCell.SortGlyphDirection != gridColumnGlyphOrder)
						documentsDataGridView.SortedColumn.HeaderCell.SortGlyphDirection = gridColumnGlyphOrder;
				}

				///////////////////////////////////////////////////////////////////////////////

				if (!_isSortingGroupsInProgress && _documentsTableReloaded && _sortOptions != null && !String.IsNullOrEmpty(_sortGroupsByColumn))
				{
					var headerCell = documentsDataGridView.Columns.Cast<DataGridViewColumn>().Select(x => x.HeaderCell).FirstOrDefault(x => x.Tag is int && (int)x.Tag == -1);
					if (headerCell != null)
						InvokeExternalSort(_sortOptions, _sortGroupsByColumn, _sortGroupsByColumnDirection);
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void toolStripButtonConvertToDynamic_Click(object sender, EventArgs e)
		{
			try
			{
				var formConvert = new FormConvertToDynamic(_views);
				formConvert.ShowDialog();

				///////////////////////////////////////////////////////////////////////////////

				_views.BeforeDocumentsTableLoad(true);

				var formProgress = new FormGenericProgress("Reloading documents...", ReloadDocuments, null, false);
				formProgress.ShowDialog();

				_views.AfterDocumentsTableLoad(true);
			}
			catch (System.Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void toolStripButtonColumns_Click(object sender, EventArgs e)
		{
			try
			{
				var form = new FormColumns(_views, GetColumns(false));
				if (form.ShowDialog() == DialogResult.OK)
				{
					SortColumns(false);
					RaiseDataModifiedEvent();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnTextHighlight_Click(object sender, EventArgs e)
		{
			try
			{
				var currentColumnID = 0;

				var cell = documentsDataGridView.CurrentCell;
				if (cell != null)
				{
					if (cell.OwningColumn.Tag is int)
						currentColumnID = (int)cell.OwningColumn.Tag;
				}

				var form = new FormColumnHighlight(_views, currentColumnID);
				if (form.ShowDialog() == DialogResult.OK)
					_views.UpdateViews();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnSortByCategory_Click(object sender, EventArgs e)
		{
			try
			{
				if (!_views.FreezeSort)
				{
					ClearMultiSort();

					var formProgress = new FormGenericProgress("Sorting by Category value", SortByCategoryOperation, null, true);
					formProgress.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void documentsDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && documentsDataGridView.IsCurrentCellDirty)
				{
					var column = documentsDataGridView.Columns[e.ColumnIndex];
					if (column.Tag is int)
					{
						var row = documentsDataGridView.Rows[e.RowIndex];
						if (e.FormattedValue != DBNull.Value && e.FormattedValue != null)
						{
							var valueStr = (string)e.FormattedValue;

							var rowView = (DataRowView)row.DataBoundItem;
							var docRow = (MainDataSet.DocumentsRow)rowView.Row;

							///////////////////////////////////////////////////////////////////////////////

							var dynamicColumn = _views.MainForm.datasetMain.DynamicColumns.Rows.Cast<MainDataSet.DynamicColumnsRow>()
								  .FirstOrDefault(x => x.ID == (int)column.Tag);

							var reloadColumns = false;

							if (dynamicColumn != null)
							{
								object newValue;
								if (dynamicColumn.Type == (int)DynamicColumnType.Numeric)
								{
									if (valueStr != null && !String.IsNullOrEmpty(valueStr.Trim()))
									{
										var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

										valueStr = valueStr.Replace(",", separator);
										valueStr = valueStr.Replace(".", separator);

										var doubleVal = double.Parse(valueStr);

										var props = NumericColumnProperties.Load(dynamicColumn);
										if (!props.IsDecimal && valueStr.Contains(separator))
										{
											var split = valueStr.Split(new[] { separator }, StringSplitOptions.None);
											if (split.Length == 2)
												props.DecimalPlaces = split[1].Length;

											reloadColumns = PromptToConvertIntegerColumnToDecimal(dynamicColumn, props);
										}

										if (!props.IsDecimal && !reloadColumns)
											doubleVal = Convert.ToInt32(doubleVal);

										if (!props.IsValidValue(Convert.ToDecimal(doubleVal)))
										{
											MessageBox.Show("Specified value does not match the allowed range", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
											e.Cancel = true;
											return;
										}
										else
											newValue = doubleVal;
									}
									else
										newValue = DBNull.Value;
								}
								else
									newValue = valueStr;

								SqlSetColumnValueByPrimaryKey(column.DataPropertyName, newValue, docRow.ED_ENC_NUM, false, true);

								docRow[column.Name] = newValue;
								docRow.AcceptChanges();

								UpdateGroupingAppearance();

								if (reloadColumns)
								{
									SynchronizationContext.Current.Post(x =>
									{
										_views.BeforeDocumentsTableLoad(true);
										_views.AfterDocumentsTableLoad(true);
									}, null);
								}
							}
							else
								e.Cancel = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				e.Cancel = true;

				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected bool PromptToConvertIntegerColumnToDecimal(MainDataSet.DynamicColumnsRow dynamicColumn, NumericColumnProperties props)
		{
			var dlgres = MessageBox.Show("You entered decimal value into integer column. Do you wish to convert column to decimal?", MainForm.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
			if (dlgres == DialogResult.Yes)
			{
				props.IsDecimal = true;
				dynamicColumn.Properties = props.Save();
				_views.MainForm.adapterDynamicColumns.Update(dynamicColumn);

				return true;
			}

			return false;
		}

		private void documentsDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			try
			{
				e.Control.KeyPress -= editBox_KeyPress;

				var tag = documentsDataGridView.CurrentCell.OwningColumn.Tag;
				if (tag is int)
				{
					var columnID = (int)tag;
					var dynamicColumn = _views.MainForm.datasetMain.DynamicColumns.Rows.Cast<MainDataSet.DynamicColumnsRow>()
											  .FirstOrDefault(x => x.ID == columnID);

					if (dynamicColumn != null)
					{
						if (dynamicColumn.Type == (int)DynamicColumnType.Numeric)
						{
							var tb = e.Control as TextBox;
							if (tb != null)
								tb.KeyPress += editBox_KeyPress;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void editBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
					e.Handled = true;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnExternalSort_Click(object sender, EventArgs e)
		{
			try
			{
				if (_views.FreezeSort)
					throw new Exception("Turn off sort lock first");

				var viewData = PaneDocumentsViewData.Load(_views);

				var formExteralSort = new FormExternalSort(_views, viewData.SortOptions);
				var dlgres = formExteralSort.ShowDialog();
				if (dlgres == DialogResult.OK)
				{
					var headerCell = documentsDataGridView.Columns.Cast<DataGridViewColumn>().Select(x => x.HeaderCell).FirstOrDefault(x => x.Tag is int && (int)x.Tag == -1);
					var newSortDirection = headerCell != null ? headerCell.SortGlyphDirection : SortOrder.None;

					InvokeExternalSort(formExteralSort.GetSortOptions(true), headerCell?.OwningColumn.DataPropertyName, newSortDirection);
				}
				else if (dlgres == DialogResult.Abort)
					ClearMultiSort();

			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Operations

		public void ShowMLScoreColumns(bool show)
		{
			columnProc1SVM.Visible = show;

			toolStripButtonShowMLScores.Checked = show;

			SortColumns(false);
		}

		public void ScrollToDynamicColumn(int dynamicColumnID)
		{
			var gridColumn = documentsDataGridView.Columns.Cast<DataGridViewColumn>()
												  .FirstOrDefault(x => x.Tag is int && (int)x.Tag == dynamicColumnID);

			if (gridColumn != null)
				documentsDataGridView.FirstDisplayedScrollingColumnIndex = gridColumn.Index;
		}

		#endregion

		#region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			base.InitPane(views, ownerView, panel, tab);

			InitDataGrid();

			LoadCategoryColors();

			if (ownerView.ViewType.Name == "Columns RegEx")
				btnTextHighlight.Visible = true;

			_views.MainForm.sourceDocuments.ListChanged += sourceDocuments_ListChanged;

			documentsDataGridView.Select();

			_views.MainForm.adapterDocuments.OnAfterDocumentsTableFilled += OnAfterDocumentsTableFilled;

			_timer.Start();
		}

		public override void DestroyPane()
		{
			_views.MainForm.sourceDocuments.ListChanged -= sourceDocuments_ListChanged;

			_views.MainForm.adapterDocuments.OnAfterDocumentsTableFilled -= OnAfterDocumentsTableFilled;

			base.DestroyPane();
		}

		protected override void InitPaneCommands(RibbonTab tab)
		{
			_panel = new RibbonPanel("Documents");
			tab.Panels.Add(_panel);

			///////////////////////////////////////////////////////////////////////////////

			_btnFilterByCategory = new RibbonButton("Filter by Category");
			_panel.Items.Add(_btnFilterByCategory);

			_btnFilterByCategory.Image = Properties.Resources.FilterByCategory;
			_btnFilterByCategory.SmallImage = Properties.Resources.FilterByCategory;
			_btnFilterByCategory.Click += new EventHandler(btnFilterByCategory_Clicked);
			_btnFilterByCategory.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			///////////////////////////////////////////////////////////////////////////////

			var btnDynamicColumns = new RibbonButton("Columns");
			_panel.Items.Add(btnDynamicColumns);

			btnDynamicColumns.Image = Properties.Resources.CategoryClasses;
			btnDynamicColumns.SmallImage = Properties.Resources.CategoryClasses;
			btnDynamicColumns.Click += new EventHandler(btnCategoryClasses_Clicked);
			btnDynamicColumns.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			///////////////////////////////////////////////////////////////////////////////

			/*if (_views.ShowOldCategoryColumn)
			{
				RibbonButton btnAddCategory = new RibbonButton("Add Category");
				_panel.Items.Add(btnAddCategory);

				btnAddCategory.Image = Properties.Resources.AddCategory;
				btnAddCategory.SmallImage = Properties.Resources.AddCategory;
				btnAddCategory.Click += new EventHandler(btnAddCategory_Clicked);
				btnAddCategory.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
			}*/

			///////////////////////////////////////////////////////////////////////////////

			_panel.Items.Add(new RibbonSeparator());

			///////////////////////////////////////////////////////////////////////////////

			_btnSortScores = new RibbonButton("Sort Documents by Score");
			_panel.Items.Add(_btnSortScores);

			_btnSortScores.Image = Properties.Resources.AutoSort;
			_btnSortScores.SmallImage = Properties.Resources.AutoSort;
			//_btnSortScores.ToolTip = "Sort Documents by Score";
			//_btnSortScores.ToolTipTitle = "Sort";
			_btnSortScores.CheckOnClick = true;
			_btnSortScores.Checked = _views.SortByDocumentScore;
			_btnSortScores.Click += new EventHandler(btnSortScores_Clicked);
			_btnSortScores.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			///////////////////////////////////////////////////////////////////////////////

			_btnFreezeSort = new RibbonButton("Lock Sort");
			_panel.Items.Add(_btnFreezeSort);

			_btnFreezeSort.Image = Properties.Resources._lock;
			_btnFreezeSort.SmallImage = Properties.Resources._lock;
			_btnFreezeSort.CheckOnClick = true;
			_btnFreezeSort.Checked = _views.FreezeSort;
			_btnFreezeSort.Click += new EventHandler(btnFreezeSort_Click);
			_btnFreezeSort.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			///////////////////////////////////////////////////////////////////////////////

			_panel.Items.Add(new RibbonSeparator());

			///////////////////////////////////////////////////////////////////////////////

			var btnHotkeys = new RibbonButton("Hotkeys");
			_panel.Items.Add(btnHotkeys);

			btnHotkeys.Image = Properties.Resources.hotkeys;
			btnHotkeys.SmallImage = Properties.Resources.hotkeys;
			btnHotkeys.Click += new EventHandler(btnHotkeys_Click);
			btnHotkeys.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
		}

		public override void UpdatePane()
		{
			base.UpdatePane();

			///////////////////////////////////////////////////////////////////////////////

			documentsDataGridView.Refresh();

			ApplyDocumentsFilters();

			///////////////////////////////////////////////////////////////////////////////

			_btnFreezeSort.Checked = _views.FreezeSort;
		}

		public override bool OnHotkey(string code)
		{
			var currentCell = documentsDataGridView.CurrentCell;
			if (currentCell == null)
				return base.OnHotkey(code);

			///////////////////////////////////////////////////////////////////////////////

			if (code == HotkeyCode_SelectCategory)
			{
				BeginEditStaticCategoryColumn(documentsDataGridView.CurrentCell, false);

				return true;
			}

			///////////////////////////////////////////////////////////////////////////////

			if (code == HotkeyCode_EditValue)
			{
				if (currentCell.OwningColumn.Tag == null)
					GotoNextEditableCell(currentCell, true);
				else
					BeginEditDynamicColumn(currentCell, false);

				return true;
			}

			///////////////////////////////////////////////////////////////////////////////

			return base.OnHotkey(code);
		}

		#endregion

		#region Implementation: general

		public void BeforeDocumentsTableLoad(bool removeColumns)
		{
			SaveScrollPosition();

			if (removeColumns)
				RemoveColumnsFromGrid();
		}

		public void AfterDocumentsTableLoad(bool addColumns)
		{
			if (documentsDataGridView.DataSource == null)
				documentsDataGridView.DataSource = GetDocumentsDataSource();

			if (addColumns)
			{
				AddExtraColumnsToGrid();
				AddDynamicColumnsToGrid();
			}

			RestoreScrollPosition();
		}

		protected void SaveScrollPosition()
		{
			var currentCell = documentsDataGridView.CurrentCell;
			_currentRowIndex = currentCell?.RowIndex ?? -1;
			_currentColumnIndex = currentCell?.ColumnIndex ?? -1;

			_scrollRowIndex = documentsDataGridView.FirstDisplayedScrollingRowIndex;
			_scrollColumnIndex = documentsDataGridView.FirstDisplayedScrollingColumnIndex;
		}

		protected void RestoreScrollPosition()
		{
			if (_currentRowIndex >= 0 && _currentRowIndex < documentsDataGridView.Rows.Count && _currentColumnIndex >= 0 && _currentColumnIndex < documentsDataGridView.Columns.Count)
			{
				var cell = documentsDataGridView[_currentColumnIndex, _currentRowIndex];
				if (cell.OwningRow.Visible && cell.OwningColumn.Visible)
					documentsDataGridView.CurrentCell = cell;
			}

			if (_scrollRowIndex >= 0 && _scrollRowIndex < documentsDataGridView.Rows.Count && documentsDataGridView.FirstDisplayedScrollingRowIndex != _scrollRowIndex)
			{
				if (documentsDataGridView.Rows[_scrollRowIndex].Visible)
					documentsDataGridView.FirstDisplayedScrollingRowIndex = _scrollRowIndex;
			}

			if (_scrollColumnIndex >= 0 && _scrollColumnIndex < documentsDataGridView.Columns.Count && documentsDataGridView.FirstDisplayedScrollingColumnIndex != _scrollColumnIndex)
			{
				if (documentsDataGridView.Columns[_scrollColumnIndex].Visible)
					documentsDataGridView.FirstDisplayedScrollingColumnIndex = _scrollColumnIndex;
			}
		}

		protected BindingSource GetDocumentsDataSource()
		{
			return _views.MainForm.sourceDocuments;
		}

		protected void InitDataGrid()
		{
			documentsDataGridView.AutoGenerateColumns = false;
			documentsDataGridView.DataSource = GetDocumentsDataSource();

			///////////////////////////////////////////////////////////////////////////////

			AddExtraColumnsToGrid();
			AddDynamicColumnsToGrid();
		}

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
				}
			}
			catch
			{
			}
		}

		protected DataGridViewColumn CreateColumnByType(Type clrType)
		{
			if (clrType == typeof(bool))
				return new DataGridViewCheckBoxColumn();

			///////////////////////////////////////////////////////////////////////////////

			if (clrType == typeof(DateTime))
			{
				var dataGridViewColumn = new DataGridViewTextBoxColumn();

				var props = DateTimeColumnProperties.Load(null);
				dataGridViewColumn.DefaultCellStyle.Format = props.GetFormatString();
				dataGridViewColumn.DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.InvariantCulture;

				return dataGridViewColumn;
			}

			///////////////////////////////////////////////////////////////////////////////

			return new DataGridViewTextBoxColumn();
		}

		protected List<MainDataSet.CategoriesRow> GetSelectedCategories()
		{
			return _views.MainForm.datasetMain.Categories.Where(x => !x.IsIsSelectedNull() && x.IsSelected)
						 .ToList();
		}

		protected bool IsAllCategoriesSelected()
		{
			return _views.MainForm.datasetMain.Categories.All(x => !x.IsIsSelectedNull() && x.IsSelected);
		}

		protected void DeleteRows()
		{
			try
			{
				var selectedRowsCount = documentsDataGridView.SelectedRows.Count;

				if (selectedRowsCount > 0)
				{
					var dlgres = MessageBox.Show("Do you want to delete selected documents?", MainForm.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
					if (dlgres == DialogResult.Yes)
					{
						if (selectedRowsCount > 0)
							_views.MainForm.adapterDocuments.StartBatchQuery();

						///////////////////////////////////////////////////////////////////////////////

						try
						{
							foreach (var gridRow in documentsDataGridView.SelectedRows.Cast<DataGridViewRow>()
															 .ToList())
							{
								var rowView = (DataRowView)gridRow.DataBoundItem;
								var row = (MainDataSet.DocumentsRow)rowView.Row;

								///////////////////////////////////////////////////////////////////////////////

								_views.MainForm.adapterDocuments.Delete(row.ED_ENC_NUM);

								_views.MainForm.datasetMain.Documents.Rows.Remove(row);
							}
						}
						finally
						{
							if (selectedRowsCount > 0)
								_views.MainForm.adapterDocuments.EndBatchQuery();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
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

		#region Implementation: dynamic columns

		protected bool ReloadDocuments(BackgroundWorker worker, object argument)
		{
			try
			{
				_views.MainForm.adapterDocuments.Fill();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			return true;
		}

		protected void AddDynamicColumnsToGrid()
		{
			var join = from row in _views.MainForm.datasetMain.DynamicColumns.Rows.Cast<MainDataSet.DynamicColumnsRow>()
					   join ci in _views.MainForm.adapterDocuments.GetDynamicColumnsCollection()
						   on row.ID equals ci.DynamicColumnID
					   orderby row.Order
					   select row;

			///////////////////////////////////////////////////////////////////////////////

			foreach (var dynamicColumnRow in join)
			{
				var dataPropertyName = dynamicColumnRow.Title;
				if (_views.MainForm.datasetMain.Documents.Columns.Contains(dataPropertyName))
				{
					if (!documentsDataGridView.Columns.Contains(dataPropertyName))
					{
						var dynamicGridColumn = new DataGridViewTextBoxColumn
						{
							ReadOnly = dynamicColumnRow.Type != (int)DynamicColumnType.FreeText && dynamicColumnRow.Type != (int)DynamicColumnType.Numeric,
							DataPropertyName = dataPropertyName,
							HeaderText = dataPropertyName,
							AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
							HeaderCell = { Style = { Font = new Font("Arial", 11) } },
							DefaultCellStyle = { Font = new Font("Arial", 10) },
							SortMode = DataGridViewColumnSortMode.Programmatic,
							Tag = dynamicColumnRow.ID,
							Name = dataPropertyName
						};

						///////////////////////////////////////////////////////////////////////////////

						var columnType = (DynamicColumnType)dynamicColumnRow.Type;
						if (columnType == DynamicColumnType.Numeric)
						{
							var props = NumericColumnProperties.Load(dynamicColumnRow);
							dynamicGridColumn.DefaultCellStyle.Format = props.GetFormatString();
						}
						else if (columnType == DynamicColumnType.DateTime)
						{
							var props = DateTimeColumnProperties.Load(dynamicColumnRow);
							dynamicGridColumn.DefaultCellStyle.Format = props.GetFormatString();
							dynamicGridColumn.DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.InvariantCulture;
						}

						///////////////////////////////////////////////////////////////////////////////

						documentsDataGridView.Columns.Add(dynamicGridColumn);

						///////////////////////////////////////////////////////////////////////////////

						//documentsDataGridView.AutoResizeColumn(dynamicGridColumn.Index, DataGridViewAutoSizeColumnMode.AllCells);
					}
					else
						documentsDataGridView.Columns[dataPropertyName].Tag = dynamicColumnRow.ID;
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			SortColumns(true);

			///////////////////////////////////////////////////////////////////////////////

			if (documentsDataGridView.RowCount > 0)
			{
				var firstVisibleColumn = documentsDataGridView.Columns.Cast<DataGridViewColumn>()
															  .Where(x => x.Visible)
															  .OrderBy(x => x.DisplayIndex)
															  .FirstOrDefault();

				if (firstVisibleColumn != null)
					documentsDataGridView.CurrentCell = documentsDataGridView[firstVisibleColumn.Index, 0];

				documentsDataGridView.Select();
				documentsDataGridView.Focus();
			}
		}

		protected void AddExtraColumnsToGrid()
		{
			this.columnID.DataPropertyName = "ED_ENC_NUM";
			this.columnID.HeaderText = "ED_ENC_NUM";
			this.columnID.Name = "dataGridViewTextBoxColumn7";
			this.columnID.ReadOnly = true;
			this.columnID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
			this.columnID.Width = 110;

			this.columnScore.DataPropertyName = "Score";
			this.columnScore.HeaderText = "Score";
			this.columnScore.MaxInputLength = 100;
			this.columnScore.Name = "dataGridViewTextBoxColumn9";
			this.columnScore.ReadOnly = true;
			this.columnScore.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
			this.columnScore.Width = 55;

			this.columnRank.DataPropertyName = "Rank";
			this.columnRank.HeaderText = "Rank";
			this.columnRank.MaxInputLength = 100;
			this.columnRank.Name = "columnRank";
			this.columnRank.ReadOnly = true;
			this.columnRank.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
			this.columnRank.Width = 110;

			this.columnProc3SVM.DataPropertyName = "Proc3SVM";
			this.columnProc3SVM.HeaderText = "SVM Score";
			this.columnProc3SVM.MaxInputLength = 100;
			this.columnProc3SVM.Name = "columnProc3SVM";
			this.columnProc3SVM.ReadOnly = true;
			this.columnProc3SVM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
			this.columnProc3SVM.Width = 110;

			this.columnProc1SVM.DataPropertyName = "Proc1SVM";
			this.columnProc1SVM.HeaderText = "SVM Score";
			this.columnProc1SVM.MaxInputLength = 100;
			this.columnProc1SVM.Name = "columnProc1SVM";
			this.columnProc1SVM.ReadOnly = true;
			this.columnProc1SVM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
			this.columnProc1SVM.Width = 55;
			this.columnProc1SVM.Visible = false;
            this.columnProc1SVM.DisplayIndex = 0;

			this.columnCategory.DataPropertyName = "Category";
			this.columnCategory.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
			this.columnCategory.DataSource = _views.MainForm.sourceCategories;
			this.columnCategory.DisplayMember = "Category";
			this.columnCategory.FlatStyle = FlatStyle.Flat;
			this.columnCategory.HeaderText = "Category";
			this.columnCategory.MaxDropDownItems = 18;
			this.columnCategory.Name = "Category";
			this.columnCategory.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.columnCategory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
			this.columnCategory.ValueMember = "ID";
			this.columnCategory.Width = 130;

			///////////////////////////////////////////////////////////////////////////////

			this.documentsDataGridView.Columns.AddRange(this.columnNo, this.columnID, this.columnScore, this.columnProc1SVM);

			///////////////////////////////////////////////////////////////////////////////

			if (_views.ShowOldCategoryColumn)
				this.documentsDataGridView.Columns.Add(columnCategory);

			///////////////////////////////////////////////////////////////////////////////

			var dataColumnsList = _views.MainForm.datasetMain.Documents.Columns.Cast<DataColumn>()
										.ToList();

			var extraColumnsList = _views.MainForm.adapterDocuments.GetExtraColumnsCollection()
										 .ToList();

            int displayIndex = 4;

			foreach (var columnInfo in extraColumnsList)
			{
				try
				{
					var dataColumn = dataColumnsList.FirstOrDefault(x => String.Equals(x.ColumnName, columnInfo.Name, StringComparison.InvariantCultureIgnoreCase));
					if (dataColumn != null)
					{
						var dataGridViewColumn = CreateColumnByType(columnInfo.Type);

						dataGridViewColumn.DataPropertyName = columnInfo.Name;
						dataGridViewColumn.HeaderText = columnInfo.Name;
						dataGridViewColumn.ReadOnly = true;

						dataGridViewColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
						dataGridViewColumn.HeaderCell.Style.Font = new Font("Arial", 11);
						dataGridViewColumn.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 10);
						dataGridViewColumn.SortMode = DataGridViewColumnSortMode.Programmatic;
						dataGridViewColumn.Name = columnInfo.Name;
                        
                        if (dataGridViewColumn.Name.StartsWith("Score"))
                        {
                            dataGridViewColumn.DisplayIndex = displayIndex;
                            this.documentsDataGridView.Columns.Insert(displayIndex, dataGridViewColumn);
                            displayIndex ++;
                        } else
                        {
                            this.documentsDataGridView.Columns.Add(dataGridViewColumn);
                        }						
					}
					else
						throw new Exception(String.Format("Failed to add column '{0}' of type '{1}'", columnInfo.Name, columnInfo.Type));
				}
				catch (Exception ex)
				{
					MainForm.ShowExceptionMessage(ex);
				}
			}


			///////////////////////////////////////////////////////////////////////////////

			foreach (DataGridViewColumn column in this.documentsDataGridView.Columns)
			{
				column.HeaderCell.Style.Font = new Font("Arial", 11);
				column.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 10);
			}
		}

		protected void SortColumns(bool initial)
		{
			var columns = GetColumns(initial);

			for (var i = 0; i < columns.Count; i++)
			{
				try
				{
					var col = columns[i];
					if (col.Settings != null)
						col.GridColumn.Visible = col.Settings.IsVisible;
					else
						col.GridColumn.Visible = true;

					col.GridColumn.DisplayIndex = i;
				}
				catch (Exception ex)
				{
					ex.ToString();
				}
			}

			var showMLScores = columns.FirstOrDefault(x => x.SqlColumn.Name == "Proc1SVM");
			if (showMLScores != null && showMLScores.Settings != null)
			{
				showMLScores.GridColumn.Visible = showMLScores.Settings.IsVisible;
				toolStripButtonShowMLScores.Checked = showMLScores.Settings.IsVisible;
			}

			_views.MainForm.datasetMain.Columns.RejectChanges();
		}

		protected List<ColumnSettings> GetColumns(bool initial)
		{
			var allColumns = _views.MainForm.adapterDocuments.GetActualColumnsList();

			var visibleColumns = (from sqlCol in allColumns
								  join gridCol in documentsDataGridView.Columns.Cast<DataGridViewColumn>()
									  on sqlCol.Name.ToLower() equals gridCol.DataPropertyName.ToLower()
								  orderby sqlCol.SqlColumnIndex
								  select new ColumnSettings
								  {
									  SqlColumn = sqlCol,
									  GridColumn = gridCol
								  }).ToList();

			var virtualColumn = new ColumnSettings
			{
				GridColumn = columnNo,
				SqlColumn = new ColumnInfo { Name = "" }
			};

			visibleColumns.Insert(0, virtualColumn);

			var columnsWithOptions = (from col in visibleColumns
									  join row in _views.MainForm.datasetMain.Columns.Rows.Cast<MainDataSet.ColumnsRow>()
										  on col.SqlColumn.Name equals row.Name
										  into tmp
									  from setting in tmp.DefaultIfEmpty()
									  select new ColumnSettings
									  {
										  SqlColumn = col.SqlColumn,
										  GridColumn = col.GridColumn,
										  Settings = setting
									  }).ToList();

			if (!initial)
			{
				var showMLScores = columnsWithOptions.FirstOrDefault(x => x.SqlColumn.Name == "Proc1SVM");
				if (showMLScores != null)
				{
					if (showMLScores.Settings == null)
						showMLScores.Settings = _views.MainForm.datasetMain.Columns.AddColumnsRow(showMLScores.SqlColumn.Name, showMLScores.GridColumn.Index, toolStripButtonShowMLScores.Checked);
					else
						showMLScores.Settings.IsVisible = toolStripButtonShowMLScores.Checked;
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			columnsWithOptions = columnsWithOptions.OrderBy(x => x.Settings != null ? x.Settings.Order : 1000).ToList();

			for (var i = 0; i < columnsWithOptions.Count; i++)
			{
				var column = columnsWithOptions[i];
				if (column.SqlColumn.IsDynamic)
				{
					var dynamicColumn = _views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.ID == column.SqlColumn.DynamicColumnID);
					if (dynamicColumn != null && dynamicColumn.Type == (int)DynamicColumnType.Category)
					{
						var svmColumnName = column.SqlColumn.Name + " (SVM)";
						var svmColumnIndex = columnsWithOptions.FindIndex(x => String.Compare(x.SqlColumn.Name, svmColumnName, StringComparison.InvariantCultureIgnoreCase) == 0);
						if (svmColumnIndex != -1)
						{
							var svmColumn = columnsWithOptions[svmColumnIndex];

							//if (!initial)
							{
								if (svmColumn.Settings == null)
									svmColumn.Settings = _views.MainForm.datasetMain.Columns.AddColumnsRow(svmColumn.SqlColumn.Name, svmColumn.GridColumn.Index, toolStripButtonShowMLScores.Checked);

								//svmColumn.Settings.IsVisible = toolStripButtonShowMLScores.Checked;
							}

							if (svmColumnIndex != i + 1)
							{
								columnsWithOptions.RemoveAt(svmColumnIndex);
								if (svmColumnIndex < i)
									columnsWithOptions.Insert(i, svmColumn);
								else
									columnsWithOptions.Insert(i + 1, svmColumn);
							}
						}
					}
				}
			}

			return columnsWithOptions;
		}

		protected void RemoveColumnsFromGrid()
		{
			documentsDataGridView.Columns.Cast<DataGridViewColumn>()
								 .ToList()
								 .ForEach(x => documentsDataGridView.Columns.Remove(x));
		}

		protected bool DoDeleteColumnValues(BackgroundWorker worker, object objArgument)
		{
			var dynamicColumnRow = (MainDataSet.DynamicColumnsRow)objArgument;

			///////////////////////////////////////////////////////////////////////////////

			_views.MainForm.adapterDocuments.SqlClearColumnValues(dynamicColumnRow.Title, null, null, ExpressionType.Default);

			///////////////////////////////////////////////////////////////////////////////

			foreach (var row in _views.MainForm.datasetMain.Documents)
			{
				row[dynamicColumnRow.Title] = DBNull.Value;
			}

			_views.MainForm.datasetMain.Documents.AcceptChanges();

			return true;
		}

		protected void RedrawCurrentCell()
		{
			if (documentsDataGridView.CurrentCell != null)
			{
				documentsDataGridView.InvalidateCell(documentsDataGridView.CurrentCell);
				documentsDataGridView.Update();
			}
		}

		protected bool BeginEditColumn(DataGridViewCell cell)
		{
			if (cell != null)
			{
				var gridColumn = cell.OwningColumn;

				///////////////////////////////////////////////////////////////////////////////

				if (gridColumn.Index == columnCategory.Index)
				{
					BeginEditStaticCategoryColumn(cell, false);

					return true;
				}

				///////////////////////////////////////////////////////////////////////////////

				if (gridColumn.Tag != null)
				{
					BeginEditDynamicColumn(cell, false);

					return true;
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			return false;
		}

		protected void BeginEditStaticCategoryColumn(DataGridViewCell cell, bool triggeredByCtrl)
		{
			var documentRow = GetDocumentRow(cell);

			///////////////////////////////////////////////////////////////////////////////

			var formSelectCategory = new FormSelectCategory(_views, FormSelectCategory.DisplayMode.Default, cell, triggeredByCtrl);

			if (!documentRow.IsCategoryNull())
				formSelectCategory.CategoryID = documentRow.Category;

			//////////////////////////////////////////////////////////////////////////

			if (formSelectCategory.ShowDialog() == DialogResult.OK)
			{
				try
				{
					_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;

					SaveScrollPosition();

					var categoryID = formSelectCategory.CategoryID != -1 ? (object)formSelectCategory.CategoryID : null;

					SqlSetColumnValueByPrimaryKey("Category", categoryID, documentRow.ED_ENC_NUM, false, false);

					if (formSelectCategory.CategoryID != -1)
						documentRow.Category = formSelectCategory.CategoryID;
					else
						documentRow.SetCategoryNull();

					documentRow.AcceptChanges();

					FilterDocumentsByCategory(false);
				}
				finally
				{
					_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
					_views.MainForm.sourceDocuments.ResetBindings(false);

					UpdateGroupingAppearance();

					RestoreScrollPosition();
					RedrawCurrentCell();
				}
			}
		}

		protected void BeginEditDynamicColumn(DataGridViewCell cell, bool triggeredByCtrl)
		{
			var documentRow = GetDocumentRow(cell);

			///////////////////////////////////////////////////////////////////////////////

			var gridColumn = cell.OwningColumn;
			if (gridColumn.Tag != null)
			{
				var dynamicColumnID = (int)gridColumn.Tag;
				var dynamicColumnRow = _views.MainForm.datasetMain.DynamicColumns.First(x => x.ID == dynamicColumnID);
				var columnType = (DynamicColumnType)dynamicColumnRow.Type;

				///////////////////////////////////////////////////////////////////////////////

				try
				{
					_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;

					SaveScrollPosition();

					documentsDataGridView.EndEdit(DataGridViewDataErrorContexts.Commit);
					if (documentsDataGridView.IsCurrentCellInEditMode)
					{
						documentsDataGridView.CancelEdit();
						documentsDataGridView.EndEdit();
					}

					if (columnType == DynamicColumnType.Category)
						BeginEditDynamicCategoryColumn(documentRow, dynamicColumnRow, dynamicColumnID, cell, triggeredByCtrl);
					else
						BeginEditDynamicSimpleColumn(documentRow, dynamicColumnRow, columnType);
				}
				finally
				{
					_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;

					RestoreScrollPosition();
					RedrawCurrentCell();
				}
			}
		}

		protected void BeginEditDynamicSimpleColumn(MainDataSet.DocumentsRow documentRow, MainDataSet.DynamicColumnsRow dynamicColumnRow, DynamicColumnType columnType)
		{
			object objValue = documentRow[dynamicColumnRow.Title];

			switch (columnType)
			{
				case DynamicColumnType.FreeText:

					var formFreeText = new FormEditColumnFreeText { Value = objValue as string };
					if (formFreeText.ShowDialog() == DialogResult.OK)
					{
						SqlSetColumnValueByPrimaryKey(dynamicColumnRow.Title, formFreeText.Value, documentRow.ED_ENC_NUM, false, false);

						documentRow[dynamicColumnRow.Title] = formFreeText.Value;
						documentRow.AcceptChanges();

						UpdateGroupingAppearance();
					}

					break;

				///////////////////////////////////////////////////////////////////////////////

				case DynamicColumnType.Numeric:

					var props = NumericColumnProperties.Load(dynamicColumnRow);
					var value = 0.0d;
					if (objValue != DBNull.Value)
					{
						if (objValue is double)
							value = (double) objValue;
						else if (objValue is int)
							value = (int) objValue;
						else if (objValue is decimal)
							value = Convert.ToDouble((decimal) objValue);
					}

					var isDecimal = props.IsDecimal;

					var reloadDocuments = false;

					var formNumeric = new FormEditColumnNumeric(props) { Value = value };
					if (formNumeric.ShowDialog() == DialogResult.OK)
					{
						if (props.IsDecimal != isDecimal)
							reloadDocuments = PromptToConvertIntegerColumnToDecimal(dynamicColumnRow, props);

						object newValue;
						if (formNumeric.Value != null)
						{
							newValue = formNumeric.Value;
							if (!isDecimal && !reloadDocuments)
								newValue = (double) Convert.ToInt32(newValue);
						}
						else
							newValue = DBNull.Value;

						SqlSetColumnValueByPrimaryKey(dynamicColumnRow.Title, newValue, documentRow.ED_ENC_NUM, false, false);

						documentRow[dynamicColumnRow.Title] = newValue;
						documentRow.AcceptChanges();

						UpdateGroupingAppearance();

						if (reloadDocuments)
						{
							SynchronizationContext.Current.Post(x =>
							{
								_views.BeforeDocumentsTableLoad(true);
								_views.AfterDocumentsTableLoad(true);
							}, null);
						}
					}

					break;

				///////////////////////////////////////////////////////////////////////////////

				case DynamicColumnType.DateTime:

					DateTime dt;
					if (objValue == DBNull.Value)
					{
						var now = DateTime.Now;
						dt = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
					}
					else
						dt = (DateTime)objValue;

					var dateTimeProps = DateTimeColumnProperties.Load(dynamicColumnRow);
					var formDateTime = new FormEditColumnDateTime(dateTimeProps) { Value = dt };
					if (formDateTime.ShowDialog() == DialogResult.OK)
					{
						SqlSetColumnValueByPrimaryKey(dynamicColumnRow.Title, formDateTime.Value, documentRow.ED_ENC_NUM, false, false);

						documentRow[dynamicColumnRow.Title] = formDateTime.Value;
						documentRow.AcceptChanges();

						UpdateGroupingAppearance();
					}

					break;

				///////////////////////////////////////////////////////////////////////////////

				default:
					throw new ArgumentOutOfRangeException();
			}

			//UpdatePane();
		}

		protected void BeginEditDynamicCategoryColumn(MainDataSet.DocumentsRow documentRow, MainDataSet.DynamicColumnsRow dynamicColumnRow, int dynamicColumnID, DataGridViewCell cell, bool triggeredByCtrl)
		{
			var selectedCategoryTitle = documentRow.Field<string>(dynamicColumnRow.Title);

			var formSelectDynamicColumnCategory = new FormSelectDynamicColumnCategory(_views, cell, triggeredByCtrl)
			{
				SelectedCategoryTitle = selectedCategoryTitle,
				DynamicColumnID = dynamicColumnID
			};

			///////////////////////////////////////////////////////////////////////////////

			object value = null;

			var dlgres = formSelectDynamicColumnCategory.ShowDialog();
			if (dlgres != DialogResult.OK && dlgres != DialogResult.Abort)
				return;

			if (dlgres == DialogResult.OK)
				value = formSelectDynamicColumnCategory.SelectedCategoryTitle;

			///////////////////////////////////////////////////////////////////////////////

			SqlSetColumnValueByPrimaryKey(dynamicColumnRow.Title, value, documentRow.ED_ENC_NUM, false, triggeredByCtrl);

			documentRow[dynamicColumnRow.Title] = value;
			documentRow.AcceptChanges();

			UpdateGroupingAppearance();

			UpdatePane();
		}

		protected Brush GetCachedBrush(int categoryColor)
		{
			Brush brush;
			if (!_brushes.TryGetValue(categoryColor, out brush))
			{
				brush = new SolidBrush(Color.FromArgb(categoryColor));
				_brushes[categoryColor] = brush;
			}

			return brush;
		}

		protected MainDataSet.DocumentsRow GetDocumentRow(DataGridViewCell cell)
		{
			var gridRow = cell.OwningRow;
			var rowView = (DataRowView)gridRow.DataBoundItem;

			return (MainDataSet.DocumentsRow)rowView.Row;
		}

		protected void GotoNextEditableCell(DataGridViewCell currentCell, bool dynamicOnly)
		{
			var columnIndex = -1;

			///////////////////////////////////////////////////////////////////////////////

			var editableColumnsList = documentsDataGridView.Columns.Cast<DataGridViewColumn>()
														   .Where(x => x.Tag != null && x.Visible)
														   .ToList();

			///////////////////////////////////////////////////////////////////////////////

			if (!dynamicOnly)
			{
				editableColumnsList.Add(columnCategory);

				editableColumnsList = editableColumnsList.OrderBy(x => x.Index)
														 .ToList();
			}

			///////////////////////////////////////////////////////////////////////////////

			for (var i = 0; i < editableColumnsList.Count; i++)
			{
				var gridColumn = editableColumnsList[i];

				///////////////////////////////////////////////////////////////////////////////

				if (gridColumn.Index > currentCell.ColumnIndex)
				{
					columnIndex = gridColumn.Index;
					break;
				}

				///////////////////////////////////////////////////////////////////////////////

				if (gridColumn.Index == currentCell.ColumnIndex)
				{
					columnIndex = i == editableColumnsList.Count - 1 ? editableColumnsList[0].Index : editableColumnsList[i + 1].Index;
					break;
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			if (columnIndex != -1)
			{
				documentsDataGridView.CurrentCell = documentsDataGridView[columnIndex, currentCell.RowIndex];

				EnsureCurrentCellVisibleHorizontally();
			}
		}

		protected void EnsureCurrentCellVisibleHorizontally()
		{
			var currentCell = documentsDataGridView.CurrentCell;
			if (currentCell != null)
			{
				/*var columnIndex = currentCell.ColumnIndex - 1;
				if (columnIndex < 0)
					columnIndex = 0;

				documentsDataGridView.FirstDisplayedScrollingColumnIndex = columnIndex;*/
			}
		}

		protected void ScrollDataGridView(bool top)
		{
			if (documentsDataGridView.RowCount == 0)
				return;

			///////////////////////////////////////////////////////////////////////////////

			if (top)
				documentsDataGridView.FirstDisplayedScrollingRowIndex = 0;
			else
				documentsDataGridView.FirstDisplayedScrollingRowIndex = documentsDataGridView.RowCount - 1;
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

		#endregion

		#region Implementation: sorting

		protected void SortUnderlyingDocumentsDataTable(string columnName, ListSortDirection sortDirection, BackgroundWorker worker)
		{
			var scrollRowIndex = documentsDataGridView.FirstDisplayedScrollingRowIndex;

			///////////////////////////////////////////////////////////////////////////////

			var source = GetDocumentsDataSource();
			source.RaiseListChangedEvents = false;

			var sort = source.Sort;
			var position = source.Position;

			source.RemoveSort();

			///////////////////////////////////////////////////////////////////////////////

			_views.MainForm.adapterDocuments.SyncSort(columnName, sortDirection, worker);

			///////////////////////////////////////////////////////////////////////////////

			source.Sort = sort;
			source.Position = position;

			source.RaiseListChangedEvents = true;
			source.ResetBindings(false);

			///////////////////////////////////////////////////////////////////////////////

			documentsDataGridView.FirstDisplayedScrollingRowIndex = scrollRowIndex;
		}

		protected void InvokeFilterByCategories()
		{
			FilterDocumentsByCategory(true);
		}

		protected void InvokeSortByScore()
		{
			var formGenericProgress = new FormGenericProgress("Sorting documents, please wait...", DoSortByScore, null, false);
			formGenericProgress.ShowDialog();
		}

		protected bool DoSortByScore(BackgroundWorker worker, object objArgument)
		{
			documentsDataGridView.Invoke((Action)SortByScoreDelegete);

			return true;
		}

		protected void SortByScoreDelegete()
		{
			_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;

			///////////////////////////////////////////////////////////////////////////////

			documentsDataGridView.Sort(columnScore, ListSortDirection.Descending);

			///////////////////////////////////////////////////////////////////////////////

			_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
			_views.MainForm.sourceDocuments.ResetBindings(false);
		}

		protected bool SortByCategoryOperation(BackgroundWorker worker, object objArgument)
		{
			try
			{
				ListSortDirection sortDirection;

				if (_sortedByCategoryValue)
					sortDirection = _lastSortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
				else
					sortDirection = ListSortDirection.Ascending;

				var sortedDocuments = _views.DocumentsService.SortByCategory(sortDirection == ListSortDirection.Ascending);

				_sortedByCategoryValue = true;

				_views.MainForm.adapterDocuments.Sort(sortDirection, GetDocumentsDataSource(), sortedDocuments, worker);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			return true;
		}

		protected bool FreezeSortOperation(BackgroundWorker worker, object objColumnName)
		{
			try
			{
				SortUnderlyingDocumentsDataTable(objColumnName as string, _lastSortDirection, worker);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			return true;
		}

		protected void UpdateSortByCategoryValueButton(ListSortDirection? sortDirection)
		{
			if (sortDirection != null)
			{
				btnSortByCategory.Checked = true;
				btnSortByCategory.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;

				if (sortDirection == ListSortDirection.Ascending)
					btnSortByCategory.Image = Properties.Resources.up_icon;
				else
					btnSortByCategory.Image = Properties.Resources.down_icon;
			}
			else
			{
				btnSortByCategory.Checked = false;
				btnSortByCategory.Image = null;
				btnSortByCategory.DisplayStyle = ToolStripItemDisplayStyle.Text;
			}
		}

		protected void ClearMultiSort()
		{
			_sortOptions = null;
			_sortGroupsByColumn = null;

			btnExternalSort.Checked = false;
			_btnFreezeSort.Enabled = true;
			_btnSortScores.Enabled = true;

			ApplyGroupsFilter(null);

			_groupBrushes.Clear();

			foreach (var col in documentsDataGridView.Columns.Cast<DataGridViewColumn>())
			{
				col.HeaderCell.SortGlyphDirection = SortOrder.None;
				col.HeaderCell.Tag = null;
			}

			if (_views.SortByDocumentScore)
				InvokeSortByScore();
		}

		protected void PaintColumnHeader(DataGridViewCellPaintingEventArgs e)
		{
			var backgroundBrush = new SolidBrush(MainForm.ColorBackground);

			var rc = new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1);
			e.Graphics.FillRectangle(backgroundBrush, rc);
			e.Graphics.DrawRectangle(Pens.White, rc);
			e.Graphics.DrawLine(Pens.Gray, rc.Right, rc.Top, rc.Right, rc.Bottom);

			///////////////////////////////////////////////////////////////////////////////

			var sf = new StringFormat
			{
				Alignment = StringAlignment.Near,
				LineAlignment = StringAlignment.Center
			};

			var headerCell = documentsDataGridView.Columns[e.ColumnIndex].HeaderCell;
			var sortOrder = headerCell.SortGlyphDirection;
			if (sortOrder != SortOrder.None)
			{
				const int edgeLength = 5;

				rc.X += edgeLength;
				rc.Width -= edgeLength;

				var center = ((rc.Bottom - rc.Top) / 2);

				Point[] points;
				if (sortOrder == SortOrder.Ascending)
				{
					center += edgeLength / 2;
					points = new[] { new Point(rc.X, center), new Point(rc.X + edgeLength, center - (edgeLength + 1)), new Point(rc.X + (edgeLength * 2), center) };
				}
				else
				{
					center -= edgeLength / 2;
					points = new[] { new Point(rc.X, center), new Point(rc.X + edgeLength, center + edgeLength), new Point(rc.X + (edgeLength * 2), center) };
				}

				var sortIndex = documentsDataGridView.Columns[e.ColumnIndex].HeaderCell.Tag;
				var triangleBrush = (sortIndex is int && (int)sortIndex == -1) ? Brushes.Black : Brushes.Gray;

				e.Graphics.FillPolygon(triangleBrush, points);

				rc.X += edgeLength * 2;
				rc.Width -= edgeLength * 2;

				if (headerCell.Tag is int)
				{
					var index = (int)headerCell.Tag;
					if (index > 0)
					{
						var indicator = index.ToString();

						var rcIndicator = new RectangleF(rc.X, rc.Y - 1, rc.Width, rc.Height);
						e.Graphics.DrawString(indicator, _headerSortIndicatorFont, Brushes.Gray, rcIndicator, sf);

						var size = e.Graphics.MeasureString(indicator, _headerSortIndicatorFont);
						rc.X += (int)size.Width;
						rc.Width -= (int)size.Width;
					}
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			var formattedValue = e.FormattedValue as string;
			if (!String.IsNullOrEmpty(formattedValue))
			{
				rc = new Rectangle(rc.X + 2, rc.Y + 2, rc.Width - 4, rc.Height - 2);

				e.Graphics.DrawString(formattedValue, _cellFontRegular, Brushes.Black, rc, sf);
			}

			e.Handled = true;
		}

		protected bool InvokeExternalSort(ExternalSortOptions sortOptions, string sortGroupsByColumn, SortOrder sortGroupsByColumnDirection)
		{
			if (_views.FreezeSort)
				throw new Exception("Turn off sort lock first");

			_sortOptions = sortOptions;
			_sortGroupsByColumn = sortGroupsByColumn;
			_sortGroupsByColumnDirection = sortGroupsByColumnDirection;

			var viewData = new PaneDocumentsViewData { SortOptions = _sortOptions };
			viewData.Save(_views);

			if (_sortOptions.SortGroupsByCriteria == SortGroupsBy.None)
				sortGroupsByColumn = null;
			else if (_sortOptions.SortByColumns.Any(x => x.ColumnName == sortGroupsByColumn))
				sortGroupsByColumn = null;

			var groupByColumn = _sortOptions.SortByColumns.FirstOrDefault(x => x.GroupSimilar);

			var showStartupMessage = false;
			var showElapsedMessage = false;

			_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;

			var isRecyclingDocuments = _sortOptions.LeaveOneDocument;

			var succeeded = false;

			try
			{
				_isSortingGroupsInProgress = true;

				var externalSortWrapper = new ExternalSortWrapper(_views, _views.MainForm.DocumentsDbPath, _sortOptions, groupByColumn?.ColumnName, sortGroupsByColumn, sortGroupsByColumnDirection, showElapsedMessage, showStartupMessage);
				string filter;
				if (externalSortWrapper.Sort(out filter))
				{
					_sortedByCategoryValue = false;
					_lastSortColumnIndex = -1;
					UpdateSortByCategoryValueButton(null);

					succeeded = true;

					btnExternalSort.Checked = succeeded;

					///////////////////////////////////////////////////////////////////////////////

					if (!isRecyclingDocuments)
					{
						ApplyGroupsFilter(filter);

						UpdateGroupingAppearance();
					}
					else
						ClearMultiSort();

					return true;
				}
				else
					MessageBox.Show("External sort failed", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			finally
			{
				_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
				_views.MainForm.sourceDocuments.ResetBindings(false);

				if (succeeded)
				{
					if (isRecyclingDocuments)
						ClearMultiSort();
					else
						UpdateSortHeaders();
				}

				_documentsTableReloaded = false;
				_isSortingGroupsInProgress = false;
			}

			return false;
		}

		protected void SqlSetColumnValueByPrimaryKey(string columnName, object newValue, double documentID, bool isBatch, bool enableRaiseListChangedEvents)
		{
			var groupByColumn = isBatch ? null : _sortOptions?.SortByColumns.FirstOrDefault(x => x.GroupSimilar);

			if (groupByColumn != null && _sortOptions.ApplyEditsToGroup)
			{
				if (enableRaiseListChangedEvents)
					_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;

				try
				{
					var args = new UpdateDocumentsInGroupArgs
					{
						ModifiedColumnName = columnName,
						NewValue = newValue,
						DocumentID = documentID,
						GroupByColumnName = groupByColumn.ColumnName
					};

					var count = GetDocumentsCountInGroup(groupByColumn.ColumnName, documentID);
					if (count >= 100)
					{
						var formGenericProgress = new FormGenericProgress("Updating documents in group...", DoUpdateDocumentsInGroup, args, true);
						formGenericProgress.ShowDialog(_views.MainForm);
					}
					else
						DoUpdateDocumentsInGroup(null, args);

					MainForm.ShowInfoToolTip("Info", $"{args.UpdatedRowsCount} row(s) updated");
				}
				finally
				{
					if (enableRaiseListChangedEvents)
						_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
				}
			}
			else
				_views.MainForm.adapterDocuments.SqlSetColumnValueByPrimaryKey(columnName, newValue, documentID);
		}

		protected int GetDocumentsCountInGroup(string groupByColumnName, double documentID)
		{
			var modifiedRow = _views.MainForm.datasetMain.Documents.Rows.Cast<MainDataSet.DocumentsRow>().Single(x => x.ED_ENC_NUM == documentID);

			var groupByColumnIndex = _views.MainForm.datasetMain.Documents.Columns.IndexOf(groupByColumnName);
			var groupByValue = modifiedRow.IsNull(groupByColumnIndex) ? null : modifiedRow[groupByColumnIndex];

			return _views.MainForm.datasetMain.Documents.Cast<DataRow>().Count(x => (groupByValue == null && x.IsNull(groupByColumnIndex)) || (Object.Equals(x[groupByColumnIndex], groupByValue)));
		}

		protected bool DoUpdateDocumentsInGroup(BackgroundWorker worker, object args)
		{
			try
			{
				var updateArgs = (UpdateDocumentsInGroupArgs)args;

				var modifiedRow = _views.MainForm.datasetMain.Documents.Rows.Cast<MainDataSet.DocumentsRow>().Single(x => x.ED_ENC_NUM == updateArgs.DocumentID);

				var groupByColumnIndex = _views.MainForm.datasetMain.Documents.Columns.IndexOf(updateArgs.GroupByColumnName);
				var groupByValue = modifiedRow.IsNull(groupByColumnIndex) ? null : modifiedRow[groupByColumnIndex];

				///////////////////////////////////////////////////////////////////////////////

				var newValueAdjusted = updateArgs.NewValue;

				var applyEditsToGroup = true;

				if (updateArgs.NewValue != null && updateArgs.NewValue != DBNull.Value)
				{
					if (updateArgs.ModifiedColumnName != "Category")
					{
						var dynamicColumn = _views.MainForm.adapterDocuments.GetDynamicColumnsCollection().FirstOrDefault(x => x.Name == updateArgs.ModifiedColumnName);
						if (dynamicColumn != null)
							newValueAdjusted = GetDerrivedCategoryDynamic(dynamicColumn.DynamicColumnID, (string) updateArgs.NewValue, ref applyEditsToGroup);
					}
					else
					{
						newValueAdjusted = GetDerrivedCategoryStandard((int) updateArgs.NewValue, ref applyEditsToGroup);
					}
				}

				var modifiedColumnIndex = _views.MainForm.datasetMain.Documents.Columns.IndexOf(updateArgs.ModifiedColumnName);

				if (applyEditsToGroup)
				{
					updateArgs.UpdatedRowsCount = _views.MainForm.adapterDocuments.SqlSetColumnValueByColumn(updateArgs.ModifiedColumnName, newValueAdjusted, updateArgs.GroupByColumnName, groupByValue, ExpressionType.Equal);
					_views.MainForm.adapterDocuments.SqlSetColumnValueByColumn(updateArgs.ModifiedColumnName, updateArgs.NewValue, _views.MainForm.adapterDocuments.PrimaryKeyColumnName, updateArgs.DocumentID, ExpressionType.Equal);
				}
				else
				{
					updateArgs.UpdatedRowsCount = _views.MainForm.adapterDocuments.SqlSetColumnValueByColumn(updateArgs.ModifiedColumnName, updateArgs.NewValue, _views.MainForm.adapterDocuments.PrimaryKeyColumnName, updateArgs.DocumentID, ExpressionType.Equal);

					var row = _views.MainForm.datasetMain.Documents.FirstOrDefault(x => x.ED_ENC_NUM == updateArgs.DocumentID);
					if (row != null)
					{
						row[modifiedColumnIndex] = updateArgs.NewValue ?? DBNull.Value;
						row.AcceptChanges();
					}

					if (worker != null)
						worker.ReportProgress(100);

					return true;
				}

				///////////////////////////////////////////////////////////////////////////////

				var rows = _views.MainForm.datasetMain.Documents.Rows.Cast<MainDataSet.DocumentsRow>()
					.Where(x => (groupByValue == null && x.IsNull(groupByColumnIndex)) || (Object.Equals(x[groupByColumnIndex], groupByValue))).ToList();

				var index = 0;

				foreach (var row in rows)
				{
					if (row.ED_ENC_NUM == updateArgs.DocumentID)
						continue;

					row[modifiedColumnIndex] = newValueAdjusted ?? DBNull.Value;

					if (worker != null)
					{
						if (index % 20 == 0)
						{
							if (worker.CancellationPending)
								return false;

							var progress = (int)((index / (double)rows.Count) * 100d);
							worker.ReportProgress(progress);
						}

						index++;
					}
				}

				_views.MainForm.datasetMain.Documents.AcceptChanges();

				return true;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			return false;
		}

		protected int GetDerrivedCategoryStandard(int categoryID, ref bool applyEditsToGroup)
		{
			var categoryRow = _views.MainForm.datasetMain.Categories.FirstOrDefault(x => x.ID == categoryID);
			if (categoryRow == null)
			{
				applyEditsToGroup = false;
				return categoryID;
			}

			if (categoryRow.Category != null && categoryRow.Category.EndsWith("+", StringComparison.InvariantCultureIgnoreCase))
			{
				applyEditsToGroup = false;
				return categoryID;
			}

			///////////////////////////////////////////////////////////////////////////////

			var categoryName = categoryRow.Category;
			if (!String.IsNullOrEmpty(categoryName) && categoryRow.Category.EndsWith("+", StringComparison.InvariantCultureIgnoreCase))
			{
				categoryName = categoryName.Substring(0, categoryName.Length - 1);
				if (_views.MainForm.datasetMain.Categories.Any(x => String.Equals(x.Category, categoryName, StringComparison.InvariantCultureIgnoreCase)))
					return categoryID;
			}

			///////////////////////////////////////////////////////////////////////////////

			var derrivedCategoryName = $"{categoryRow.Category}+";
			var derrivedCategoryRow = _views.MainForm.datasetMain.Categories.FirstOrDefault(x => x.Category == derrivedCategoryName);
			if (derrivedCategoryRow != null)
				return derrivedCategoryRow.ID;

			///////////////////////////////////////////////////////////////////////////////

			derrivedCategoryRow = _views.MainForm.datasetMain.Categories.NewCategoriesRow();
			derrivedCategoryRow.IsSelected = true;
			derrivedCategoryRow.Category = derrivedCategoryName;

			derrivedCategoryRow.ID = categoryRow.ID + 1000;
			while (_views.MainForm.datasetMain.Categories.Any(x => x.ID == derrivedCategoryRow.ID))
			{
				derrivedCategoryRow.ID += 1000;
			}

			if (categoryRow.IsColorNull())
				derrivedCategoryRow.SetColorNull();
			else
				derrivedCategoryRow.Color = categoryRow.Color;

			///////////////////////////////////////////////////////////////////////////////

			var id = derrivedCategoryRow.ID;

			_views.MainForm.datasetMain.Categories.AddCategoriesRow(derrivedCategoryRow);
			_views.MainForm.adapterCategories.Update(_views.MainForm.datasetMain.Categories);
			_views.MainForm.adapterCategories.Fill(_views.MainForm.datasetMain.Categories);

			return id;
		}

		protected string GetDerrivedCategoryDynamic(int dynamicColumnID, string categoryTitle, ref bool applyEditsToGroup)
		{
			if (categoryTitle != null && categoryTitle.EndsWith("+", StringComparison.InvariantCultureIgnoreCase))
			{
				applyEditsToGroup = false;
				return categoryTitle;
			}

			var categoryRow = _views.MainForm.datasetMain.DynamicColumnCategories.FirstOrDefault(x => x.DynamicColumnID == dynamicColumnID && x.Title == categoryTitle);
			if (categoryRow == null)
				return categoryTitle;

			var derrivedCategoryName = $"{categoryTitle}+";
			var derrivedCategoryRow = _views.MainForm.datasetMain.DynamicColumnCategories.FirstOrDefault(x => x.Title == derrivedCategoryName);
			if (derrivedCategoryRow != null)
				return derrivedCategoryName;

			///////////////////////////////////////////////////////////////////////////////

			derrivedCategoryRow = _views.MainForm.datasetMain.DynamicColumnCategories.NewDynamicColumnCategoriesRow();
			derrivedCategoryRow.DynamicColumnID = dynamicColumnID;
			derrivedCategoryRow.Title = derrivedCategoryName;

			derrivedCategoryRow.Number = categoryRow.Number + 1000;
			while (_views.MainForm.datasetMain.DynamicColumnCategories.Any(x => x.Number == derrivedCategoryRow.Number))
			{
				derrivedCategoryRow.Number += 1000;
			}

			if (categoryRow.IsPropertiesNull())
				derrivedCategoryRow.SetPropertiesNull();
			else
				derrivedCategoryRow.Properties = categoryRow.Properties;

			///////////////////////////////////////////////////////////////////////////////

			_views.MainForm.datasetMain.DynamicColumnCategories.AddDynamicColumnCategoriesRow(derrivedCategoryRow);
			_views.MainForm.adapterDynamicColumnCategories.Update(_views.MainForm.datasetMain.DynamicColumnCategories);
			_views.MainForm.adapterDynamicColumnCategories.Fill(_views.MainForm.datasetMain.DynamicColumnCategories);

			return derrivedCategoryName;
		}

		protected void UpdateGroupingAppearance()
		{
			var enableSortFeatures = _sortOptions == null;

			_btnSortScores.Enabled = enableSortFeatures;
			_btnFreezeSort.Enabled = enableSortFeatures;

			UpdateSortHeaders();
			UpdateGroupColors();
		}

		protected void UpdateSortHeaders()
		{
			if (_sortOptions == null)
				return;

			foreach (var col in documentsDataGridView.Columns.Cast<DataGridViewColumn>())
			{
				var sortInfo = _sortOptions.SortByColumns.FirstOrDefault(x =>
					String.Equals(x.ColumnName, col.DataPropertyName, StringComparison.InvariantCultureIgnoreCase));

				if (!String.IsNullOrEmpty(col.DataPropertyName) && col.DataPropertyName == _sortGroupsByColumn)
				{
					col.HeaderCell.Tag = -1;
					col.HeaderCell.SortGlyphDirection = _sortGroupsByColumnDirection;
				}
				else
				{
					col.HeaderCell.Tag = sortInfo != null ? (object)(_sortOptions.SortByColumns.IndexOf(sortInfo) + 1) : null;
					col.HeaderCell.SortGlyphDirection = sortInfo != null ? sortInfo.SortOrder : SortOrder.None;
				}
			}
		}

		protected void UpdateGroupColors()
		{
			_groupBrushes.Clear();

			var groupByColumn = _sortOptions?.SortByColumns.FirstOrDefault(x => x.GroupSimilar);
			if (groupByColumn != null && groupByColumn.AlternateColor)
			{
				var dataGridColumn = _views.MainForm.datasetMain.Documents.Columns[groupByColumn.ColumnName];
				var groupByColumnIndex = dataGridColumn.Ordinal;

				foreach (var row in _views.MainForm.sourceDocuments.Cast<DataRowView>().Select(x => x.Row))
				{
					var groupByValue = row.IsNull(groupByColumnIndex) ? DBNull.Value : row[groupByColumnIndex];
					if (groupByValue == null)
						groupByValue = DBNull.Value;

					if (_groupBrushes.ContainsKey(groupByValue))
						continue;

					_groupBrushes[groupByValue] = _groupBrushes.Count % 2 == 0 ? Brushes.LightGray : Brushes.FloralWhite;
				}
			}
			//RedrawCurrentCell();
			//documentsDataGridView.Invalidate();
			//documentsDataGridView.Refresh();
		}

		protected void ApplyCategoryFilter(string filterString)
		{
			_categoryFilter = filterString;

			ApplyDocumentsFilters();
		}

		protected void ApplyGroupsFilter(string filterString)
		{
			_groupsFilter = filterString;

			ApplyDocumentsFilters();
		}

		protected void ApplyDocumentsFilters()
		{
			var filters = new List<string>
			{
				_categoryFilter,
				_groupsFilter
			};

			var newFilter = String.Join(" AND ", filters.Where(x => !String.IsNullOrEmpty(x)).Select(x => $"({x})"));
			if (String.IsNullOrEmpty(newFilter))
				newFilter = null;

			var currentFilter = _views.MainForm.sourceDocuments.Filter;
			if (String.IsNullOrEmpty(currentFilter))
				currentFilter = null;

			if (newFilter != currentFilter)
				_views.MainForm.sourceDocuments.Filter = newFilter;

			UpdateGroupingAppearance();
		}

		protected void OnAfterDocumentsTableFilled(object sender, EventArgs eventArgs)
		{
			if (_sortOptions != null && _sortOptions.ShowGroupsMinDocuments && _sortOptions.MinDocuments > 0)
			{
				var groupByColumn = _sortOptions.SortByColumns.FirstOrDefault(x => x.GroupSimilar);
				if (groupByColumn != null)
				{
					var filter = ExternalSortWrapper.FillGroupSize(_views, groupByColumn.ColumnName, _sortOptions);
					ApplyGroupsFilter(filter);
				}
			}

			UpdateGroupingAppearance();

			_documentsTableReloaded = true;
		}

		#endregion
	}

	public class PaneDocumentsViewData
	{
		#region Fields

		public ExternalSortOptions SortOptions { get; set; }

		#endregion

		#region Static operations

		public static PaneDocumentsViewData Load(ViewsManager views)
		{
			try
			{
				var json = BrowserManager.GetViewData(views, "PaneDocuments");
				if (!String.IsNullOrEmpty(json))
					return JsonConvert.DeserializeObject<PaneDocumentsViewData>(json);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			return new PaneDocumentsViewData();
		}

		#endregion

		#region Operations

		public void Save(ViewsManager views)
		{
			var json = JsonConvert.SerializeObject(this);
			BrowserManager.SetViewData(views, "PaneDocuments", json);
		}

		#endregion
	}

	public class UpdateDocumentsInGroupArgs
	{
		#region Fields

		public string GroupByColumnName { get; set; }
		public string ModifiedColumnName { get; set; }
		public object NewValue { get; set; }
		public double DocumentID { get; set; }

		public int UpdatedRowsCount { get; set; }

		#endregion
	}

	public class ExternalSortOptions
	{
		#region Fields

		public List<SortByColumn> SortByColumns { get; set; }
		public SortGroupsBy SortGroupsByCriteria { get; set; }
		public bool ShowGroupsMinDocuments { get; set; }
		public int MinDocuments { get; set; }
		public bool ApplyEditsToGroup { get; set; }
		public bool LeaveOneDocument { get; set; }
		public LeaveOneDocumentCriteria LeaveOneDocumentCriteria { get; set; }
		public string LeaveOneDocumentColumn { get; set; }

		#endregion

		#region Ctors

		public ExternalSortOptions()
		{
			this.SortByColumns = new List<SortByColumn>();
			this.SortGroupsByCriteria = SortGroupsBy.None;
		}

		#endregion
	}
}