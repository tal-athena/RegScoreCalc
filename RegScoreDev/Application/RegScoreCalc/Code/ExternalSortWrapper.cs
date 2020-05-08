using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Helpers;

namespace RegScoreCalc.Code
{
	public class ExternalSortWrapper
	{
		#region Constants

		protected const string _externalToolFileName = "ExternalSort.exe";
		protected const string _groupSizeColumnName = "GroupSize";

		#endregion

		#region Fields

		private readonly ViewsManager _views;
		private readonly string _databaseFilePath;
		private readonly ExternalSortOptions _sortOptions;
		private readonly string _groupByColumn;
		private readonly string _sortGroupsByColumn;
		private readonly SortOrder _sortGroupsByColumnDirection;
		private readonly bool _showElapsedTime;
		private readonly bool _showStartupMessage;

		protected Stopwatch _stopwatch;

		protected bool _closeConnection;

		protected string _filter;

		#endregion

		#region Ctors

		public ExternalSortWrapper(ViewsManager views, string databaseFilePath, ExternalSortOptions sortOptions, string groupByColumn, string sortGroupsByColumn, SortOrder sortGroupsByColumnDirection, bool showElapsedTime, bool showStartupMessage)
		{
			_views = views;
			_databaseFilePath = databaseFilePath;
			_sortOptions = sortOptions;
			_groupByColumn = groupByColumn;
			_sortGroupsByColumn = sortGroupsByColumn;
			_sortGroupsByColumnDirection = sortGroupsByColumnDirection;
			_showElapsedTime = showElapsedTime;
			_showStartupMessage = showStartupMessage;
		}

		#endregion

		#region Operations

