using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using Newtonsoft.Json;

namespace Plumbing.Code
{
	public class PlumbingCore : IDisposable
	{
		#region Constants

		protected const string _trainFileName = "train.sqlite3";
		protected const string _testileName = "test.sqlite3";

		#endregion

		#region Fields

		protected readonly string[] _arguments;

		protected Logger _logger;

		#endregion

		#region Properties

		public Logger Logger
		{
			get {return _logger; }
		}

		public string OutputFolder
		{
			get { return _arguments[0]; }
		}

		public string InputFileName
		{
			get { return _arguments[1]; }
		}

		public string ParametersFileName
		{
			get { return _arguments[2]; }
		}

		public string CsvFileName
		{
			get { return _arguments[3]; }
		}

		public string LogFileName
		{
			get { return _arguments[4]; }
		}

        public string NoteColumnName
        {
            get { return _arguments[5] == "0" ? "NOTE_TEXT" : ("NOTE_TEXT" + _arguments[5]); }
        }

        public string AnacondaPath
        {
            get { return _arguments[6] == "" ? null : _arguments[6]; }
        }

        public string PythonEnv
        {
            get { return _arguments[7] == "NULL" ? "" : _arguments[7]; }
        }

        public int PythonVersion
        {
            get { return Int32.Parse(_arguments[8]); }
        }

        public string ProcessLanguage
        {
            get { return _arguments[9] == "" ? "en" : _arguments[9]; }
        }

        #endregion

        #region Ctors

        public PlumbingCore(string[] arguments)
		{
			if (arguments.Length < 10)
				throw new ArgumentException(String.Format("Invalid number of arguments: expected 10, specified: {0}", arguments.Length));

			_arguments = arguments;

			_logger = new Logger(Path.Combine(this.OutputFolder, this.LogFileName));
		}

		#endregion

		#region Operations

		public int StartProcessing()
		{
			Directory.SetCurrentDirectory(this.OutputFolder);

			///////////////////////////////////////////////////////////////////////////////

			var parameters = DeserializeParameters(this.ParametersFileName);

			if (parameters.displaySettings)
			{
				LogParameters(parameters);
				_logger.LogSection("Processing started");
			}
			else
				_logger.LogSection("Processing started", false);

			///////////////////////////////////////////////////////////////////////////////

			if (parameters.version == PlumbingToolVersion.Version2 && parameters.dynamicColumnID > 0)
			{
				_logger.Log("Converting dynamic categories");

				var converter = new DynamicCategoriesConverter(_logger);
				converter.Convert(this.InputFileName, parameters);

				_logger.Log("Finished converting dynamic categories");
			}

			///////////////////////////////////////////////////////////////////////////////

			_logger.Log("Converting Access database to SQLite 3...");

			var db = new Database(this.InputFileName, parameters, _logger);
			db.ConvertMdbToSqlite(_trainFileName, _testileName, parameters.version == PlumbingToolVersion.Version2 ? parameters.dynamicColumnID : 0, NoteColumnName);

            _logger.Log("Finished converting databases");
            _logger.Log(String.Format("Output folder: {0}", this.OutputFolder));

            ///////////////////////////////////////////////////////////////////////////////

            if (!parameters.runPython)
			{
				if (IsCleanupRequired())
				{
					SafeDeleteFile(InputFileName);
					SafeDeleteFile(LogFileName);
				}

				return 111;
			}

			///////////////////////////////////////////////////////////////////////////////

			_logger.LogSection("Starting machine learning...");

			var python = new Python(this.OutputFolder, _logger);

			var result = python.StartML(_trainFileName, _testileName, AnacondaPath, PythonEnv, PythonVersion, ProcessLanguage);

			Directory.SetCurrentDirectory(this.OutputFolder);

			_logger.Log("Finished machine learning");

			///////////////////////////////////////////////////////////////////////////////

			if (result == 0)
			{
				_logger.LogSection("Converting SQlite 3 database to CSV...");

				db.ConvertSqliteToCSV(_testileName, this.CsvFileName);

				_logger.Log("Finished converting database");
			}
			else
			{
				_logger.LogError(String.Format("Machine learning process returned code {0}, aborting...", result));
			}

            using (var writer = new StreamWriter(new FileStream("NoteTextColumnName.csv", FileMode.Create)))
            {
                writer.WriteLine(NoteColumnName);
            }

            PerformCleanup();

			return result;
		}

		protected void LogParameters(Parameters parameters)
		{
			_logger.LogSection("Tool started with settings:");

			var html = "<table><thead><tr><th>Setting</th><th>Value</th></tr></thead>";

			foreach (var prop in parameters.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				var value = prop.GetValue(parameters);
				var valueStr = String.Empty;

				if (prop.PropertyType.IsArray)
				{
					var list = new List<object>();
					var array = (Array) value;
					var enumerator = array.GetEnumerator();
					while (enumerator.MoveNext())
						list.Add(enumerator.Current);

					if (list.Count > 5)
					{
						list = list.Take(5).ToList();
						list.Add("...");
					}

					valueStr = String.Join(", ", list);
				}
				else if (value != null)
					valueStr = value.ToString();

				html += "<tr><td>" + prop.Name + "</td><td>" + valueStr + "</td></tr>";
			}

			html += "</table>";

			_logger.Log(html);
		}

		#endregion

		#region Implementation

		protected Parameters DeserializeParameters(string filePath)
		{
			var parameters = JsonConvert.DeserializeObject<Parameters>(File.ReadAllText(filePath));

			if (parameters.positiveCategories == null)
				parameters.positiveCategories = new int[0];

			if (parameters.excludedCategories == null)
				parameters.excludedCategories = new int[0];

			return parameters;
		}

		protected void PerformCleanup()
		{
			if (!IsCleanupRequired())
				return;

			_logger.LogSection("Removing intermediate files...");

			try
			{
				var di = new DirectoryInfo(this.OutputFolder);
				foreach (var fi in di.GetFiles().ToList())
				{
					try
					{
						var delete = (String.Compare(fi.Name, this.CsvFileName, StringComparison.InvariantCultureIgnoreCase) != 0)
						             && (String.Compare(fi.Name, this.LogFileName, StringComparison.InvariantCultureIgnoreCase) != 0);
						
						if (delete)
							File.Delete(fi.FullName);
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
