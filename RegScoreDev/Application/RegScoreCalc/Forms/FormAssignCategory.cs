using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
	public partial class FormAssignCategory : Form
	{
		#region Fields

		protected readonly ViewsManager _views;

		#endregion

		#region Types

		protected class ScoreOptions
		{
			#region Fields

			public int ResultCount { get; set; }
			public object SelectedCategory { get; set; }
			public int Score { get; set; }
			public ScoreCondition ScoreCondition { get; set; }

			#endregion
		}

		protected enum ScoreCondition
		{
			#region Constants

			LessThan,
			EqualTo,
			GraterThan

			#endregion
		}

		#endregion

		#region Ctors

		public FormAssignCategory(ViewsManager views)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			///////////////////////////////////////////////////////////////////////////////

			lbDynamicColumns.DisplayMember = "Title";

			cmbCondition.SelectedIndex = 0;

			txtScoreValue.Minimum = Int32.MinValue;
			txtScoreValue.Maximum = Int32.MaxValue;

			///////////////////////////////////////////////////////////////////////////////

			_views = views;
		}

		#endregion

		#region Events

		private void FormAssignCategory_Load(object sender, EventArgs e)
		{
			try
			{
				FillColumnsList();

				cmbCondition.SelectedIndex = Properties.Settings.Default.FormAutoAssignCattegoryScoreCondition;
				txtScoreValue.Value = Properties.Settings.Default.FormAutoAssignCattegoryScoreValue;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void FormAssignCategory_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				Properties.Settings.Default.FormAutoAssignCattegoryScoreCondition = cmbCondition.SelectedIndex;
				Properties.Settings.Default.FormAutoAssignCattegoryScoreValue = (int)txtScoreValue.Value;

				Properties.Settings.Default.Save();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void lbCategoryColumns_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				FillCategoryValues();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void lbSelectCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateRules();

				cmbCondition.Focus();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			try
			{
				AssignCategories();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnApplyAndClose_Click(object sender, EventArgs e)
		{
			try
			{
				if (AssignCategories())
					this.Close();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation

		protected bool AssignCategories()
		{
			var result = false;

			if (lbDynamicColumns.SelectedIndex != -1 && lbSelectCategory.SelectedIndex != -1)
			{
				if (lbDynamicColumns.SelectedIndex > 0)
				{
					var selectedDynamicColumn = lbDynamicColumns.SelectedItem as MainDataSet.DynamicColumnsRow;
					if (selectedDynamicColumn != null)
					{
						var selectedCategory = lbSelectCategory.SelectedItem as MainDataSet.DynamicColumnCategoriesRow;
						if (selectedCategory != null)
						{
							var count = AssignDynamicCategory(selectedCategory);
							
							MessageBox.Show(String.Format("Assigned category to {0} documents", count), MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

							result = true;
						}
					}
				}
				else
				{
					var selectedCategory = lbSelectCategory.SelectedItem as MainDataSet.CategoriesRow;
					if (selectedCategory != null)
					{
						var count = AssignBuiltInCategory(selectedCategory);

						MessageBox.Show(String.Format("Assigned category to {0} documents", count), MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

						result = true;
					}
				}
			}

			if (!result)
				MessageBox.Show("Please select category", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

			return result;
		}

		protected int AssignDynamicCategory(MainDataSet.DynamicColumnCategoriesRow selectedCategory)
		{
			var options = FillScoreOptions(selectedCategory);

			var formProgress = new FormGenericProgress("Assigning categories...", DoAssignDynamicCategory, options, true);
			formProgress.ShowDialog(_views.MainForm);

			return options.ResultCount;
		}

		protected int AssignBuiltInCategory(MainDataSet.CategoriesRow selectedCategory)
		{
			var options = FillScoreOptions(selectedCategory);

			var formProgress = new FormGenericProgress("Assigning categories...", DoAssignBuiltInCategory, options, true);
			formProgress.ShowDialog(_views.MainForm);

			return options.ResultCount;
		}

		protected bool DoAssignDynamicCategory(BackgroundWorker worker, object objArgument)
		{
			var options = (ScoreOptions) objArgument;
			var categoryRow = (MainDataSet.DynamicColumnCategoriesRow) options.SelectedCategory;
			var dynamicColumnRow = _views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.ID == categoryRow.DynamicColumnID);
			if (dynamicColumnRow == null)
				throw new Exception("Column does not exist");

			///////////////////////////////////////////////////////////////////////////////

			var expressionType = ScoreConditionToExpressionType(options.ScoreCondition);

			options.ResultCount = _views.MainForm.adapterDocuments.SqlSetColumnValueByColumn(dynamicColumnRow.Title, categoryRow.Title, "Score", options.Score, expressionType);

			///////////////////////////////////////////////////////////////////////////////

			_views.MainForm.adapterDocuments.Fill();

			///////////////////////////////////////////////////////////////////////////////

			return true;
		}

		protected bool DoAssignBuiltInCategory(BackgroundWorker worker, object objArgument)
		{
			var options = (ScoreOptions) objArgument;
			var category = (MainDataSet.CategoriesRow) options.SelectedCategory;

			///////////////////////////////////////////////////////////////////////////////

			var expressionType = ScoreConditionToExpressionType(options.ScoreCondition);

			options.ResultCount = _views.MainForm.adapterDocuments.SqlSetColumnValueByColumn("Category", category.ID, "Score", options.Score, expressionType);

			///////////////////////////////////////////////////////////////////////////////

			_views.MainForm.adapterDocuments.Fill();

			///////////////////////////////////////////////////////////////////////////////

			return true;
		}

		protected ScoreOptions FillScoreOptions(object selectedCategory)
		{
			return new ScoreOptions
			       {
				       SelectedCategory = selectedCategory, ScoreCondition = (ScoreCondition) cmbCondition.SelectedIndex, Score = (int) txtScoreValue.Value,
			       };
		}

		protected ExpressionType ScoreConditionToExpressionType(ScoreCondition condition)
		{
			switch (condition)
			{
				case ScoreCondition.LessThan:
					return ExpressionType.LessThan;

				case ScoreCondition.EqualTo:
					return ExpressionType.Equal;

				case ScoreCondition.GraterThan:
					return ExpressionType.GreaterThan;
			}

			throw new Exception("Unsupported condition '" + condition + "'");
		}

		protected void UpdateRules()
		{
			tabAssignmentRules.Enabled = lbSelectCategory.SelectedIndex != -1;
		}

		protected void FillColumnsList()
		{
			lbDynamicColumns.Items.Clear();
			lbSelectCategory.Items.Clear();

			UpdateRules();

			lbDynamicColumns.Items.Add("[Built-in category]");

			lbDynamicColumns.Items.AddRange(Enumerable.Cast<object>(_views.MainForm.datasetMain.DynamicColumns.Where(x => x.Type == (int) DynamicColumnType.Category))
			                                          .ToArray());

			lbDynamicColumns.SelectedIndex = 0;
		}

		protected void FillCategoryValues()
		{
			lbSelectCategory.Items.Clear();

			if (lbDynamicColumns.SelectedIndex > 0)
			{
				var selectedColumnClass = lbDynamicColumns.SelectedItem as MainDataSet.DynamicColumnsRow;
				if (selectedColumnClass != null)
				{
					var categories = _views.MainForm.datasetMain.DynamicColumnCategories.Where(x => x.DynamicColumnID == selectedColumnClass.ID);

					lbSelectCategory.DisplayMember = "Title";
					lbSelectCategory.Items.AddRange(categories.Cast<object>()
					                                          .ToArray());
				}
			}
			else
			{
				lbSelectCategory.DisplayMember = "Category";
				lbSelectCategory.Items.AddRange(_views.MainForm.datasetMain.Categories.Cast<object>()
				                                      .ToArray());
			}

			UpdateRules();
		}

		#endregion
	}
}
