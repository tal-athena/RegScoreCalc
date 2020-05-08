using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

using Newtonsoft.Json;
using RegExpLib.Core;
using RegExpLib.Model;
using RegExpLib.Processing;


namespace RegScoreCalc.Code
{
	public class ExternalEntitiesToolWrapper : IDisposable
	{
		#region Constants

		protected const string _paramsFileName = "parameters.json";
		protected const string _logFileName = "log.txt";
		protected const string _progressFileName = "progress.txt";

		protected const string _eventName = "EntitiesPlumbing.exe";
        protected const string _outputFileName = "output.json";

		#endregion

		#region Fields

		protected Process _process;
		protected FileSystemWatcher _watcher;
		protected EventWaitHandle _event;

		protected string _outputFolder;

		protected BackgroundWorker _worker;
		protected bool _reportProgress;

		protected CultureInfo _cultureInfo;

		protected StreamReader _readerProgress;
		protected StreamReader _readerLog;

		protected bool _hasErrors;

        protected EntitiesProcessingParam _parm;

        #endregion

        #region Properties

        public bool HasErrors
		{
			get { return _hasErrors; }
		}

		#endregion

		#region Ctors

		public ExternalEntitiesToolWrapper(BackgroundWorker worker, EntitiesProcessingParam parm, bool reportProgress = true)
		{
			if (worker == null)
				throw new ArgumentNullException();

			_worker = worker;
			_reportProgress = reportProgress;

            _outputFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_outputFolder);

            _parm = parm;
            _parm.OutputFileName = _outputFileName;
            _parm.LogFileName = _logFileName;
            _parm.ProgressFileName = _progressFileName;
            _parm.WorkingFolder = _outputFolder;

            var paramsFilePath = _parm.GetFullPath(_paramsFileName);

            _parm.Serialize(paramsFilePath);
        }

		#endregion

		#region Events

