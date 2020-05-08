using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Helpers;

using Newtonsoft.Json;

using RegExpLib.Common;
using RegExpLib.Core;
using RegExpLib.Database;
using RegExpLib.Model;


namespace RegExpLib.Processing
{
	public class ColRegExpProcessor
	{
		#region Fields

		protected List<ColRegExp> _listRegExps;

		protected bool _hasEmptyItems;

		protected int _prevPosition;

		protected string _password;

		public Logger Logger { get; set; }

		#endregion

		#region Properties

		public ColRegExp Single
		{
			get { return _listRegExps.FirstOrDefault(); }
		}

		public List<ColRegExp> Items
		{
			get { return _listRegExps; }
		}

		public bool HasEmptyItems
		{
			get { return _hasEmptyItems; }
		}

		#endregion

		#region Ctors

		private ColRegExpProcessor(Logger logger)
		{
			_listRegExps = new List<ColRegExp>();

			this.Logger = logger;
		}

		public ColRegExpProcessor(Logger logger, string regExpDatabasePath, string password, IEnumerable<int> columnIDs, bool ignoreCase = true, bool compiled = false)
			: this(logger)
		{
			using (var connection = DatabaseHelper.CreateConnection(regExpDatabasePath, password))
			{
				var options = RegExpOptions.CreateOptions(ignoreCase, compiled);

				///////////////////////////////////////////////////////////////////////////////

				var query = "SELECT ID, ColumnID, RegExp, [Extract], [RegExpColor], [lookahead], [lookbehind], [neg lookahead], [neg lookbehind], [exceptions] FROM ColRegExp";

				var regExpRows = DatabaseHelper.GetDataRows(connection, query);

				foreach (var row in regExpRows)
				{
					try
					{
						var regExp = RegExpFactory.Create_ColRegExp(row, columnIDs, true, options);
						if (regExp != null)
							_listRegExps.Add(regExp);
					}
					catch
					{
						this.Logger.HandleRegExpException(row);
					}
				}

				///////////////////////////////////////////////////////////////////////////////

				_listRegExps = _listRegExps.OrderBy(x => x.ExtractOptions != null ? x.ExtractOptions.Order : 0)
				                           .ToList();

				_hasEmptyItems = Sanitize();
			}
		}

		public ColRegExpProcessor(IEnumerable<DataRow> enumerable, IEnumerable<int> columnIDs, bool ignoreCase = true, bool compiled = false)
			: this(null)
		{
			var options = RegExpOptions.CreateOptions(ignoreCase, compiled);

			///////////////////////////////////////////////////////////////////////////////

			foreach (var row in enumerable)
			{
				var regExp = RegExpFactory.Create_ColRegExp(row, columnIDs, true, options);
				if (regExp != null)
					_listRegExps.Add(regExp);
			}

			_listRegExps = _listRegExps.OrderBy(x => x.ExtractOptions != null ? x.ExtractOptions.Order : 0)
			                           .ToList();

			_hasEmptyItems = Sanitize();
		}

		public ColRegExpProcessor(DataRow row, IEnumerable<int> columnIDs, bool ignoreCase = true, bool compiled = false)
			: this(null)
		{
			var regExp = RegExpFactory.Create_ColRegExp(row, columnIDs, true, RegExpOptions.CreateOptions(ignoreCase, compiled));
			if (regExp == null)
				throw new DataException("Row is deleted or detached");

			_listRegExps.Add(regExp);

			_hasEmptyItems = Sanitize();
		}

		public ColRegExpProcessor(int id, int columnID, string regExp, bool build, string extract, int score, int factor, string prefixMatch, string suffixMatch, int color, string lookAhead, string lookBehind, string negLookAhead, string negLookBehind, string exceptions, bool ignoreCase, bool compiled)
			: this(null)
		{
			_listRegExps.Add(new ColRegExp(RegExpOptions.CreateOptions(ignoreCase, compiled), id, columnID, regExp, build, extract, score, factor, prefixMatch, suffixMatch, color, lookAhead, lookBehind, negLookAhead, negLookBehind, exceptions, String.Empty));

			_hasEmptyItems = Sanitize();
		}

		public ColRegExpProcessor(string regExpDatabasePath, string password, int regExpID, bool ignoreCase = true, bool compiled = false)
			: this(null)
		{
			_password = password;

			using (var connection = DatabaseHelper.CreateConnection(regExpDatabasePath, password))
			{
				var options = RegExpOptions.CreateOptions(ignoreCase, compiled);

				///////////////////////////////////////////////////////////////////////////////

				var query = "SELECT ID, ColumnID, RegExp, [Extract], [RegExpColor], [lookahead], [lookbehind], [neg lookahead], [neg lookbehind], [exceptions] FROM ColRegExp WHERE ID = " + regExpID;

				var regExpRows = DatabaseHelper.GetDataRows(connection, query).ToList();
				if (!regExpRows.Any())
					throw new Exception("Invalid RegExpID");

				var regExp = RegExpFactory.Create_ColRegExp(regExpRows.First(), null, true, options);
				_listRegExps.Add(regExp);

				///////////////////////////////////////////////////////////////////////////////

				_hasEmptyItems = Sanitize();
			}
		}

