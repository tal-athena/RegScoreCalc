using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using Helpers;
using Newtonsoft.Json;

namespace RegScoreCalc.Code
{
	public class BrowserManager : IDisposable
	{
		#region Fields

		protected readonly HtmlViewInfo _viewInfo;
		protected readonly ViewsManager _views;
		protected readonly ChromiumWebBrowser _browser;

		protected SynchronizationContext _context;

		protected Process _process;
		protected FileSystemWatcher _watcher;
		protected string _stateEventHandler;

		protected StreamReader _reader;
		protected const string _logFileName = "log.txt";

		#endregion

		#region Properties

		public ChromiumWebBrowser Browser
		{
			get { return _browser; }
		}

		#endregion

		#region Ctors

		public BrowserManager(HtmlViewInfo viewInfo, ViewsManager views, ChromiumWebBrowser browser)
		{
			_viewInfo = viewInfo;
			_views = views;
			_browser = browser;

			_context = SynchronizationContext.Current;

			_browser.MenuHandler = new JsContextMenuHandler();
            
			//_browser.JsDialogHandler = new JsDialogHandler(_viewName);

			var options = new BindingOptions
			              {
				              CamelCaseJavascriptNames = false,
			              };

			CefSharpSettings.LegacyJavascriptBindingEnabled = true;

			_browser.RegisterJsObject("Host", this, options);
             
		}

		#endregion

		#region Events

		private void process_Exited(object sender, EventArgs e)
		{
			try
			{
				Directory.SetCurrentDirectory(_viewInfo.ViewFolderPath);

				if (!String.IsNullOrEmpty(_stateEventHandler))
					_browser.ExecuteScriptAsync(_stateEventHandler, 0, _process.ExitCode.ToString());

				Dispose();
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}
		}

