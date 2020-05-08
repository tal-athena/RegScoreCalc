using System;
using System.Linq;
using System.Windows.Forms;

using RegScoreCalc.Data;

namespace RegScoreCalc
{
	public partial class AddCategoryBilling : Form
	{
		#region Fields

		protected ViewsManager _views;

		protected int? _selectedCategoryID;

		#endregion

		#region Properties

		public int? SelectedCategoryID
		{
			get { return _selectedCategoryID; }
		}

		#endregion

		#region Ctors

		public AddCategoryBilling(ViewsManager views)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			_views = views;
		}

		#endregion

		#region Events

		private void AddCategory_Load(object sender, EventArgs e)
		{
			try
			{
				////Populate cmbFilters
				//foreach (var filter in _views.MainForm.datasetBilling.ICDFilters)
				//{
				//    cmbExcludeFromFilter.Items.Add(new AddGroupComboboxItem() { Text = filter.Name, GroupID = filter.FilterID });
				//}

				txtID.Text = GetNextRowID()
					.ToString();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void AddCategory_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					if (!String.IsNullOrEmpty(textBox_AddCategory.Text))
					{
						BillingDataSet.CategoriesRow row = _views.MainForm.datasetBilling.Categories.NewCategoriesRow();
						row.Category = textBox_AddCategory.Text;

						var categoryID = Convert.ToInt32(txtID.Text);
						row.ID = categoryID;

						_views.MainForm.datasetBilling.Categories.AddCategoriesRow(row);
						_views.MainForm.adapterCategoriesBilling.Update(_views.MainForm.datasetBilling.Categories);
						_views.MainForm.adapterCategoriesBilling.Fill(_views.MainForm.datasetBilling.Categories);

						_selectedCategoryID = categoryID;

						//if (cmbExcludeFromFilter.SelectedIndex > -1)
						//{
						//    var selectedItem = (AddGroupComboboxItem)cmbExcludeFromFilter.SelectedItem;
						//    var filterId = (int)selectedItem.GroupID;
						//    var categoryType = selectedItem.

						//    var newRow = _views.MainForm.datasetBilling.CategoryToFilterExclusion.NewCategoryToFilterExclusionRow();
						//    newRow.CategoryID = categoryID;
						//    newRow.FilterID = filterId;
						//    newRow.Type = "";

						//    _views.MainForm.datasetBilling.CategoryToFilterExclusion.Rows.Add(newRow);
						//    _views.MainForm.datasetBilling.CategoryToFilterExclusion.AcceptChanges();

						//    _views.MainForm.adapterCategoryToFilterExclusionBilling.Insert(categoryID, filterId,"");

						//}
					}
					else
					{
						MessageBox.Show("Category name cannot be empty");
						e.Cancel = true;
					}
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
				e.Cancel = true;
			}
		}

		#endregion

		#region Implementation

		protected int GetNextRowID()
		{
			if (_views.MainForm.datasetBilling.Categories.Count > 0)
				return _views.MainForm.datasetBilling.Categories.Max(x => x.ID) + 1;

			return 1;
		}

		#endregion
	}
}