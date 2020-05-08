using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

using RegScoreCalc.Code;

namespace RegScoreCalc
{
	public partial class FormSelectDynamicColumnCategory : Form
	{
		#region Types

		public class CategoryModel
		{
			public int ID { get; set; }
			public int DynamicColumnID { get; set; }
			public string Title { get; set; }
			public int Number { get; set; }
			public string Properties { get; set; }
		}

		#endregion

		#region Fields

		protected ViewsManager _views;

		protected List<CategoryModel> _dataSource;

		protected CellRenderer _cellRenderer;

		protected bool _triggeredByCtrl;

		#endregion

		#region Properties

		public int DynamicColumnID { get; set; }
		public int? SelectedCategoryID { get; set; }
		public string SelectedCategoryTitle { get; set; }

		#endregion

		#region Ctors

		public FormSelectDynamicColumnCategory(ViewsManager views, DataGridViewCell cell, bool triggeredByCtrl)
		{
			InitializeComponent();

			///////////////////////////////////////////////////////////////////////////////

			_views = views;

			this.BackColor = MainForm.ColorBackground;
			gridCategories.ColumnHeadersDefaultCellStyle.BackColor = MainForm.ColorBackground;

			gridCategories.EnableHeadersVisualStyles = false;
			gridCategories.AutoGenerateColumns = false;

			gridCategories.Font = _views.SelectCategoryFont;

			_triggeredByCtrl = triggeredByCtrl;

			_cellRenderer = new CellRenderer(gridCategories);

			UpdateFilter(null);

			InitLocation(cell);

			EnableDoubleBuffering();
		}

		#endregion

		#region Events

