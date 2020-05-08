using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Helpers;

namespace RegScoreCalc.Code
{
	public class DocumentsFilter
	{
		#region Constants

		protected const string _recycledDocumentsTableName = "RecycledDocuments";
		protected const string _deletedColumnName = "ToBeDeleted";

		#endregion

		#region Operations

		public void Filter(BackgroundWorker worker, DataTable documentsTable, OleDbConnection inputConnection, string primaryKeyName, string groupByColumn, string filterByColumn, LeaveOneDocumentCriteria filterCriteria)
		{
			var primaryKeyColumnIndex = documentsTable.Columns.IndexOf(primaryKeyName);
			var groupByColumnIndex = documentsTable.Columns.IndexOf(groupByColumn);
			var filterByColumnIndex = documentsTable.Columns.IndexOf(filterByColumn);

			var documentsTableName = documentsTable.TableName;

			var grouping = documentsTable.Rows.Cast<DataRow>().GroupBy(x => x[groupByColumnIndex]).Select(x => new { Key = x.Key, Rows = x.ToList() }).ToList();

			var docsCount = documentsTable.Rows.Count;
			var processedDocuments = 0;

			var cmdCopy = CreateCopyCommand(inputConnection, documentsTableName, primaryKeyName, groupByColumn);
			var cmdUpdate = CreateUpdateCommand(inputConnection, documentsTableName, primaryKeyName, groupByColumn);

			///////////////////////////////////////////////////////////////////////////////

			var deleteColumn = false;

			try
			{
				if (!IsTableExist(inputConnection, _recycledDocumentsTableName))
					CreateRecycledDocumentsTable(inputConnection, documentsTableName, primaryKeyName, groupByColumn);

				if (!IsColumnExist(inputConnection, documentsTableName, _deletedColumnName))
					CreateColumn(inputConnection, documentsTableName, _deletedColumnName, "YESNO");

				deleteColumn = true;

				///////////////////////////////////////////////////////////////////////////////

				var filterByType = documentsTable.Columns[filterByColumnIndex].DataType;
				var isInt = filterByType == typeof(int);
				var isDouble = filterByType == typeof(double);
				var isDecimal = filterByType == typeof(decimal);

				var isAbsolute = (isInt || isDouble || isDecimal) && (filterCriteria == LeaveOneDocumentCriteria.AbsoluteMinimum || filterCriteria == LeaveOneDocumentCriteria.AbsoluteMaximum);

				foreach (var item in grouping)
				{
					var orderedRows = item.Rows.OrderBy(x =>
					{
						if (x.IsNull(filterByColumnIndex))
							return null;

						if (!isAbsolute)
							return x[filterByColumnIndex];

						if (isInt)
							return Math.Abs((int) x[filterByColumnIndex]);

						if (isDouble)
							return Math.Abs((double) x[filterByColumnIndex]);

						if (isDecimal)
							return Math.Abs((decimal) x[filterByColumnIndex]);

						return Math.Abs(Convert.ToInt32(x[filterByColumnIndex]));
					}).ToList();

					for (var rowIndex = 0; rowIndex < orderedRows.Count; rowIndex++)
					{
						var row = orderedRows[rowIndex];
						if (rowIndex == 0 && (filterCriteria == LeaveOneDocumentCriteria.Minimum || filterCriteria == LeaveOneDocumentCriteria.AbsoluteMinimum))
						{
							processedDocuments++;
							continue;
						}

						if (rowIndex == orderedRows.Count - 1 && (filterCriteria == LeaveOneDocumentCriteria.Maximum || filterCriteria == LeaveOneDocumentCriteria.AbsoluteMaximum))
						{
							processedDocuments++;
							continue;
						}

						///////////////////////////////////////////////////////////////////////////////

						var primaryKey = row[primaryKeyColumnIndex];

						cmdCopy.Parameters[0].Value = primaryKey;
						cmdCopy.ExecuteNonQuery();

						cmdUpdate.Parameters[0].Value = primaryKey;
						cmdUpdate.ExecuteNonQuery();

						///////////////////////////////////////////////////////////////////////////////

						if (processedDocuments % 20 == 0)
						{
							if (worker.CancellationPending)
								return;

							var progress = (int)((processedDocuments / (double)docsCount) * 100d);
							progress = (int) (progress * 0.95);

							worker.ReportProgress(progress);
						}

						processedDocuments++;
					}
				}

				DeleteDocuments(inputConnection, documentsTableName);
			}
			finally
			{
				if (deleteColumn)
					DeleteColumn(inputConnection, documentsTableName, _deletedColumnName);
			}
		}

