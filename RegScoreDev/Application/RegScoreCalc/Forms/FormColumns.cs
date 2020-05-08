using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormColumns : Form
	{
		#region Fields

		protected ViewsManager _views;
		protected List<ColumnSettings> _columns;

		#endregion

		#region Ctors

		public FormColumns(ViewsManager views, List<ColumnSettings> columns)
		{
			InitializeComponent();

			_views = views;
			_columns = columns;

			this.BackColor = MainForm.ColorBackground;

			gridColumns.AutoGenerateColumns = false;
			gridColumns.EnableHeadersVisualStyles = false;

			colCheckbox.HeaderCell.Style.BackColor = MainForm.ColorBackground;
			colName.HeaderCell.Style.BackColor = MainForm.ColorBackground;
		}

		#endregion

		#region Events

		private void FormColumns_Load(object sender, EventArgs e)
		{
			try
			{
				FillGrid();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void FormColumns_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					SaveColumnSettings();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnMoveUp_Click(object sender, EventArgs e)
		{
			try
			{
				MoveUp();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnMoveDown_Click(object sender, EventArgs e)
		{
			try
			{
				MoveDown();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			try
			{
				SetVisibility(x => x.Cells[colCheckbox.Index].Value = true);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnDeselectAll_Click(object sender, EventArgs e)
		{
			try
			{
				SetVisibility(x => x.Cells[colCheckbox.Index].Value = false);
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
				SetVisibility(x => x.Cells[colCheckbox.Index].Value = !(bool) x.Cells[colCheckbox.Index].Value);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation

		protected void FillGrid()
		{
			gridColumns.Rows.Clear();

            //foreach (var col in _columns.Where(x => !SvmColumnService.IsSvmColumn(_views, x.SqlColumn.Name)))
            foreach (var col in _columns)
            {
				var visible = col.Settings != null ? col.Settings.IsVisible : true;

				var index = gridColumns.Rows.Add(visible, col.GridColumn.HeaderText);
				gridColumns.Rows[index].Tag = col;
			}
		}

		protected void SaveColumnSettings()
		{
			for (var i = 0; i < gridColumns.Rows.Count; i++)
			{
				var row = gridColumns.Rows[i];
				var isVisible = (bool) row.Cells[colCheckbox.Index].Value;

				var col = (ColumnSettings) row.Tag;
				if (col.Settings != null)
				{
					col.Settings.IsVisible = isVisible;
					col.Settings.Order = i;
				}
				else
				{
					_views.MainForm.datasetMain.Columns.AddColumnsRow(col.SqlColumn.Name, i, isVisible);
				}
			}

			_views.MainForm.adapterColumns.Update(_views.MainForm.datasetMain.Columns);
			_views.MainForm.adapterColumns.Fill(_views.MainForm.datasetMain.Columns);
		}

		protected void MoveUp()
		{
			if (gridColumns.SelectedRows.Count != 1)
				return;

			var row = gridColumns.SelectedRows[0];

			var index = row.Index;
			if (index <= 0)
				return;

			gridColumns.Rows.RemoveAt(index);
			gridColumns.Rows.Insert(index - 1, row);

			row.Selected = true;
			gridColumns.FirstDisplayedScrollingRowIndex = row.Index;
		}

		protected void MoveDown()
		{
			if (gridColumns.SelectedRows.Count != 1)
				return;

			var row = gridColumns.SelectedRows[0];

			var index = row.Index;
			if (index >= gridColumns.Rows.Count - 1)
				return;

			gridColumns.Rows.RemoveAt(index);
			gridColumns.Rows.Insert(index + 1, row);

			row.Selected = true;
			gridColumns.FirstDisplayedScrollingRowIndex = row.Index;
		}

		protected void SetVisibility(Action<DataGridViewRow> func)
		{
			foreach (DataGridViewRow row in gridColumns.Rows)
			{
				func(row);
			}
		}

		#endregion
	}

	public class ColumnSettings
	{
		public DataGridViewColumn GridColumn { get; set; }
		public ColumnInfo SqlColumn { get; set; }
		public MainDataSet.ColumnsRow Settings { get; set; }
	}
}