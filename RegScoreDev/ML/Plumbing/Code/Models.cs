using System;

using SQLite;

namespace Plumbing.Code
{
	public enum PlumbingToolVersion
	{
		Version1 = 0,
		Version2 = 2
	}

	public class Parameters
	{
		#region Fields

		public PlumbingToolVersion version { get; set; }
		public bool runPython { get; set; }
		public bool displaySettings { get; set; }
		public bool divideCategories { get; set; }
		public bool scoreAll { get; set; }
		public bool scoreUncategorized { get; set; }
		public int validationPercentage { get; set; }
		public int[] positiveCategories { get; set; }
		public int[] excludedCategories { get; set; }
		public int dynamicColumnID { get; set; }
		public string dynamicColumnTitle { get; set; }
		public string[] dynamicPositiveCategories { get; set; }
		public string[] dynamicExcludedCategories { get; set; }
		public bool includeBigrams { get; set; }
		public bool includeTrigrams { get; set; }
		public int numberOfDimensions { get; set; }
		public string password { get; set; }

		#endregion
	}

	public class Documents
	{
		#region Fields

		[PrimaryKey]
		public double ED_ENC_NUM { get; set; }

		public string NOTE_TEXT { get; set; }

		public int Score { get; set; }

		public int? Category { get; set; }

		#endregion
	}

	public class Categories
	{
		#region Fields

		[PrimaryKey]
		public long ID { get; set; }

		public string Category { get; set; }

		#endregion
	}

	public class EmptyDatasetException : Exception
	{
		#region Ctors

		public EmptyDatasetException(string message) : base(message)
		{

		}

		#endregion
	}
}
