using System.Collections.Generic;

namespace RegScoreCalc.Views.Models
{
	public class ReviewMLParameters
	{
		#region Fields

		public string PathToCSV { get; set; }
		public List<int> PositiveCategories { get; set; }

		#endregion
	}
}
