using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

using CefSharp.WinForms.Internals;

using RegScoreCalc.Code;
using RegScoreCalc.Data;

namespace RegScoreCalc
{
	public partial class FormSelectCategory : Form
	{
		#region Types

		public class CategoryModel
		{
			public int ID { get; set; }
			public string Category { get; set; }
			public int? Color { get; set; }
		}

		public enum DisplayMode
		{
			Default,
			Billing
		}

		#endregion

		#region Fields

		protected ViewsManager _views;

		protected DisplayMode _displayMode;

		protected int _categoryID;

		protected List<CategoryModel> _dataSource;

		protected CellRenderer _cellRenderer;

		protected bool _triggeredByCtrl;

		protected string _initialFilter;

		#endregion

		#region Properties

		public int CategoryID
		{
			get { return _categoryID; }
			set { _categoryID = value; }
		}

		#endregion

		#region Ctors

		public FormSelectCategory(ViewsManager views, DisplayMode displayMode, DataGridViewCell cell, bool triggeredByCtrl, string filter = null)
		{
			InitializeComponent();

			_views = views;
			_displayMode = displayMode;

			colColor.Visible = _displayMode == DisplayMode.Default;

			this.BackColor = MainForm.ColorBackground;
			gridCategories.ColumnHeadersDefaultCellStyle.BackColor = MainForm.ColorBackground;

			gridCategories.AutoGenerateColumns = false;
			gridCategories.EnableHeadersVisualStyles = false;

			gridCategories.Font = _views.SelectCategoryFont;

			_triggeredByCtrl = triggeredByCtrl;

			_cellRenderer = new CellRenderer(gridCategories);

			_initialFilter = filter;
			UpdateFilter(_initialFilter);

			InitLocation(cell);

			EnableDoubleBuffering();
		}

		#endregion

		#region Events