		#endregion

		#region Operations

		public void CalcScores(ColRegExpStatisticsProcessingParams param)
		{
            //MessageBox.Show("RegExpProcessor: CalcScores");
			using (var docsConnection = DatabaseHelper.CreateConnection(param.DocumentsDatabaseFilePath, param.Password))
			{
				docsConnection.Open();

				///////////////////////////////////////////////////////////////////////////////

				var docsCount = DatabaseHelper.GetRowsCount(docsConnection, "Documents", param.OnlyPositiveScore ? "Score > 0" : null);
                
				string query = "SELECT ED_ENC_NUM, NOTE_TEXT, Score FROM Documents";

				if (param.OnlyPositiveScore)
					query += " WHERE Score > 0";

				var documentRecords = DatabaseHelper.GetDataRecords(docsConnection, query);

				///////////////////////////////////////////////////////////////////////////////

				Parallel_CalcScores(documentRecords, docsCount, 1, 2, param);

				///////////////////////////////////////////////////////////////////////////////

				var matchResults = new RegExpProcessingResultsCollection<ColRegExpStatisticsProcessingResult>(_listRegExps.Select(x => new ColRegExpStatisticsProcessingResult
				                                                                                                                  {
					                                                                                                                  ID = x.ID,
					                                                                                                                  TotalMatches = x.TotalMatches,
					                                                                                                                  TotalDocuments = x.TotalDocuments
				                                                                                                                  }));

				matchResults.Serialize(param.GetFullPath(param.MatchesOutputFileName));
			}
		}

        // not used function
		public RegExpProcessingResultsCollection<ColRegExpStatisticsProcessingResult> CalcScores(DataTable table, IEnumerable<DataRow> documentRows, ColRegExpStatisticsProcessingParams param)
		{
			var columnIndexID = table.Columns.IndexOf("ED_ENC_NUM");
			var columnIndexText = table.Columns.IndexOf("NOTE_TEXT");

			if (columnIndexID == -1 || columnIndexText == -1)
				throw new Exception("Cannot find source columns");

			///////////////////////////////////////////////////////////////////////////////

			var docsCount = table.Rows.Count;

			Parallel_CalcScores(DatabaseHelper.AsDataRecordEnumerable(documentRows), docsCount, columnIndexID, columnIndexText, param);

			var matchResults = new RegExpProcessingResultsCollection<ColRegExpStatisticsProcessingResult>(_listRegExps.Select(x => new ColRegExpStatisticsProcessingResult
			{
				ID = x.ID,
				TotalMatches = x.TotalMatches,
				TotalDocuments = x.TotalDocuments
			}));

			return matchResults;
		}

		public void ExtractValues(ColRegExpExtractProcessingParams param)
		{
            ///////////////////////////////////////////////////////////////////////////////
            //MessageBox.Show("ColRegExpProcessor: ExtractValues");

			using (var docsConnection = DatabaseHelper.CreateConnection(param.DocumentsDatabaseFilePath, param.Password))
			{
				docsConnection.Open();

                ///////////////////////////////////////////////////////////////////////////////

                var docsCount = DatabaseHelper.GetRowsCount(docsConnection, "Documents", param.OnlyPositiveScore ? "Score > 0" : null);

                string query = "SELECT ED_ENC_NUM, NOTE_TEXT FROM Documents"; ;

                if (param.ScriptExtract && param.ScriptCode != null)
                {
                    int index = GetNoteColumnIndexFromCode(param.ScriptCode);

                    if (index != 0)                    
                        query = "SELECT ED_ENC_NUM, NOTE_TEXT" + index.ToString() + " FROM Documents";
                }
                
                if (param.OnlyPositiveScore)
					query += " WHERE Score > 0";

				var documentRecords = DatabaseHelper.GetDataRecords(docsConnection, query);

				///////////////////////////////////////////////////////////////////////////////

				Dictionary<int, CSScriptManager> scripts = null;

				if (param.ScriptExtract)
				{
					if (param.ColumnID != -1)
					{
						scripts = new Dictionary<int, CSScriptManager>
						          {
							          {
								          param.ColumnID,
										  CreateScriptManager(param.ScriptCode)
							          }
						          };
					}
					else
						scripts = GetScripts(param.RegExpDatabaseFilePath, param.Password);
				}

				///////////////////////////////////////////////////////////////////////////////

				var results = Parallel_ExtractValues(documentRecords, docsCount, 0, 1, param, scripts);

				results.Serialize(param.GetFullPath(param.ExtractOutputFileName));
			}
		}

