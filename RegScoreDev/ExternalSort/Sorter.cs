using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Helpers;

namespace ExternalSort
{
	public class Sorter
	{
		#region Constants

		protected const string _externalSortColumnName = "SortOrder";

		#endregion

		#region Fields

		private readonly string _inputFilePath;
		private readonly string _outputFilePath;
		private readonly string _documentsTableName;
		private readonly string _orderBy;
		private readonly string _groupByColumn;
		private readonly string _sortGroupsByColumn;
		private readonly string _password;
		private readonly SortGroupsBy _sortGroupsByCriteria;

		#endregion

		#region Ctor

		public Sorter(string inputFilePath, string outputFilePath, string documentsTableName, string orderBy, string groupByColumn, SortGroupsBy sortGroupsByCriteria, string sortGroupsByColumn, string password)
		{
			_inputFilePath = inputFilePath;
			_outputFilePath = outputFilePath;
			_documentsTableName = documentsTableName;
			_orderBy = orderBy;
			_groupByColumn = groupByColumn;
			_sortGroupsByCriteria = sortGroupsByCriteria;
			_sortGroupsByColumn = sortGroupsByColumn;
			_password = password;
		}

		#endregion

		#region Operations

		public int Sort()
		{
			if (String.IsNullOrEmpty(_groupByColumn) || String.IsNullOrEmpty(_sortGroupsByColumn) || _sortGroupsByCriteria == SortGroupsBy.None)
				throw new Exception("Invalid arguments");

			try
			{
				OleDbConnection outputConnection = null;

				try
				{
					File.Copy(_inputFilePath, _outputFilePath, true);

					///////////////////////////////////////////////////////////////////////////////

					bool invalidPassword;
					outputConnection = new OleDbConnection(ConnectionStringHelper.GetConnectionString(_outputFilePath, _password, out invalidPassword));
					outputConnection.Open();

					DeleteTables(outputConnection);

					DeleteIndex(outputConnection, GetIndexName(outputConnection));

					InsertOrderColumn(outputConnection);

					var count = ApplyGroupOrderingSql(outputConnection);

					return count;
				}
				finally
				{
					CloseConnection(outputConnection);
				}
			}
			catch (Exception ex)
			{
				if (File.Exists(_outputFilePath))
					File.Delete(_outputFilePath);

				throw ex;
			}
		}

		#endregion

		#region Implementation: copying input data

		protected void DeleteTables(OleDbConnection connection)
		{
			var schema = connection.GetSchema("Tables");
			foreach (var row in schema.Rows.Cast<DataRow>().Where(x => x.Field<string>("TABLE_TYPE") == "TABLE"))
			{
				var tableName = row["TABLE_NAME"] as string;
				if (String.Equals(tableName, _documentsTableName, StringComparison.InvariantCultureIgnoreCase))
					continue;

				DeleteTable(connection, tableName);
			}
		}

		protected void DeleteTable(OleDbConnection connection, string tableName)
		{
			var command = connection.CreateCommand();
			command.CommandText = $"DROP TABLE [{tableName}]";

			command.ExecuteNonQuery();
		}

		protected void DeleteAllDocuments(OleDbConnection connection)
		{
			var command = connection.CreateCommand();
			command.CommandText = $"DELETE FROM [{_documentsTableName}]";

			command.ExecuteNonQuery();
		}

		protected void CloseConnection(OleDbConnection connection)
		{
			if (connection == null)
				return;

			if (connection.State == ConnectionState.Open)
				connection.Close();
		}

		protected string GetIndexName(OleDbConnection connection)
		{
			var table = connection.GetSchema("Indexes");
			var indexRow = table.Rows
				.Cast<DataRow>()
				.FirstOrDefault(x => x.Field<string>("TABLE_NAME") == "Documents" && x.Field<string>("COLUMN_NAME") == "ED_ENC_NUM");

			if (indexRow != null)
				return (string)indexRow["INDEX_NAME"];

			return string.Empty;
		}

		protected void DeleteIndex(OleDbConnection connection, string indexName)
		{
			if (!String.IsNullOrEmpty(indexName))
			{
				var query = $"DROP INDEX [{indexName}] ON [{_documentsTableName}]";

				var cmd = new OleDbCommand(query, connection);
				cmd.ExecuteNonQuery();
			}
		}

		#endregion

		#region Sorting

