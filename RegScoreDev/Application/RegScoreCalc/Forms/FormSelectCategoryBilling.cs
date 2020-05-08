using RegScoreCalc.Data;

using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormSelectCategoryBilling : Form
	{
		#region Fields

		protected ViewsManager _views;

		protected int _nCategoryID;

		#endregion

		#region Properties

		public int CategoryID
		{
			get { return Property_Get_CategoryID(); }
			set { _nCategoryID = value; }
		}

		#endregion

		#region Ctors

		public FormSelectCategoryBilling(ViewsManager views, DataGridViewCell cell)
		{
			InitializeComponent();

			_views = views;

			this.BackColor = MainForm.ColorBackground;

			gridCategories.AutoGenerateColumns = false;

			InitLocation(cell);
		}

		#endregion

		#region Events

		private void FormSelectCategory_Load(object sender, EventArgs e)
		{
			try
			{
				gridCategories.DataSource = _views.MainForm.sourceCategoriesBilling;

				SelectCategory();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void FormSelectCategory_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					if (this.CategoryID == -1)
						e.Cancel = true;
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			try
			{
				AddCategoryBilling addCategoryForm = new AddCategoryBilling(_views);
				addCategoryForm.ShowDialog();
			}
			catch (System.Exception ex)
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
					DialogResult dlgres = MessageBox.Show("Do you wish to delete category?", "Delete category", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
					if (dlgres == DialogResult.Yes)
					{
						DataRowView rowview = (DataRowView) gridCategories.SelectedRows[0].DataBoundItem;
						BillingDataSet.CategoriesRow rowCategories = (BillingDataSet.CategoriesRow) rowview.Row;

						int nCategoryID = Convert.ToInt32(rowCategories["ID"]);

						string idC = rowCategories[1].ToString();
						string categoryName = rowCategories.Category;

						rowCategories.Delete();

						_views.MainForm.adapterCategoriesBilling.Update(_views.MainForm.datasetBilling.Categories);
						_views.MainForm.adapterCategoriesBilling.Fill(_views.MainForm.datasetBilling.Categories);

						//////////////////////////////////////////////////////////////////////////

						foreach (BillingDataSet.DocumentsRow rowDocuments in _views.MainForm.datasetBilling.Documents)
						{
							if (!rowDocuments.IsCategoryNull() && rowDocuments.Category == nCategoryID)
								rowDocuments.SetCategoryNull();
						}

						_views.MainForm.adapterDocumentsBilling.Update(_views.MainForm.datasetBilling.Documents);
						_views.MainForm.adapterDocumentsBilling.Fill(_views.MainForm.datasetBilling.Documents);

						//////////////////////////////////////////////////////////////////////////

						_views.MainForm.sourceCategoriesBilling.ResetBindings(false);

						////
						//Remove from saved if it exists there
						////
						OleDbConnection conn = _views.MainForm.adapterDocumentsBilling.Connection;
						if (conn.State != ConnectionState.Open)
							conn.Open();

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

							if (_views.MainForm.datasetMain.Categories.Count > 0)
							{
								_views.MainForm.datasetMain.Categories.First(x => x.ID == nCategoryID).Delete();

								_views.MainForm.adapterCategories.Update(_views.MainForm.datasetMain.Categories);
								_views.MainForm.adapterCategories.Fill(_views.MainForm.datasetMain.Categories);
							}

							_views.MainForm.adapterCategoriesBilling.Delete(nCategoryID, categoryName);
							_views.MainForm.datasetBilling.Categories.First(x => x.ID == nCategoryID).Delete();
							_views.MainForm.datasetBilling.Categories.AcceptChanges();
						}
						catch
						{
						}

						conn.Close();
					}
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		#endregion

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

		protected int Property_Get_CategoryID()
		{
			int nResult = -1;

			if (gridCategories.SelectedRows.Count == 1)
			{
				DataGridViewRow row = gridCategories.SelectedRows[0];
				DataRowView rowview = (DataRowView) row.DataBoundItem;

				BillingDataSet.CategoriesRow rowCategories = (BillingDataSet.CategoriesRow) rowview.Row;
				nResult = rowCategories.ID;
			}

			return nResult;
		}

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

		protected void SelectCategory()
		{
			try
			{
				if (_nCategoryID != -1)
				{
					DataRowView rowview;

					BillingDataSet.CategoriesRow rowCategories;

					foreach (DataGridViewRow row in gridCategories.Rows)
					{
						rowview = (DataRowView) row.DataBoundItem;
						rowCategories = (BillingDataSet.CategoriesRow) rowview.Row;

						if (rowCategories.ID == _nCategoryID)
						{
							row.Selected = true;
							gridCategories.FirstDisplayedScrollingRowIndex = row.Index;
							break;
						}
					}
				}
			}
			catch
			{
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
		}
	}
}