		public void CalcStatisticsSingle(RegExpStatisticsSingleProcessingParams param)
		{
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

        private int GetNoteColumnIndexFromCode(string codeText)
        {
            string index = "0";
            int st = codeText.IndexOf("NOTE_TEXT") + 9;
            int i;
            for (i = st; st != -1 && i < codeText.Length; i++)
            {
                if ('0' > codeText[i] || codeText[i] > '9')
                    break;
                index += codeText[i];
            }

            return Int32.Parse(index);
        }

        private string GetAvailableCompileCode(string codeText)
        {
            int index = GetNoteColumnIndexFromCode(codeText);

            if (index != 0)
                codeText = codeText.Replace("NOTE_TEXT" + index.ToString(), "NOTE_TEXT");

            return codeText;
        }

        #endregion

        #region Implementation: calculation

        protected void Parallel_CalcScores(IEnumerable<IDataRecord> enumerableDocs, long docsCount, int columnIndexText, int columnIndexScore, ColRegExpStatisticsProcessingParams param)
		{
			long progressStep = _listRegExps.Count;
			long progressMax = docsCount * progressStep;
			long progressValue = 0;

			Parallel.ForEach(enumerableDocs, (record, state) =>
			                                 {
				                                 try
				                                 {
					                                 CalcDocumentScores(columnIndexText, columnIndexScore, record, param);

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
		}

		protected void CalcDocumentScores(int columnIndexText, int columnIndexScore, IDataRecord record, ColRegExpStatisticsProcessingParams param)
		{
			if (record.IsDBNull(columnIndexText))
				return;

            ///////////////////////////////////////////////////////////////////////////////

            double documentId = record.GetDouble(0);

			var docText = record.GetString(columnIndexText);

			var score = DatabaseHelper.GetInt32ValueInvariant(record, columnIndexScore);

            ///////////////////////////////////////////////////////////////////////////////
            using (var docsConnection = DatabaseHelper.CreateConnection(param.DocumentsDatabaseFilePath, param.Password))
            {
                docsConnection.Open();
                foreach (var regExp in _listRegExps)
			    {
                    var noteText = DatabaseHelper.GetNoteText(docsConnection, documentId, regExp.ExtractOptions.NoteTextColumn);

                    CalcRegExpScore(regExp, noteText, score);
                }
                
			}
		}

		protected void CalcRegExpScore(ColRegExp regExp, string text, int score)
		{
			try
			{
				var matches = regExp.GetFilteredMatches(text);
				if (matches.Any())
				{
					regExp.IncrementTotalDocuments();
					if (score > 0)
						regExp.IncrementPositiveDocuments();

					regExp.AddTotalMatches(matches.Count);
				}
			}
			catch (Exception ex)
			{
				Logger.HandleException(ex);
			}
		}

		#endregion

		#region Implementation: extraction

		protected RegExpProcessingResultsCollection<ColRegExpExtractProcessingResult> Parallel_ExtractValues(IEnumerable<IDataRecord> enumerableDocs, long docsCount, int columnIndexID, int columnIndexText, ColRegExpExtractProcessingParams param, Dictionary<int, CSScriptManager> scripts)
		{
			var results = new RegExpProcessingResultsCollection<ColRegExpExtractProcessingResult>();

			///////////////////////////////////////////////////////////////////////////////

			long progressStep = _listRegExps.Count;
			long progressMax = docsCount * progressStep;
			long progressValue = 0;

			///////////////////////////////////////////////////////////////////////////////

			Parallel.ForEach(enumerableDocs, (record, state) =>
			                                 {
				                                 try
				                                 {
													if (record.IsDBNull(columnIndexID))
														 return;

					                                 ///////////////////////////////////////////////////////////////////////////////

													 var documentID = record.GetDouble(columnIndexID);
					                                 if (param.DocumentsList.Any() && !param.DocumentsList.Contains(documentID))
						                                 return;

					                                 ///////////////////////////////////////////////////////////////////////////////

													 ExtractDocumentValues(documentID, columnIndexText, record, scripts, param.ColumnID, results, param);

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

		protected void ExtractDocumentValues(double documentID, int columnIndexText, IDataRecord record, Dictionary<int, CSScriptManager> scripts, int columnID, RegExpProcessingResultsCollection<ColRegExpExtractProcessingResult> results, ColRegExpExtractProcessingParams param)
		{
			if (record.IsDBNull(columnIndexText))
				return;

			///////////////////////////////////////////////////////////////////////////////

			var docText = record.GetString(columnIndexText);

			if (scripts == null)
			{
                using (var docsConnection = DatabaseHelper.CreateConnection(param.DocumentsDatabaseFilePath, param.Password))
                {
                    docsConnection.Open();

                    foreach (var regExp in _listRegExps.Where(x => x.ExtractOptions != null && x.ExtractOptions.Extract))
				    {
                        var noteText = DatabaseHelper.GetNoteText(docsConnection, documentID, regExp.ExtractOptions.NoteTextColumn);
                        
                        var extractResult = ExtractRegExpValues(regExp, documentID, noteText);
                        if (extractResult != null)
                            results.Add(extractResult);
                    }

					///////////////////////////////////////////////////////////////////////////////

					
				}
			}
			else
			{
				foreach (var pair in scripts)
				{
					var extractResult = ScriptExtractRegExpValues(documentID, docText, pair.Value, pair.Key);
					if (extractResult != null)
						results.Add(extractResult);
				}
			}
		}

		protected ColRegExpExtractProcessingResult ExtractRegExpValues(ColRegExp regExp, double documentID, string text)
		{
			ColRegExpExtractProcessingResult result = null;

			try
			{
				var matches = regExp.GetFilteredMatches(text);
				var matchValues = matches.Select(match => match.Value)
											.ToList();

				if (matchValues.Any())
				{
					var returnValue = String.Empty;

					switch (regExp.ExtractOptions.InstanceNo)
					{
						case 1:
							returnValue = matchValues[0];
							break;
						case 2:
							returnValue = matchValues[matches.Count - 1];
							break;
						case 3:
							returnValue = matchValues.ElementAtOrDefault(regExp.ExtractOptions.NthInstaceNumber.Value - 1);
							break;
						case 4:
							returnValue = matchValues.Count > 1 ? Newtonsoft.Json.JsonConvert.SerializeObject(matchValues.ToArray()) : matchValues.First();
							break;
					}

					///////////////////////////////////////////////////////////////////////////////

					if (!String.IsNullOrEmpty(returnValue))
					{
						result = new ColRegExpExtractProcessingResult()
						{
							DocumentID = documentID,
							ColumnID = regExp.ColumnID,
							Value = returnValue,
							ExtractOptions = regExp.ExtractOptions
						};
					}
				}
			}
			catch (Exception ex)
			{
				Logger.HandleException(ex);
			}

			return result;
		}

		protected ColRegExpExtractProcessingResult ScriptExtractRegExpValues(double documentID, string text, CSScriptManager scriptManager, int columnID)
		{
			ColRegExpExtractProcessingResult result = null;

			try
			{
                List<string> regExpList = new List<string>();
                foreach (var regExp in _listRegExps.Where(x => x.ExtractOptions != null && x.ExtractOptions.Extract))
                {
                    regExpList.Add(regExp.Expression);
                }                

                var returnValue = scriptManager.Run(text, regExpList);

				if (!String.IsNullOrEmpty(returnValue))
				{
					result = new ColRegExpExtractProcessingResult
					{
						ColumnID = columnID,
						DocumentID = documentID,
						Value = returnValue
					};
				}
			}
			catch (Exception ex)
			{
				Logger.HandleException(ex);
			}

			return result;
		}

		protected Dictionary<int, CSScriptManager> GetScripts(string regExpDatabasePath, string password)
		{
			var scripts = new Dictionary<int, CSScriptManager>();

			///////////////////////////////////////////////////////////////////////////////

			var query = "SELECT ColumnID, Data FROM ColScript";

			using (var connection = DatabaseHelper.CreateConnection(regExpDatabasePath, password))
			{
				connection.Open();

				var scriptRecords = DatabaseHelper.GetDataRecords(connection, query);

				foreach (var record in scriptRecords)
				{
					try
					{
						if (record.IsDBNull(0) || record.IsDBNull(1))
							continue;

						///////////////////////////////////////////////////////////////////////////////

						var columnID = record.GetInt32(0);
						var data = record.GetString(1);

						if (!String.IsNullOrEmpty(data) && data.StartsWith("{"))
						{
							var scriptData = JsonConvert.DeserializeObject<CSScriptData>(data);
							if (scriptData.ExtractValuesWithScript && !String.IsNullOrEmpty(scriptData.Script))
							{
								var scriptManager = CreateScriptManager(scriptData.Script);
								scripts.Add(columnID, scriptManager);
							}
						}
					}
					catch (Exception ex)
					{
						this.Logger.HandleException(ex);
					}
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			return scripts;
		}

		protected CSScriptManager CreateScriptManager(string scriptCode)
		{
			var scriptManager = new CSScriptManager();
			var compileResult = scriptManager.Compile(GetAvailableCompileCode(scriptCode));
			if (!String.IsNullOrEmpty(compileResult))
				throw new ArgumentException(compileResult);
			
			return scriptManager;
		}

		#endregion
	}
}