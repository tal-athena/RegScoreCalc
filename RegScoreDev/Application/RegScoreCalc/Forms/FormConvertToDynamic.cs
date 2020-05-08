using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
	public partial class FormConvertToDynamic : Form
	{
		#region Fields

		protected readonly ViewsManager _views;
		protected ObservableCollection<ColumnInfo> _columns;
		protected int _expandHeight;

		#endregion

		#region Ctors

		public FormConvertToDynamic(ViewsManager views)
		{
			InitializeComponent();

			_views = views;

			this.BackColor = MainForm.ColorBackground;

			gridCategories.AutoGenerateColumns = false;
			gridCategories.EnableHeadersVisualStyles = false;
			gridCategories.ColumnHeadersDefaultCellStyle.BackColor = MainForm.ColorBackground;

			gridColumns.AutoGenerateColumns = false;
			gridColumns.EnableHeadersVisualStyles = false;
			gridColumns.ColumnHeadersDefaultCellStyle.BackColor = MainForm.ColorBackground;

			toolStrip.Renderer = new CustomToolStripRenderer { RoundedEdges = false };
		}

		#endregion

		#region Events

		#region Form

		private void FormConvertToDynamic_Load(object sender, EventArgs e)
		{
			try
			{
				splitContainer.Panel2Collapsed = true;
				_expandHeight = gridColumns.Height;

				BindColumnsList();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void FormConvertToDynamic_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (worker.IsBusy)
					e.Cancel = true;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Columns grid

		private void gridColumns_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0)
				{
					var row = gridColumns.Rows[e.RowIndex];
					var ci = row.DataBoundItem as ColumnInfo;
					if (ci != null)
					{
						if (e.ColumnIndex == colDbType.Index)
						{
							e.Value = ci.Type.ToString()
							            .Replace("System.", "");
						}
						else if (e.ColumnIndex == colDynamicType.Index)
						{
							var cell = (DataGridViewComboBoxCell) row.Cells[e.ColumnIndex];
							cell.DisplayStyle = cell.ReadOnly ? DataGridViewComboBoxDisplayStyle.Nothing : DataGridViewComboBoxDisplayStyle.ComboBox;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridColumns_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				OnColumnSelectionChanged();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridColumns_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0)
				{
					if (!gridColumns.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
					{
						gridColumns.BeginEdit(true);
						if (e.ColumnIndex == colDynamicType.Index)
						{
							var cmb = gridColumns.EditingControl as DataGridViewComboBoxEditingControl;
							if (cmb != null)
								cmb.DroppedDown = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridColumns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex == colDynamicType.Index)
				{
					var value = gridColumns.Rows[e.RowIndex]
					                       .Cells[e.ColumnIndex]
					                       .Value as string;
					if (!String.IsNullOrEmpty(value))
						UpdateSelectedColumnType((DynamicColumnType) Enum.Parse(typeof(DynamicColumnType), value));
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridColumns_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			try
			{
				gridColumns.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Categories grid

		private void gridCategories_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0)
				{
					var formattingRow = gridCategories.Rows[e.RowIndex];
					var formattingCategoryInfo = (ColumnCategoryInfo)formattingRow.DataBoundItem;

					var cell = formattingRow.Cells[e.ColumnIndex];

					Color cellForeColor = gridCategories.DefaultCellStyle.ForeColor;
					Color cellSelectionForeColor = gridCategories.DefaultCellStyle.SelectionForeColor;
					Color cellBackColor;

					if (e.ColumnIndex == colConvertTo.Index)
					{
						cell.ReadOnly = formattingCategoryInfo.Empty;
						cellForeColor = formattingCategoryInfo.Empty ? gridCategories.DefaultCellStyle.BackColor : gridCategories.DefaultCellStyle.ForeColor;
						cellSelectionForeColor = formattingCategoryInfo.Empty ? gridCategories.DefaultCellStyle.SelectionBackColor : gridCategories.DefaultCellStyle.SelectionForeColor;
					}

					///////////////////////////////////////////////////////////////////////////////

					if (gridCategories.CurrentCell != null && gridCategories.CurrentCell.RowIndex == e.RowIndex)
						cellBackColor = gridCategories.DefaultCellStyle.BackColor;
					else
						cellBackColor = formattingRow.Selected ? gridCategories.DefaultCellStyle.SelectionBackColor : gridCategories.DefaultCellStyle.BackColor;

					if (e.ColumnIndex == colConvertTo.Index && !formattingRow.Selected && !formattingCategoryInfo.Empty)
					{
						if (gridCategories.SelectedRows.Count == 1)
						{
							var selectedRow = gridCategories.SelectedRows[0];
							var selectedCategoryInfo = (ColumnCategoryInfo)selectedRow.DataBoundItem;

							if (!selectedCategoryInfo.Empty)
							{
								if (String.Equals(formattingCategoryInfo.ConvertTo, selectedCategoryInfo.ConvertTo, StringComparison.InvariantCultureIgnoreCase))
								{
									cellForeColor = formattingCategoryInfo.Empty ? MainForm.ColorBackground : gridCategories.DefaultCellStyle.ForeColor;
									cellBackColor = MainForm.ColorBackground;
								}
							}
						}
					}

					///////////////////////////////////////////////////////////////////////////////

					cell.Style.ForeColor = cellForeColor;
					cell.Style.BackColor = cellBackColor;
					cell.Style.SelectionForeColor = cellSelectionForeColor;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridCategories_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				gridCategories.BeginEdit(true);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridCategories_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0)
				{
					if (e.ColumnIndex == colEmpty.Index)
					{
						var cell = gridCategories.Rows[e.RowIndex]
						                         .Cells[e.ColumnIndex];
						if (cell.Value is bool)
							cell.Value = !(bool) cell.Value;

						gridCategories.NotifyCurrentCellDirty(true);

						gridCategories.Invalidate();
						gridCategories.RefreshEdit();
						gridCategories.Refresh();
					}
					else
						gridCategories.BeginEdit(true);
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridCategories_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			try
			{
				gridCategories.CommitEdit(DataGridViewDataErrorContexts.Commit);
				gridCategories.Invalidate();
				gridCategories.Refresh();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridCategories_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				gridCategories.Invalidate();
				gridCategories.Refresh();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridCategories_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0)
				{
					if (e.ColumnIndex == colConvertTo.Index)
					{
						var selectColumnInfo = (ColumnCategoryInfo)gridCategories.Rows[e.RowIndex].DataBoundItem;

						var sameCategories = gridCategories.Rows.Cast<DataGridViewRow>().Select(x => (ColumnCategoryInfo)x.DataBoundItem).Where(x => x.ConvertTo == selectColumnInfo.ConvertTo).OrderBy(x => x.Number).ToList();

						var firstCategory = sameCategories.First();
						sameCategories.ForEach(x => x.Number = firstCategory.Number);

						gridCategories.Invalidate();
						gridCategories.Refresh();
					}
					else if (e.ColumnIndex == colNumber.Index)
					{
						var selectColumnInfo = (ColumnCategoryInfo)gridCategories.Rows[e.RowIndex].DataBoundItem;

						var sameCategories = gridCategories.Rows.Cast<DataGridViewRow>().OrderBy(x => x.Index).Select(x => (ColumnCategoryInfo)x.DataBoundItem).Where(x => x.ConvertTo == selectColumnInfo.ConvertTo).ToList();

						sameCategories.ForEach(x => x.Number = selectColumnInfo.Number);

						gridCategories.Invalidate();
						gridCategories.Refresh();
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Buttons

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			try
			{
				SetRowsChecked(x => x.Empty = true);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnSelectNone_Click(object sender, EventArgs e)
		{
			try
			{
				SetRowsChecked(x => x.Empty = false);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnInvert_Click(object sender, EventArgs e)
		{
			try
			{
				SetRowsChecked(x => x.Empty = !x.Empty);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnConvert_Click(object sender, EventArgs e)
		{
			try
			{
				ConvertSelectedColumnAsync();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			try
			{
				Close();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Worker

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				var columnInfo = e.Argument as ColumnInfo;
				if (columnInfo != null)
				{
					e.Result = QueryCategories(columnInfo);
					return;
				}

				e.Result = ConvertColumn((ColumnConvertArgs) e.Argument);
			}
			catch (Exception ex)
			{
				worker.ReportProgress(-1, ex.Message);
			}
		}

		private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			try
			{
				var message = e.UserState as string;
				if (!String.IsNullOrEmpty(message))
					MessageBox.Show(message, MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				UpdateUI(false);

				var columnConvertInfo = e.Result as IEnumerable<ColumnCategoryInfo>;
				if (columnConvertInfo != null)
				{
					SetColumnConvertInfo(columnConvertInfo);
					return;
				}

				if (e.Result is bool)
				{
					MessageBox.Show("Conversion succeeded", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					BindColumnsList();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#endregion

		#region Implementation

		protected void BindColumnsList()
		{
			_columns = new ObservableCollection<ColumnInfo>(_views.MainForm.adapterDocuments.GetExtraColumnsCollection()
			                                                      .Where(x => !SvmColumnService.IsSvmColumn(_views, x.Name)));

			gridColumns.DataSource = _columns;

			///////////////////////////////////////////////////////////////////////////////

			if (gridColumns.Rows.Count > 0)
			{
				foreach (DataGridViewRow row in gridColumns.Rows)
				{
					var columnInfo = (ColumnInfo) row.DataBoundItem;

					var cell = (DataGridViewComboBoxCell) row.Cells[colDynamicType.Index];
					cell.Items.Clear();

					if (columnInfo.Type == typeof(decimal) || columnInfo.Type == typeof(double) || columnInfo.Type == typeof(int))
						cell.Items.Add("Numeric");
					else if (columnInfo.Type == typeof(string))
						cell.Items.AddRange("FreeText", "Category");
					else if (columnInfo.Type == typeof(DateTime))
						cell.Items.Add("DateTime");
					else
						cell.Items.Add("[not supported]");

					cell.Value = cell.Items[0] as string;
					cell.ReadOnly = cell.Items.Count <= 1;
				}

				gridColumns.Rows[0]
				           .Selected = true;
				OnColumnSelectionChanged();
			}
		}

		protected void UpdateSelectedColumnType(DynamicColumnType type)
		{
			switch (type)
			{
				case DynamicColumnType.FreeText:
				case DynamicColumnType.Numeric:
				case DynamicColumnType.DateTime:
					ShowColumnOptions(false);
					break;
				case DynamicColumnType.Category:
					ShowColumnOptions(true);
					break;
				case DynamicColumnType.None:
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		protected void ShowColumnOptions(bool show)
		{
			if (splitContainer.Panel2Collapsed)
			{
				if (show)
				{
					UpdateCategories();

					splitContainer.Panel2Collapsed = false;

					var expandHeight = _expandHeight + splitContainer.SplitterWidth;
					this.Height += expandHeight;
				}
			}
			else
			{
				if (!show)
				{
					gridCategories.DataSource = null;
					splitContainer.Panel2Collapsed = true;

					var expandHeight = _expandHeight + splitContainer.SplitterWidth;
					this.Height -= expandHeight;
				}
				else
					UpdateCategories();
			}
		}

		protected void UpdateCategories()
		{
			var row = gridColumns.SelectedRows[0];

			var selectedColumnOptions = row.Tag as IEnumerable<ColumnCategoryInfo>;
			if (selectedColumnOptions == null)
			{
				var columnInfo = (ColumnInfo)row.DataBoundItem;
				SetColumnConvertInfoAsync(columnInfo);
			}
			else
				SetColumnConvertInfo(selectedColumnOptions);
		}

		protected void OnColumnSelectionChanged()
		{
			if (gridColumns.SelectedRows.Count == 1)
			{
				var cell = (DataGridViewComboBoxCell) gridColumns.SelectedRows[0]
				                                                 .Cells[colDynamicType.Index];
				var value = cell.Value as string;
				if (!String.IsNullOrEmpty(value))
					UpdateSelectedColumnType((DynamicColumnType) Enum.Parse(typeof(DynamicColumnType), value));
			}
		}

		protected void SetColumnConvertInfoAsync(ColumnInfo columnInfo)
		{
			UpdateUI(true);

			worker.RunWorkerAsync(columnInfo);
		}

		protected void ConvertSelectedColumnAsync()
		{
			if (gridColumns.SelectedRows.Count == 0)
				return;

			///////////////////////////////////////////////////////////////////////////////

			var row = gridColumns.SelectedRows[0];
			var columnInfo = row.DataBoundItem as ColumnInfo;
			if (columnInfo != null)
			{
				UpdateUI(true);

				///////////////////////////////////////////////////////////////////////////////

				var args = new ColumnConvertArgs
				           {
					           ColumnInfo = columnInfo,
					           Categories = gridCategories.DataSource as IEnumerable<ColumnCategoryInfo>
				           };

				worker.RunWorkerAsync(args);
			}
		}

		private void SetColumnConvertInfo(IEnumerable<ColumnCategoryInfo> columnConvertInfo)
		{
			var observable = new BindingList<ColumnCategoryInfo>(columnConvertInfo.ToList());
			gridColumns.SelectedRows[0].Tag = observable;
			gridCategories.DataSource = observable;
		}

		#endregion

		#region	Implementation: conversion

		private bool ConvertColumn(ColumnConvertArgs args)
		{
			DynamicColumnType type;

			if (args.ColumnInfo.Type == typeof(decimal) || args.ColumnInfo.Type == typeof(double) || args.ColumnInfo.Type == typeof(int))
				type = DynamicColumnType.Numeric;
			else if (args.ColumnInfo.Type == typeof(string))
				type = args.Categories != null ? DynamicColumnType.Category : DynamicColumnType.FreeText;
			else if (args.ColumnInfo.Type == typeof(DateTime))
				type = DynamicColumnType.DateTime;
			else
				throw new Exception("Unsupported column data type");

			///////////////////////////////////////////////////////////////////////////////

			if (args.Categories != null)
			{
				var emptyNames = args.Categories
				                     .Where(x => !x.Empty && String.IsNullOrEmpty(x.ConvertTo))
				                     .ToList();

				if (emptyNames.Any())
					throw new Exception(String.Format("Provided names are invalid for categories that are not marked as Empty:{0}{0}{1}", Environment.NewLine, String.Join(", ", emptyNames.Select(x => x.Value))));

				///////////////////////////////////////////////////////////////////////////////

				var duplicateNumbers = args.Categories.GroupBy(x => new { x.ConvertTo, x.Number })
				                           .Select(y => y.Key)
										   .GroupBy(x => x.Number)
										   .Where(x => x.Count() > 1)
										   .Select(x => x.Key)
				                           .ToList();

				if (duplicateNumbers.Any())
					throw new Exception(String.Format("Duplicate category numbers:{0}{0}{1}", Environment.NewLine, String.Join(", ", duplicateNumbers)));
			}

			///////////////////////////////////////////////////////////////////////////////

			AddDynamicColumn(args, type);

			return true;
		}

		private void AddDynamicColumn(ColumnConvertArgs args, DynamicColumnType type)
		{
			var order = _views.MainForm.datasetMain.DynamicColumns.Count(x => x.RowState != DataRowState.Deleted) + 1;

			_views.MainForm.adapterDynamicColumns.Insert(args.ColumnInfo.Name, (int) type, order, String.Empty);
			_views.MainForm.adapterDynamicColumns.Fill(_views.MainForm.datasetMain.DynamicColumns);

			var newRow = _views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.Title == args.ColumnInfo.Name);
			if (newRow != null)
			{
				args.ColumnInfo.DynamicColumnID = newRow.ID;
				args.ColumnInfo.IsDynamic = true;
				args.ColumnInfo.IsExtra = false;

				_views.MainForm.adapterDocuments.SetColumnDynamic(args.ColumnInfo.Name, args.ColumnInfo.DynamicColumnID);

				if (args.Categories != null)
					ConvertValuesToCategories(args);
			}
			else
				throw new Exception("Failed to add column");
		}

		private List<ColumnCategoryInfo> QueryCategories(ColumnInfo columnInfo)
		{
			var index = _views.MainForm.datasetMain.Documents.Columns.IndexOf(columnInfo.Name);
			var allValues = _views.MainForm.datasetMain.Documents.Select(x => x[index] as string)
			                      .Where(x => !String.IsNullOrEmpty(x))
			                      .ToList();
			var totalCount = (double) allValues.Count;

			var result = allValues.GroupBy(x => x)
			                      .OrderBy(x => x.Key)
			                      .Select((x, i) => new ColumnCategoryInfo
			                                        {
				                                        Empty = false,
				                                        Value = x.Key,
				                                        ConvertTo = x.Key,
				                                        Number = i + 1,
				                                        Percentage = (x.Count() / totalCount) * 100d
			                                        })
			                      .ToList();

			return result;
		}

		private void ConvertValuesToCategories(ColumnConvertArgs args)
		{
			var dict = new HashSet<string>();

			foreach (var categoryInfo in args.Categories)
			{
				if (!categoryInfo.Empty)
				{
					if (!dict.Contains(categoryInfo.ConvertTo))
					{
						_views.MainForm.adapterDynamicColumnCategories.Insert(args.ColumnInfo.DynamicColumnID, categoryInfo.ConvertTo, categoryInfo.Number, String.Empty);
						dict.Add(categoryInfo.ConvertTo);
					}
				}

				_views.MainForm.adapterDocuments.SqlSetColumnValueByColumn(args.ColumnInfo.Name, categoryInfo.Empty ? String.Empty : categoryInfo.ConvertTo, args.ColumnInfo.Name, categoryInfo.Value, ExpressionType.Equal);
			}

			///////////////////////////////////////////////////////////////////////////////

			_views.MainForm.datasetMain.Documents.AcceptChanges();
			_views.MainForm.adapterDynamicColumnCategories.Fill(_views.MainForm.datasetMain.DynamicColumnCategories);
		}

		#endregion

		#region	Helpers

		protected void UpdateUI(bool busy)
		{
			loadingImage.Visible = busy;
			gridCategories.Visible = !busy;

			this.Enabled = !busy;
		}

		private void SetRowsChecked(Action<ColumnCategoryInfo> action)
		{
			var selectedColumnOptions = gridCategories.DataSource as IEnumerable<ColumnCategoryInfo>;
			if (selectedColumnOptions != null)
			{
				foreach (var row in selectedColumnOptions)
				{
					action(row);
				}

				gridCategories.Invalidate();
				gridCategories.Refresh();
			}
		}

		#endregion
	}

	internal class ColumnConvertArgs
	{
		#region	Fields

		public ColumnInfo ColumnInfo { get; set; }
		public IEnumerable<ColumnCategoryInfo> Categories { get; set; }

		#endregion
	}

	internal class ColumnCategoryInfo
	{
		#region Fields

		public bool Empty { get; set; }
		public double Percentage { get; set; }
		public int Number { get; set; }
		public string Value { get; set; }
		public string ConvertTo { get; set; }
		public int CategoryID { get; set; }

		#endregion
	}
}