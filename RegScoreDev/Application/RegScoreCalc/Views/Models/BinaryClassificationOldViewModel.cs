using System;
using System.Collections.Generic;
using System.Drawing;

using Newtonsoft.Json;

namespace RegScoreCalc.Views.Models
{
	public class BinaryClassificationOldViewModel
	{
		#region Fields

		public SolidBrush positiveColor { get; set; }
		public SolidBrush negativeColor { get; set; }
		public SolidBrush excludedColor { get; set; }

		public List<int> positiveCategories { get; set; }
		public List<int> excludedCategories { get; set; }

		#endregion

		#region Operations

		public static BinaryClassificationOldViewModel FromJSON(string json)
		{
			if (String.IsNullOrEmpty(json))
				return null;

			///////////////////////////////////////////////////////////////////////////////

			return JsonConvert.DeserializeObject<BinaryClassificationOldViewModel>(json, new ColorConverter());
		}

		#endregion
	}
}