		protected int InsertOrderedRows(OleDbConnection connection)
		{
			var command = connection.CreateCommand();

			var queryColumnsListString = GetQueryColumnsString(GetColumnsList(connection));
			command.CommandText = $"INSERT INTO [{_documentsTableName}] SELECT {queryColumnsListString} FROM [MS Access;DATABASE={_inputFilePath};].[{_documentsTableName}] ORDER BY {_orderBy}";

			return command.ExecuteNonQuery();
		}

		protected string GetQueryColumnsString(List<ColumnInfo> columns)
		{
			return String.Join(", ", columns.Select(x => $"[{x.ColumnName}]"));
		}

		protected List<ColumnInfo> GetColumnsList(OleDbConnection connection)
		{
			using (var cmd = new OleDbCommand("SELECT TOP 1 * FROM " + _documentsTableName, connection))
			using (var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
			{
				var table = reader.GetSchemaTable();
				var nameCol = table.Columns["ColumnName"];
				var typeCol = table.Columns["DataType"];

				///////////////////////////////////////////////////////////////////////////////

				var actualColumns = table.Rows.Cast<DataRow>().Select(row => new ColumnInfo
				{
					ColumnName = row[nameCol.Ordinal].ToString(),
					ColumnType = ClrTypeFromTypeName(row[typeCol.Ordinal].ToString())
				}).Where(column => column.ColumnName != "NOTE_TEXT" && column.ColumnName != "DISC_TEXT").ToList();

				return actualColumns;
			}
		}

		#endregion

		#region Grouping

		protected int ApplyGroupOrdering(OleDbConnection outputConnection)
		{
			var command = outputConnection.CreateCommand();

			var sortGroupsByColumnInfo = GetSortColumnsInfo(_sortGroupsByColumn).Single();

			var queryColumnsListString = GetQueryColumnsString(new List<ColumnInfo>
			{
				new ColumnInfo
				{
					ColumnName = _groupByColumn
				},
				sortGroupsByColumnInfo
			});

			///////////////////////////////////////////////////////////////////////////////

			command.CommandText = $"SELECT {queryColumnsListString} FROM [{_documentsTableName}]";

			var table = new DataTable();
			var adapter = new OleDbDataAdapter(command);

			adapter.Fill(table);

			///////////////////////////////////////////////////////////////////////////////

			var groupRows = GroupAndOrderRows(table, sortGroupsByColumnInfo.Ascending);

			InsertOrderColumn(outputConnection);

			return UpdateRowGroups(outputConnection, groupRows);
		}

		protected List<object> GroupAndOrderRows(DataTable table, bool isAscending)
		{
			var inputRows = table.Rows.Cast<DataRow>();

			///////////////////////////////////////////////////////////////////////////////

			var groupByColumnIndex = 0;
			var sortGroupsByColumnIndex = 1;

			var query = inputRows.GroupBy(x => x.IsNull(groupByColumnIndex) ? null : x.ItemArray[groupByColumnIndex]);
			IOrderedEnumerable<IGrouping<object, DataRow>> sortedRows;

			Func<DataRow, object> func = (r) => r.IsNull(sortGroupsByColumnIndex) ? null : r.ItemArray[sortGroupsByColumnIndex];

			switch (_sortGroupsByCriteria)
			{
				case SortGroupsBy.Min:
					sortedRows = isAscending ? query.OrderBy(g => g.Min(func)) : query.OrderByDescending(g => g.Min(func));
					break;
				case SortGroupsBy.Max:
					sortedRows = isAscending ? query.OrderBy(g => g.Max(func)) : query.OrderByDescending(g => g.Max(func));
					break;
				case SortGroupsBy.Sum:
					sortedRows = isAscending ? query.OrderBy(g => g.Sum(r => Convert.ToInt32(func(r)))) : query.OrderByDescending(g => g.Sum(r => Convert.ToInt32(func(r))));
					break;

				case SortGroupsBy.None:
				default:
					throw new ArgumentOutOfRangeException();
			}

			var keys = sortedRows.Select(x => x.Key).ToList();

			return keys;
		}

		protected List<SortColumnInfo> GetSortColumnsInfo(string queryColumnsString)
		{
			var list = new List<SortColumnInfo>();

			var split = queryColumnsString.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
			foreach (var orderBy in split)
			{
				if (orderBy.IndexOf("[", StringComparison.InvariantCultureIgnoreCase) != 0)
					continue;

				var pos = orderBy.LastIndexOf("]", StringComparison.InvariantCultureIgnoreCase);
				if (pos == -1)
					continue;

				///////////////////////////////////////////////////////////////////////////////

				var columnName = orderBy.Substring(1, pos - 1);

				list.Add(new SortColumnInfo
				{
					ColumnName = columnName,
					Ascending = String.Equals(orderBy.Substring(pos + 1).Trim(), "ASC", StringComparison.InvariantCultureIgnoreCase)
				});
			}

			return list;
		}

		protected int UpdateRowGroups(OleDbConnection outputConnection, List<object> groups)
		{
			var command = outputConnection.CreateCommand();
			command.Parameters.AddWithValue("@param1", 0);
			command.Parameters.AddWithValue("@param2", null);

			command.CommandText = $"UPDATE [{_documentsTableName}] SET [{_externalSortColumnName}] = @param1 WHERE [{_groupByColumn}] = @param2";

			///////////////////////////////////////////////////////////////////////////////

			var count = 0;
			for (var index = 0; index < groups.Count; index++)
			{
				var key = groups[index];
				command.Parameters[0].Value = index;
				command.Parameters[1].Value = key ?? DBNull.Value;

				command.ExecuteNonQuery();

				count++;
			}

			return count;
		}

		protected void InsertOrderColumn(OleDbConnection connection)
		{
			var commandText = $"ALTER TABLE [{_documentsTableName}] ADD COLUMN [{_externalSortColumnName}] INTEGER";

			var cmd = new OleDbCommand(commandText, connection);
			cmd.ExecuteNonQuery();
		}

		#endregion

		#region Grouping: alternative

		protected int ApplyGroupOrderingSql(OleDbConnection outputConnection)
		{
			var command = outputConnection.CreateCommand();

			string aggregateFunc;
			switch (_sortGroupsByCriteria)
			{
				case SortGroupsBy.Min:
					aggregateFunc = "MIN";
					break;
				case SortGroupsBy.Max:
					aggregateFunc = "MAX";
					break;
				case SortGroupsBy.Sum:
					aggregateFunc = "SUM";
					break;

				case SortGroupsBy.None:
				default:
					throw new ArgumentOutOfRangeException();
			}

			var sortGroupsByColumnInfo = GetSortColumnsInfo(_sortGroupsByColumn).Single();

			var aggregateExpr = $"{aggregateFunc}([{sortGroupsByColumnInfo.ColumnName}])";
			var aggregateOrder = sortGroupsByColumnInfo.Ascending ? "ASC" : "DESC";

			command.CommandText = $"SELECT [{_groupByColumn}] as GroupedBy, {aggregateExpr} as AggregatedBy INTO TMP FROM [{_documentsTableName}] GROUP BY [{_groupByColumn}] ORDER BY {aggregateExpr} {aggregateOrder}";
			command.ExecuteNonQuery();

			command.CommandText = $"ALTER TABLE TMP ADD COLUMN [ID] AUTOINCREMENT PRIMARY KEY";
			command.ExecuteNonQuery();

			command.CommandText = $"UPDATE [{_documentsTableName}] INNER JOIN TMP ON [{_documentsTableName}].[{_groupByColumn}] = TMP.GroupedBy SET SortOrder = TMP.ID";
			var nonNullCount = command.ExecuteNonQuery();

			command.CommandText = $"SELECT TOP 1 [ID] FROM TMP WHERE GroupedBy IS NULL";
			var value = command.ExecuteScalar();

			command.CommandText = $"UPDATE [{_documentsTableName}] SET SortOrder = @param1 WHERE [{_groupByColumn}] IS NULL";
			command.Parameters.AddWithValue("@param1", value ?? DBNull.Value);

			var nullCount = command.ExecuteNonQuery();

			return nonNullCount + nullCount;
		}

		#endregion

		#region Helpers

		protected DbType DbTypeFromClrType(Type type)
		{
			if (type == typeof(int))
				return DbType.Int32;

			if (type == typeof(decimal))
				return DbType.Decimal;

			if (type == typeof(double))
				return DbType.Double;

			if (type == typeof(DateTime))
				return DbType.DateTime;

			if (type == typeof(string))
				return DbType.String;

			if (type == typeof(bool))
				return DbType.Boolean;

			throw new Exception("Unsupported type: " + type);
		}

		protected Type ClrTypeFromTypeName(string typeStr)
		{
			return Type.GetType(typeStr);
		}

		#endregion
	}

	[DebuggerDisplay("{ColumnName}, {ColumnType.Name}")]
	public class ColumnInfo
	{
		public string ColumnName { get; set; }
		public Type ColumnType { get; set; }
	}

	[DebuggerDisplay("{ColumnName}, {Ascending ? \"ASC\" : \"DESC\"}, {ColumnType.Name}")]
	public class SortColumnInfo : ColumnInfo
	{
		public bool Ascending { get; set; }
	}
}
