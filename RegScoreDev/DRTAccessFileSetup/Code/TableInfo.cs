using System.Collections.Generic;

namespace DRTAccessFileSetup.Code
{
	public class TableInfo
	{
		#region Fields

		public string TableName { get; set; }
		public string ProperSchemaXml { get; set; }
		public List<string> Differences { get; set; }

		#endregion

		#region Ctors

		public TableInfo()
		{
			this.Differences = new List<string>();
		}

		#endregion
	}
}
