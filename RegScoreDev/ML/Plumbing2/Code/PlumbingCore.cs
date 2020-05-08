using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using Newtonsoft.Json;

namespace Plumbing2.Code
{
	public class PlumbingCore : IDisposable
	{
		#region Constants

		protected const string sqliteFileName = "train.sqlite3";		

		#endregion

		#region Fields

		protected readonly string[] _arguments;

		protected Logger _logger;
        protected Logger _progresser;

        #endregion

        #region Properties

        public Logger Logger
		{
			get {return _logger; }
		}

        public string PythonFile
        {
            get { return _arguments[0]; }
        }

        public string OutputFolder
        {
            get { return _arguments[1]; }
        }
        public string DatabaseFileName
        {
            get { return _arguments[2]; }
        }


        public string DBPassword
		{
			get { return _arguments[3] == "NULL" ? null : _arguments[3]; }
		}
		
		public int ColumnID
		{
			get { return Int32.Parse(_arguments[4]); }
		}

		public int NoteColumnIndex
		{
			get { return Int32.Parse(_arguments[5]); }
		}

        public string NoteColumnName
        {
            get { return _arguments[5] == "0" ? "NOTE_TEXT" : ("NOTE_TEXT" + _arguments[5]); }
        }

        public string OutputFile
		{
			get { return _arguments[6]; }
		}        

        public string LogFileName
        {
            get { return _arguments[7]; }
        }

        public string ProgressFileName
        {
            get { return _arguments[8]; }
        }

        public List<double> FilteredDocument
        {
            get { return JsonConvert.DeserializeObject<List<double>>(_arguments[9]); }
        }
        

        #endregion

        #region Ctors

        public PlumbingCore(string[] arguments)
		{
			if (arguments.Length < 10)
				throw new ArgumentException(String.Format("Invalid number of arguments: expected 10, specified: {0}", arguments.Length));

			_arguments = arguments;

            _logger = new Logger(Path.Combine(OutputFolder, LogFileName));
            _progresser = new Logger(Path.Combine(OutputFolder, ProgressFileName));
        }

		#endregion

		#region Operations

		public int StartProcessing()
		{
            try
            {            
                Directory.SetCurrentDirectory(OutputFolder);

			    ///////////////////////////////////////////////////////////////////////////////

			    _logger.Log("Converting Access database to SQLite 3...");

			    var db = new Database(this.DatabaseFileName,this.DBPassword,  _logger, _progresser);
			    db.ConvertMdbToSqlite(sqliteFileName, NoteColumnName, FilteredDocument);

                _logger.Log("Finished converting databases");
            

			    _logger.LogSection("Starting python...");

                File.Copy(PythonFile, Path.Combine(this.OutputFolder, Path.GetFileName(PythonFile)));

			    var python = new Python(this.OutputFolder, _logger);

			    var result = python.StartML(this.PythonFile, sqliteFileName);

			    Directory.SetCurrentDirectory(this.OutputFolder);

			    _logger.Log("Finished python");

			    ///////////////////////////////////////////////////////////////////////////////

			    if (result == 0)
			    {
				    _logger.LogSection("Get output data from SQlite 3 ...");

				    List<ExtractResult> extractResults = db.GetResultFromSqlite(sqliteFileName, NoteColumnName, ColumnID);

                    string json = JsonConvert.SerializeObject(extractResults);

                    using (var writer = new StreamWriter(new FileStream(OutputFile, FileMode.Create)))
                    {
                        writer.Write(json);
                    }

                    _logger.Log("Finished converting SQlite3 to output");
			    }
			    else
			    {
				    _logger.LogError(String.Format("Python script process returned code {0}, aborting...", result));
			    }

                return result;
                //PerformCleanup();
            } catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return -1;
            
		}


        #endregion

        #region Implementation

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
    public class ExtractResult
    {
        public double DocumentID { get; set; }
        public string Result { get; set; }
        public int ColumnID { get; set; }
        public string NoteTextColumnName { get; set; }
    }
    
}