		public bool Sort(out string filter)
		{
			var reportsProgress = _sortOptions.LeaveOneDocument;
			var title = reportsProgress ? "Recycling documents..." : "Sorting documents...";

			var formProgress = new FormGenericProgress(title, DoSort, null, reportsProgress);
			formProgress.ShowDialog(_views.MainForm);

			if (formProgress.Result && _stopwatch != null)
				MessageBox.Show($"Sorting done in {_stopwatch.Elapsed.TotalMilliseconds}", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

			filter = _filter;

			return formProgress.Result;
		}

		public static string FillGroupSize(ViewsManager views, string groupByColumnName, ExternalSortOptions sortOptions)
		{
			var filter = String.Empty;

			if (!String.IsNullOrEmpty(groupByColumnName) && sortOptions.ShowGroupsMinDocuments && sortOptions.MinDocuments > 0)
			{
				var groupSizeColumnIndex = views.MainForm.datasetMain.Documents.Columns.IndexOf(_groupSizeColumnName);
				if (groupSizeColumnIndex == -1)
				{
					views.MainForm.datasetMain.Documents.Columns.Add(_groupSizeColumnName);
					groupSizeColumnIndex = views.MainForm.datasetMain.Documents.Columns.IndexOf(_groupSizeColumnName);
				}

				///////////////////////////////////////////////////////////////////////////////

				var groupByColumn = views.MainForm.datasetMain.Documents.Columns[groupByColumnName];
				var groupByColumnIndex = groupByColumn.Ordinal;
				var defaultValue = groupByColumn.DataType == typeof(string) ? String.Empty : Activator.CreateInstance(groupByColumn.DataType);

				var expr = views.MainForm.datasetMain.Documents.Cast<DataRow>().GroupBy(x => x.IsNull(groupByColumnIndex) ? defaultValue : x.ItemArray[groupByColumnIndex]);
				foreach (var g in expr)
				{
					var count = g.Count();

					foreach (var row in g)
					{
						row[groupSizeColumnIndex] = count;
					}
				}

				views.MainForm.datasetMain.Documents.AcceptChanges();

				///////////////////////////////////////////////////////////////////////////////

				filter = $"{_groupSizeColumnName} >= {sortOptions.MinDocuments}";
			}

			return filter;
		}

		#endregion

		#region Implementation

		protected string SortOrderToSqlString(SortOrder order)
		{
			return (order == SortOrder.Ascending ? "ASC" : "DESC");
		}

		protected string GetOrderByString(List<SortByColumn> sortByColumns)
		{
			var orderByList = sortByColumns.Select(x => $"[{x.ColumnName}] {SortOrderToSqlString(x.SortOrder)}");

			return String.Join(", ", orderByList);
		}

		protected string SurroundWithQuotes(string argument)
		{
			return $"\"{argument}\"";
		}

		protected bool DoSort(BackgroundWorker worker, object objArgument)
		{
			try
			{
				_stopwatch = _showElapsedTime ? Stopwatch.StartNew() : null;

				///////////////////////////////////////////////////////////////////////////////

				if (!String.IsNullOrEmpty(_groupByColumn) && !String.IsNullOrEmpty(_sortGroupsByColumn) && _sortOptions.SortGroupsByCriteria != SortGroupsBy.None)
				{
					var mainToolFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
					var externalToolFilePath = Path.Combine(mainToolFolderPath, _externalToolFileName);

					var showMessage = _showStartupMessage ? "-m" : "-n";

					var outputFilePath = Path.GetTempFileName();

					var orderBy = GetOrderByString(_sortOptions.SortByColumns);
					var documentsTableName = _views.MainForm.adapterDocuments.TableName;

					var arguments = $"{SurroundWithQuotes(showMessage)} {SurroundWithQuotes(_databaseFilePath)} {SurroundWithQuotes(outputFilePath)} {SurroundWithQuotes(documentsTableName)} {SurroundWithQuotes(orderBy)}";

					///////////////////////////////////////////////////////////////////////////////

					arguments += $" {SurroundWithQuotes(_groupByColumn)} ";

					switch (_sortOptions.SortGroupsByCriteria)
					{
						case SortGroupsBy.Min:
							arguments += SurroundWithQuotes("-min");
							break;

						case SortGroupsBy.Max:
							arguments += SurroundWithQuotes("-max");
							break;

						case SortGroupsBy.Sum:
							arguments += SurroundWithQuotes("-sum");
							break;
					}

					arguments += " " + SurroundWithQuotes($"[{_sortGroupsByColumn}] {SortOrderToSqlString(_sortGroupsByColumnDirection)}");

					arguments += $" {_views.MainForm.DbPassword}";

					///////////////////////////////////////////////////////////////////////////////

					var process = new Process
					{
						StartInfo = new ProcessStartInfo
						{
							FileName = externalToolFilePath,
							WorkingDirectory = mainToolFolderPath,
							CreateNoWindow = true,
							UseShellExecute = false,
							WindowStyle = ProcessWindowStyle.Hidden,
							Arguments = arguments
						}
					};

					process.Start();

					worker.ReportProgress(-1);

					while (!process.HasExited)
					{
						Thread.Sleep(300);
					}

					if (process.ExitCode >= 0)
					{
						try
						{
							var outputConnection = new OleDbConnection(_views.MainForm.GetConnectionString(outputFilePath));

							if (_views.MainForm.sourceDocuments.Sort != null)
								_views.MainForm.sourceDocuments.Sort = null;

							var sortByColumns = new List<SortByColumn>(_sortOptions.SortByColumns)
							{
								new SortByColumn
								{
									ColumnName = "ED_ENC_NUM",
									SortOrder = SortOrder.Ascending
								}
							};

							sortByColumns.Insert(0, new SortByColumn
							{
								ColumnName = "SortOrder",
								SortOrder = SortOrder.Ascending
							});

							var orderByExpr = GetOrderByString(sortByColumns);

							try
							{
								OpenConnection(outputConnection);

								FillDocumentsTable(outputConnection, orderByExpr);
							}
							finally
							{
								CloseConnection(outputConnection);
							}

							_filter = FillGroupSize(_views, _groupByColumn, _sortOptions);

							_stopwatch?.Stop();

							return true;
						}
						finally
						{
							File.Delete(outputFilePath);
						}
					}
				}
				else
				{
					var sortByColumns = new List<SortByColumn>(_sortOptions.SortByColumns)
					{
						new SortByColumn
						{
							ColumnName = "ED_ENC_NUM",
							SortOrder = SortOrder.Ascending
						}
					};

					var orderByExpr = GetOrderByString(sortByColumns);

					if (!String.IsNullOrEmpty(_groupByColumn) && _sortOptions.LeaveOneDocument)
					{
						var connection = _views.MainForm.adapterDocuments.Connection;

						try
						{
							worker.ReportProgress(0);

							OpenConnection(connection);

							FilterAndFillDocuments(worker, connection, orderByExpr);
						}
						finally
						{
							CloseConnection(connection);
						}

						if (_views.MainForm.sourceDocuments.Sort != null)
							_views.MainForm.sourceDocuments.Sort = null;
					}
					else
					{
						_views.MainForm.sourceDocuments.Sort = GetOrderByString(sortByColumns);

						_filter = FillGroupSize(_views, _groupByColumn, _sortOptions);
					}

					///////////////////////////////////////////////////////////////////////////////

					_stopwatch?.Stop();

					return true;
				}
			}
			catch (Exception ex)
			{
				_stopwatch = null;

				MainForm.ShowExceptionMessage(ex);
			}

			return false;
		}

		protected void FilterAndFillDocuments(BackgroundWorker worker, OleDbConnection connection, string orderByExpr)
		{
			var filter = new DocumentsFilter();

			filter.Filter(worker, _views.MainForm.datasetMain.Documents, connection, _views.MainForm.adapterDocuments.PrimaryKeyColumnName, _groupByColumn, _sortOptions.LeaveOneDocumentColumn, _sortOptions.LeaveOneDocumentCriteria);

			FillDocumentsTable(connection, orderByExpr);
		}

		protected void FillDocumentsTable(OleDbConnection connection, string orderByColumns)
		{
			_views.MainForm.adapterDocuments.FillExternal(connection, _views.MainForm.adapterDocuments.Table.TableName, orderByColumns);
		}

		#endregion

		#region Helpers

		protected void OpenConnection(OleDbConnection connection)
		{
			_closeConnection = connection.State != ConnectionState.Open;

			if (_closeConnection)
				connection.Open();
		}

		protected void CloseConnection(OleDbConnection connection)
		{
			if (_closeConnection && connection.State == ConnectionState.Open)
				connection.Close();
		}

		#endregion
	}

	public class SortByColumn
	{
		#region Fields

		public string ColumnName { get; set; }
		public SortOrder SortOrder { get; set; }
		public bool GroupSimilar { get; set; }
		public bool AlternateColor { get; set; }

		#endregion
	}
}
