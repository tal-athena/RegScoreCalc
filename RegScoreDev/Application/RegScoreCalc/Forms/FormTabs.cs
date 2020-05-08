using DocumentsServiceInterfaceLib;
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
	public partial class FormTabs : Form
	{
        #region Fields

        protected ViewsManager _views;
		protected Pane _tabPane;
		protected List<TabSetting> _tabSettings;
        protected int _nPaneType;

		#endregion

		#region Ctors

		public FormTabs(ViewsManager views, Pane tabPane, int paneType = 0)
		{
			InitializeComponent();

            _views = views;
            _tabPane = tabPane;
			_tabSettings = _views.TabSettings;
            _nPaneType = paneType;

            this.BackColor = MainForm.ColorBackground;

			gridColumns.AutoGenerateColumns = false;
			gridColumns.EnableHeadersVisualStyles = false;

			colCheckbox.HeaderCell.Style.BackColor = MainForm.ColorBackground;
			colName.HeaderCell.Style.BackColor = MainForm.ColorBackground;
            colDisplayName.HeaderCell.Style.BackColor = MainForm.ColorBackground;
            colScore.HeaderCell.Style.BackColor = MainForm.ColorBackground;
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

			foreach (var row in _tabSettings)
			{
				var visible = row.Visible;

				var index = gridColumns.Rows.Add(visible, row.ColumnName, row.DisplayName, row.Score);
				gridColumns.Rows[index].Tag = row;

                if (row.Index == 0)
                {
                    gridColumns.Rows[index].Cells[0].ReadOnly = true;
                    gridColumns.Rows[index].Cells[3].ReadOnly = true;
                }
			}
		}

		protected void SaveColumnSettings()
		{
            List<TabSetting> newSettings = new List<TabSetting>();

			for (var i = 0; i < gridColumns.Rows.Count; i++)
			{
				var row = gridColumns.Rows[i];
				var isVisible = (bool) row.Cells[colCheckbox.Index].Value;

				var col = (TabSetting) row.Tag;
                col.Visible = isVisible;
                col.Order = i;
                col.DisplayName = row.Cells[2].Value.ToString();
                col.Score = (bool)row.Cells[3].Value;
                
                newSettings.Add(col);                
			}
            /*
            _views.SaveTabSettings(newSettings);
            _views.InvokeRefreshTabSettings();
            */

            /*
            _views.MainForm.datasetMain.ColRegExp.Clear();
            _views.MainForm.adapterColRegExp.Update(_views.MainForm.datasetMain.ColRegExp);
            _views.MainForm.adapterColRegExp.Fill(_views.MainForm.datasetMain.ColRegExp);

            _views.MainForm.datasetMain.DynamicColumns.Clear();
            _views.MainForm.adapterDynamicColumns.Update(_views.MainForm.datasetMain.DynamicColumns);
            _views.MainForm.adapterDynamicColumns.Fill(_views.MainForm.datasetMain.DynamicColumns);
            */
            if (_nPaneType == 0)
            {

                ((PaneTabNotes)_tabPane).SetTabSettings(newSettings);
                _views.InvokeRefreshTabSettings();
                //((PaneTabNotes)_tabPane).RefreshTabs();
            }
            else if (_nPaneType == 1)
            {
                ((PaneRegTabNotes)_tabPane).SetTabSettings(newSettings);
                _views.InvokeRefreshTabSettings();
                //((PaneRegTabNotes)_tabPane).RefreshTabs();
            }
            
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

        private void ribbonStyleButton1_Click(object sender, EventArgs e)
        {
            int lastIndex = 0;
            
            for (int i = 0; i < gridColumns.Rows.Count; i ++)
            {
                var row = gridColumns.Rows[i];
                var colName = (string)row.Cells[1].Value;
                if (colName.StartsWith("NOTE_TEXT"))
                {
                    colName = colName.Replace("NOTE_TEXT", "0");
                    if (lastIndex < Int32.Parse(colName))
                        lastIndex = Int32.Parse(colName);

                }
            }

            lastIndex++;

            TabSetting newRow = new TabSetting();
            newRow.Index = lastIndex;
            newRow.Order = gridColumns.Rows.Count + 1;
            newRow.ColumnName = "NOTE_TEXT" + lastIndex;
            newRow.DisplayName = "Document " + (lastIndex + 1);
            newRow.Visible = true;
            newRow.Dynamic = true;
            newRow.Score = true;

            var index = gridColumns.Rows.Add(newRow.Visible, newRow.ColumnName, newRow.DisplayName, newRow.Score);
            gridColumns.Rows[index].Tag = newRow;
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            
            if (_views.DocumentsService.DeleteAllExtraDocumentColumns() == true)
            {
                _views.InvokeRefreshTabSettings();

                _tabSettings = _views.DocumentsService.GetDocumentColumnSettings();
                FillGrid();
            } else
            {
                MessageBox.Show("Error occured while deleting columns");
            }
            
        }
    }
}