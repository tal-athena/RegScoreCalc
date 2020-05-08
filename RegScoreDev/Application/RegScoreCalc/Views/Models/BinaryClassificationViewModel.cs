using System;
using System.Collections.Generic;
using System.Drawing;

using Newtonsoft.Json;

namespace RegScoreCalc.Views.Models
{
	public class BinaryClassificationViewModel
	{
		#region Fields

		public SolidBrush positiveColor { get; set; }
		public SolidBrush negativeColor { get; set; }
		public SolidBrush excludedColor { get; set; }

		public List<BinaryClassificationColumnInfo> columns { get; set; }

		public List<FolderInfo> outputFolders { get; set; }

		#endregion

		#region Ctors

		public BinaryClassificationViewModel()
		{
			this.columns = new List<BinaryClassificationColumnInfo>();
			this.outputFolders = new List<FolderInfo>();
		}

		#endregion

		#region Operations

		public static BinaryClassificationViewModel FromJSON(string json)
		{
			if (String.IsNullOrEmpty(json))
				return null;

			///////////////////////////////////////////////////////////////////////////////

			return JsonConvert.DeserializeObject<BinaryClassificationViewModel>(json, new ColorConverter());
		}

		#endregion
	}

	public class BinaryClassificationColumnInfo
	{
		public int ID { get; set; }
		public List<CategoryInfo> positiveCategories { get; set; }
		public List<CategoryInfo> excludedCategories { get; set; }

		public BinaryClassificationColumnInfo()
		{
			positiveCategories = new List<CategoryInfo>();
			excludedCategories = new List<CategoryInfo>();
		}
	}

	public class CategoryInfo
	{
		public int ID { get; set; }
		public string Title { get; set; }
	}

	public class FolderInfo
	{
		public string fullPath { get; set; }

		public long unixCreatedTime { get; set; }
		public DateTime createdTime
		{
			get
			{
				var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
				return dt.AddMilliseconds(this.unixCreatedTime).ToLocalTime();
			}
		}
	}
}