using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using RegExpLib.Common;
using RegExpLib.Core;
using RegExpLib.Database;
using RegExpLib.Model;

namespace RegExpLib.Processing
{
	public class SingleStatisticsCalculator
	{
		#region Fields

		protected readonly Logger _logger;
		protected readonly RegExpBase _regExp;
		protected bool _maxTotalMatchesReached;
		protected bool _maxUniqueMatchesReached;

		#endregion

		#region Ctors

		public SingleStatisticsCalculator(Logger logger, RegExpBase regExp)
		{
			_logger = logger;
			_regExp = regExp;
        }

		#endregion

		#region Operations

		public void CalcStatisticsSingle(RegExpProcessingParamsBase param, bool excludeLookarounds, string outputFileName)
		{
			using (var docsConnection = DatabaseHelper.CreateConnection(param.DocumentsDatabaseFilePath, param.Password))
			{
				docsConnection.Open();

                ///////////////////////////////////////////////////////////////////////////////

                var noteColumnCount = DatabaseHelper.GetNoteColumnCount(docsConnection, "Documents", "NOTE_TEXT");

				var docsCount = DatabaseHelper.GetRowsCount(docsConnection, "Documents");

                //////////////////////////////////////////////////////////////////////////////

                List<int> noteColumnIndexList = new List<int>();
                noteColumnIndexList.Add(1);
                ///////////////////////////////////////////////////////////////////////////////

                var selectQuery = "SELECT ED_ENC_NUM, NOTE_TEXT";

                for (var i = 1; i < noteColumnCount; i ++)
                {
                    selectQuery += ", NOTE_TEXT" + i.ToString();
                    noteColumnIndexList.Add(i + 1);
                }
                selectQuery += " FROM Documents";

                var documentRecords = DatabaseHelper.GetDataRecords(docsConnection, selectQuery);

				///////////////////////////////////////////////////////////////////////////////

				if (excludeLookarounds)
				{
					_regExp.LookAhead.Items.Clear();
					_regExp.NegLookAhead.Items.Clear();
					_regExp.LookBehind.Items.Clear();
					_regExp.NegLookBehind.Items.Clear();

					_regExp.Build();
				}

				///////////////////////////////////////////////////////////////////////////////

				var results = Parallel_CalcStatisticsSingle(documentRecords, docsCount, 0, noteColumnIndexList);

				results.Serialize(param.GetFullPath(outputFileName));

				if (_maxTotalMatchesReached)
					_logger.AppendToLog("More than 20K overall matches found, aborting...");

				if (_maxUniqueMatchesReached)
					_logger.AppendToLog("More than 500 unique matches found, aborting...");
			}
		}

		#endregion

		#region Implementation

		private RegExpProcessingResultsCollection<RegExpStatisticsSingleProcessingResult> Parallel_CalcStatisticsSingle(IEnumerable<IDataRecord> enumerableDocs, long docsCount, int columnIndexID, List<int> columnIndexList)
		{
			var results = new RegExpProcessingResultsCollection<RegExpStatisticsSingleProcessingResult>();

			///////////////////////////////////////////////////////////////////////////////

			long progressStep = 1;
			long progressMax = docsCount * progressStep;
			long progressValue = 0;

			//////////////////////////////////////////////////////////////////////////

			Parallel.ForEach(enumerableDocs, (record, state) =>
			{
				try
				{
					var count = results.Items.Count();
					if (count > 0)
					{
						if (results.Items.Count() > 20000)
						{
							state.Stop();

							_maxTotalMatchesReached = true;

							return;
						}

						///////////////////////////////////////////////////////////////////////////////

						count = results.Items.GroupBy(x => x.Word).Count();

						if (count > 500)
						{
							state.Stop();

							_maxUniqueMatchesReached = true;

							return;
						}
					}

					///////////////////////////////////////////////////////////////////////////////

					CalcDocumentStatisticsSingle(columnIndexID, columnIndexList, record, results);

					///////////////////////////////////////////////////////////////////////////////

					var threadProgressValue = Interlocked.Add(ref progressValue, progressStep);

					var progressPercentage = (int)(threadProgressValue / (double)progressMax * 100D);

					if (!_logger.ReportProgress(progressPercentage, threadProgressValue))
						state.Stop();
				}
				catch (Exception ex)
				{
					_logger.HandleException(ex);
				}
			});

			///////////////////////////////////////////////////////////////////////////////

			return results;
		}

		private void CalcDocumentStatisticsSingle(int columnIndexID, List<int> columnIndexList, IDataRecord record, RegExpProcessingResultsCollection<RegExpStatisticsSingleProcessingResult> results)
		{
			if (record.IsDBNull(columnIndexID))
				return;

            ///////////////////////////////////////////////////////////////////////////////

            for (var i = 0; i < columnIndexList.Count; i++)
            {

                if (record.IsDBNull(columnIndexList[i]))
                    return;

                ///////////////////////////////////////////////////////////////////////////////

                var documentID = record.GetDouble(columnIndexID);
                var docText = record.GetString(columnIndexList[i]);

                ///////////////////////////////////////////////////////////////////////////////

                var matches = _regExp.GetFilteredMatches(docText);
                if (matches.Any())
                {
                    matches.ForEach(x => results.Add(new RegExpStatisticsSingleProcessingResult
                    {
                        Word = x.Value,
                        DocumentID = documentID,
                        ColumnID = columnIndexList[i] - 1,
                        Start = x.Index,
                        Length = x.Length
                    }));
                }
            }
		}

		#endregion
	}
}
