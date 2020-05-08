using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormColumnPropsCategories : Form
	{
		#region Fields

		protected ViewsManager _views;
        protected string _dynamicColumnTitle;
		protected int _dynamicColumnID;

		#endregion

		#region Ctors

		public FormColumnPropsCategories(ViewsManager views, int dynamicColumnID, string dynamicColumnTitle)
		{
			InitializeComponent();

			_views = views;
            _dynamicColumnTitle = dynamicColumnTitle;
            _dynamicColumnID = dynamicColumnID;

			this.BackColor = MainForm.ColorBackground;

			FillCategoriesList();

			headerCategoryName.Width = lvCategories.ClientSize.Width - headerCategoryNumber.Width;

            InitializeCheckBox();
		}

		#endregion

		#region Events

		private void btnAddCategory_Click(object sender, EventArgs e)
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

		private void btnRenameCategory_Click(object sender, EventArgs e)
		{
			try
			{
				RenameCategory();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnDeleteCategory_Click(object sender, EventArgs e)
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

		private void lvCategories_ItemActivate(object sender, EventArgs e)
		{
			try
			{
				RenameCategory();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation

		protected void FillCategoriesList()
		{
			lvCategories.Items.Clear();

			///////////////////////////////////////////////////////////////////

			var categories = _views.MainForm.datasetMain.DynamicColumnCategories.Where(x => x.RowState != DataRowState.Deleted && x.DynamicColumnID == _dynamicColumnID);
			foreach (var row in categories)
			{
				var item = lvCategories.Items.Add(row.Title);
				item.SubItems.Add(row.Number.ToString());

				item.Tag = row;
			}
		}

		protected void AddCategory()
		{
			var classCategories = _views.MainForm.datasetMain.DynamicColumnCategories.Where(
				x => x.RowState != DataRowState.Deleted && x.DynamicColumnID == _dynamicColumnID)
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
				_views.MainForm.datasetMain.DynamicColumnCategories.AddDynamicColumnCategoriesRow(_dynamicColumnID, formCategory.Title, formCategory.Number, "");

				FillCategoriesList();
			}
		}

		protected void RenameCategory()
		{
			if (lvCategories.SelectedItems.Count == 1)
			{
				var categoryRow = (MainDataSet.DynamicColumnCategoriesRow) lvCategories.SelectedItems[0].Tag;

				var formCategory = new FormEditColumnCategory();
				formCategory.ViewsManager = _views;
				formCategory.DynamicColumnID = _dynamicColumnID;
				formCategory.ID = categoryRow.ID;
				formCategory.Title = categoryRow.Title;
				formCategory.Number = categoryRow.Number;

				if (formCategory.ShowDialog() == DialogResult.OK)
				{
					categoryRow.Title = formCategory.Title;
					categoryRow.Number = formCategory.Number;

					FillCategoriesList();
				}
			}
		}

		protected void DeleteCategory()
		{
			if (lvCategories.SelectedItems.Count == 1)
			{
				var categoryRow = (MainDataSet.DynamicColumnCategoriesRow) lvCategories.SelectedItems[0].Tag;

				string message = "Do you wish to delete this category?";

				var dlgres = MessageBox.Show(message, MainForm.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

				if (dlgres == DialogResult.Yes)
				{
					var dynamicColumnRow = _views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.ID == _dynamicColumnID);
					if (dynamicColumnRow == null)
						throw new Exception("Column does not exist");

					///////////////////////////////////////////////////////////////////////////////

					_views.MainForm.adapterDocuments.SqlClearColumnValues(
						columnName: dynamicColumnRow.Title,
						whereColumn: dynamicColumnRow.Title,
						whereValue: categoryRow.Title,
						whereCondition: ExpressionType.Equal);

					///////////////////////////////////////////////////////////////////////////////

					_views.MainForm.datasetMain.Documents.Where(x => !x.IsNull(dynamicColumnRow.Title) && (string) x[dynamicColumnRow.Title] == categoryRow.Title)
					      .ToList()
					      .ForEach(x =>
					               {
						               x[dynamicColumnRow.Title] = DBNull.Value;
									   x.AcceptChanges();
					               });

					///////////////////////////////////////////////////////////////////////////////

					categoryRow.Delete();

					FillCategoriesList();
				}
			}
		}

        #endregion

        private void chkSVMColumn_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            var svmColumnName = _dynamicColumnTitle + " (SVM)";
            var svmColumn = _views.MainForm.adapterDocuments.GetExtraColumnsCollection()
                                  .FirstOrDefault(x => String.Compare(x.Name, svmColumnName, StringComparison.InvariantCultureIgnoreCase) == 0);
            if( chkSVMColumn.Checked == true)
            {
                if (svmColumn == null)
                {
                    _views.MainForm.adapterDocuments.AddExtraColumn(svmColumnName, "INTEGER", true);
                    _views.MainForm.adapterReviewMLDocumentsNew.AddExtraColumn(svmColumnName, "INTEGER", false);
                }
            }
            else
            {
                if (svmColumn != null)
                {
                    _views.MainForm.adapterDocuments.DeleteColumn(svmColumnName, true);
                    _views.MainForm.adapterReviewMLDocumentsNew.DeleteColumn(svmColumnName, false);
                }
            }
            this.Cursor = Cursors.Default;
        }
    }
}