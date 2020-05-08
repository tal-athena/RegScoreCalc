using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;

using RegExpLib.Common;
using RegExpLib.Core;
using RegExpLib.Database;
using RegExpLib.Model;

namespace RegExpLib.Processing
{
	public class RegExpProcessor
	{
		#region Fields

		protected List<RegExpBase> _listRegExps;

		protected bool _hasEmptyItems;

		protected int _prevPosition;

		public Logger Logger { get; set; }

		#endregion

		#region Properties

		public RegExpBase Single
		{
			get { return _listRegExps.FirstOrDefault(); }
		}

		public List<RegExpBase> Items
		{
			get { return _listRegExps; }
		}

		public bool HasEmptyItems
		{
			get { return _hasEmptyItems; }
		}

		#endregion

		#region Ctors

		protected RegExpProcessor(Logger logger)
		{
			_listRegExps = new List<RegExpBase>();

			this.Logger = logger;
		}

		public RegExpProcessor(Logger logger, string regExpDatabasePath, string password, bool ignoreCase = true, bool compiled = false)
			: this(logger)
		{
			using (var connection = DatabaseHelper.CreateConnection(regExpDatabasePath, password))
			{
				var options = RegExpOptions.CreateOptions(ignoreCase, compiled);

				///////////////////////////////////////////////////////////////////////////////

				var query = "SELECT ID, RegExp, score, [Arithmetic factor], [prefix match], [suffix match], [RegExpColor], [lookahead], [lookbehind], [neg lookahead], [neg lookbehind], [exceptions], [categoryID] FROM RegExp";

				var regExpRows = DatabaseHelper.GetDataRows(connection, query);

				foreach (var row in regExpRows)
				{
					try
					{
						var regExp = RegExpFactory.Create_RegExp(row, true, options);
						if (regExp != null)
							_listRegExps.Add(regExp);
					}
					catch
					{
						this.Logger.HandleRegExpException(row);
					}
				}

				_hasEmptyItems = Sanitize();
			}
		}

		public RegExpProcessor(IEnumerable<DataRow> regExpRows, bool ignoreCase = true, bool compiled = false)
			: this(null)
		{
			var options = RegExpOptions.CreateOptions(ignoreCase, compiled);

			///////////////////////////////////////////////////////////////////////////////

			foreach (var row in regExpRows)
			{
				var regExp = RegExpFactory.Create_RegExp(row, true, options);
				if (regExp != null)
					_listRegExps.Add(regExp);
			}

			_hasEmptyItems = Sanitize();
		}

		public RegExpProcessor(DataRow row, bool ignoreCase = true, bool compiled = false)
			: this(null)
		{
			var regExp = RegExpFactory.Create_RegExp(row, true, RegExpOptions.CreateOptions(ignoreCase, compiled));
			if (regExp == null)
				throw new DataException("Row is deleted or detached");

			_listRegExps.Add(regExp);

			_hasEmptyItems = Sanitize();
		}

		public RegExpProcessor(Logger logger, object id, string regExp, object score, object factor, string prefixMatch, string suffixMatch, object color, string lookAhead, string lookBehind, string negLookAhead, string negLookBehind, string exceptions, object categoryID, bool ignoreCase, bool compiled)
			: this(logger)
		{
			try
			{
				_listRegExps.Add(new RegExp(RegExpOptions.CreateOptions(ignoreCase, compiled), id, regExp, true, score, factor, prefixMatch, suffixMatch, color, lookAhead, lookBehind, negLookAhead, negLookBehind, exceptions, String.Empty, categoryID));
			}
			catch
			{
				this.Logger.HandleRegExpException(regExp);
			}

			_hasEmptyItems = Sanitize();
		}

		public RegExpProcessor(string regExpDatabasePath, string password, int regExpID, bool ignoreCase = true, bool compiled = false)
			: this(null)
		{
			using (var connection = DatabaseHelper.CreateConnection(regExpDatabasePath, password))
			{
				var options = RegExpOptions.CreateOptions(ignoreCase, compiled);

				///////////////////////////////////////////////////////////////////////////////

				var query = "SELECT ID, RegExp, score, [Arithmetic factor], [prefix match], [suffix match], [RegExpColor], [lookahead], [lookbehind], [neg lookahead], [neg lookbehind], [exceptions], [categoryID] FROM RegExp WHERE ID = " + regExpID;

				var regExpRows = DatabaseHelper.GetDataRows(connection, query).ToList();
				if (!regExpRows.Any())
					throw new Exception("Invalid RegExpID");

				var regExp = RegExpFactory.Create_RegExp(regExpRows.First(), true, options);
				_listRegExps.Add(regExp);

				///////////////////////////////////////////////////////////////////////////////

				_hasEmptyItems = Sanitize();
			}
		}

