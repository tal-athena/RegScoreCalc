using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using RegScoreCalc.Code;

namespace RegScoreCalc
{
	public partial class FormFilterByCategory : Form
	{
		#region Fields

		protected readonly ViewsManager _views;

		protected List<int> _selectedCategories;

		#endregion

		#region Ctors

		public FormFilterByCategory(ViewsManager views)
		{
			InitializeComponent();

			_views = views;

			gridCategories.AutoGenerateColumns = false;
		}

		#endregion

		#region Events

		private void FormFilterByCategory_Load(object sender, EventArgs e)
		{
			try
			{
				LoadSettings();

				LoadFilterSettings();

				///////////////////////////////////////////////////////////////////////////////

				gridCategories.DataSource = _views.MainForm.sourceCategories;
				
				gridCategories.Select();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void FormFilterByCategory_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				SaveSettings();

				if (this.DialogResult == DialogResult.OK)
				{
					if (!chkbShowAllDocuments.Checked && !chkbShowUncategorisedDocuments.Checked && !IsAnyCategorySelected())
					{
						MessageBox.Show("Please select at least one category", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
						e.Cancel = true;
					}

					///////////////////////////////////////////////////////////////////////////////

					if (!e.Cancel)
					{
						SaveCheckedCategoriesList();
						SaveFilterSettings();
					}
				}
				else
					LoadCheckedCategoriesList();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void gridCategories_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
				else if (e.KeyCode == Keys.Space)
				{
					if (gridCategories.SelectedRows.Count > 0)
					{
						if (gridCategories.CurrentCell == null || gridCategories.CurrentCell.ColumnIndex != colCheckmark.Index)
						{
							var cell = gridCategories.SelectedRows[0].Cells[colCheckmark.Index];

							if (cell.Value is bool)
								cell.Value = !(bool) cell.Value;
							else
								cell.Value = true;
						}
					}
				}
				else if (e.KeyCode == Keys.Home)
				{
					if (gridCategories.RowCount > 0)
						gridCategories.Rows[0].Selected = true;
				}
				else if (e.KeyCode == Keys.End)
				{
					if (gridCategories.RowCount > 0)
						gridCategories.Rows[gridCategories.RowCount - 1].Selected = true;
				}
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
				if (e.RowIndex >= 0 && e.ColumnIndex == colCheckmark.Index)
				{
					var cell = (DataGridViewCheckBoxCell) gridCategories[e.ColumnIndex, e.RowIndex];
					if (cell.Value is bool)
						cell.Value = !(bool)cell.Value;
					else
						cell.Value = true;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridCategories_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
				{
					var cell = (DataGridViewCheckBoxCell) gridCategories[colCheckmark.Index, e.RowIndex];
					if (cell.Value is bool)
						cell.Value = !(bool)cell.Value;
					else
						cell.Value = true;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnCheckAll_Click(object sender, EventArgs e)
		{
			try
			{
				CheckCategories(x => x.IsSelected = true);

				gridCategories.Select();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnUncheckAll_Click(object sender, EventArgs e)
		{
			try
			{
				CheckCategories(x => x.IsSelected = false);

				gridCategories.Select();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnInvertCheck_Click(object sender, EventArgs e)
		{
			try
			{
				CheckCategories(x => x.IsSelected = !x.IsSelected);

				gridCategories.Select();
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

		private void chkbShowAllDocuments_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateCategorySelectionEnabledState(!chkbShowAllDocuments.Checked);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void chkbShowUncategorisedDocuments_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				
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
			var pt = Properties.Settings.Default.FormSelectCategory_Location;
			if (pt.X >= 0 && pt.Y >= 0)
			{
				var size = Properties.Settings.Default.FormSelectCategory_Size;
				if (!size.IsEmpty)
				{
					// TODO
					//this.Location = pt;
					this.Size = size;
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			FitToScreenBounds();
		}

		protected void SaveSettings()
		{
			Properties.Settings.Default.FormSelectCategory_Location = this.Location;
			Properties.Settings.Default.FormSelectCategory_Size = this.Size;

			Properties.Settings.Default.Save();
		}

		protected void FitToScreenBounds()
		{
			var screen = Screen.FromControl(this);

			///////////////////////////////////////////////////////////////////////////////

			if (this.Width > screen.WorkingArea.Width)
				this.Width = 300;

			if (this.Height > screen.WorkingArea.Height)
				this.Height = 600;

			///////////////////////////////////////////////////////////////////////////////

			if (this.Left < screen.WorkingArea.X)
				this.Left = screen.WorkingArea.X;

			if (this.Top < screen.WorkingArea.Y)
				this.Top = screen.WorkingArea.Y;

			if (this.Right > screen.WorkingArea.Right)
				this.Left = screen.WorkingArea.Right - this.Width;

			if (this.Bottom > screen.WorkingArea.Bottom)
				this.Top = screen.WorkingArea.Bottom - this.Height;
		}

		protected bool IsAnyCategorySelected()
		{
			return _views.MainForm.datasetMain.Categories.Any(x => x.IsSelected);
		}

		protected void LoadCheckedCategoriesList()
		{
			_views.MainForm.adapterCategories.Fill(_views.MainForm.datasetMain.Categories);
		}

		protected void SaveCheckedCategoriesList()
		{
			_views.MainForm.adapterCategories.Update(_views.MainForm.datasetMain.Categories);
			_views.MainForm.adapterCategories.Fill(_views.MainForm.datasetMain.Categories);
		}

		protected void CheckCategories(Action<MainDataSet.CategoriesRow> action)
		{
			if (action != null)
				_views.MainForm.datasetMain.Categories.ToList().ForEach(x => action(x));
		}

		protected void LoadFilterSettings()
		{
			var filter = CategoryFilterViewModel.GetCategoryFilter(_views);

			chkbShowAllDocuments.Checked = filter.ShowAllDocuments;
			chkbShowUncategorisedDocuments.Checked = filter.ShowUncategorizedDocuments;
		}

		protected void SaveFilterSettings()
		{
			try
			{
				CategoryFilterViewModel.SaveCategoryFilter(_views, new CategoryFilterViewModel
				                                                   {
					                                                   ShowAllDocuments = chkbShowAllDocuments.Checked,
					                                                   ShowUncategorizedDocuments = chkbShowUncategorisedDocuments.Checked
				                                                   });
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected void UpdateCategorySelectionEnabledState(bool enable)
		{
			chkbShowUncategorisedDocuments.Enabled = enable;

			gridCategories.Enabled = enable;
			btnUncheckAll.Enabled = enable;
			btnInvertCheck.Enabled = enable;

			gridCategories.ForeColor = enable ? Color.Black : Color.Gray;
		}

		#endregion
	}
}