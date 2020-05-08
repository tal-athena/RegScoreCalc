using RegExpLib.Model;
using System.Collections.Generic;

namespace RegExpLib.Processing
{
	public class RegExpScoreProcessingParams : RegExpProcessingParamsBase
	{
		#region Fields

		public string SynergiesFileName { get; set; }
		public string ScoreOutputFileName { get; set; }
		public string MatchesOutputFileName { get; set; }

		#endregion
	}

	public class RegExpScoreProcessingResult : RegExpProcessingResultBase
	{
		#region Fields

		public double DocumentID { get; set; }
		public List<long> Score { get; set; }
		public int CategoryID { get; set; }
		
		#endregion
	}

	public class RegExpMatchProcessingResult : RegExpProcessingResultBase
	{
		#region Fields

		public int RegExpID { get; set; }
		public long TotalDocuments { get; set; }
		public long TotalMatches { get; set; }
        public long TotalRecords { get; set; }
        public Dictionary<int, int> CategorizedRecords { get; set; }

		#endregion
	}

	///////////////////////////////////////////////////////////////////////////////

	public class RegExpStatisticsProcessingParams : RegExpProcessingParamsBase
	{
		#region Fields

		public string Expression { get; set; }
		public bool Replace { get; set; }
		public string ReplacementeString { get; set; }

		public string OutputFileName { get; set; }

		#endregion
	}

	public class RegExpStatisticsProcessingResult : RegExpProcessingResultBase
	{
		#region Fields

		public string Word { get; set; }
		public int Count { get; set; }

		#endregion
	}

	///////////////////////////////////////////////////////////////////////////////

	internal class RegExpStatisticsIntermediateResult : RegExpProcessingResultBase
	{
		#region Fields

		public string Word { get; set; }
		public int Count { get; set; }

		public bool Modified { get; set; }
		public double DocumentID { get; set; }
		public string UpdatedText { get; set; }

		#endregion
	}

	///////////////////////////////////////////////////////////////////////////////

	public class RegExpStatisticsSingleProcessingParams : RegExpProcessingParamsBase
	{
		#region Fields

		public int RegExpID { get; set; }

		public bool ExcludeLookArounds { get; set; }

		public string OutputFileName { get; set; }

		#endregion
	}

	public class RegExpStatisticsSingleProcessingResult : RegExpProcessingResultBase
	{
		#region Fields

		public string Word { get; set; }
		public double DocumentID { get; set; }
		public int Start { get; set; }
		public int Length { get; set; }
        public int ColumnID { get; set; }

		#endregion
	}
    public class RegExpPythonExtractSingleProcessingResult : RegExpProcessingResultBase
    {
        #region Fields
        
        public double DocumentID { get; set; }
        public string Result { get; set; }        
        public int ColumnID { get; set; }
        public string NoteTextColumnName { get; set; }

        #endregion
    }
}