		#endregion

		#region Operations

		public void CalcScores(RegExpScoreProcessingParams param)
		{
			var synergies = LoadSynergies(Path.Combine(param.WorkingFolder, param.SynergiesFileName));

			using (var docsConnection = DatabaseHelper.CreateConnection(param.DocumentsDatabaseFilePath, param.Password))
			{
				docsConnection.Open();


                ///////////////////////////////////////////////////////////////////////////////

                var noteColumnCount = DatabaseHelper.GetNoteColumnCount(docsConnection, "Documents", "NOTE_TEXT");

                ///////////////////////////////////////////////////////////////////////////////

                List<int> noteColumnIndexList = new List<int>();
                noteColumnIndexList.Add(2);
                ///////////////////////////////////////////////////////////////////////////////

                var docsCount = DatabaseHelper.GetRowsCount(docsConnection, "Documents");                               

				var query = "SELECT ED_ENC_NUM, Category, NOTE_TEXT";

                for (var noteColumnIndex = 1; noteColumnIndex < noteColumnCount; noteColumnIndex ++)
                {
                    query += ", NOTE_TEXT" + noteColumnIndex.ToString();

                    noteColumnIndexList.Add(noteColumnIndex + 2);
                }

                query += " FROM Documents";

				var documentRecords = DatabaseHelper.GetDataRecords(docsConnection, query);

                ///////////////////////////////////////////////////////////////////////////////

            
                var results = Parallel_CalcScores(documentRecords, docsCount, 0, noteColumnIndexList, synergies);

				results.Serialize(param.GetFullPath(param.ScoreOutputFileName));

				///////////////////////////////////////////////////////////////////////////////

				var matchResults = new RegExpProcessingResultsCollection<RegExpMatchProcessingResult>(_listRegExps.Select(x => new RegExpMatchProcessingResult
																															   {
																																   RegExpID = x.ID,
																																   TotalMatches = x.TotalMatches,
																																   TotalDocuments = x.TotalDocuments,
                                                                                                                                   TotalRecords = x.TotalRecords,
                                                                                                                                   CategorizedRecords = x.CategorizedRecords
																															   }));

				matchResults.Serialize(param.GetFullPath(param.MatchesOutputFileName));
			}
		}

		public RegExpProcessingResultsCollection<RegExpMatchProcessingResult> CalcScores(DataTable table, IEnumerable<DataRow> documentRows, List<RegExpSynergy> synergies)
		{
			var columnIndexID = table.Columns.IndexOf("ED_ENC_NUM");

            List<int> columnIndexTextList = new List<int>();

            for (int i = 0; i < table.Columns.Count; i++)
                if (table.Columns[i].ColumnName.StartsWith("NOTE_TEXT"))
                    columnIndexTextList.Add(i);
        
			// var columnIndexText = table.Columns.IndexOf("NOTE_TEXT");

			if (columnIndexID == -1 || columnIndexTextList.Count < 1)
				throw new Exception("Cannot find source columns");

			///////////////////////////////////////////////////////////////////////////////

			var docsCount = table.Rows.Count;
                        
			Parallel_CalcScores(DatabaseHelper.AsDataRecordEnumerable(documentRows), docsCount, columnIndexID, columnIndexTextList, synergies);

			var matchResults = new RegExpProcessingResultsCollection<RegExpMatchProcessingResult>(_listRegExps.Select(x => new RegExpMatchProcessingResult
																														   {
																															   RegExpID = x.ID,
																															   TotalMatches = x.TotalMatches,
																															   TotalDocuments = x.TotalDocuments
																														   }));

			return matchResults;
		}

