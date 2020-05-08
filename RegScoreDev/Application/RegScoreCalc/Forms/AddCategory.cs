using System;
using System.Linq;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class AddCategory : Form
	{
		#region Fields

		protected ViewsManager _views;
		protected int? _selectedCategoryID;

		#endregion

		#region Ctors

		public AddCategory(ViewsManager views)
		{
			InitializeComponent();

			_views = views;

			this.BackColor = MainForm.ColorBackground;
		}

		public int? SelectedCategoryID
		{
			get { return _selectedCategoryID; }
		}

		#endregion

		#region Events

		private void AddCategory_Load(object sender, EventArgs e)
		{
			try
			{
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
						var rowID = Convert.ToInt32(txtID.Text);

						MainDataSet.CategoriesRow row = _views.MainForm.datasetMain.Categories.NewCategoriesRow();
						row.Category = textBox_AddCategory.Text;
						row.ID = rowID;
						row.Color = panelColor.BackColor.ToArgb();
						row.IsSelected = true;

						_views.MainForm.datasetMain.Categories.AddCategoriesRow(row);
						_views.MainForm.adapterCategories.Update(_views.MainForm.datasetMain.Categories);
						_views.MainForm.adapterCategories.Fill(_views.MainForm.datasetMain.Categories);

						_selectedCategoryID = rowID;
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

		private void panelColor_Click(object sender, EventArgs e)
		{
			try
			{
				var dlg = new ColorDialog { Color = panelColor.BackColor };
				if (dlg.ShowDialog(this) == DialogResult.OK)
					panelColor.BackColor = dlg.Color;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation

		protected int GetNextRowID()
		{
			if (_views.MainForm.datasetMain.Categories.Count > 0)
				return _views.MainForm.datasetMain.Categories.Max(x => x.ID) + 1;
			else
				return 1;
		}

		#endregion
	}
}