		private void watcher_Changed(object sender, FileSystemEventArgs e)
		{
			try
			{
				var fileName = Path.GetFileName(e.FullPath);

				if (String.Compare(fileName, _progressFileName, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					if (_readerProgress == null)
						_readerProgress = new StreamReader(File.Open(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

					var content = _readerProgress.ReadToEnd();
					if (!String.IsNullOrEmpty(content))
					{
						var progressLines = content.Trim().Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
						if (progressLines.Any())
						{
							var progressValue = Convert.ToInt32(progressLines.Last());
							ReportProgress(progressValue);
						}
					}
				}
				else if (String.Compare(fileName, _logFileName, StringComparison.InvariantCultureIgnoreCase) == 0)
					ReadErrorLog();
			}
			catch (Exception ex)
			{
				ReportErrors(ex.Message);
			}
		}

		private void ReadErrorLog()
		{
			var content = String.Empty;

			try
			{
				if (_readerLog == null)
				{
					var logFilePath = Path.Combine(_outputFolder, _logFileName);

					if (File.Exists(logFilePath))
						_readerLog = new StreamReader(File.Open(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				}

				if (_readerLog != null)
					content = _readerLog.ReadToEnd();
			}
			catch (Exception ex)
			{
				content = ex.Message;
			}

			if (!String.IsNullOrEmpty(content))
				ReportErrors(content.Trim());
		}

		#endregion

		#region Operations

        public bool RunAction(ViewsManager views)
        {
            try
            {
                ///////////////////////////////////////////////////////////////////////////////
                ReportProgress(0);
                ///////////////////////////////////////////////////////////////////////////////

                if (!StartEntitiesPlumbingOperation(_parm.GetFullPath(_paramsFileName)))
                    return false;

                ///////////////////////////////////////////////////////////////////////////////
                WaitForExit();

                ///////////////////////////////////////////////////////////////////////////////
                
                if (_parm.Command == "Calculate")
                {
                    ReportProgress(70);
                    CalculatedEntitiesResult result = null;
                    if (File.Exists(_parm.GetFullPath(_parm.OutputFileName)))
                    {
                        var json = File.ReadAllText(_parm.GetFullPath(_parm.OutputFileName));
                        result = JsonConvert.DeserializeObject<CalculatedEntitiesResult>(json);
                    }

                    if (result == null)
                        return false;

                    if (!WriteEntitiesNames(views, result.EntityLabels))
                    {
                        return false;
                    }
                    if (!WriteEntitiesValues(views, result.Documents))
                    {
                        return false;
                    }
                    if (!CalculateStatistics(views, result))
                    {
                        return false;
                    }

                } else if (_parm.Command == "GetEntityNames")
                {
                    ReportProgress(50);
                    // write to database
                    EntityLabelResult result = null;
                    if (File.Exists(_parm.GetFullPath(_parm.OutputFileName)))
                    {
                        var json = File.ReadAllText(_parm.GetFullPath(_parm.OutputFileName));
                        result = JsonConvert.DeserializeObject<EntityLabelResult>(json);
                    }

                    if (result == null)
                        return false;

                    if (!WriteEntitiesNames(views, result))
                    {
                        return false;
                    }                    
                }
                return true;
            }
            catch (Exception e)
            {
                HandleException(e);
            }
            return false;
        }

        public bool WriteEntitiesValues(ViewsManager views, List<EntityResult> entityResult)
        {
            ReportProgress(70);

            ClearColumns(views, "NOTE_ENTITIES");

            int totalCnt = entityResult.Count;

            views.MainForm.adapterDocuments.StartBatchQuery();
            var connection = views.MainForm.adapterDocuments.Connection;

            var query = "UPDATE Documents SET [NOTE_ENTITIES] = @NOTE_ENTITIES WHERE [ED_ENC_NUM] = @ED_ENC_NUM";

            int step = totalCnt / 30 + 1;
            int current = 0;
            foreach (var entity in entityResult)
            {
                var cmd = new OleDbCommand(query, connection);
                cmd.Parameters.AddWithValue("@NOTE_ENTITIES", entity.Entities);
                cmd.Parameters.AddWithValue("@ED_ENC_NUM", entity.DocumentID);
                cmd.ExecuteNonQuery();

                if (current % step == 0)
                    ReportProgress((int)(1.0 * current / totalCnt * 30), "Writting to database");
                
            }
            
            //views.MainForm.adapterDocuments.Fill();

            ReportProgress(100);
            return true;
        }
        public bool CalculateStatistics(ViewsManager views, CalculatedEntitiesResult result)
        {
            var connection = views.MainForm.adapterEntities.Connection;
            if (connection.State != ConnectionState.Open)
                connection.Open();

            foreach (var label in result.EntityLabels.Labels)
            {
                int totalMatches = 0;
                int totalRecords = 0;
                int totalDocuments = 0;


                foreach (var entityResult in result.Documents)
                {
                    totalMatches += Regex.Matches(entityResult.Entities, label).Count;
                    if (entityResult.Entities.Contains(label))
                    {
                        totalRecords++;
                        totalDocuments++;
                    }
                }

                var query = $"UPDATE [Entities] SET [TotalMatches]=@TotalMatches, [TotalDocuments] = @TotalDocuments, [TotalRecords] = @TotalRecords,[TotalCategorized] = @TotalCategorized WHERE [Entity]=\"{label}\"";
                var cmd = new OleDbCommand(query, connection);

                cmd.Parameters.AddWithValue("@TotalMatches", totalMatches);
                cmd.Parameters.AddWithValue("@TotalDocuments", totalDocuments);
                cmd.Parameters.AddWithValue("@totalRecords", totalRecords);
                cmd.Parameters.AddWithValue("@TotalCategorized", 0);
                cmd.ExecuteNonQuery();
            }
            
            
            return true;
        }

        public bool WriteEntitiesNames(ViewsManager views, EntityLabelResult entityLabels)
        {
            if (entityLabels.Labels == null)
                return false;

            var connection = views.MainForm.adapterEntities.Connection;
            if (connection.State != ConnectionState.Open)
                connection.Open();

            //Delete unnecessary labels

            var query = "DELETE * FROM Entities ";

            if (entityLabels.Labels.Any())
            {

                query += "WHERE Entity NOT IN (";

                bool first = true;
                foreach (var label in entityLabels.Labels)
                {
                    if (first)
                    {
                        query += "\"" + label + "\"";
                        first = false;
                    }
                    else
                        query += ", " + "\"" + label + "\"";

                }
                query += ")";
            }


            var cmd = new OleDbCommand(query, connection);
            cmd.ExecuteNonQuery();

            var insertQuery = "INSERT INTO [Entities] (Entity) VALUES (@Entity)";
            var checkQuery = "SELECT COUNT (Id) FROM [Entities] WHERE Entity = @Entity";

            foreach (var label in entityLabels.Labels)
            {
                var cmdCheck = new OleDbCommand(checkQuery, connection);
                cmdCheck.Parameters.AddWithValue("@Entity", label);
                int count = (int)cmdCheck.ExecuteScalar();
                if (count != 1)
                {
                    var cmdInsert = new OleDbCommand(insertQuery, connection);
                    cmdInsert.Parameters.AddWithValue("@Entity", label);
                    cmdInsert.ExecuteNonQuery();
                }
            }            
            return true;
        }

        public int ClearColumns(ViewsManager views, string columnName, List<double> filteredDocuments = null)
        {
            
            var query = "UPDATE [Documents] SET [" + columnName + "] = \"abc\"";

            if (filteredDocuments != null)
            {
                var idList = "(";
                foreach (var id in filteredDocuments)
                {
                    if (idList == "(")
                        idList += (int)id;
                    else idList += ", " + (int)id;
                }
                idList += ")";

                if (filteredDocuments.Any())
                    query += "WHERE ED_ENC_NUM IN " + idList;
            }

            views.MainForm.adapterDocuments.StartBatchQuery();
            var connection = views.MainForm.adapterDocuments.Connection;

            var cmd = new OleDbCommand(query, connection);

            return cmd.ExecuteNonQuery();            
        }

      
		#endregion

		#region Operations: helpers

		public void WaitForExit()
		{
			if (_process != null)
			{
				while (!_worker.CancellationPending)
				{
					if (_process.WaitForExit(2000))
						break;
				}

				///////////////////////////////////////////////////////////////////////////////

				if (!_process.HasExited && _worker.CancellationPending)
				{
					_event.Set();

					if (!_process.WaitForExit(5000))
						_process.Kill();
				}

				///////////////////////////////////////////////////////////////////////////////

				ReadErrorLog();
			}
		}

		#endregion

		#region Implementation

		protected bool StartOperation(EntitiesProcessingParam param)
		{
			try
			{
				_watcher = new FileSystemWatcher(_outputFolder)
				           {
					           EnableRaisingEvents = true,
					           Filter = "*.txt"
				           };

				_watcher.Changed += watcher_Changed;

				///////////////////////////////////////////////////////////////////////////////

				var toolFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
				                                                 .Location);
				toolFilePath = Path.Combine(toolFilePath, "RegExpProcessor.exe");

				///////////////////////////////////////////////////////////////////////////////

				param.WorkingFolder = _outputFolder;
				param.ProgressFileName = _progressFileName;
				param.LogFileName = _logFileName;

				var paramsFilePath = param.GetFullPath(_paramsFileName);

				param.Serialize(paramsFilePath);

				///////////////////////////////////////////////////////////////////////////////

				_event = new EventWaitHandle(false, EventResetMode.ManualReset, _eventName);

				_process = new Process
				           {
					           StartInfo = new ProcessStartInfo(toolFilePath, QuotePath(paramsFilePath)),
					           EnableRaisingEvents = true
				           };

				_process.Start();

				return true;
			}
			catch (Exception ex)
			{
				Dispose();

				MainForm.ShowExceptionMessage(ex);
			}

			return false;
		}
        protected bool StartEntitiesPlumbingOperation(string _paramenterFileName)
        {   
            try
            {
                _watcher = new FileSystemWatcher(_outputFolder)
                {
                    EnableRaisingEvents = true,
                    Filter = "*.txt"
                };

                _watcher.Changed += watcher_Changed;

                ///////////////////////////////////////////////////////////////////////////////

                var toolFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                                 .Location);
                toolFilePath = Path.Combine(toolFilePath, "EntitiesPlumbing.exe");

                ///////////////////////////////////////////////////////////////////////////////

                _event = new EventWaitHandle(false, EventResetMode.ManualReset, _eventName);

                var arguments = String.Format("{0}", _paramenterFileName);
                _process = new Process
                {
                    StartInfo = new ProcessStartInfo(toolFilePath, arguments),
                    EnableRaisingEvents = true,                    
                };

                _process.Start();                

                return true;
            }
            catch (Exception ex)
            {
                Dispose();

                MainForm.ShowExceptionMessage(ex);
            }

            return false;
        }
        protected OleDbCommand CreateUpdateCommand(ViewsManager views, bool includeCategoryColumn, int noteColumnCount = 1)
		{
			var connection = views.MainForm.adapterDocuments.Connection;
			if (connection.State != ConnectionState.Open)
				connection.Open();

			var query = "UPDATE Documents SET Score = @Score";
            
            for (int i = 1; i < noteColumnCount; i ++)
            {
                query += ", Score" + i.ToString() + " = @Score" + i.ToString();
            }            

			if (includeCategoryColumn)
				query += ", Category = @Category";

			 query += " WHERE ED_ENC_NUM = @ED_ENC_NUM";

			var cmd = new OleDbCommand(query, connection);

			cmd.Parameters.AddWithValue("@Score", 0).DbType = DbType.Int32;

            for (int i = 1; i < noteColumnCount; i++)
            {
                cmd.Parameters.AddWithValue("@Score" + i.ToString(), 0).DbType = DbType.Int32;                
            }

            if (includeCategoryColumn)
				cmd.Parameters.AddWithValue("@Category", 0).DbType = DbType.Int32;

			cmd.Parameters.AddWithValue("@ED_ENC_NUM", 0).DbType = DbType.Double;

			cmd.Prepare();

			return cmd;
		}

		protected void CleanupIntermediateFiles()
		{
			try
			{
				if (!this.HasErrors)
				{
					if (Directory.Exists(_outputFolder))
						Directory.Delete(_outputFolder, true);

					_outputFolder = String.Empty;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected string QuotePath(string path)
		{
			return "\"" + path + "\"";
		}

		protected void HandleException(Exception ex)
		{
			ReportErrors(ex.Message);
		}

		protected void ReportProgress(int progress, string message = null)
		{
			if (_reportProgress)
				_worker.ReportProgress(progress, message);
		}

		protected void ReportErrors(string message)
		{
			_hasErrors = true;

			_worker.ReportProgress(-1, message);
		}

		#endregion

		#region Implementation: writing results

		protected void RegExp_WriteScores(ViewsManager views, RegExpProcessingResultsCollection<RegExpMatchProcessingResult> matches, RegExpProcessingResultsCollection<RegExpScoreProcessingResult> scores)
		{
			var regExpJoin = from row in views.MainForm.datasetMain.RegExp.Cast<MainDataSet.RegExpRow>()
			                                  .Where(x => x.RowState != DataRowState.Deleted)
			                 join match in matches.Items
				                 on row.ID equals match.RegExpID
			                 select new { RegExpRow = row, Match = match };

			foreach (var item in regExpJoin)
			{
                item.RegExpRow["TotalRecords"] = item.Match.TotalRecords;

                int categorized = 0;

                foreach (var key in item.Match.CategorizedRecords.Keys)
                {
                    categorized += item.Match.CategorizedRecords[key];
                }
                item.RegExpRow["TotalCategorized"] = categorized;

                item.RegExpRow["CategorizedRecords"] = item.Match.CategorizedRecords;

                item.RegExpRow["TotalDocuments"] = item.Match.TotalDocuments;
				item.RegExpRow["TotalMatches"] = item.Match.TotalMatches;                
			}

			///////////////////////////////////////////////////////////////////////////////

			var docJoin = (from row in views.MainForm.datasetMain.Documents.Cast<MainDataSet.DocumentsRow>()
			               join score in scores.Items
				               on row.ED_ENC_NUM equals score.DocumentID
			               select new { DocumentRow = row, Scores = score })
				.ToList();

			if (docJoin.Count == 0)
				return;

            ///////////////////////////////////////////////////////////////////////////////

            var noteColumnCount = docJoin[0].Scores.Score.Count();

            for (int i = 1; i < docJoin[0].Scores.Score.Count; i++)
            {
                try
                {
                    var score = docJoin[0].DocumentRow["Score" + i.ToString()];
                }
                catch (Exception e)
                {
                    noteColumnCount = i;
                    break;
                }
            }

            

			double progressMax = docJoin.Count;
			var progressValue = 0;

			foreach (var item in docJoin)
			{
                item.DocumentRow["Score"] = item.Scores.Score[0];
                for (int i = 1; i < noteColumnCount; i ++)
                {
                   item.DocumentRow["Score" + i.ToString()] = item.Scores.Score[i];
                   
                }
				    

				if (item.Scores.CategoryID != -1)
					item.DocumentRow["Category"] = item.Scores.CategoryID;

				///////////////////////////////////////////////////////////////////////////////

				progressValue++;
				if (progressValue % 50 == 0)
				{
					if (_worker.CancellationPending)
						break;

					var progressPercentage = (int) ((progressValue / progressMax) * 100d);

					ReportProgress(progressPercentage, "Writing value " + progressValue + " of " + progressMax);
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			progressValue = 0;
			ReportProgress(0);

			///////////////////////////////////////////////////////////////////////////////

			var cmdWithCategory = CreateUpdateCommand(views, true, noteColumnCount);
			var cmdNoCategory = CreateUpdateCommand(views, false, noteColumnCount);

			foreach (var item in docJoin)
			{
				if (item.Scores.CategoryID != -1)
				{
                    int i = 0;
                    for (i = 0; i < noteColumnCount; i ++)
					    cmdWithCategory.Parameters[i].Value = item.Scores.Score[i];

					cmdWithCategory.Parameters[i].Value = item.Scores.CategoryID;
					cmdWithCategory.Parameters[i + 1].Value = item.Scores.DocumentID;

					cmdWithCategory.ExecuteNonQuery();
				}
				else
				{
                    int i = 0;
                    for (i = 0; i < noteColumnCount; i++)
                        cmdNoCategory.Parameters[i].Value = item.Scores.Score[i];

                    cmdNoCategory.Parameters[i].Value = item.Scores.DocumentID;

					cmdNoCategory.ExecuteNonQuery();
				}

				item.DocumentRow.AcceptChanges();

				///////////////////////////////////////////////////////////////////////////////

				progressValue++;
				if (progressValue % 50 == 0)
				{
					if (_worker.CancellationPending)
						break;

					var progressPercentage = (int) ((progressValue / progressMax) * 100d);

					ReportProgress(progressPercentage, "Updating document row " + progressValue + " of " + progressMax);
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			views.MainForm.adapterDocuments.Fill();
		}

		protected void ColRegExp_WriteScores(ViewsManager views, RegExpProcessingResultsCollection<ColRegExpStatisticsProcessingResult> matches)
		{
			var positiveDocumentsCount = (double) views.MainForm.datasetMain.Documents.Count(p => !p.IsScoreNull() && p.Score > 0);

			var regExpJoin = from row in views.MainForm.datasetMain.ColRegExp.Cast<MainDataSet.ColRegExpRow>()
			                                  .Where(x => x.RowState != DataRowState.Deleted)
			                 join match in matches.Items
				                 on row.ID equals match.ID
			                 select new { RegExpRow = row, Match = match };

			foreach (var item in regExpJoin)
			{
				item.RegExpRow["TotalDocuments"] = item.Match.TotalDocuments;
				item.RegExpRow["TotalMatches"] = item.Match.TotalMatches;
				item.RegExpRow["PosDocuments"] = item.Match.PostitiveDocuments;

				///////////////////////////////////////////////////////////////////////////////

				var positiveDocumetsPercentage = positiveDocumentsCount > 0 ? (item.Match.PostitiveDocuments / positiveDocumentsCount) * 100D : 0;

				item.RegExpRow["PercentPosDocuments"] = positiveDocumetsPercentage;
			}

			///////////////////////////////////////////////////////////////////////////////

			//views.MainForm.adapterColRegExp.Update(views.MainForm.datasetMain.ColRegExp);
			//views.MainForm.adapterColRegExp.Fill(views.MainForm.datasetMain.ColRegExp);
		}

        protected int ColRegExp_WritePythonExtractedValues(ViewsManager views, RegExpProcessingResultsCollection<RegExpPythonExtractSingleProcessingResult> values)
        {
            var progressMax = 0;

            ///////////////////////////////////////////////////////////////////////////////

            var join = (from extractResult in values.Items

                        join rowDynamicColumn in views.MainForm.datasetMain.DynamicColumns
                            on extractResult.ColumnID equals rowDynamicColumn.ID

                        join rowDocument in views.MainForm.datasetMain.Documents
                            on extractResult.DocumentID equals rowDocument.ED_ENC_NUM                       

                        select new { extractResult, rowDocument, rowDynamicColumn })
                .ToList();

            if (!join.Any())
                return 0;

            ///////////////////////////////////////////////////////////////////////////////

            var rowsToUpdate = new List<MainDataSet.DocumentsRow>();

            ///////////////////////////////////////////////////////////////////////////////
                        
            int progressValue = 0;

            foreach (var j in join)
            {
                try
                {
                    var columnName = j.rowDynamicColumn.Title;

                    ///////////////////////////////////////////////////////////////////////////////
                    if (columnName.StartsWith("NOTE_TEXT"))
                    {
                        var newValue = JsonConvert.SerializeObject(new Tuple<object, string>(JsonConvert.DeserializeObject(j.extractResult.Result), j.extractResult.NoteTextColumnName));

                        views.MainForm.adapterDocuments.StartBatchQuery();

                        var connection = views.MainForm.adapterDocuments.Connection;
                        
                        var cmdText = String.Format("UPDATE [{0}] SET [{1}] = @NewValue WHERE [ED_ENC_NUM] = {2}", "Documents", columnName, j.extractResult.DocumentID);                        
                        var cmd = new OleDbCommand(cmdText, connection);
                        cmd.Parameters.AddWithValue("@NewValue", newValue);

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        PythonExtract_WriteFreeTextValue(j.extractResult, j.rowDocument, columnName);
                    }
                    
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }

                ///////////////////////////////////////////////////////////////////////////////

                progressValue ++;
                if (progressValue % 20 == 0)
                {
                    if (_worker.CancellationPending)
                        break;

                    var progressPercentage = (int)((double)progressValue / join.Count * 50D);
                    ReportProgress(progressPercentage + 50, "Writing values " + progressValue + " of " + join.Count);
                }
                j.rowDocument.AcceptChanges();
            }
            ReportProgress(100);
            return progressMax;
        }

        protected int ColRegExp_WriteExtractedValues(ViewsManager views, RegExpProcessingResultsCollection<ColRegExpExtractProcessingResult> values, bool scriptExtract)
		{
			var progressMax = 0;

			///////////////////////////////////////////////////////////////////////////////

			var join = (from extractResult in values.Items

			            join rowDynamicColumn in views.MainForm.datasetMain.DynamicColumns
				            on extractResult.ColumnID equals rowDynamicColumn.ID

			            join rowDocument in views.MainForm.datasetMain.Documents
				            on extractResult.DocumentID equals rowDocument.ED_ENC_NUM

			            orderby extractResult.ExtractOptions != null ? extractResult.ExtractOptions.Order : 0 descending

			            select new { extractResult, rowDocument, rowDynamicColumn })
				.ToList();

			if (!join.Any())
				return 0;

			///////////////////////////////////////////////////////////////////////////////

			var rowsToUpdate = new List<MainDataSet.DocumentsRow>();

			///////////////////////////////////////////////////////////////////////////////

			int progressValue = 0;

			foreach (var j in join)
			{
				try
				{
					var columnName = j.rowDynamicColumn.Title;

					///////////////////////////////////////////////////////////////////////////////

					var needUpdate = false;

					switch ((DynamicColumnType) j.rowDynamicColumn.Type)
					{
						case DynamicColumnType.FreeText:
							needUpdate = Extract_WriteFreeTextValue(j.extractResult, j.rowDocument, columnName, scriptExtract);
							break;

						case DynamicColumnType.Numeric:
							needUpdate = Extract_WriteNumericValue(j.extractResult, j.rowDocument, columnName, scriptExtract);
							break;

						case DynamicColumnType.DateTime:
							needUpdate = Extract_WriteDateTimeValue(j.extractResult, j.rowDocument, columnName, scriptExtract);
							break;
					}
                   
					if (needUpdate && !j.rowDynamicColumn.Title.StartsWith("NOTE_TEXT"))
					{
						rowsToUpdate.Add(j.rowDocument);

						progressMax++;
					}
                    if (needUpdate && j.rowDynamicColumn.Title.StartsWith("NOTE_TEXT"))
                    {
                        progressMax++;
                    }

                    ///////////////////////////////////////////////////////////////////////////////

                    j.rowDocument.AcceptChanges();
				}
				catch (Exception ex)
				{
					HandleException(ex);
				}

				///////////////////////////////////////////////////////////////////////////////

				progressValue++;
				if (progressValue % 50 == 0)
				{
					if (_worker.CancellationPending)
						break;

					var progressPercentage = (int) ((double) progressValue / join.Count * 100D);
					ReportProgress(progressPercentage, "Writing values " + progressValue + " of " + join.Count);
				}
			}

            ///////////////////////////////////////////////////////////////////////////////
            progressValue = 0;

            if (rowsToUpdate.Count > 0)
			{
				progressMax = rowsToUpdate.Count;

				progressValue = 0;
				ReportProgress(0);

				var columnsToUpdate = join.Where(x => !x.rowDynamicColumn.Title.StartsWith("NOTE_TEXT")).Select(x => x.rowDynamicColumn.Title).Distinct().ToList();
				var columnsInfo = views.MainForm.adapterDocuments.GetColumnsInfo(columnsToUpdate);
				if (columnsToUpdate.Count != columnsInfo.Count)
					throw new Exception("Failed to create update command");

                if (columnsInfo.Count != 0)
                {
                    var updateCommand = views.MainForm.adapterDocuments.CreateUpdateColumnsCommand(columnsInfo);

                    foreach (var row in rowsToUpdate)
                    {
                        views.MainForm.adapterDocuments.Update(row, columnsInfo, updateCommand);

                        ///////////////////////////////////////////////////////////////////////////////

                        progressValue++;
                        if (progressValue % 50 == 0)
                        {
                            if (_worker.CancellationPending)
                                break;

                            var progressPercentage = (int)((double)progressValue / progressMax * 100D);
                            ReportProgress(progressPercentage, "Writing values " + progressValue + " of " + join.Count);
                        }
                    }

                    views.MainForm.adapterDocuments.Fill();
                }               
            }

            ///////////////////////////////////////////////////////////////////////////////
            /* For Note_Text Columns */
            var noteColumnsToUpdate = join.Where(x => x.rowDynamicColumn.Title.StartsWith("NOTE_TEXT")).Distinct().ToList();
            
            List<string> noteColumnsValue = new List<string>();

            foreach (var row in noteColumnsToUpdate)
            {
                List<ColumnInfo> noteColumnsInfo = new List<ColumnInfo>();
                ColumnInfo colInfo = new ColumnInfo();
                colInfo.Name = row.rowDynamicColumn.Title;
                // to give string type using name's type
                colInfo.Type = colInfo.Name.GetType();

                noteColumnsInfo.Add(colInfo);

                views.MainForm.adapterDocuments.StartBatchQuery();
                var updateCommand = views.MainForm.adapterDocuments.CreateUpdateColumnsCommand(noteColumnsInfo);

                updateCommand.Parameters[0].Value = row.extractResult.Value;
                updateCommand.Parameters[1].Value = row.extractResult.DocumentID;

                var count = updateCommand.ExecuteNonQuery();
                if (count != 1)
                    throw new Exception(String.Format("Command updated {0} of expected 1 rows", count));

                views.MainForm.adapterDocuments.StartBatchQuery();

                progressValue++;
                if (progressValue % 50 == 0)
                {
                    if (_worker.CancellationPending)
                        break;

                    var progressPercentage = (int)((double)progressValue / progressMax * 100D);
                    ReportProgress(progressPercentage, "Writing values " + progressValue + " of " + join.Count);
                }
            }

            ///////////////////////////////////////////////////////////////////////////////

            return progressMax;
		}

		protected bool Extract_WriteFreeTextValue(ColRegExpExtractProcessingResult extractResult, MainDataSet.DocumentsRow rowDocument, string columnName, bool scriptExtract)
		{
			string newValue;

			if (!scriptExtract)
			{
				List<string> currentValues = null;

				if (extractResult.ExtractOptions.AddToPrevious.HasValue && extractResult.ExtractOptions.AddToPrevious.Value)
				{
					if (!rowDocument.IsNull(columnName))
					{
						var textValue = (string)rowDocument[columnName];

						try
						{
							if (textValue.StartsWith("["))
								currentValues = JsonConvert.DeserializeObject<List<string>>(textValue);
						}
						catch
						{
						}

						if (currentValues == null)
							currentValues = new List<string> { textValue };
					}
				}

				///////////////////////////////////////////////////////////////////////////////

				if (currentValues == null)
					currentValues = new List<string>();

				if (extractResult.Value.StartsWith("["))
				{
					try
					{
						var extractedValues = JsonConvert.DeserializeObject<string[]>(extractResult.Value);
						currentValues.AddRange(extractedValues);
					}
					catch
					{
						currentValues.Add(extractResult.Value);
					}
				}
				else
					currentValues.Add(extractResult.Value);

				newValue = currentValues.Count > 1
					? JsonConvert.SerializeObject(currentValues)
					: currentValues.First();
			}
			else
				newValue = extractResult.Value;

			///////////////////////////////////////////////////////////////////////////////

            if (!columnName.StartsWith("NOTE_TEXT"))
			    rowDocument[columnName] = newValue;

			return true;
		}
        protected bool PythonExtract_WriteFreeTextValue(RegExpPythonExtractSingleProcessingResult extractResult, MainDataSet.DocumentsRow rowDocument, string columnName)
        {
            string newValue;


            newValue = JsonConvert.SerializeObject(new Tuple<object, string> (JsonConvert.DeserializeObject(extractResult.Result), extractResult.NoteTextColumnName));

            ///////////////////////////////////////////////////////////////////////////////

            if (!columnName.StartsWith("NOTE_TEXT"))
                rowDocument[columnName] = newValue;

            return true;
        }
        protected bool Extract_WriteNumericValue(ColRegExpExtractProcessingResult extractResult, MainDataSet.DocumentsRow rowDocuments, string columnName, bool scriptExtract)
		{
			decimal numericValue;
			if (Decimal.TryParse(extractResult.Value, out numericValue))
			{
				rowDocuments[columnName] = numericValue;
				return true;
			}

			return false;
		}

		protected bool Extract_WriteDateTimeValue(ColRegExpExtractProcessingResult extractResult, MainDataSet.DocumentsRow rowDocuments, string columnName, bool scriptExtract)
		{
			var result = false;

			DateTime dt;

			if (!scriptExtract)
			{
				var dateTimeFormat = extractResult.ExtractOptions.DateTimeFormat;
				if (!String.IsNullOrEmpty(dateTimeFormat))
				{
					var formats = dateTimeFormat.Split(',')
												.Select(x => x.Replace("\"", ""));

					result = DateTime.TryParseExact(extractResult.Value, formats.ToArray(), _cultureInfo, DateTimeStyles.None, out dt);
				}
				else
				{
					result = DateTime.TryParse(extractResult.Value, out dt);
				}
			}
			else
				result = DateTime.TryParse(extractResult.Value, out dt);

			///////////////////////////////////////////////////////////////////////////////

			if (result)
			{
				rowDocuments[columnName] = dt;
			}

			return result;
		}

		#endregion

		#region Implementation: IDisposable

		public void Dispose()
		{
			try
			{
				if (_process != null)
				{
					try
					{
						if (!_process.HasExited)
							_process.Kill();
					}
					catch
					{
					}

					///////////////////////////////////////////////////////////////////////////////

					try
					{
						_process.Dispose();
					}
					catch
					{
					}

					_process = null;
				}

				///////////////////////////////////////////////////////////////////////////////

				if (_watcher != null)
				{
					try
					{
						_watcher.Dispose();
					}
					catch
					{
					}

					_watcher = null;
				}

				///////////////////////////////////////////////////////////////////////////////

				if (_event != null)
				{
					try
					{
						_event.Dispose();
					}
					catch
					{
					}
					_event = null;
				}

				///////////////////////////////////////////////////////////////////////////////

				if (_readerProgress != null)
				{
					try
					{
						_readerProgress.Dispose();
						_readerProgress = null;
					}
					catch (Exception ex)
					{
						MainForm.ShowExceptionMessage(ex);
					}
				}

				///////////////////////////////////////////////////////////////////////////////

				if (_readerLog != null)
				{
					try
					{
						_readerLog.Dispose();
						_readerLog = null;
					}
					catch (Exception ex)
					{
						MainForm.ShowExceptionMessage(ex);
					}
				}

				///////////////////////////////////////////////////////////////////////////////

				CleanupIntermediateFiles();

				///////////////////////////////////////////////////////////////////////////////

				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion
	}

}