		public void CalcStatistics(RegExpStatisticsProcessingParams param)
		{
			using (var docsConnection = DatabaseHelper.CreateConnection(param.DocumentsDatabaseFilePath, param.Password))
			{
				docsConnection.Open();

				///////////////////////////////////////////////////////////////////////////////

				var docsCount = DatabaseHelper.GetRowsCount(docsConnection, "Documents");

				var selectQuery = "SELECT ED_ENC_NUM, NOTE_TEXT FROM Documents";

				var documentRecords = DatabaseHelper.GetDataRecords(docsConnection, selectQuery);

				///////////////////////////////////////////////////////////////////////////////

				var intermediateResults = Parallel_CalcStatistics(documentRecords, docsCount, 0, 1, param);

				UpdateModifiedRows(intermediateResults, docsConnection);

				var results = AggregateStatistics(intermediateResults);

				results.Serialize(param.GetFullPath(param.OutputFileName));
			}
		}

		public void CalcStatisticsSingle(RegExpStatisticsSingleProcessingParams param)
		{
            //MessageBox.Show("asd");

            var calculator = new SingleStatisticsCalculator(this.Logger, this.Single);
			calculator.CalcStatisticsSingle(param, param.ExcludeLookArounds, param.OutputFileName);
		}

		public List<RegExpMatchResult> GetAllMatches(string text)
		{
			var result = new List<RegExpMatchResult>();

			foreach (var regExp in this.Items)
			{
				if (null == regExp)
					continue;
				if (string.IsNullOrWhiteSpace(regExp.Expression))
					continue;

				result.AddRange(regExp.GetFilteredMatches(text)
									  .Select(x => new RegExpMatchResult(regExp, 0, x)));
			}

			result = result.OrderBy(x => x.Start)
						   .ToList();

			return result;
		}

		public List<RegExpSynergy> LoadSynergies(string synergiesFilePath)
		{
			if (File.Exists(synergiesFilePath))
			{
				var json = File.ReadAllText(synergiesFilePath);
				if (!String.IsNullOrEmpty(json))
					return JsonConvert.DeserializeObject<List<RegExpSynergy>>(json);
			}

			return new List<RegExpSynergy>();
		}

		#endregion

		#region Implementation: general

		protected bool Sanitize()
		{
			var hasEmptyItems = false;

			for (var i = _listRegExps.Count - 1; i >= 0; i--)
			{
				var regExp = _listRegExps[i];
				if (String.IsNullOrEmpty(regExp.BuiltExpression))
				{
					_listRegExps.RemoveAt(i);
					hasEmptyItems = true;
				}
			}

			return hasEmptyItems;
		}

		private void UpdateModifiedRows(ConcurrentQueue<RegExpStatisticsIntermediateResult> intermediateResults, OleDbConnection connection)
		{
			var query = "UPDATE Documents SET NOTE_TEXT = @NOTE_TEXT WHERE ED_ENC_NUM = @ED_ENC_NUM";
			var command = new OleDbCommand(query, connection);

			command.Parameters.AddWithValue("@NOTE_TEXT", "");
			command.Parameters.AddWithValue("@ED_ENC_NUM", 0d);

			foreach (var item in intermediateResults.Where(x => x.Modified))
			{
				try
				{
					command.Parameters[0].Value = item.UpdatedText;
					command.Parameters[1].Value = item.DocumentID;

					var rowsUpdated = command.ExecuteNonQuery();
					if (rowsUpdated != 1)
						this.Logger.HandleException(new Exception("Failed to update document " + item.DocumentID.ToString("N0")));
				}
				catch (Exception ex)
				{
					this.Logger.HandleException(ex);
				}
			}
		}

		#endregion

		#region Implementation: score calculation

		protected RegExpProcessingResultsCollection<RegExpScoreProcessingResult> Parallel_CalcScores(IEnumerable<IDataRecord> enumerableDocs, long docsCount, int columnIndexID, List<int> columnIndexTextList, List<RegExpSynergy> synergies)
		{
			var results = new RegExpProcessingResultsCollection<RegExpScoreProcessingResult>();

			///////////////////////////////////////////////////////////////////////////////

			long progressStep = _listRegExps.Count;
			long progressMax = docsCount * progressStep;
			long progressValue = 0;

            //////////////////////////////////////////////////////////////////////////

			Parallel.ForEach(enumerableDocs, (record, state) =>
											 {
												 try
												 {
                                                     //List<int> tmp = new List<int>();
                                                     //tmp.Add(1);
													 var scoreResult = CalcDocumentScore(columnIndexID, columnIndexTextList, record, synergies);
                                                                                                              
													 if (scoreResult != null)
                                                     {
                                                         results.Add(scoreResult);
                                                     }
														 

													 ///////////////////////////////////////////////////////////////////////////////

													 var threadProgressValue = Interlocked.Add(ref progressValue, progressStep);

													 var progressPercentage = (int)(threadProgressValue / (double)progressMax * 100D);

													 if (!this.Logger.ReportProgress(progressPercentage, threadProgressValue))
														 state.Stop();
												 }
												 catch (Exception ex)
												 {                                                     
                                                     //Logger.AppendToLog("Parallel_CalcScores");
                                                     Logger.HandleException(ex);
												 }
											 });

			///////////////////////////////////////////////////////////////////////////////

			return results;
		}

