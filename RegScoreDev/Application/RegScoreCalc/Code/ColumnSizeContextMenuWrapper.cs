using System;
using System.Windows.Forms;

namespace RegScoreCalc.Code
{
	public class ColumnSizeContextMenuWrapper
	{
		#region Fields

		protected readonly ViewsManager _views;
		protected readonly View _view;
		protected readonly DataGridView _grid;

		protected ContextMenuStrip _columnContextMenu;

		#endregion

		#region Ctors

		public ColumnSizeContextMenuWrapper(ViewsManager views, View view, DataGridView grid)
		{
			_views = views;
			_view = view;
			_grid = grid;

			///////////////////////////////////////////////////////////////////////////////

			_grid.CellMouseClick += grid_CellMouseClick;

			///////////////////////////////////////////////////////////////////////////////

			_columnContextMenu = new ContextMenuStrip();

			ToolStripItem item;

			item = _columnContextMenu.Items.Add("Auto-size", null, columnsContextMenu_Click);
			item.Tag = DataGridViewAutoSizeColumnMode.AllCells;

			item = _columnContextMenu.Items.Add("Fill", null, columnsContextMenu_Click);
			item.Tag = DataGridViewAutoSizeColumnMode.Fill;

			item = _columnContextMenu.Items.Add("Manual", null, columnsContextMenu_Click);
			item.Tag = DataGridViewAutoSizeColumnMode.None;
		}

		private void grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					if (e.RowIndex == -1 && e.ColumnIndex >= 0)
					{
						var column = _grid.Columns[e.ColumnIndex];

						Show(column);
					}
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		#endregion

		#region Operations

		protected void Show(DataGridViewColumn column)
		{
			_columnContextMenu.Tag = column;

			ToolStripMenuItem item;

			item = (ToolStripMenuItem)_columnContextMenu.Items[0];
			item.Checked = column.AutoSizeMode == DataGridViewAutoSizeColumnMode.AllCells;

			item = (ToolStripMenuItem)_columnContextMenu.Items[1];
			item.Checked = column.AutoSizeMode == DataGridViewAutoSizeColumnMode.Fill;

			item = (ToolStripMenuItem)_columnContextMenu.Items[2];
			item.Checked = column.AutoSizeMode == DataGridViewAutoSizeColumnMode.None;

			_columnContextMenu.Show(Cursor.Position);
		}

		#endregion

		#region Events

		private void columnsContextMenu_Click(object sender, EventArgs eventArgs)
		{
			try
			{
				var column = _columnContextMenu.Tag as DataGridViewColumn;
				if (column != null)
				{
					var item = (ToolStripItem) sender;

					column.AutoSizeMode = (DataGridViewAutoSizeColumnMode) item.Tag;
				}

				///////////////////////////////////////////////////////////////////////////////

				_columnContextMenu.Tag = null;

				_views.SaveLayout(_view);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion
	}
}