using System.Collections.Generic;

using RegExpLib.Model;

namespace RegExpLib.Processing
{
	public class ColRegExpStatisticsProcessingParams : RegExpProcessingParamsBase
	{
		#region Fields

		public int ColumnID { get; set; }
		public bool OnlyPositiveScore { get; set; }

		public string MatchesOutputFileName { get; set; }

		#endregion
	}

	public class ColRegExpStatisticsProcessingResult : RegExpProcessingResultBase
	{
		#region Fields

		public int ID { get; set; }
		public long TotalDocuments { get; set; }
		public long TotalMatches { get; set; }
		public long PostitiveDocuments { get; set; }
        public long TotalRecords { get; set; }

		#endregion
	}

	///////////////////////////////////////////////////////////////////////////////

	public class ColRegExpExtractProcessingParams : RegExpProcessingParamsBase
	{
		#region Fields

		public int ColumnID { get; set; }
		public bool ScriptExtract { get; set; }
		public string ScriptCode { get; set; }

		public bool OnlyPositiveScore { get; set; }
		public List<double> DocumentsList { get; set; }

		public string ExtractOutputFileName { get; set; }
        
		#endregion
	}

	public class ColRegExpExtractProcessingResult : RegExpProcessingResultBase
	{
		#region Fields

		public double DocumentID { get; set; }
		public int ColumnID { get; set; }
		public string Value { get; set; }
		public ExtractOptions ExtractOptions { get; set; }

		#endregion
	}
}
