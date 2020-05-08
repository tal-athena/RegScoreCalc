using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using Newtonsoft.Json;
using RegExpLib.Model;
using RegExpLib.Processing;

namespace EntitiesPlumbing.Code
{
	public class PlumbingCore : IDisposable
	{
        #region Constants

        private EntitiesProcessingParam _param;

        #endregion

        #region Fields

        protected readonly string[] _arguments;

		protected Logger _logger;
        protected Logger _progressor;

        #endregion

        #region Properties

        public Logger Logger
		{
			get {return _logger; }
		}

        public Logger Progresser
        {
            get { return _progressor; }
        }

		public string ParameterFile
		{
			get { return _arguments[0]; }
		}


        #endregion

        #region Ctors

        public PlumbingCore(string[] arguments)
		{
			if (arguments.Length != 1)
				throw new ArgumentException(String.Format("Invalid number of arguments: expected 1, specified: {0}", arguments.Length));

			_arguments = arguments;

            _param = EntitiesProcessingParam.Deserialize<EntitiesProcessingParam>(ParameterFile);

            _logger = new Logger(Path.Combine(_param.WorkingFolder, _param.LogFileName));
            _progressor = new Logger(Path.Combine(_param.WorkingFolder, _param.ProgressFileName), true);
		}

        public int StartProcessing()
        {
            if (_param.Command == "Calculate")
            {
                return CalculateEntities();
            } else if (_param.Command == "Generate")
            {
                return GenerateSqlite();
            } else if (_param.Command == "GetEntityNames")
            {
                return GetEntitiesNames();
            }
            return -1;
        }

        #endregion

        #region Operations

        public int GenerateSqlite()
        {
            try
            {
                Directory.SetCurrentDirectory(_param.WorkingFolder);

                ///////////////////////////////////////////////////////////////////////////////

                _logger.LogSection("Processing started", false);

                _logger.Log("Converting Access database to SQLite 3...");

                if (File.Exists(_param.SqliteFilePath))
                {
                    File.Delete(_param.SqliteFilePath);
                }

                var db = new Database(_param.AccessFilePath, "",  _logger, _progressor);
                db.ConvertMdbToSqlite(_param.SqliteFilePath);

                _logger.Log("Finished converting databases");
                return 0;

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return -1;
            }            
        }

        public int CalculateEntities()
        {
            try
            {
                _logger.Log("Starting python....");
                var python = new Python(_param, _logger, _progressor);

                var result = python.StartML("get_entities.py");

                _progressor.Log("50");

                if (result == 0)
                {
                    var db = new Database(_param.AccessFilePath, "", _logger, _progressor);

                    CalculatedEntitiesResult outputResult = new CalculatedEntitiesResult();

                    outputResult.Documents = db.GetResultFromSqlite(_param.SqliteFilePath);
                    outputResult.EntityLabels = db.GetEntityLabelsFromSqlite(_param.SqliteFilePath);

                    string json = JsonConvert.SerializeObject(outputResult);

                    using (var writer = new StreamWriter(new FileStream(_param.GetFullPath(_param.OutputFileName), FileMode.Create)))
                    {
                        writer.Write(json);
                    }

                    _logger.Log("Finished converting SQlite3 to output");

                    _progressor.Log("70");
                }
                else
                {
                    _logger.LogError(String.Format("Python script process returned code {0}, aborting...", result));
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);                
            }
            return -1;
        }

        public int GetEntitiesNames()
        {
            try
            {
                _logger.Log("Starting python....");
                var python = new Python(_param, _logger, _progressor);

                var result = python.GetEntityNames("get_entity_names.py");

                _logger.Log("Finished python");

                if (result != null)
                {
                    string json = JsonConvert.SerializeObject(result);

                    using (var writer = new StreamWriter(new FileStream(_param.GetFullPath(_param.OutputFileName), FileMode.Create)))
                    {
                        writer.Write(json);
                    }
                    _logger.Log("Finished writting to json");

                    _logger.Log("Finished python");
                    return 0;
                }
                else
                {
                    _logger.LogError(String.Format("Python script process returned code {0}, aborting...", -1));
                    return -1;
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return -1;
        }

        #endregion

        #region Implementation

        protected void PerformCleanup()
		{
			if (!IsCleanupRequired())
				return;

			_logger.LogSection("Removing intermediate files...");

			try
			{
				var di = new DirectoryInfo(_param.WorkingFolder);
				foreach (var fi in di.GetFiles().ToList())
				{
					try
					{

                        fi.Delete();
					}
					catch (Exception ex)
					{
						_logger.LogError(ex.Message);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			_logger.Log("Finished removing intermediate files");
		}

		protected bool IsCleanupRequired()
		{
			if (_arguments.Length == 11)
			{
				var cleanupArg = _arguments[10].ToLower();
				if (cleanupArg == "/i" || cleanupArg == "-i")
					return false;
			}

			return true;
		}

		protected void SafeDeleteFile(string fileName)
		{
			try
			{
				if (File.Exists(fileName))
					File.Delete(fileName);
			}
			catch { }
		}

		#endregion

		#region Implementation: IDisposable

		public void Dispose()
		{
			try
			{
				var wait = new EventWaitHandle(false, EventResetMode.ManualReset);

				_logger.StopHeartBeat(wait);

				wait.WaitOne(3000);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
		}

		#endregion
	}    
}
