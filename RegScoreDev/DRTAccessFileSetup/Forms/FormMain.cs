using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using DRTAccessFileSetup.Code;
using Helpers;
using Microsoft.Win32;

namespace DRTAccessFileSetup.Forms
{
	public partial class FormMain : Form
	{
		#region Constants

		protected string _appSettingsRegKey = @"SOFTWARE\" + Program.AppName;

		protected const string _sqlFolderName = "SQL";
		protected const string _sqlFilePattern = "*.sql";
		protected const string _dbVersionKey = "DBVersion";

		#endregion

		#region Fields

		protected UpgradeOptions _options;
		protected string _dbPassword;

		#endregion

		#region Properties

		public string DatabaseFile { get; set; }

		#endregion

		#region Ctors

		public FormMain()
		{
			InitializeComponent();

			_options = new UpgradeOptions();
		}

		#endregion

		#region Events

		private void FormMain_Load(object sender, EventArgs e)
		{
			try
			{
				var selectDatabase = new FormSelectDatabase();
				if (selectDatabase.ShowDialog() == DialogResult.OK)
				{
					this.DatabaseFile = selectDatabase.SelectedFile;
					this.Text += " - " + Path.GetFileName(this.DatabaseFile);

					LoadAppSettings();

					var isGetVersionError = false;
					var isAlreadyUpgraded = false;

					_options.AppVersion = GetApplicationVersion();
					var dbVersion = GetDatabaseVersion();
					if (dbVersion != null)
					{
						_options.DbVersion = dbVersion.Value;

						lblAppVersion.Text = _options.AppVersion.ToString();
						lblDbVersion.Text = _options.DbVersion.ToString();

						if (_options.DbVersion >= _options.AppVersion)
							isAlreadyUpgraded = true;
						else
							LoadSqlFilesList();
					}
					else
						isGetVersionError = true;

					///////////////////////////////////////////////////////////////////////////////

					if (isGetVersionError || isAlreadyUpgraded)
					{
						groupQueries.Enabled = false;
						groupLog.Enabled = false;
					}

					if (isGetVersionError)
					{
						txtLog.ForeColor = Color.Red;
						txtLog.Text = "Cannot read database version";

						btnValidateDatabase.Enabled = false;
					}

					if (isAlreadyUpgraded)
					{
						txtLog.ForeColor = Color.Green;
						txtLog.Text = "Database version is already " + _options.AppVersion;
					}
				}
				else
					this.Close();
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (worker.IsBusy)
					e.Cancel = true;
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void btnUpgrade_Click(object sender, EventArgs e)
		{
			try
			{
				StartUpgrade();
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void btnOpenFile_Click(object sender, EventArgs e)
		{
			try
			{
				var entry = lbQueries.SelectedItem as SqlFileEntry;
				if (entry != null)
					Process.Start(entry.FullPath);
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void btnOpenFolder_Click(object sender, EventArgs e)
		{
			try
			{
				var entry = lbQueries.SelectedItem as SqlFileEntry;
				if (entry != null)
					Process.Start("explorer.exe", string.Format("/select, \"{0}\"", entry.FullPath));
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void btnClearLog_Click(object sender, EventArgs e)
		{
			try
			{
				txtLog.Clear();
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void btnValidateDatabase_Click(object sender, EventArgs e)
		{
			try
			{
				txtLog.Clear();

				this.Enabled = false;

				progress.Value = 0;
				progress.Visible = true;

				///////////////////////////////////////////////////////////////////////////////

				var connectionString = GetConnectionString(this.DatabaseFile);
				if (!String.IsNullOrEmpty(connectionString))
					validationWorker.RunWorkerAsync(connectionString);
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				var result = UpgradeDatabase();

				WriteLogLine(Environment.NewLine + "----------------------------------------------------", LogEntryType.Info);

				if (result)
					WriteLogLine("Upgrade completed successfully", LogEntryType.Success);
				else
					WriteLogLine("All queries executed successfully. Commit aborted by user", LogEntryType.Success);
			}
			catch (Exception ex)
			{
				WriteLogLine(ex.Message, LogEntryType.Error);
			}
		}

		private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			try
			{
				var logEntry = e.UserState as LogEntry;
				if (logEntry != null)
				{
					txtLog.DeselectAll();

					switch (logEntry.EntryType)
					{
						case LogEntryType.Info:
							txtLog.SelectionColor = Color.Black;
							break;

						case LogEntryType.Success:
							txtLog.SelectionColor = Color.Green;
							logEntry.Message = logEntry.Message + Environment.NewLine;
							break;

						case LogEntryType.Error:
							txtLog.SelectionColor = Color.Red;
							logEntry.Message = Environment.NewLine + logEntry.Message;
							break;
					}

					logEntry.Message += Environment.NewLine;

					txtLog.AppendText(logEntry.Message);

					txtLog.SelectionStart = txtLog.TextLength;
					txtLog.SelectionLength = 0;
					txtLog.ScrollToCaret();
				}
				else
					progress.Value = e.ProgressPercentage;
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				progress.Visible = false;
				this.Enabled = true;

				lblDbVersion.Text = _options.DbVersion.ToString();

				this.Activate();
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void validationWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				var schemaValidator = new DatabaseSchemaValidator();

				// schemaValidator.DumpSchemaXml(e.Argument as string, new List<string> { "DynamicColumnCategories", "DynamicColumns" });

				e.Result = schemaValidator.GetSchemaDifferences(e.Argument as string);
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void validationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				var differences = e.Result as List<string>;
				if (differences != null)
				{
					if (differences.Any())
						txtLog.Text = String.Join(Environment.NewLine, differences);
					else
						MessageBox.Show("No schema differences found", Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}

			try
			{
				this.Enabled = true;

				progress.Visible = false;
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		#endregion

		#region Implementation

		protected void LoadSqlFilesList()
		{
			lbQueries.Items.Clear();

			string sqlFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _sqlFolderName);
			if (Directory.Exists(sqlFolderPath))
			{
				var di = new DirectoryInfo(sqlFolderPath);
				var fi = di.GetFiles(_sqlFilePattern, SearchOption.TopDirectoryOnly);

				IEnumerable<string> list = fi.Select(x => x.FullName);

				FilterSqlFilesList(list);
			}
		}

		protected void FilterSqlFilesList(IEnumerable<string> filesList)
		{
			var entries = new List<SqlFileEntry>();

			foreach (var item in filesList)
			{
				var fileName = Path.GetFileNameWithoutExtension(item);
				if (!String.IsNullOrEmpty(fileName))
				{
					var split = fileName.Split(new [] { "_" }, StringSplitOptions.RemoveEmptyEntries);
					if (split.Length == 2)
					{
						string versionFrom = split.GetValue(0) as string;
						string versionTo = split.GetValue(1) as string;

						try
						{
							int from = Convert.ToInt32(versionFrom);
							int to = Convert.ToInt32(versionTo);

							if (to == from + 1)
							{
								if (from >= _options.DbVersion && from < _options.AppVersion)
								{
									var entry = new SqlFileEntry();
									entry.DisplayValue = versionFrom + " to" + versionTo;
									entry.FullPath = item;
									entry.FromVersion = from;
									entry.ToVersion = to;

									entries.Add(entry);
								}
							}
						}
						catch { }
					}
				}
			}

			if (entries.Any())
				entries.OrderBy(x => x.FromVersion).ToList().ForEach(x => lbQueries.Items.Add(x, true));
		}

		protected void StartUpgrade()
		{
			if (_options.DbVersion >= _options.AppVersion)
			{
				Program.ShowInfoMessage("Database is already upgraded");
				return;
			}

			if (lbQueries.CheckedItems.Count == 0)
			{
				Program.ShowInfoMessage("No queries to execute");
				return;
			}

			if (lbQueries.CheckedItems.Count < lbQueries.Items.Count)
			{
				var dlgres = MessageBox.Show("Some queries are not checked in execution list. This may produce incorred database structure." + Environment.NewLine + Environment.NewLine + "Do you wish to continue?" + Environment.NewLine,
						Program.AppName, MessageBoxButtons.YesNo,
						MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

				if (dlgres == DialogResult.No)
					return;
			}

			this.Enabled = false;

			progress.Value = 0;
			progress.Visible = true;

			_options.Queries = lbQueries.CheckedItems.OfType<SqlFileEntry>();
			_options.AutoCommit = chkbAutoCommit.Checked;

			txtLog.Clear();

			SaveAppSettings();

			worker.RunWorkerAsync();
		}

		protected bool UpgradeDatabase()
		{
			WriteLogLine("Upgrade started");

			double total = _options.Queries.Count() + 1;
			double done = 0;

			var connectionString = GetConnectionString(this.DatabaseFile);
			if (!String.IsNullOrEmpty(connectionString))
			{
				using (var connection = new OleDbConnection(connectionString))
				{
					connection.Open();

					WriteLogLine("Opened database");

					OleDbTransaction trans = connection.BeginTransaction();

					foreach (var item in _options.Queries)
					{
						WriteLogLine("Performing upgrade from version '" + item.DisplayValue + "'..." + Environment.NewLine);

						var sql = File.ReadAllText(item.FullPath);

						var split = sql.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
						foreach (var s in split)
						{
							var trimmed = s.Trim();

							if (!String.IsNullOrEmpty(trimmed.Trim()))
							{
								WriteLogLine("Running query..." + Environment.NewLine);

								var command = new OleDbCommand(trimmed, connection, trans);

								WriteCommandLogLine(command);

								command.ExecuteNonQuery();

								WriteLogLine("Succeeded", LogEntryType.Success);
							}
						}

						done++;
						ReportProgress(done, total);
					}

					///////////////////////////////////////////////////////////////////////////////

					FillGUIDs(trans);

					UpdateDatabaseVersion(trans);

					///////////////////////////////////////////////////////////////////////////////

					done++;
					ReportProgress(done, total);

					if (!_options.AutoCommit)
					{
						var dlgres = MessageBox.Show("All queries executed successfully. Do you wish to commit changes to database?" + Environment.NewLine,
							Program.AppName, MessageBoxButtons.YesNo,
							MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

						_options.AutoCommit = dlgres == DialogResult.Yes;
					}

					if (_options.AutoCommit)
					{
						WriteLogLine("Committing transaction...");

						trans.Commit();

						_options.DbVersion = _options.AppVersion;

						return true;
					}
				}
			}

			return false;
		}

		protected void UpdateDatabaseVersion(OleDbTransaction trans)
		{
			WriteLogLine("Updating database version info..." + Environment.NewLine);

			OleDbCommand command;

			if (_options.DbVersion == 0)
			{
				if (!IsDbSettingsTableExist(trans.Connection))
					CreateSettingsTable(trans);

				command = new OleDbCommand(Properties.Resources.InsertDbVersionSQL, trans.Connection, trans);
			}
			else
			{
				command = new OleDbCommand(Properties.Resources.UpdateDbVersionSQL, trans.Connection, trans);
			}

			command.Parameters.Add(new OleDbParameter("@Value", _options.AppVersion));
			command.Parameters.Add(new OleDbParameter("@Key", _dbVersionKey));

			WriteLogLine("Updating version info record...");
			WriteCommandLogLine(command);

			if (command.ExecuteNonQuery() != 1)
				throw new Exception("Failed to update database version");

			WriteLogLine("Database version info updated", LogEntryType.Success);
		}

		protected void CreateSettingsTable(OleDbTransaction trans)
		{
			WriteLogLine("Creating DBSettings table..." + Environment.NewLine);

			var command = new OleDbCommand(Properties.Resources.CreateSettingsTableSQL, trans.Connection, trans);
			WriteCommandLogLine(command);

			command.ExecuteNonQuery();

			WriteLogLine("Succeeded", LogEntryType.Success);
		}

		protected void WriteLogLine(string line)
		{
			WriteLogLine(line, LogEntryType.Info);
		}

		protected void WriteCommandLogLine(OleDbCommand command)
		{
			var message = String.Empty;

			var lines = command.CommandText.Split(new [] {Environment.NewLine}, StringSplitOptions.None);
			message = lines.Aggregate(message, (current, line) => current + ("\t" + line + Environment.NewLine));

			WriteLogLine(message, LogEntryType.Info);
		}

		protected void WriteLogLine(string line, LogEntryType entryType)
		{
			var entry = new LogEntry { Message = line, EntryType = entryType };

			worker.ReportProgress(-1, entry);
		}

		protected void ReportProgress(double done, double total)
		{
			int progress = (int)(done / total * 100);

			worker.ReportProgress(progress);
		}

		protected int GetApplicationVersion()
		{
			int version = 0;

			try
			{
				var attribute = Assembly.GetExecutingAssembly()
				.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
				.Cast<AssemblyDescriptionAttribute>()
				.FirstOrDefault();

				if (attribute != null)
				{
					var description = attribute.Description;
					version = Convert.ToInt32(description);
				}
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}

			return version;
		}

		protected int? GetDatabaseVersion()
		{
			int? version = null;

			string connectionString = GetConnectionString(this.DatabaseFile);
			if (!String.IsNullOrEmpty(connectionString))
			{
				using (var connection = new OleDbConnection(connectionString))
				{
					connection.Open();

					try
					{
						if (IsDbSettingsTableExist(connection))
						{
							var command = new OleDbCommand(Properties.Resources.SelectDbVersionSQL, connection);
							command.Parameters.Add(new OleDbParameter("@Key", _dbVersionKey));

							var value = command.ExecuteScalar() as string;
							if (value != null)
								version = Convert.ToInt32(value);
						}
					}
					catch { }
				}
			}

			return version;
		}

		protected bool IsDbSettingsTableExist(OleDbConnection connection)
		{
			var table = connection.GetSchema("Tables").Select("TABLE_NAME = 'DBSettings'");
			return table.Any();
		}

		protected void LoadAppSettings()
		{
			try
			{
				using (var key = Registry.CurrentUser.OpenSubKey(_appSettingsRegKey))
				{
					chkbAutoCommit.Checked = Convert.ToBoolean((int) key.GetValue("AutoCommit", 0));
				}
			}
			catch { }
		}

		protected void SaveAppSettings()
		{
			try
			{
				using (var key = Registry.CurrentUser.CreateSubKey(_appSettingsRegKey))
				{
					key.SetValue("AutoCommit", Convert.ToInt32(chkbAutoCommit.Checked));
				}
			}
			catch { }
		}

		protected void FillGUIDs(OleDbTransaction trans)
		{
			if (_options.DbVersion < 15)
			{
				FillGUIDs(trans, "RegExp");
				FillGUIDs(trans, "ColRegExp");
			}
		}

		protected void FillGUIDs(OleDbTransaction trans, string tableName)
		{
			var connection = trans.Connection;

			var cmdSelect = connection.CreateCommand();
			cmdSelect.Transaction = trans;
			cmdSelect.CommandText = String.Format("SELECT ID FROM {0}", tableName);

			var table = new DataTable();

			WriteCommandLogLine(cmdSelect);

			var adapter = new OleDbDataAdapter(cmdSelect);
			adapter.Fill(table);

			///////////////////////////////////////////////////////////////////////////////

			var cmdUpdate = connection.CreateCommand();
			cmdUpdate.Transaction = trans;
			cmdUpdate.CommandText = "UPDATE " + tableName + " SET [GUID] = @P1 WHERE [ID] = @P2";

			cmdUpdate.Parameters.AddWithValue("@P1", String.Empty);
			cmdUpdate.Parameters.AddWithValue("@P2", 0);

			///////////////////////////////////////////////////////////////////////////////

			foreach (var row in table.Rows.Cast<DataRow>())
			{
				var id = row[0];

				cmdUpdate.Parameters[0].Value = Guid.NewGuid().ToString();
				cmdUpdate.Parameters[1].Value = id;

				WriteCommandLogLine(cmdUpdate);

				var count = cmdUpdate.ExecuteNonQuery();
			}
		}

		#endregion

		#region Helpers

		protected string GetConnectionString(string databaseFilePath)
		{
			bool invalidPasword;
			var connectionString = ConnectionStringHelper.GetConnectionString(databaseFilePath, _dbPassword, out invalidPasword);

			while (String.IsNullOrEmpty(connectionString) && invalidPasword)
			{
				var formPassword = new FormDatabasePassword();
				var dlgres = formPassword.ShowDialog();
				if (dlgres != DialogResult.OK)
					break;

				connectionString = ConnectionStringHelper.GetConnectionString(databaseFilePath, formPassword.Password, out invalidPasword);
				if (!String.IsNullOrEmpty(connectionString))
					_dbPassword = formPassword.Password;
				else
					MessageBox.Show("Invalid password", null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			return connectionString;
		}

		#endregion
	}

	public class UpgradeOptions
	{
		#region Properties

		public int AppVersion { get; set; }
		public int DbVersion { get; set; }
		public IEnumerable<SqlFileEntry> Queries { get; set; }
		public bool AutoCommit { get; set; }

		#endregion
	}

	public class SqlFileEntry
	{
		#region Properties

		public string DisplayValue { get; set; }
		public string FullPath { get; set; }
		public int FromVersion { get; set; }
		public int ToVersion { get; set; }

		#endregion

		#region Overrides

		public override string ToString()
		{
			return this.DisplayValue;
		}

		#endregion
	}

	public class LogEntry
	{
		#region Properties

		public string Message { get; set; }
		public LogEntryType EntryType { get; set; }

		#endregion
	}

	public enum LogEntryType
	{
		#region Constants

		Info,
		Success,
		Error,
		SQL

		#endregion
	}
}
