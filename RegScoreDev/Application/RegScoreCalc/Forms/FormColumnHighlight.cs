using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using RegScoreCalc.Code;

namespace RegScoreCalc
{
	public partial class FormColumnHighlight : Form
	{
		#region Fields

		protected ViewsManager _views;
		protected int _selectedColumnID;

		protected ColumnsViewModel _viewModel;

		#endregion

		#region Ctors

		public FormColumnHighlight(ViewsManager views, int selectedColumnID)
		{
			InitializeComponent();

			_views = views;
			_selectedColumnID = selectedColumnID;

			this.BackColor = MainForm.ColorBackground;

			gridDynamicColumns.AutoGenerateColumns = false;
			gridDynamicColumns.EnableHeadersVisualStyles = false;

			gridHighlightColumns.AutoGenerateColumns = false;
			gridHighlightColumns.EnableHeadersVisualStyles = false;

			colCheckbox2.HeaderCell.Style.BackColor = MainForm.ColorBackground;

			colName.HeaderCell.Style.BackColor = MainForm.ColorBackground;
			colName2.HeaderCell.Style.BackColor = MainForm.ColorBackground;
		}

		#endregion

		#region Events

		private void FormColumnHighlight_Load(object sender, EventArgs e)
		{
			try
			{
				LoadSettings();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void FormColumnHighlight_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					SaveSettings();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
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

		private void gridDynamicColumns_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				if (gridDynamicColumns.SelectedRows.Count == 1)
				{
					var gridModel = (DynamicColumnSettings) gridDynamicColumns.SelectedRows[0]
					                                                          .DataBoundItem;

					gridHighlightColumns.DataSource = gridModel.HighlightColumnsList;
				}
				else
					gridHighlightColumns.DataSource = null;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridHighlightColumns_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			try
			{
				gridHighlightColumns.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation

		protected void LoadSettings()
		{
			_viewModel = ColumnsViewModel.FromJSON(BrowserManager.GetViewData(_views, "Columns RegEx"));

			var dynamicColumns = _views.MainForm.datasetMain.DynamicColumns.Rows.Cast<MainDataSet.DynamicColumnsRow>()
			                           .OrderBy(x => x.Order)
			                           .ToList();

			var columns = (from dynamicCol in dynamicColumns
			               join setting in _viewModel.ColumnsSettingsList
				               on dynamicCol.ID equals setting.ColumnID
				               into tmp
			               from t in tmp.DefaultIfEmpty()
			               select new { Column = dynamicCol, Settings = t }).ToList();

			_viewModel.ColumnsSettingsList.Clear();

			foreach (var col in columns)
			{
				var settings = col.Settings ?? new DynamicColumnSettings { ColumnID = col.Column.ID };

				settings.Name = col.Column.Title;

				///////////////////////////////////////////////////////////////////////////////

				var highlightColumns = (from dynamicCol in dynamicColumns
				                        join setting in settings.HighlightColumnsList
					                        on dynamicCol.ID equals setting.ColumnID
					                        into tmp
				                        from t in tmp.DefaultIfEmpty()
				                        select new { Column = dynamicCol, Settings = t }).ToList();

				settings.HighlightColumnsList.Clear();

				foreach (var highlightCol in highlightColumns)
				{
					var highlightSetting = highlightCol.Settings ?? new DynamicColumnSettings
					                                                {
						                                                IsSelected = highlightCol.Column.ID == col.Column.ID,
						                                                ColumnID = highlightCol.Column.ID
					                                                };

					highlightSetting.Name = highlightCol.Column.Title;

					settings.HighlightColumnsList.Add(highlightSetting);
				}

				///////////////////////////////////////////////////////////////////////////////
	
				_viewModel.ColumnsSettingsList.Add(settings);
			}

			gridDynamicColumns.DataSource = _viewModel.ColumnsSettingsList;

			var selectedColumn = _viewModel.ColumnsSettingsList.FirstOrDefault(x => x.ColumnID == _selectedColumnID) ?? _viewModel.ColumnsSettingsList.First();

			var row = gridDynamicColumns.Rows.Cast<DataGridViewRow>()
			                            .FirstOrDefault(x => x.DataBoundItem == selectedColumn);
			if (row != null)
				row.Selected = true;
		}

		protected void SaveSettings()
		{
			gridHighlightColumns.CommitEdit(DataGridViewDataErrorContexts.Commit);

			BrowserManager.SetViewData(_views, "Columns RegEx", _viewModel.ToJson());
		}

		#endregion
	}
}