		#endregion

		#region Implementation

		protected bool IsTableExist(OleDbConnection connection, string tableName)
		{
			var schema = connection.GetSchema("Tables");
			return schema.Rows.Cast<DataRow>().Any(x => String.Equals(x.Field<string>("TABLE_TYPE"), "TABLE", StringComparison.InvariantCultureIgnoreCase)
			                                            && String.Equals(x.Field<string>("TABLE_NAME"), tableName, StringComparison.InvariantCultureIgnoreCase));
		}

		protected bool IsColumnExist(OleDbConnection connection, string tableName, string columnName)
		{
			using (var cmd = new OleDbCommand($"SELECT TOP 1 * FROM {tableName}", connection))
			using (var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
			{
				var table = reader.GetSchemaTable();
				var nameCol = table.Columns["ColumnName"];

				if (table.Rows.Cast<DataRow>().Any(x => x[nameCol] as string == columnName))
					return true;

				return false;
			}
		}

		protected void CreateColumn(OleDbConnection connection, string tableName, string columnName, string columnType)
		{
			var commandText = $"ALTER TABLE [{tableName}] ADD COLUMN [{columnName}] {columnType}";

			var cmd = new OleDbCommand(commandText, connection);
			cmd.ExecuteNonQuery();
		}

		protected void DeleteColumn(OleDbConnection connection, string tableName, string columnName)
		{
			var commandText = $"ALTER TABLE [{tableName}] DROP COLUMN [{columnName}]";

			var cmd = new OleDbCommand(commandText, connection);
			cmd.ExecuteNonQuery();
		}

		protected void CreateRecycledDocumentsTable(OleDbConnection connection, string documentsTableName, string primaryKeyName, string groupByColumn)
		{
			var cmd = connection.CreateCommand();
			cmd.CommandText = $"SELECT TOP 1 [{primaryKeyName}], [{groupByColumn}] INTO [{_recycledDocumentsTableName}] FROM [{documentsTableName}]";
			cmd.ExecuteNonQuery();

			cmd.CommandText = $"DELETE * FROM [{_recycledDocumentsTableName}]";
			cmd.ExecuteNonQuery();
		}

		protected OleDbCommand CreateCopyCommand(OleDbConnection connection, string documentsTableName, string primaryKeyColumn, string groupByColumn)
		{
			var cmd = connection.CreateCommand();

			cmd.CommandText = $"INSERT INTO [{_recycledDocumentsTableName}] SELECT [{primaryKeyColumn}], [{groupByColumn}] FROM [{documentsTableName}] WHERE [{primaryKeyColumn}] = @param1";

			cmd.Parameters.AddWithValue("@param1", 0);

			return cmd;
		}

		protected OleDbCommand CreateUpdateCommand(OleDbConnection connection, string documentsTableName, string primaryKeyColumn, string primaryKey)
		{
			var cmd = connection.CreateCommand();

			cmd.CommandText = $"UPDATE [{documentsTableName}] SET [{_deletedColumnName}] = -1 WHERE [{primaryKeyColumn}] = @param1";

			cmd.Parameters.AddWithValue("@param1", 0);

			return cmd;
		}

		protected void DeleteDocuments(OleDbConnection connection, string documentsTableName)
		{
			var cmd = connection.CreateCommand();

			cmd.CommandText = $"DELETE FROM [{documentsTableName}] WHERE [{_deletedColumnName}] = -1";
			cmd.ExecuteNonQuery();
		}

		#endregion
	}

	public enum LeaveOneDocumentCriteria
	{
		#region Constants

		Minimum = 0,
		Maximum,
		AbsoluteMinimum,
		AbsoluteMaximum

		#endregion
	}
}