		protected RegExpScoreProcessingResult CalcDocumentScore(int columnIndexID, List<int> columnIndexTextList, IDataRecord record, List<RegExpSynergy> synergies)
		{
            //Logger.AppendToLog(record.GetString(3));
            if (record.IsDBNull(columnIndexID))
				return null;
            
			///////////////////////////////////////////////////////////////////////////////

			var scoreResult = new RegExpScoreProcessingResult
							  {
								  DocumentID = record.GetDouble(columnIndexID),
								  CategoryID = -1
							  };

            scoreResult.Score = new List<long>();


            for (int i = 0; i < columnIndexTextList.Count; i++)
            {
                scoreResult.Score.Add(0);
            }

            List<bool> isRecordMatch = new List<bool>();

            for (int i = 0; i < _listRegExps.Count; i++)
                isRecordMatch.Add(false);

            for (int i = 0; i < columnIndexTextList.Count; i++)
            {
                scoreResult.Score.Add(0);

                if (record.IsDBNull(columnIndexTextList[i]))
                {
                    //Logger.AppendToLog("CalcScore" + columnIndexID.ToString() + ":" + columnIndexTextList[i].ToString() + "dbnull");
                    continue;
                }
                //Logger.AppendToLog("CalcScore" + columnIndexID.ToString() + ":" + columnIndexTextList[i].ToString() + "calc");

                ///////////////////////////////////////////////////////////////////////////////

                var docText = record.GetString(columnIndexTextList[i]);
                
                var matchingRegExps = new List<int>();

                var j = 0;
                foreach (var regExp in _listRegExps)
                {
                    bool isMatch;

                    var regExpScore = CalcRegExpScore(regExp, docText, out isMatch);
                    if (isMatch)
                    {
                        scoreResult.Score[i] += regExpScore;

                        matchingRegExps.Add(regExp.ID);

                        if (regExp.Category != null)
                            scoreResult.CategoryID = regExp.Category.Value;

                        isRecordMatch[j] = true;
                    }
                    j++;
                }

                scoreResult.Score[i] = ApplySynergies(scoreResult.Score[i], matchingRegExps, synergies);                                
            }

            for (int i = 0; i < _listRegExps.Count; i++)
                if (isRecordMatch[i])
                {
                    _listRegExps[i].IncrementTotalRecords();
                    if (!record.IsDBNull(1) && record.GetInt32(1) != 0)
                        _listRegExps[i].IncrementCategoryMatch(record.GetInt32(1));
                }

			return scoreResult;
		}

		protected int CalcRegExpScore(RegExpBase regExp, string text, out bool isMatch)
		{
			var scoreResult = 0;

			isMatch = false;

			try
			{
				var matches = regExp.GetFilteredMatches(text);
				if (matches.Any())
				{
					isMatch = true;

					var totalMatches = matches.Count;

					regExp.IncrementTotalDocuments();
					regExp.AddTotalMatches(totalMatches);

					///////////////////////////////////////////////////////////////////////////////

					var score = regExp.Score ?? 0;
					var factor = regExp.Factor ?? 0;

					scoreResult = totalMatches * score;
					scoreResult += (totalMatches / 2) * (totalMatches - 1) * factor;
				}
			}
			catch (Exception ex)
			{
				Logger.HandleException(ex);
			}

			return scoreResult;
		}