		private void watcher_Changed(object sender, FileSystemEventArgs e)
		{
			try
			{
				if (!String.IsNullOrEmpty(_stateEventHandler))
				{
					if (_reader == null)
						_reader = new StreamReader(File.Open(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

					var content = _reader.ReadToEnd();
					if (!String.IsNullOrEmpty(content))
					{
						content = content.Replace(Environment.NewLine, "");
                        content = content.Replace("\\", "&#92;");
						_browser.ExecuteScriptAsync(_stateEventHandler, 1, content);
					}
				}
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}
		}

		#endregion

		#region Operations: calls to JavaScript

		public void InvokeJs_OnCmd_Event(string buttonText)
		{
			try
			{
				_browser.ExecuteScriptAsync(String.Format("OnCmd('{0}');", buttonText));
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}
		}

		public bool InvokeJs_OnBeforeClose_Event()
		{
			try
			{
				var result = _browser.EvaluateScriptAsync("OnBeforeClose();");
				result.Wait(new TimeSpan(0, 0, 15));
			}
			catch (Exception ex)
			{
				ex.ToString();
			}

			Dispose();

			return true;
		}

		#endregion

		#region Operations: calls from JavaScript

		#region General

		public static string GetViewData(ViewsManager views, string viewName)
		{
			OleDbConnection connection = null;
			bool needClose = false;

			try
			{
				connection = GetOpenedConnection(views, ref needClose);

				var sql = "SELECT TOP 1 ViewData FROM ViewData WHERE (ViewName = @ViewName)";

				var command = new OleDbCommand(sql, connection);
				command.Parameters.AddWithValue("@ViewName", viewName);

				var viewData = command.ExecuteScalar() as string;

				return viewData;
			}
			finally
			{
				CloseConnection(connection, needClose);
			}
		}

		public string GetViewData()
		{
			try
			{
				return GetViewData(_views, _viewInfo.Name);
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}

			return String.Empty;
		}

		public static bool SetViewData(ViewsManager views, string viewName, string data)
		{
			OleDbConnection connection = null;
			bool needClose = false;

			try
			{
				connection = GetOpenedConnection(views, ref needClose);

				var command = new OleDbCommand();

				string sql;
				if (String.IsNullOrEmpty(GetViewData(views, viewName)))
				{
					sql = "INSERT INTO ViewData (ViewName, ViewData) VALUES (@ViewName, @ViewData)";
					command.Parameters.AddWithValue("@ViewName", viewName);
					command.Parameters.AddWithValue("@ViewData", data);
				}
				else
				{
					sql = "UPDATE ViewData SET ViewData = @ViewData WHERE ViewName = @ViewName";
					command.Parameters.AddWithValue("@ViewData", data);
					command.Parameters.AddWithValue("@ViewName", viewName);
				}

				command.CommandText = sql;
				command.Connection = connection;

				if (command.ExecuteNonQuery() == 1)
					return true;
			}
			finally
			{
				CloseConnection(connection, needClose);
			}

			return false;
		}

		public bool SetViewData(string data)
		{
			try
			{
				return SetViewData(_views, _viewInfo.Name, data);
			}
			catch (Exception ex)
			{
				OnException(ex.Message);

				return false;
			}
		}

		public void ShowInfoMessage(string message)
		{
			try
			{
				_context.Send(state =>
				{
					try
					{
						MessageBox.Show(message, _viewInfo.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					catch (Exception ex)
					{
						OnException(ex.Message);
					}

				}, null);
			}
			catch { }
		}

		public void ShowWarningMessage(string message)
		{
			try
			{
				_context.Send(state =>
				{
					try
					{
						MessageBox.Show(message, _viewInfo.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					catch (Exception ex)
					{
						OnException(ex.Message);
					}

				}, null);
			}
			catch { }
		}

		public void OnException(string message)
		{
			MessageBox.Show(message, _viewInfo.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public void SetViewArgument(object argument, string typeName)
		{
			try
			{
				var json = argument as string;
				if (json != null && !String.IsNullOrEmpty(typeName))
				{
					var type = Type.GetType(typeName);
					argument = JsonConvert.DeserializeObject(json, type);
				}

				_viewInfo.View.Argument = argument;
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}
		}

		public void OpenView(string viewName, bool activate, object argument)
		{
			try
			{
				_context.Send(state =>
				{
					try
					{
						_views.CreateView(viewName, activate, true, argument);
					}
					catch (Exception ex)
					{
						OnException(ex.Message);
					}

				}, null);
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}
		}

		public string GetDbPassword()
		{
			return _views.MainForm.DbPassword;
		}

		#endregion

		#region Database operations

		public object ExecuteScalarSQL(string sql)
		{
			object result = null;

			OleDbConnection connection = null;
			bool needClose = false;

			try
			{
				connection = GetOpenedConnection(_views, ref needClose);

				var command = new OleDbCommand(sql, connection);

				result = command.ExecuteScalar();
			}
			catch (Exception ex)
			{
				OnException(ex.Message);

				return result;
			}
			finally
			{
				CloseConnection(connection, needClose);
			}

			return result;
		}

		public string ExecuteSQL(string sql, bool writeToFile = false)
		{
			var json = String.Empty;

			OleDbConnection connection = null;
			bool needClose = false;

			try
			{
				connection = GetOpenedConnection(_views, ref needClose);

				var command = new OleDbCommand(sql, connection);

				using (var reader = command.ExecuteReader())
				{
					var dictionary = DataReaderToDictionary(reader);
					json = JsonConvert.SerializeObject(dictionary);

					if (writeToFile)
					{
						var tempFile = Path.GetTempFileName();
						File.WriteAllText(tempFile, json);

						var uri = new Uri(tempFile);

						json = uri.AbsoluteUri;
					}
				}
			}
			catch (Exception ex)
			{
				OnException(ex.Message);

				return null;
			}
			finally
			{
				CloseConnection(connection, needClose);
			}

			return json;
		}

		public bool CopyDocumentsTable(string filePath, int mode, object[] otherTables)
		{
			OleDbConnection connection = null;

			try
			{
				filePath = GetFullFilePath(filePath);

				var originalDbPath = GetDatabasePath();
				File.Copy(originalDbPath, filePath, true);

				///////////////////////////////////////////////////////////////////////////////

				connection = new OleDbConnection(_views.MainForm.GetConnectionString(filePath));
				connection.Open();

				///////////////////////////////////////////////////////////////////////////////

				var tableExclusion = new TableExclusion
				                     {
					                     TableName = "Documents"
				                     };

				///////////////////////////////////////////////////////////////////////////////

				var exclusions = new List<TableExclusion>
				                 {
					                 tableExclusion
				                 };

				///////////////////////////////////////////////////////////////////////////////

				if (otherTables != null)
				{
					exclusions.AddRange(otherTables.Cast<string>()
					                               .Select(tableName => new TableExclusion
					                                                    {
						                                                    TableName = tableName
					                                                    }));
				}

				///////////////////////////////////////////////////////////////////////////////

				DeleteTables(connection, exclusions);

				return true;
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}
			finally
			{
				CloseConnection(connection, true);
			}

			return false;
		}

		#endregion

		#region File operations

		public string GetViewFolderPath()
		{
			return _viewInfo.ViewFolderPath;
		}

		public string GetFullPath(string fileName)
		{
			try
			{
				return GetFullFilePath(fileName);
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}

			return null;
		}

		public string SelectFileDialog(string initialFolder, bool saveFile, string filter)
		{
			try
			{
				var dlg = saveFile ? new SaveFileDialog() : new OpenFileDialog() as FileDialog;
				if (!String.IsNullOrEmpty(initialFolder))
					dlg.InitialDirectory = initialFolder;
				else
					dlg.InitialDirectory = GetDefaultFolder();

				if (dlg.ShowDialog(_views.MainForm) == DialogResult.OK)
					return dlg.FileName;
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}

			return null;
		}

		public string SelectFolderDialog(string initialFolder)
		{
			try
			{
				var folder = new SelectFolderParam { SelectedFolder = initialFolder };

				_context.Send(state =>
							  {
								  var param = (SelectFolderParam)state;

								  var dlg = new FolderBrowserDialog();
								  if (Directory.Exists(param.SelectedFolder))
									  dlg.SelectedPath = param.SelectedFolder;
								  else
									  dlg.SelectedPath = GetDefaultFolder();

								  if (dlg.ShowDialog(_views.MainForm) == DialogResult.OK)
									  param.SelectedFolder = dlg.SelectedPath;

							  }, folder);

				return folder.SelectedFolder;
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}

			return null;
		}

		public string CreateOutputFolder(string outputFolderPath)
		{
			string result = null;

			try
			{
				var dbFileName = Path.GetFileNameWithoutExtension(_views.MainForm.DocumentsDbPath);
				var now = DateTime.Now;

				dbFileName += now.ToString(" yyMMddHHmm");

				outputFolderPath = Path.Combine(outputFolderPath, dbFileName);

				if (Directory.Exists(outputFolderPath))
				{
					var dlgres = MessageBox.Show(String.Format("Output folder '{0}' already exists.{1}{1}Do you wish to replace its contents?", dbFileName, Environment.NewLine), _viewInfo.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (dlgres == DialogResult.Yes)
						Directory.Delete(outputFolderPath, true);
					else
						return result;
				}
				
				Directory.CreateDirectory(outputFolderPath);

				result = outputFolderPath;
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}

			return result;
		}

		public bool DeleteOutputFolder(string outputFolderPath)
		{
			bool result = false;

			try
			{
				if (Directory.Exists(outputFolderPath))
					Directory.Delete(outputFolderPath, true);

				result = true;
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}

			return result;
		}

		public bool FileExists(string filePath)
		{
			try
			{
				return File.Exists(filePath);
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}

			return false;
		}

		public bool DeleteFile(string filePath)
		{
			try
			{
				if (File.Exists(filePath))
					File.Delete(filePath);

				return true;
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}

			return false;
		}

		public bool RunExecutable(string filePath, string arguments, string stateEventHandler, string monitorFilePath)
		{
			var savedCurrentFolder = String.Empty;

			try
			{
				KillExecutable();

				///////////////////////////////////////////////////////////////////////////////

				savedCurrentFolder = Directory.GetCurrentDirectory();

				Directory.SetCurrentDirectory(_viewInfo.ViewFolderPath);

				filePath = GetFullFilePath(filePath);

				///////////////////////////////////////////////////////////////////////////////

				_stateEventHandler = stateEventHandler;

				_watcher = new FileSystemWatcher(monitorFilePath)
						   {
							   EnableRaisingEvents = true
						   };

				_watcher.Changed += watcher_Changed;

				///////////////////////////////////////////////////////////////////////////////

				_process = new Process
						   {
							   StartInfo = new ProcessStartInfo(filePath, arguments),
							   EnableRaisingEvents = true
						   };

				_process.Exited += process_Exited;
				_process.Start();

				return true;
			}
			catch (Exception ex)
			{
				OnException(ex.Message);

				Dispose();
			}
			finally
			{
				if (Directory.Exists(savedCurrentFolder))
					Directory.SetCurrentDirectory(savedCurrentFolder);
			}

			return false;
		}

		public void KillExecutable()
		{
			try
			{
				Dispose();
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}
		}

		public int GetFileSize(string filePath)
		{
			try
			{
				filePath = GetFullFilePath(filePath);

				if (File.Exists(filePath))
				{
					var fi = new FileInfo(filePath);
					return (int)fi.Length;
				}
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}

			return -1;
		}

		public string ReadTextFile(string filePath)
		{
			try
			{
				filePath = GetFullFilePath(filePath);
				if (File.Exists(filePath))
					return File.ReadAllText(filePath);
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}

			return null;
		}

		public bool WriteTextFile(string filePath, string content)
		{
			try
			{
				filePath = GetFullFilePath(filePath);

				File.WriteAllText(filePath, content);

				return true;
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}

			return false;
		}

		public bool IsNetworkPath(string path)
		{
			try
			{
				return PathIsNetworkPath(path);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			return false;
		}

		#endregion

		#region Machine learning

		public bool StartML(string outputFolder, string databaseFileName, string parametersFileName, string csvFileName, bool leaveIntermediateFiles, string stateEventHandler, int noteColumnIndex = 0, string language = "en")
		{
			var savedCurrentFolder = String.Empty;

			try
			{
				KillExecutable();

				///////////////////////////////////////////////////////////////////////////////

				savedCurrentFolder = Directory.GetCurrentDirectory();

				Directory.SetCurrentDirectory(outputFolder);

				///////////////////////////////////////////////////////////////////////////////

				_stateEventHandler = stateEventHandler;

				_watcher = new FileSystemWatcher(outputFolder)
				{
					EnableRaisingEvents = true,
					Filter = _logFileName
				};

				_watcher.Changed += watcher_Changed;

				///////////////////////////////////////////////////////////////////////////////

				var plumbingToolFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				plumbingToolFilePath = Path.Combine(plumbingToolFilePath, "plumbing.exe");

				var arguments = String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", QuotePath(outputFolder), databaseFileName, parametersFileName, csvFileName, _logFileName, noteColumnIndex, QuotePath(_views.AnacondaPath), _views.PythonEnv, _views.PythonVersion, language);

				if (leaveIntermediateFiles)
					arguments += " /i";

				///////////////////////////////////////////////////////////////////////////////

				_process = new Process
				{
					StartInfo = new ProcessStartInfo(plumbingToolFilePath, arguments),
					EnableRaisingEvents = true
				};

				_process.Exited += process_Exited;
				_process.Start();

				return true;
			}
			catch (Exception ex)
			{
				OnException(ex.Message);

				Dispose();
			}
			finally
			{
				if (Directory.Exists(savedCurrentFolder))
					Directory.SetCurrentDirectory(savedCurrentFolder);
			}

			return false;
		}

		public string PickColor(string initialValue)
		{
			string result = null;

			try
			{
				_context.Send(state =>
				{
					try
					{
						var dlg = new ColorDialog
						{
							Color = ColorTranslator.FromHtml(initialValue)
						};

						if (dlg.ShowDialog(_views.MainForm) == DialogResult.OK)
							result = String.Format("#{0}{1}{2}", dlg.Color.R.ToString("X2"), dlg.Color.G.ToString("X2"), dlg.Color.B.ToString("X2"));
					}
					catch (Exception ex)
					{
						OnException(ex.Message);
					}

				}, result);
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}

			return result;
		}

		#endregion

		#endregion

		#region Implementation

		protected static OleDbConnection GetOpenedConnection(ViewsManager views, ref bool needClose)
		{
			var connection = new OleDbConnection(views.MainForm.adapterRegExp.Connection.ConnectionString);
			if (connection.State != ConnectionState.Open)
			{
				connection.Open();

				needClose = true;
			}
			else
				needClose = false;

			return connection;
		}

		protected static void CloseConnection(OleDbConnection connection, bool needClose)
		{
			if (connection == null)
				return;

			if (connection.State == ConnectionState.Open && needClose)
				connection.Close();
		}

		protected IEnumerable<Dictionary<string, object>> DataReaderToDictionary(OleDbDataReader reader)
		{
			var dictionary = new List<Dictionary<string, object>>();
			var cols = new List<string>();

			for (var i = 0; i < reader.FieldCount; i++)
			{
				cols.Add(reader.GetName(i));
			}

			while (reader.Read())
			{
				dictionary.Add(DataReaderToRow(cols, reader));
			}

			return dictionary;
		}

		protected Dictionary<string, object> DataReaderToRow(IEnumerable<string> cols, OleDbDataReader reader)
		{
			return cols.ToDictionary(col => col, col => reader[col]);
		}

		protected string GetFullFilePath(string filePath)
		{
			if (Path.IsPathRooted(filePath))
				return filePath;

			return Path.Combine(_viewInfo.ViewFolderPath, filePath);
		}

		protected string QuotePath(string path)
		{
			return "\"" + path + "\"";
		}

		protected string GetDatabasePath()
		{
			return _views.MainForm.adapterRegExp.Connection.DataSource;
		}

		protected string GetDefaultFolder()
		{
			string result = String.Empty;

			try
			{
				const string appFolder = "RegScoreCalc";

				result = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				result = Path.Combine(result, appFolder);

				if (!Directory.Exists(result))
					Directory.CreateDirectory(result);
			}
			catch
			{
				result = String.Empty;
			}

			if (String.IsNullOrEmpty(result))
				result = _viewInfo.ViewFolderPath;

			return result;
		}

		protected void DeleteTables(OleDbConnection connection, List<TableExclusion> exclusions)
		{
			var schema = connection.GetSchema("Tables");
			foreach (var row in schema.Rows.Cast<DataRow>().Where(x => x.Field<string>("TABLE_TYPE") == "TABLE"))
			{
				var tableName = row["TABLE_NAME"] as string;

				var exclusion = exclusions.FirstOrDefault(x => String.Compare(x.TableName, tableName, StringComparison.InvariantCultureIgnoreCase) == 0);
				if (exclusion != null)
				{
					if (exclusion.ColumnValueConditions.Any())
						DeleteRows(connection, exclusion);
				}
				else
					DeleteTable(connection, tableName);
			}
		}

		protected void DeleteRows(OleDbConnection connection, TableExclusion exclusion)
		{
			var command = connection.CreateCommand();

			var cmdText = String.Format("DELETE FROM {0}", exclusion.TableName);

			cmdText += " WHERE ";

			var where = String.Empty;

			foreach (var condition in exclusion.ColumnValueConditions)
			{
				if (!String.IsNullOrEmpty(where))
					where += " AND ";

				///////////////////////////////////////////////////////////////////////////////

				cmdText += condition.ColumnName + " ";

				var addParameter = false;

				switch (condition.Operator)
				{
					case ValueConditionOperator.Equals:
						cmdText += String.Format("= @{0}", condition.ColumnName);
						addParameter = true;
						break;

					case ValueConditionOperator.MoreThan:
						cmdText += String.Format("> @{0}", condition.ColumnName);
						addParameter = true;
						break;

					case ValueConditionOperator.MoreOrEqual:
						cmdText += String.Format(">= @{0}", condition.ColumnName);
						addParameter = true;
						break;

					case ValueConditionOperator.LessThan:
						cmdText += String.Format("< @{0}", condition.ColumnName);
						addParameter = true;
						break;

					case ValueConditionOperator.LessOrEqual:
						cmdText += String.Format("<= @{0}", condition.ColumnName);
						addParameter = true;
						break;

					case ValueConditionOperator.IsNull:
						cmdText += "IS NULL";
						break;

					case ValueConditionOperator.IsNotNull:
						cmdText += "IS NOT NULL";
						break;
				}

				///////////////////////////////////////////////////////////////////////////////

				if (addParameter)
					command.Parameters.AddWithValue("@" + condition.ColumnName, condition.Value);

				command.CommandText = cmdText;

				command.ExecuteNonQuery();
			}
		}

		protected void DeleteTable(OleDbConnection connection, string tableName)
		{
			var command = connection.CreateCommand();
			command.CommandText = String.Format("DROP TABLE {0}", tableName);

			command.ExecuteNonQuery();
		}

		#endregion

		#region Implementation: IDisposable

		public void Dispose()
		{
			try
			{
				_stateEventHandler = null;

				if (_process != null)
				{
					try
					{
						if (!_process.HasExited)
							_process.Kill();
					}
					catch { }

					try
					{
						_process.Dispose();
					}
					catch { }

					_process = null;
				}

				try
				{
					var python = Process.GetProcessesByName("python");
					foreach (var process in python.ToList())
					{
						try
						{
							process.Kill();
						}
						catch { }

					}
				}
				catch { }

				if (_watcher != null)
				{
					_watcher.Dispose();
					_watcher = null;
				}

				if (_reader != null)
				{
					_reader.Dispose();
					_reader = null;
				}

				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
			catch (Exception ex)
			{
				OnException(ex.Message);
			}
		}

		#endregion

		#region WinAPI

		[DllImport("shlwapi.dll")]
		protected static extern bool PathIsNetworkPath(string path);

		#endregion
	}

	public class JsContextMenuHandler : IContextMenuHandler
	{
		#region Implementation: IContextMenuHandler

		public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
		{
            model.Clear();
            model.AddItem(CefMenuCommand.Copy, "Copy");            
        }

		public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
		{
            if (commandId == CefMenuCommand.Copy)
            {
                Clipboard.SetText(parameters.SelectionText);
                return true;
            }
			return false;
		}

		public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
		{
		}

		public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
		{
			return false;
		}

		#endregion
	}

	public class JsDialogHandler : IJsDialogHandler
	{
		#region Fields

		protected readonly string _viewName;

		#endregion

		#region Ctors

		public JsDialogHandler(string viewName)
		{
			_viewName = viewName;
		}

		#endregion

		#region Implementation: IJsDialogHandler

		public bool OnJSDialog(IWebBrowser browserControl, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
		{
			if (dialogType == CefJsDialogType.Alert)
			{
				suppressMessage = true;

				MessageBox.Show(messageText, _viewName);

				return true;
			}

			return false;
		}

		public bool OnJSBeforeUnload(IWebBrowser browserControl, IBrowser browser, string message, bool isReload, IJsDialogCallback callback)
		{
			return false;
		}

		public void OnResetDialogState(IWebBrowser browserControl, IBrowser browser)
		{
		}

		public void OnDialogClosed(IWebBrowser browserControl, IBrowser browser)
		{
		}

		#endregion
	}

	public class SelectFolderParam
	{
		#region Fields

		public string SelectedFolder { get; set; }

		#endregion
	}

	public class TableExclusion
	{
		#region Fields

		public string TableName { get; set; }
		public List<ColumnValueCondition> ColumnValueConditions { get; set; }

		#endregion

		#region Ctors

		public TableExclusion()
		{
			ColumnValueConditions = new List<ColumnValueCondition>();
		}

		#endregion
	}

	public class ColumnValueCondition
	{
		#region Fields

		public string ColumnName { get; set; }
		public ValueConditionOperator Operator { get; set; }
		public object Value { get; set; }

		#endregion
	}

	public enum ValueConditionOperator
	{
		#region Constants

		None,
		Equals,
		MoreThan,
		MoreOrEqual,
		LessThan,
		LessOrEqual,
		IsNull,
		IsNotNull

		#endregion
	}
}
