using System;
using System.Linq;

namespace RegScoreCalc
{
	public static class SvmColumnService
	{
		public static ColumnInfo GetSvmColumnInfo(ViewsManager views, int dynamicCategoryColumnID, bool needCreate)
		{
			if (dynamicCategoryColumnID > 0)
			{
				var dynamicCategory = views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.ID == dynamicCategoryColumnID);
				if (dynamicCategory == null)
					return null;

				var svmColumnName = dynamicCategory.Title + " (SVM)";

				///////////////////////////////////////////////////////////////////////////////

				var columnInfo = views.MainForm.adapterDocuments.GetActualColumnsList()
					                    .FirstOrDefault(x => String.Compare(x.Name, svmColumnName, StringComparison.InvariantCultureIgnoreCase) == 0);
				if (columnInfo == null && needCreate)
				{
					views.MainForm.adapterDocuments.AddExtraColumn(svmColumnName, "INTEGER", true);
					columnInfo = views.MainForm.adapterReviewMLDocumentsNew.AddExtraColumn(svmColumnName, "INTEGER", false);
					views.BeforeDocumentsTableLoad(true);
					views.AfterDocumentsTableLoad(true);
				}

				return columnInfo;
			}
			else
			{
				var columnInfo = views.MainForm.adapterDocuments.GetActualColumnsList()
				                      .FirstOrDefault(x => String.Compare(x.Name, "Proc1SVM", StringComparison.InvariantCultureIgnoreCase) == 0);
				return columnInfo;
			}
		}

		public static bool IsSvmColumn(ViewsManager views, string columnName)
		{
			if (!columnName.EndsWith(" (SVM)"))
				return false;

			foreach (var column in views.MainForm.adapterDocuments.GetActualColumnsList()
			                            .Where(x => x.IsDynamic))
			{
				var dynamicColumn = views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.ID == column.DynamicColumnID && x.Type == (int) DynamicColumnType.Category);
				if (dynamicColumn != null)
				{
					if (String.Compare(columnName, dynamicColumn.Title + " (SVM)", StringComparison.InvariantCultureIgnoreCase) == 0)
						return true;
				}
			}

			return false;
		}
	}
}