		protected long ApplySynergies(long scoreResult, List<int> matchingRegExps, List<RegExpSynergy> synergies)
		{
			if (!synergies.Any() || matchingRegExps.Count <= 1)
				return scoreResult;

			var preciseScore = (double) scoreResult;

			///////////////////////////////////////////////////////////////////////////////

			for (var row = 0; row < matchingRegExps.Count; row++)
			{
				var rowRegExpID = matchingRegExps[row];

				for (var col = 0; col < matchingRegExps.Count; col++)
				{
					if (col >= row)
						break;

					var factor = GetFactor(synergies, rowRegExpID, matchingRegExps[col]);
					if (factor > 0)
					{
						preciseScore *= factor;
					}
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			return Convert.ToInt64(Math.Round(preciseScore, 0));
		}

		protected double GetFactor(List<RegExpSynergy> synergies, int rowRegExpID, int colRegExpID)
		{
			const double defaultValue = 0.0d;

			var regExpSynergy = synergies.FirstOrDefault(x => x.RegExpID == rowRegExpID);
			if (regExpSynergy == null)
				return defaultValue;

			var synergy = regExpSynergy.Synergies.FirstOrDefault(x => x.MatchingRegExpId == colRegExpID);
			if (synergy == null)
				return defaultValue;

			return synergy.Factor;
		}

		#endregion

		#region Implementation: statistics

		private ConcurrentQueue<RegExpStatisticsIntermediateResult> Parallel_CalcStatistics(IEnumerable<IDataRecord> enumerableDocs, long docsCount, int columnIndexID, int columnIndexText, RegExpStatisticsProcessingParams param)
		{
			var results = new ConcurrentQueue<RegExpStatisticsIntermediateResult>();

			///////////////////////////////////////////////////////////////////////////////

			long progressStep = _listRegExps.Count;
			long progressMax = docsCount * progressStep;
			long progressValue = 0;

			//////////////////////////////////////////////////////////////////////////

			Parallel.ForEach(enumerableDocs, (record, state) =>
			{
				try
				{
					CalcDocumentStatistics(columnIndexID, columnIndexText, record, param, results);
	
					///////////////////////////////////////////////////////////////////////////////

					var threadProgressValue = Interlocked.Add(ref progressValue, progressStep);

					var progressPercentage = (int)(threadProgressValue / (double)progressMax * 100D);

					if (!this.Logger.ReportProgress(progressPercentage, threadProgressValue))
						state.Stop();
				}
				catch (Exception ex)
				{
					Logger.HandleException(ex);
				}
			});

			///////////////////////////////////////////////////////////////////////////////

			return results;
		}

		private void CalcDocumentStatistics(int columnIndexID, int columnIndexText, IDataRecord record, RegExpStatisticsProcessingParams param, ConcurrentQueue<RegExpStatisticsIntermediateResult> results)
		{
			if (record.IsDBNull(columnIndexID))
				return;

			///////////////////////////////////////////////////////////////////////////////

			if (record.IsDBNull(columnIndexText))
				return;

			///////////////////////////////////////////////////////////////////////////////

			var documentID = record.GetDouble(columnIndexID);
			var docText = record.GetString(columnIndexText);

			///////////////////////////////////////////////////////////////////////////////

			var matches = this.Single.GetFilteredMatches(docText);
			if (matches.Any())
			{
				var aggregatedList = matches.GroupBy(x => x.Value)
											.Select(g => new RegExpStatisticsIntermediateResult
														 {
															 Word = g.Key,
															 Count = g.Count()
														 }).ToList();

				///////////////////////////////////////////////////////////////////////////////

				try
				{
					if (param.Replace)
					{
						var singleResult = aggregatedList.First();
						singleResult.Modified = true;
						singleResult.UpdatedText = this.Single.ReplaceMatches(docText, param.ReplacementeString);
						singleResult.DocumentID = documentID;
					}
				}
				catch (Exception ex)
				{
					this.Logger.HandleException(ex);
				}

				///////////////////////////////////////////////////////////////////////////////

				aggregatedList.ForEach(x => results.Enqueue(x));
			}
		}

		private RegExpProcessingResultsCollection<RegExpStatisticsProcessingResult> AggregateStatistics(ConcurrentQueue<RegExpStatisticsIntermediateResult> words)
		{
			var statistics = words.GroupBy(x => x.Word)
								  .Select(group => new RegExpStatisticsProcessingResult
								  {
									  Word = group.Key,
									  Count = group.Sum(x => x.Count)
								  });

			return new RegExpProcessingResultsCollection<RegExpStatisticsProcessingResult>(statistics);
		}

		#endregion
	}
}
