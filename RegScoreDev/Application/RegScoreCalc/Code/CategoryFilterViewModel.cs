using System;

using Newtonsoft.Json;

namespace RegScoreCalc.Code
{
	public class CategoryFilterViewModel
	{
		public bool ShowAllDocuments { get; set; }
		public bool ShowUncategorizedDocuments { get; set; }

		public static CategoryFilterViewModel GetCategoryFilter(ViewsManager views)
		{
			try
			{
				var json = BrowserManager.GetViewData(views, "CategoryFilter");
				if (!String.IsNullOrEmpty(json))
				{
					var filter = JsonConvert.DeserializeObject<CategoryFilterViewModel>(json);
					return filter;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			return new CategoryFilterViewModel
			       {
				       ShowAllDocuments = true,
				       ShowUncategorizedDocuments = false
			       };
		}

		public static void SaveCategoryFilter(ViewsManager views, CategoryFilterViewModel filter)
		{
			try
			{
				var json = JsonConvert.SerializeObject(filter);

				BrowserManager.SetViewData(views, "CategoryFilter", json);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}
	}
}