		private void FormSelectCategory_Load(object sender, EventArgs e)
		{
			try
			{
				LoadSettings();

				FillCategories();
				SelectByCategoryID(_categoryID);

				if (_triggeredByCtrl)
					DisableControls();

				gridCategories.Select();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void FormSelectCategory_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				SaveSettings();

				if (this.DialogResult == DialogResult.OK)
					_categoryID = GetSelectedCategoryID();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
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

		private void gridCategories_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0)
				{
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
			}
			catch
			{
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

		private void btnAdd_Click(object sender, EventArgs e)
		{
			try
			{
				if (_displayMode == DisplayMode.Default)
				{
					var addCategoryForm = new AddCategory(_views);

					if (addCategoryForm.ShowDialog() == DialogResult.OK)
					{
						FillCategories();

						if (addCategoryForm.SelectedCategoryID != null)
							SelectByCategoryID(addCategoryForm.SelectedCategoryID.Value);
					}
				}
				else if (_displayMode == DisplayMode.Billing)
				{
					var addCategoryForm = new AddCategoryBilling(_views);
					if (addCategoryForm.ShowDialog() == DialogResult.OK)
					{
						FillCategories();

						if (addCategoryForm.SelectedCategoryID != null)
							SelectByCategoryID(addCategoryForm.SelectedCategoryID.Value);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			try
			{
				if (gridCategories.SelectedRows.Count == 1)
				{
					var dlgres = MessageBox.Show("Do you wish to delete category?", "Delete category", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
					if (dlgres == DialogResult.Yes)
					{
						if (_displayMode == DisplayMode.Default)
							DeleteStaticCategory();
						else if (_displayMode == DisplayMode.Billing)
							DeleteStaticBillingCategory();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnNone_Click(object sender, EventArgs e)
		{
			try
			{
				gridCategories.ClearSelection();

				this.DialogResult = DialogResult.OK;
				Close();
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

		private void gridCategories_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex == colColor.Index)
				{
					var categoryModel = (CategoryModel)gridCategories.Rows[e.RowIndex]
																		.DataBoundItem;

					e.CellStyle.BackColor = categoryModel.Color != null ? Color.FromArgb(categoryModel.Color.Value) : Color.White;
					e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
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
				if (!_triggeredByCtrl && e.RowIndex >= 0 && e.ColumnIndex == colColor.Index)
				{
					var categoryModel = (CategoryModel) gridCategories.Rows[e.RowIndex]
																		.DataBoundItem;

					var dlg = new ColorDialog { Color = categoryModel.Color != null ? Color.FromArgb(categoryModel.Color.Value) : Color.White };
					if (dlg.ShowDialog(this) == DialogResult.OK)
					{
						categoryModel.Color = dlg.Color.ToArgb();

						var categoryRow = _views.MainForm.datasetMain.Categories.FindByID(categoryModel.ID);
						if (categoryRow != null)
						{
							categoryRow.Color = dlg.Color.ToArgb();
							_views.MainForm.adapterCategories.Update(categoryRow);
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

		private void gridCategories_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && (e.ColumnIndex == colName.Index || e.ColumnIndex == colID.Index))
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

			var query = GetCategories();
			if (filter.Length > 0)
			{
				query = query.Where(x => (!String.IsNullOrEmpty(x.Category) && x.Category.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) != -1)
				                         || x.ID.ToString()
				                             .IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) != -1)
				             .OrderBy(x => x.ID)
				             .ToList();

				var bestMatchIndex = query.FindIndex(x => (String.Equals(x.Category, filter, StringComparison.InvariantCultureIgnoreCase) || String.Equals(x.ID.ToString(), filter, StringComparison.InvariantCultureIgnoreCase)));
				if (bestMatchIndex == -1)
					bestMatchIndex = query.FindIndex(x => (!String.IsNullOrEmpty(x.Category) && x.Category.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) == 0) || x.ID.ToString().IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) == 0);

				if (bestMatchIndex != -1)
					selectedIndex = bestMatchIndex;
			}
			else
				query = query.OrderBy(x => x.ID).ToList();

			///////////////////////////////////////////////////////////////////////////////

			_dataSource = query;

			gridCategories.DataSource = _dataSource;

			if (selectedIndex >= 0 && selectedIndex < gridCategories.Rows.Count)
				gridCategories.Rows[selectedIndex].Selected = true;
		}

		protected List<CategoryModel> GetCategories()
		{
			if (_displayMode == DisplayMode.Default)
			{
				return _views.MainForm.datasetMain.Categories.Rows.Cast<MainDataSet.CategoriesRow>()
				             .Select(x => new CategoryModel
				                          {
					                          ID = x.ID,
					                          Category = x.Category,
					                          Color = !x.IsColorNull() ? (int?) x.Color : null
				                          }).ToList();
			}

			if (_displayMode == DisplayMode.Billing)
			{
				return _views.MainForm.datasetBilling.Categories.Rows.Cast<BillingDataSet.CategoriesRow>()
				             .Select(x => new CategoryModel
				                          {
					                          ID = x.ID,
					                          Category = x.Category,
					                          Color = null
				                          }).ToList();
			}

			throw new ArgumentOutOfRangeException(nameof(DisplayMode));
		}

		protected int GetSelectedCategoryID()
		{
			var selectedCategoryID = -1;

			if (gridCategories.SelectedRows.Count == 1)
			{
				var categoryModel = (CategoryModel)gridCategories.SelectedRows[0].DataBoundItem;

				selectedCategoryID = categoryModel.ID;
			}

			return selectedCategoryID;
		}

		protected void SelectByCategoryID(int categoryID)
		{
			try
			{
				gridCategories.Activate();

				if (categoryID > 0)
				{
					foreach (DataGridViewRow row in gridCategories.Rows)
					{
						var categoryModel = (CategoryModel) row.DataBoundItem;

						if (categoryModel.ID == categoryID)
						{
							row.Selected = true;
							gridCategories.FirstDisplayedScrollingRowIndex = row.Index;

							break;
						}
					}
				}
				else
					gridCategories.ClearSelection();
			}
			catch
			{
			}
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
			else if (e.KeyCode != Keys.Tab && (Char.IsLetterOrDigit((char)e.KeyCode) || Char.IsWhiteSpace((char)e.KeyCode) || (e.KeyCode == Keys.Space)))
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
			if (e.KeyCode == Keys.ControlKey)
			{
				if (_triggeredByCtrl)
				{
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
			}
		}

		protected void MoveSelection(bool up)
		{
			if (gridCategories.SelectedRows.Count > 0)
			{
				var index = gridCategories.SelectedRows[0].Index;
				index += up ? -1 : 1;

				if (index < 0)
					index = 0;
				else if (index >= gridCategories.Rows.Count)
					index = gridCategories.Rows.Count - 1;

				gridCategories.Rows[index].Selected = true;
			}
			else
			{
				if (gridCategories.Rows.Count > 0)
					gridCategories.Rows[0].Selected = true;
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

		protected void DeleteStaticCategory()
		{
			var categoryModel = (CategoryModel)gridCategories.SelectedRows[0].DataBoundItem;

			var idC = categoryModel.ID.ToString();

			var categoryRow = _views.MainForm.datasetMain.Categories.FindByID(categoryModel.ID);
			if (categoryRow == null)
				return;

			_views.MainForm.adapterDocuments.SqlSetColumnValueByColumn("Category", null, "Category", categoryModel.ID, ExpressionType.Equal);
			_views.MainForm.adapterDocuments.Fill();

			//////////////////////////////////////////////////////////////////////////

			categoryRow.Delete();

			_views.MainForm.adapterCategories.Update(_views.MainForm.datasetMain.Categories);
			_views.MainForm.adapterCategories.Fill(_views.MainForm.datasetMain.Categories);

			FillCategories();

			///////////////////////////////////////////////////////////////////////////////

			////
			//Remove from saved if it exists there
			////
			var conn = _views.MainForm.adapterDocuments.Connection;
			if (conn.State != ConnectionState.Open)
				conn.Open();

			var cmd = conn.CreateCommand();
			cmd.CommandText = "SELECT * FROM SavedCategories";

			var positive = "";
			var negative = "";

			var reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				try
				{
					var type = reader["PosNeg"]
						.ToString();
					var ids = reader["categoryIDs"]
						.ToString();
					var idArray = ids.Split(',');

					if (!string.IsNullOrEmpty(idArray[0]))
					{
						if (type == "Positive")
						{
							foreach (var id in idArray)
							{
								if (id != idC)
									positive += id + ",";
							}
						}
						else //Add to negative
						{
							foreach (var id in idArray)
							{
								if (id != idC)
									negative += id + ",";
							}
						}
					}
				}
				catch
				{
				}
			}
			if (positive.Length > 0)
				positive = positive.Substring(0, positive.Length - 1);
			if (negative.Length > 0)
				negative = negative.Substring(0, negative.Length - 1);

			var cmd1 = conn.CreateCommand();
			//UPDATE SavedCategories SET categoryIDs=? WHERE PosNeg=?
			cmd1.CommandText = "UPDATE SavedCategories SET categoryIDs=? WHERE PosNeg=?";
			cmd1.Parameters.Add("categoryIDs", OleDbType.VarChar)
				.Value = positive;
			cmd1.Parameters.Add("PosNeg", OleDbType.VarChar)
				.Value = "Positive";
			cmd1.ExecuteNonQuery();

			var cmd2 = conn.CreateCommand();
			cmd2.CommandText = "UPDATE SavedCategories SET categoryIDs=? WHERE PosNeg=?";
			cmd2.Parameters.Add("categoryIDs", OleDbType.VarChar)
				.Value = negative;
			cmd2.Parameters.Add("PosNeg", OleDbType.VarChar)
				.Value = "Negative";
			cmd2.ExecuteNonQuery();

			try
			{
				var cmd3 = conn.CreateCommand();

				cmd3.CommandText = "DELETE FROM CategoryToFilterExclusion WHERE CategoryID = " + categoryModel.ID;
				cmd3.ExecuteNonQuery();

				if (_views.MainForm.datasetBilling.Categories.Count > 0)
				{
					_views.MainForm.adapterCategoriesBilling.Delete(categoryModel.ID, categoryModel.Category);
					_views.MainForm.datasetBilling.Categories.FindByID(categoryModel.ID)
						  .Delete();

					_views.MainForm.datasetBilling.Categories.AcceptChanges();
				}
			}
			catch
			{
			}

			conn.Close();

			this.Close();
		}

		protected void DeleteStaticBillingCategory()
		{
			var categoryModel = (CategoryModel)gridCategories.SelectedRows[0].DataBoundItem;

			var categoryRow = _views.MainForm.datasetBilling.Categories.FindByID(categoryModel.ID);
			if (categoryRow == null)
				return;

			int nCategoryID = Convert.ToInt32(categoryRow["ID"]);

			var idC = categoryModel.ID.ToString();
			string categoryName = categoryRow.Category;

			//////////////////////////////////////////////////////////////////////////

			OleDbConnection conn = _views.MainForm.adapterDocumentsBilling.Connection;
			if (conn.State != ConnectionState.Open)
				conn.Open();

			///////////////////////////////////////////////////////////////////////////////

			var clearCategoriesCmd = _views.MainForm.adapterDocumentsBilling.Connection.CreateCommand();
			clearCategoriesCmd.CommandText = "UPDATE Documents SET Category = NULL WHERE Category = @Category";
			clearCategoriesCmd.Parameters.AddWithValue("@Category", idC);
			clearCategoriesCmd.ExecuteNonQuery();

			foreach (BillingDataSet.DocumentsRow rowDocuments in _views.MainForm.datasetBilling.Documents)
			{
				if (!rowDocuments.IsCategoryNull() && rowDocuments.Category == nCategoryID)
					rowDocuments.SetCategoryNull();
			}

			_views.MainForm.datasetBilling.Documents.AcceptChanges();

			//////////////////////////////////////////////////////////////////////////

			categoryRow.Delete();

			_views.MainForm.adapterCategoriesBilling.Update(_views.MainForm.datasetBilling.Categories);
			_views.MainForm.adapterCategoriesBilling.Fill(_views.MainForm.datasetBilling.Categories);

			FillCategories();

			///////////////////////////////////////////////////////////////////////////////

			_views.MainForm.sourceCategoriesBilling.ResetBindings(false);

			////
			//Remove from saved if it exists there
			////

			OleDbCommand cmd = conn.CreateCommand();
			cmd.CommandText = "SELECT * FROM SavedCategories";

			string positive = "";
			string negative = "";

			var reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				try
				{
					var type = reader["PosNeg"].ToString();
					var ids = reader["categoryIDs"].ToString();
					var idArray = ids.Split(',');

					if (!String.IsNullOrEmpty(idArray[0]))
					{
						if (type == "Positive")
						{
							foreach (var id in idArray)
							{
								if (id != idC)
									positive += id + ",";
							}
						}
						else //Add to negative
						{
							foreach (var id in idArray)
							{
								if (id != idC)
									negative += id + ",";
							}
						}
					}
				}
				catch
				{
				}
			}
			if (positive.Length > 0)
				positive = positive.Substring(0, positive.Length - 1);
			if (negative.Length > 0)
				negative = negative.Substring(0, negative.Length - 1);

			OleDbCommand cmd1 = conn.CreateCommand();
			//UPDATE SavedCategories SET categoryIDs=? WHERE PosNeg=?
			cmd1.CommandText = "UPDATE SavedCategories SET categoryIDs=? WHERE PosNeg=?";
			cmd1.Parameters.Add("categoryIDs", OleDbType.VarChar)
				.Value = positive;
			cmd1.Parameters.Add("PosNeg", OleDbType.VarChar)
				.Value = "Positive";
			cmd1.ExecuteNonQuery();

			OleDbCommand cmd2 = conn.CreateCommand();
			cmd2.CommandText = "UPDATE SavedCategories SET categoryIDs=? WHERE PosNeg=?";
			cmd2.Parameters.Add("categoryIDs", OleDbType.VarChar)
				.Value = negative;
			cmd2.Parameters.Add("PosNeg", OleDbType.VarChar)
				.Value = "Negative";
			cmd2.ExecuteNonQuery();

			try
			{
				OleDbCommand cmd3 = conn.CreateCommand();

				cmd3.CommandText = "DELETE FROM CategoryToFilterExclusion WHERE CategoryID = " + nCategoryID;
				cmd3.ExecuteNonQuery();
			}
			catch
			{
			}

			_views.MainForm.adapterCategories.Fill(_views.MainForm.datasetMain.Categories);
			_views.MainForm.adapterCategoriesBilling.Fill(_views.MainForm.datasetBilling.Categories);

			conn.Close();
		}

		#endregion
	}
}