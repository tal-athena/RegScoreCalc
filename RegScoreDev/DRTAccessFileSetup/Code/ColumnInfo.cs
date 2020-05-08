using System;
using System.Collections.Generic;
using System.Reflection;

namespace DRTAccessFileSetup.Code
{
	public class ColumnInfo
	{
		#region Fields

		public string Name { get; set; }
		public string Type { get; set; }
		public string IsKey { get; set; }
		public string IsLong { get; set; }
		public string NumericScale { get; set; }
		public string ColumnSize { get; set; }
		public string NumericPrecision { get; set; }
		public string IsUnique { get; set; }
		public string AllowDBNull { get; set; }
		public string IsAutoIncrement { get; set; }

		#endregion

		#region Operations

		public List<string> GetColumnDifferences(ColumnInfo target)
		{
			var differences = new List<string>();

			var type = typeof(ColumnInfo);

			var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

			foreach (var prop in props)
			{
				var thisPropValue = prop.GetValue(this) as string;
				var targetPropValue = prop.GetValue(target) as string;

				if (!AreStringsEqual(thisPropValue, targetPropValue))
					differences.Add(String.Format("Property '{0}' of column '{1}' does not match. Found: '{2}', expected: '{3}'", prop.Name, this.Name, targetPropValue, thisPropValue));
			}

			///////////////////////////////////////////////////////////////////////////////

			return differences;
		}

		#endregion

		#region Implementation

		protected bool AreStringsEqual(string val1, string val2)
		{
			return String.Compare(val1, val2, StringComparison.CurrentCultureIgnoreCase) == 0;
		}

		#endregion
	}
}