		private void FormSelectDynamicColumnCategory_Load(object sender, EventArgs e)
		{
			try
			{
				LoadSettings();

				FillCategories();

				if (this.SelectedCategoryID != null)
					SelectByCategoryNumber(this.SelectedCategoryID);
				else if (!String.IsNullOrEmpty(this.SelectedCategoryTitle))
					SelectByCategoryName(this.SelectedCategoryTitle);
				else
					gridCategories.ClearSelection();

				if (_triggeredByCtrl)
					DisableControls();

				gridCategories.Select();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void FormSelectDynamicColumnCategory_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				SaveSettings();

				if (this.DialogResult == DialogResult.OK)
				{
					if (gridCategories.SelectedRows.Count == 1)
					{
						var categoryModel = (CategoryModel) gridCategories.SelectedRows[0].DataBoundItem;

						this.SelectedCategoryID = categoryModel.ID;
						this.SelectedCategoryTitle = categoryModel.Title;
					}
					else
					{
						this.SelectedCategoryID = null;
						this.SelectedCategoryTitle = null;

						//MessageBox.Show("Please select category", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
						//e.Cancel = true;
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnNone_Click(object sender, EventArgs e)
		{
			try
			{
				gridCategories.ClearSelection();

				this.DialogResult = DialogResult.Abort;

				this.Close();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			try
			{
				AddCategory();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			try
			{
				DeleteCategory();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnSelectFont_Click(object sender, EventArgs e)
		{
			try
			{
				var fontDialog = new FontDialog();
				fontDialog.Font = _views.SelectCategoryFont;
				if (fontDialog.ShowDialog() == DialogResult.OK)
				{
					_views.SelectCategoryFont = fontDialog.Font;
					_views.SaveConfig();

					gridCategories.Font = _views.SelectCategoryFont;

					_cellRenderer.UpdateFonts(gridCategories);
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
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridCategories_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				ProcessKeyDown(sender, e);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridCategories_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				ProcessKeyUp(sender, e);
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
				if (!_triggeredByCtrl && e.RowIndex >= 0 && e.ColumnIndex == colColor.Index)
				{
					var gridRow = gridCategories.Rows[e.RowIndex];

					var categoryModel = (CategoryModel) gridRow.DataBoundItem;

					var properties = !String.IsNullOrEmpty(categoryModel.Properties) ? DynamicCategoryProperties.FromJSON(categoryModel.Properties) : new DynamicCategoryProperties();

					var dlg = new ColorDialog { Color = properties.Background };
					if (dlg.ShowDialog(this) == DialogResult.OK)
					{
						properties.Background = dlg.Color;
						categoryModel.Properties = properties.ToJson();

						var categoryRow = _views.MainForm.datasetMain.DynamicColumnCategories.FindByID(categoryModel.ID);
						if (categoryRow != null)
						{
							categoryRow.Properties = categoryModel.Properties;
							_views.MainForm.adapterDynamicColumnCategories.Update(categoryRow);
						}

						gridCategories.Refresh();
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridCategories_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex == colColor.Index)
				{
					var categoryModel = (CategoryModel) gridCategories.Rows[e.RowIndex].DataBoundItem;

					e.CellStyle.BackColor = !String.IsNullOrEmpty(categoryModel.Properties)
						? DynamicCategoryProperties.FromJSON(categoryModel.Properties)
						                           .Background
						: Color.White;
					e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void gridCategories_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && (e.ColumnIndex == colName.Index || e.ColumnIndex == colNumber.Index))
				{
					_cellRenderer.RenderCell(gridCategories[e.ColumnIndex, e.RowIndex], e, txtFilter.Text);

					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btn_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				ProcessKeyUp(sender, e);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btn_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				ProcessKeyDown(sender, e);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void txtFilter_TextChanged(object sender, EventArgs e)
		{
			try
			{
				FillCategories();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation

		protected void InitLocation(DataGridViewCell cell)
		{
			if (cell != null)
			{
				var rc = cell.DataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true);
				rc = cell.DataGridView.RectangleToScreen(rc);
				this.Left = rc.Right + 1;

				var screen = Screen.FromControl(cell.DataGridView);

				int top = (rc.Top + (rc.Height / 2)) - (this.Height / 2);

				if (top + this.Height > screen.WorkingArea.Bottom)
					top = screen.WorkingArea.Bottom - this.Height;

				if (top < 0)
					top = 0;

				this.Top = top;
			}
			else
				this.StartPosition = FormStartPosition.CenterScreen;
		}

		protected void LoadSettings()
		{
			var pt = Properties.Settings.Default.FormSelectCategory_Location;
			if (pt.X >= 0 && pt.Y >= 0)
			{
				var size = Properties.Settings.Default.FormSelectCategory_Size;
				if (!size.IsEmpty)
					this.Size = size;
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

		protected void FillCategories()
		{
			var filter = txtFilter.Text;

			var selectedIndex = 0;

			var query = _views.MainForm.datasetMain.DynamicColumnCategories.Rows.Cast<MainDataSet.DynamicColumnCategoriesRow>()
				.Where(x => x.DynamicColumnID == DynamicColumnID)
				.ToList();

			if (filter.Length > 0)
			{
				query = query.Where(x => (!String.IsNullOrEmpty(x.Title) && x.Title.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) != -1)
				                         || x.Number.ToString()
				                             .IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) != -1)
				             .OrderBy(x => x.Number)
				             .ToList();

				var bestMatchIndex = query.FindIndex(x => (String.Equals(x.Title, filter, StringComparison.InvariantCultureIgnoreCase) || String.Equals(x.Number.ToString(), filter, StringComparison.InvariantCultureIgnoreCase)));
				if (bestMatchIndex == -1)
					bestMatchIndex = query.FindIndex(x => (!String.IsNullOrEmpty(x.Title) && x.Title.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) == 0) || x.Number.ToString()
					                                                                                                                                                       .IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) == 0);

				if (bestMatchIndex != -1)
					selectedIndex = bestMatchIndex;
			}
			else
				query = query.OrderBy(x => x.Number)
				            .ToList();

			///////////////////////////////////////////////////////////////////////////////

			_dataSource = query.Select(x => new CategoryModel
			                                {
				                                ID = x.ID,
				                                DynamicColumnID = x.DynamicColumnID,
				                                Title = x.Title,
												Number = x.Number,
												Properties = !x.IsPropertiesNull() ? x.Properties : null
											})
			                   .ToList();

			gridCategories.DataSource = _dataSource;
			if (selectedIndex >= 0 && selectedIndex < gridCategories.Rows.Count)
				gridCategories.Rows[selectedIndex]
				              .Selected = true;
		}

		protected void AddCategory()
		{
			var classCategories = _views.MainForm.datasetMain.DynamicColumnCategories.Where(
				                            x => x.RowState != DataRowState.Deleted && x.DynamicColumnID == this.DynamicColumnID)
			                            .ToList();

			int nNextNumber = 1;
			if (classCategories.Any())
				nNextNumber = classCategories.Max(x => x.Number) + 1;

			var formCategory = new FormEditColumnCategory
			                   {
				                   ViewsManager = _views,
				                   Title = "New Category",
				                   Number = nNextNumber
			                   };

			if (formCategory.ShowDialog() == DialogResult.OK)
			{
				gridCategories.DataSource = null;

				_views.MainForm.datasetMain.DynamicColumnCategories.AddDynamicColumnCategoriesRow(this.DynamicColumnID, formCategory.Title, formCategory.Number, "");

				_views.MainForm.adapterDynamicColumnCategories.Update(_views.MainForm.datasetMain.DynamicColumnCategories);
				_views.MainForm.adapterDynamicColumnCategories.Fill(_views.MainForm.datasetMain.DynamicColumnCategories);

				FillCategories();

				SelectByCategoryNumber(formCategory.Number);
			}
		}

		protected void DeleteCategory()
		{
			if (gridCategories.SelectedRows.Count == 1)
			{
				var categoryModel = (CategoryModel) gridCategories.SelectedRows[0]
				                                                                         .DataBoundItem;

				string message = "Do you wish to delete this category?";

				var dlgres = MessageBox.Show(message, MainForm.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

				if (dlgres == DialogResult.Yes)
				{
					gridCategories.DataSource = null;

					var dynamicColumnRow = _views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.ID == this.DynamicColumnID);
					if (dynamicColumnRow == null)
						throw new Exception("Column does not exist");

					var categoryRow = _views.MainForm.datasetMain.DynamicColumnCategories.FindByID(categoryModel.ID);
					if (categoryRow == null)
						throw new Exception("Column does not exist");

					///////////////////////////////////////////////////////////////////////////////

					_views.MainForm.adapterDocuments.SqlClearColumnValues(
						columnName: dynamicColumnRow.Title,
						whereColumn: dynamicColumnRow.Title,
						whereValue: categoryModel.Title,
						whereCondition: ExpressionType.Equal);

					///////////////////////////////////////////////////////////////////////////////

					_views.MainForm.datasetMain.Documents.Where(x => !x.IsNull(dynamicColumnRow.Title) && (string) x[dynamicColumnRow.Title] == categoryModel.Title)
					      .ToList()
					      .ForEach(x =>
					               {
						               x[dynamicColumnRow.Title] = DBNull.Value;
						               x.AcceptChanges();
					               });

					///////////////////////////////////////////////////////////////////////////////

					categoryRow.Delete();

					_views.MainForm.adapterDynamicColumnCategories.Update(_views.MainForm.datasetMain.DynamicColumnCategories);
					_views.MainForm.adapterDynamicColumnCategories.Fill(_views.MainForm.datasetMain.DynamicColumnCategories);

					FillCategories();

					SelectByCategoryNumber(null);
				}
			}
		}

		protected void SelectByCategoryNumber(int? categoryNumber)
		{
			if (categoryNumber != null)
			{
				var selected = gridCategories.Rows.Cast<DataGridViewRow>()
				                             .FirstOrDefault(x => ((CategoryModel) x.DataBoundItem).Number == categoryNumber);

				if (selected != null)
					selected.Selected = true;
			}
			else
				gridCategories.ClearSelection();
		}

		protected void SelectByCategoryName(string categoryName)
		{
			if (!String.IsNullOrEmpty(categoryName))
			{
				var selected = gridCategories.Rows.Cast<DataGridViewRow>()
				                             .FirstOrDefault(x => ((CategoryModel) x.DataBoundItem).Title == this.SelectedCategoryTitle);

				if (selected != null)
					selected.Selected = true;
			}
			else
				gridCategories.ClearSelection();
		}

		protected void UpdateFilter(string filter)
		{
			if (filter == null)
				filter = String.Empty;

			txtFilter.Text = filter.ToLower();
			txtFilter.SelectionStart = filter.Length;
			txtFilter.SelectionLength = 0;
		}

		protected void ProcessKeyDown(object sender, KeyEventArgs e)
		{
			var filter = txtFilter.Text;

			if (e.KeyCode == Keys.Return)
			{
				if (sender is DataGridView)
				{
					this.DialogResult = DialogResult.OK;
					this.Close();

					e.Handled = true;
				}
			}
			else if (e.KeyCode == Keys.Up)
			{
				MoveSelection(true);

				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Down)
			{
				MoveSelection(false);

				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Home)
			{
				if (gridCategories.RowCount > 0)
					gridCategories.Rows[0]
					              .Selected = true;

				UpdateFilter(null);

				e.Handled = true;
			}
			else if (e.KeyCode == Keys.End)
			{
				if (gridCategories.RowCount > 0)
					gridCategories.Rows[gridCategories.RowCount - 1]
					              .Selected = true;

				UpdateFilter(null);

				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Back)
			{
				if (!String.IsNullOrEmpty(filter))
				{
					if (e.Control && txtFilter.Focused)
						UpdateFilter(null);
					else
						UpdateFilter(filter.Substring(0, filter.Length - 1));
				}

				e.Handled = true;
			}
			else if (e.KeyCode != Keys.Tab && (Char.IsLetterOrDigit((char) e.KeyCode) || Char.IsWhiteSpace((char) e.KeyCode) || (e.KeyCode == Keys.Space)))
			{
				var letter = e.KeyCode == Keys.Space ? " " : new KeysConverter().ConvertToString(e.KeyCode);
				if (!String.IsNullOrEmpty(letter))
				{
					filter += letter;

					UpdateFilter(filter);
				}

				e.Handled = true;
			}
		}

		protected void ProcessKeyUp(object sender, KeyEventArgs e)
		{
			if (_triggeredByCtrl && e.KeyCode == Keys.ControlKey)
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		protected void MoveSelection(bool up)
		{
			if (gridCategories.SelectedRows.Count > 0)
			{
				var index = gridCategories.SelectedRows[0]
				                          .Index;
				index += up ? -1 : 1;

				if (index < 0)
					index = 0;
				else if (index >= gridCategories.Rows.Count)
					index = gridCategories.Rows.Count - 1;

				gridCategories.Rows[index]
				              .Selected = true;
			}
			else
			{
				if (gridCategories.Rows.Count > 0)
					gridCategories.Rows[0]
					              .Selected = true;
			}
		}

		protected void DisableControls()
		{
			btnAdd.Enabled = false;
			btnRemove.Enabled = false;
			btnSelectFont.Enabled = false;
		}

		protected void EnableDoubleBuffering()
		{
			var type = gridCategories.GetType();
			var prop = type.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
			prop.SetValue(gridCategories, true, null);
		}

		#endregion
	}
}