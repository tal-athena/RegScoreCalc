using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

using RegScoreCalc.Data;

namespace RegScoreCalc
{
	public class CustomOleDbDataAdapter : IDisposable
	{
		#region Fields

		protected OleDbConnection _connection;

		///////////////////////////////////////////////////////////////////////////////

		protected DataTable _table;
		protected string _tableName;

		protected ColumnInfo _primaryKey;

		protected string _sortColumn;
		protected ListSortDirection _sortDirection;

		///////////////////////////////////////////////////////////////////////////////

		protected List<ColumnInfo> _allColumnsList;

		protected List<DataColumn> _initialColumnsList;

        protected int _noteColumnCount;

		///////////////////////////////////////////////////////////////////////////////

		protected OleDbCommand _selectAllColumnsCommand;
		protected OleDbCommand _updateAllColumnsCommand;
		protected OleDbCommand _deleteCommand;

		#endregion

		#region Delegates

		public event EventHandler OnAfterDocumentsTableFilled;

		#endregion

		#region Properties

		public DataTable Table
		{
			get { return _table; }
		}

		public OleDbConnection Connection
		{
			get { return _connection; }
		}

		public bool IsSorted
		{
			get { return !String.IsNullOrEmpty(_sortColumn); }
		}

		public string SortColumn
		{
			get { return _sortColumn; }
		}

		public ListSortDirection SortDirection
		{
			get { return _sortDirection; }
		}

		public string TableName => this.Table?.TableName;

		public string PrimaryKeyColumnName => _primaryKey?.Name;

		#endregion

		#region Ctors

		public CustomOleDbDataAdapter()
		{
		}

		#endregion

		#region Operations

		public List<ColumnInfo> Initialize(OleDbConnection connection, DataTable table, string tableName, string primaryKeyName, List<ColumnInfo> dynamicColumns)
		{
			var closeConnection = connection.State != ConnectionState.Open;

			try
			{
				if (connection.State != ConnectionState.Open)
					connection.Open();

				///////////////////////////////////////////////////////////////////////////////

				_connection = connection;
				_table = table;
				_tableName = tableName;

				///////////////////////////////////////////////////////////////////////////////

				_allColumnsList = SqlGetActualColumnsList();

                ///////////////////////////////////////////////////////////////////////////////

                ///////////////////////////////////////////////////////////////////////////////

                _noteColumnCount = SqlGetNoteColumnCount();

                ///////////////////////////////////////////////////////////////////////////////

                var deletedColumns = AdjustListOfDynamicColumnsToActualColumnsList(dynamicColumns);

				AdjustDataTableColumnsToActualColumnsList(table);

				///////////////////////////////////////////////////////////////////////////////

				_primaryKey = GetPrimaryKey(table, primaryKeyName);

				///////////////////////////////////////////////////////////////////////////////

				ResetCommands();

				///////////////////////////////////////////////////////////////////////////////

				return deletedColumns;
			}
			finally
			{
				if (closeConnection && _connection.State == ConnectionState.Open)
					_connection.Close();
			}
		}

		public List<ColumnInfo> GetActualColumnsList()
		{
			if (_allColumnsList != null)
				return new List<ColumnInfo>(_allColumnsList.Select(x => x.Clone()));

			return Enumerable.Empty<ColumnInfo>().ToList();
		}

        public List<ColumnInfo> GetAllColumnsList()
        {
            return SqlGetAllColumnsList();
        }

        public int GetNoteColumnCount()
        {
            return _noteColumnCount;
        }

		public void SetColumnDynamic(string columnName, int dynamicColumnID)
		{
			var columnInfo = _allColumnsList.FirstOrDefault(x => x.Name == columnName);
			if (columnInfo != null)
			{
				columnInfo.IsExtra = false;
				columnInfo.IsDynamic = true;
				columnInfo.DynamicColumnID = dynamicColumnID;

				ResetCommands();
			}
		}

		public IEnumerable<ColumnInfo> GetDynamicColumnsCollection()
		{
			if (_allColumnsList != null)
				return _allColumnsList.Where(x => x.IsDynamic).Select(x => x.Clone());

			return Enumerable.Empty<ColumnInfo>();
		}

		public IEnumerable<ColumnInfo> GetExtraColumnsCollection()
		{
			if (_allColumnsList != null)
				return _allColumnsList.Where(x => x.IsExtra).Select(x => x.Clone());

			return Enumerable.Empty<ColumnInfo>();
		}

		public void StartBatchQuery()
		{
			if (_connection.State == ConnectionState.Closed)
				_connection.Open();
		}

		public void EndBatchQuery()
		{
			if (_connection.State == ConnectionState.Open)
				_connection.Close();
		}

		public void Fill()
		{
			var succeeded = false;

			try
			{
				BeforeSqlQuery();

				///////////////////////////////////////////////////////////////////////////////

				FillTable();

				succeeded = true;
			}
			finally
			{
				AfterSqlQuery();

				if (succeeded)
					OnAfterDocumentsTableFilled?.Invoke(this, EventArgs.Empty);
			}
		}

		public void FillExternal(OleDbConnection connection, string tableName, string orderByClause)
		{
			var command = PrepareExternalCommand_SelectAllColumns(connection, tableName, orderByClause);

			///////////////////////////////////////////////////////////////////////////////

			using (var reader = command.ExecuteReader())
			{
				_table.Clear();

				///////////////////////////////////////////////////////////////////////////////

				if (reader.HasRows)
					_table.Load(reader);
			}
		}

		public List<ColumnInfo> GetColumnsInfo(List<string> listOfColumns)
		{
			var allColumns = GetListOfColumnsToUpdate();
			var join = (from col in allColumns
						join updateCol in listOfColumns
							on col.Name equals updateCol
						select col).ToList();

			return join;
		}

		public OleDbCommand CreateUpdateColumnsCommand(List<ColumnInfo> listOfColumnsToUpdate)
		{
			var commandText = String.Format("UPDATE [{0}] SET ", _tableName);

			///////////////////////////////////////////////////////////////////////////////

			var allColumnsArray = listOfColumnsToUpdate.Select(x => String.Format("[{0}] = [@{0}]", x.Name))
													   .ToArray();

			var columns = String.Join(", ", allColumnsArray);

			///////////////////////////////////////////////////////////////////////////////

			commandText += String.Format("{0} WHERE `{1}` = @{1}", columns, _primaryKey.Name);

			///////////////////////////////////////////////////////////////////////////////

			var updateColumnsCommand = new OleDbCommand(commandText, _connection);

			///////////////////////////////////////////////////////////////////////////////

			AddCommandParameters(updateColumnsCommand, listOfColumnsToUpdate);

			updateColumnsCommand.Parameters.AddWithValue(_primaryKey.Name, GetDefaultValue(_primaryKey.Type));

			return updateColumnsCommand;
		}

		public bool Update(DataRow row, List<ColumnInfo> listOfColumnsToUpdate, OleDbCommand command)
		{
			try
			{
				BeforeSqlQuery();

				///////////////////////////////////////////////////////////////////////////////

				FillCommandParameters_Update(listOfColumnsToUpdate, command.Parameters, row);

				///////////////////////////////////////////////////////////////////////////////

				var count = command.ExecuteNonQuery();
				if (count != 1)
					throw new Exception(String.Format("Command updated {0} of expected 1 rows", count));

				///////////////////////////////////////////////////////////////////////////////

				row.AcceptChanges();
			}
			finally
			{
				AfterSqlQuery();
			}

			///////////////////////////////////////////////////////////////////////////////

			return true;
		}

		public bool Update(DataRow row)
		{
			try
			{
				BeforeSqlQuery();

				///////////////////////////////////////////////////////////////////////////////

				PrepareCommand_UpdateAllColumns();

				///////////////////////////////////////////////////////////////////////////////

				FillCommandParameters_Update(GetListOfColumnsToUpdate(), _updateAllColumnsCommand.Parameters, row);

				///////////////////////////////////////////////////////////////////////////////

				var count = _updateAllColumnsCommand.ExecuteNonQuery();
				if (count != 1)
					throw new Exception(String.Format("Command updated {0} of expected 1 rows", count));

				///////////////////////////////////////////////////////////////////////////////

				row.AcceptChanges();
			}
			finally
			{
				AfterSqlQuery();
			}

			///////////////////////////////////////////////////////////////////////////////

			return true;
		}

		public int Update()
		{
			try
			{
				BeforeSqlQuery();

				///////////////////////////////////////////////////////////////////////////////

				PrepareCommand_UpdateAllColumns();

				///////////////////////////////////////////////////////////////////////////////

				var totalUpdatedCount = 0;

				var listOfColumnsToUpdate = GetListOfColumnsToUpdate();

				var modifiedRows = GetModifiedRows();
				foreach (var row in modifiedRows)
				{
					FillCommandParameters_Update(listOfColumnsToUpdate, _updateAllColumnsCommand.Parameters, row);

					///////////////////////////////////////////////////////////////////////////////

					var count = _updateAllColumnsCommand.ExecuteNonQuery();
					if (count != 1)
						throw new Exception(String.Format("Command updated {0} of expected 1 rows", count));

					///////////////////////////////////////////////////////////////////////////////

					row.AcceptChanges();

					totalUpdatedCount++;
				}

				///////////////////////////////////////////////////////////////////////////////

				return totalUpdatedCount;
			}
			finally
			{
				AfterSqlQuery();
			}
		}

		public bool Delete(double id)
		{
			try
			{
				BeforeSqlQuery();

				///////////////////////////////////////////////////////////////////////////////

				PrepareCommand_Delete();

				///////////////////////////////////////////////////////////////////////////////

				_deleteCommand.Parameters[0].Value = id;

				///////////////////////////////////////////////////////////////////////////////

				var count = _deleteCommand.ExecuteNonQuery();
				if (count == 1)
					return true;
			}
			finally
			{
				AfterSqlQuery();
			}

			return false;
		}

		public ColumnInfo AddDynamicColumn(string columnName, DynamicColumnType dynamicColumnType, bool updateDatabase)
		{
			return SqlAddColumn(columnName, DynamicColumnTypeToAccessType(dynamicColumnType), true, updateDatabase);
		}

		public ColumnInfo AddExtraColumn(string columnName, string accessType, bool updateDatabase)
		{
			return SqlAddColumn(columnName, accessType, false, updateDatabase);
		}

		public ColumnInfo RenameDynamicColumn(string oldColumnName, string newColumnName, int dynamicColumnID, bool updateDatabase)
		{
			ColumnInfo result = null;

			if (oldColumnName == newColumnName)
				throw new Exception("New column name is same as existing");

			///////////////////////////////////////////////////////////////////////////////

			var dynamicColumn = _allColumnsList.FirstOrDefault(x => x.IsDynamic && String.Equals(x.Name, oldColumnName, StringComparison.InvariantCulture));
			if (dynamicColumn == null)
				throw new Exception("Column not found");

			///////////////////////////////////////////////////////////////////////////////

			if (updateDatabase)
			{
				try
				{
					BeforeSqlQuery();

					///////////////////////////////////////////////////////////////////////////////

					result = SqlAddColumn(newColumnName, DynamicColumnTypeToAccessType(ClrTypeToDynamicColumnType(dynamicColumn.Type)), true, true);
					result.DynamicColumnID = dynamicColumnID;

					SqlCopyColumnValues(oldColumnName, newColumnName);

					DeleteColumn(oldColumnName, true);
				}
				finally
				{
					AfterSqlQuery();
				}
			}
			else
			{
				result = SqlAddColumn(newColumnName, DynamicColumnTypeToAccessType(ClrTypeToDynamicColumnType(dynamicColumn.Type)), true, false);
				result.DynamicColumnID = dynamicColumnID;

				DeleteColumn(oldColumnName, false);

				ResetCommands();
			}

			return result;
		}

		public ColumnInfo RenameExtraColumn(string oldColumnName, string newColumnName, bool updateDatabase)
		{
			ColumnInfo result = null;

			if (oldColumnName == newColumnName)
				throw new Exception("New column name is same as existing");

			///////////////////////////////////////////////////////////////////////////////

			var columnInfo = GetExtraColumnsCollection().Single(x => String.Compare(x.Name, oldColumnName, StringComparison.InvariantCultureIgnoreCase) == 0);

			if (updateDatabase)
			{
				try
				{
					BeforeSqlQuery();

					///////////////////////////////////////////////////////////////////////////////

					result = SqlAddColumn(newColumnName, ClrTypeToAccessType(columnInfo.Type), false, true);

					SqlCopyColumnValues(oldColumnName, newColumnName);

					DeleteColumn(oldColumnName, true);
				}
				finally
				{
					AfterSqlQuery();
				}
			}
			else
			{
				result = SqlAddColumn(newColumnName, ClrTypeToAccessType(columnInfo.Type), false, false);
				DeleteColumn(oldColumnName, false);

				ResetCommands();
			}

			return result;
		}

		public void DeleteColumn(string columnName, bool updateDatabase)
		{
			if (updateDatabase)
			{
				try
				{
					BeforeSqlQuery();

					///////////////////////////////////////////////////////////////////////////////

					SqlDropColumn(columnName);

					ResetCommands();
				}
				finally
				{
					AfterSqlQuery();
				}
			}
			else
				ResetCommands();

			///////////////////////////////////////////////////////////////////////////////

			_table.Columns.Remove(columnName);

			///////////////////////////////////////////////////////////////////////////////

			var index = _allColumnsList.FindIndex(x => String.Equals(x.Name, columnName, StringComparison.InvariantCultureIgnoreCase));
			if (index != -1)
				_allColumnsList.RemoveAt(index);

			///////////////////////////////////////////////////////////////////////////////

			AdjustSqlColumnIndices();
		}

		public int SqlSetColumnValueByPrimaryKey(string columnName, object value, object whereValue)
		{
			return SqlSetColumnValueByColumn(columnName, value, null, whereValue, ExpressionType.Equal);
		}

		public int SqlSetColumnValueByColumn(string columnName, object value, string whereColumn, object whereValue, ExpressionType whereCondition)
		{
			try
			{
				BeforeSqlQuery();

				///////////////////////////////////////////////////////////////////////////////

				return SqlSetColumnValue(columnName, value, whereColumn, whereValue, whereCondition);
			}
			finally
			{
				AfterSqlQuery();
			}
		}

		public int SqlClearColumnValues(string columnName, string whereColumn, object whereValue, ExpressionType whereCondition)
		{
			return SqlSetColumnValueByColumn(columnName, DBNull.Value, whereColumn, whereValue, whereCondition);
		}

		public void SyncSort(string columnName, ListSortDirection sortDirection, BackgroundWorker worker)
		{
			worker.ReportProgress(0);

			var sortString = columnName + " " + (sortDirection == ListSortDirection.Ascending ? "ASC" : "DESC");

			var source = new BindingSource(_table, "")
			{
				RaiseListChangedEvents = false,
				Sort = sortString
			};

			///////////////////////////////////////////////////////////////////////////////

			var rows = _table.Rows;

			var sortedRows = source.Cast<DataRowView>()
								   .Select(x => x.Row.ItemArray)
								   .ToList();

			///////////////////////////////////////////////////////////////////////////////

			var totalDocuments = rows.Count;
			var processedDocuments = 0;

			rows.Clear();
			foreach (var itemArray in sortedRows)
			{
				var row = _table.Rows.Add();
				row.ItemArray = itemArray;

				processedDocuments++;
				if (processedDocuments % 50 == 0)
				{
					var progress = (int) (((double) processedDocuments / totalDocuments) * 100);
					worker.ReportProgress(progress);
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			_table.AcceptChanges();

			///////////////////////////////////////////////////////////////////////////////

			_sortColumn = columnName;
			_sortDirection = sortDirection;
		}

		public void Sort(ListSortDirection sortDirection, BindingSource bindingSource, double[] sortedDocuments, BackgroundWorker worker)
		{
			bindingSource.RaiseListChangedEvents = false;

			try
			{
				var totalDocuments = _table.Rows.Count;
				var processedDocuments = 0;
				worker.ReportProgress(0);

				var sortColumnName = "internal_sort_column";
				var sortColumnIndex = _table.Columns.IndexOf(sortColumnName);
				if (sortColumnIndex == -1)
				{
					var column = _table.Columns.Add(sortColumnName, typeof(int));
					column.AllowDBNull = true;
					sortColumnIndex = column.Ordinal;
				}

				///////////////////////////////////////////////////////////////////////////////

				var index = 0;
				var sortedDocsDicdict = sortedDocuments.Select(x => new { Index = index++, DocumentID = x });

				var join = from docRow in _table.Rows.Cast<DataRow>()
						   join docSorted in sortedDocsDicdict
							   on docRow.Field<double>("ED_ENC_NUM") equals docSorted.DocumentID
							   into tmp
						   from t in tmp.DefaultIfEmpty()
						   select new { docRow, index = t != null ? (int?)t.Index : 0 };

				join = join.OrderBy(x => x.index);

				index = 1;
				foreach (var j in join)
				{
					if (j.index > 0)
					{
						j.docRow.SetField(sortColumnIndex, index);
						index++;
					}
					else
						j.docRow.SetField(sortColumnIndex, Int32.MaxValue);

					processedDocuments++;
					if (processedDocuments % 50 == 0)
					{
						var progress = (int)(((double)processedDocuments / totalDocuments) * 100);
						worker.ReportProgress(progress);
					}
				}

				_table.AcceptChanges();

				///////////////////////////////////////////////////////////////////////////////

				bindingSource.Sort = sortColumnName + " " + (sortDirection == ListSortDirection.Ascending ? "ASC" : "DESC");
			}
			finally
			{
				bindingSource.RaiseListChangedEvents = true;
				bindingSource.ResetBindings(false);
			}
		}

		public bool SqlIsColumnExist(string columnName)
		{
			using (var cmd = new OleDbCommand("SELECT TOP 1 * FROM " + _tableName, _connection))
			using (var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
			{
				var table = reader.GetSchemaTable();
				var nameCol = table.Columns["ColumnName"];

				if (table.Rows.Cast<DataRow>().Any(x => x[nameCol] as string == columnName))
					return true;

				return false;
			}
		}

		#endregion

		#region Implementation: general

		protected void ResetCommands()
		{
			_selectAllColumnsCommand = null;
			_updateAllColumnsCommand = null;
		}

		protected void BeforeSqlQuery()
		{
			if (_connection.State == ConnectionState.Closed)
				_connection.Open();
		}

		protected void AfterSqlQuery()
		{
			
		}

		protected DataRow[] GetModifiedRows()
		{
			return _table.Select(null, null, DataViewRowState.ModifiedCurrent);
		}

		protected ColumnInfo GetPrimaryKey(DataTable table, string primaryKeyName)
		{
			var primaryKey = _allColumnsList.FirstOrDefault(x => x.Name == primaryKeyName);
			if (primaryKey == null)
				throw new DataException(String.Format("No primary key found in table {0}", _tableName));

			///////////////////////////////////////////////////////////////////////////////

			return primaryKey;
		}

		protected void AdjustSqlColumnIndices()
		{
			for (var i = 0; i < _allColumnsList.Count; i++)
			{
				var columnInfo = _allColumnsList[i];
				columnInfo.SqlColumnIndex = i;
			}
		}

		#endregion

		#region Implementation: SQL operations

		protected void FillTable()
		{
			PrepareCommand_SelectAllColumns();

			///////////////////////////////////////////////////////////////////////////////

			using (var reader = _selectAllColumnsCommand.ExecuteReader())
			{
				_table.Clear();

				///////////////////////////////////////////////////////////////////////////////

				if (reader.HasRows)
					_table.Load(reader);
			}
		}
        protected int SqlGetNoteColumnCount()
        {
            int count = 0;

            using (var cmd = new OleDbCommand("SELECT TOP 1 * FROM " + _tableName, _connection))
            using (var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
            {
                var table = reader.GetSchemaTable();
                var nameCol = table.Columns["ColumnName"];
                var typeCol = table.Columns["DataType"];

                ///////////////////////////////////////////////////////////////////////////////

                foreach (var row in table.Rows.Cast<DataRow>())
                {
                    var columnName = row[nameCol].ToString();
                    if (columnName.StartsWith("NOTE_TEXT"))
                    {
                        count ++;
                        continue;
                    }                    
                }
            }
            return count;
        }

        protected List<ColumnInfo> SqlGetActualColumnsList()
		{
			var actualColumns = new List<ColumnInfo>();

			///////////////////////////////////////////////////////////////////////////////

			using (var cmd = new OleDbCommand("SELECT TOP 1 * FROM " + _tableName, _connection))
			using (var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
			{
				var table = reader.GetSchemaTable();
				var nameCol = table.Columns["ColumnName"];
				var typeCol = table.Columns["DataType"];

				///////////////////////////////////////////////////////////////////////////////

				var i = 0;

				foreach (var row in table.Rows.Cast<DataRow>())
				{
					var columnName = row[nameCol].ToString();
					if (columnName.StartsWith("NOTE_TEXT") || columnName.StartsWith("NOTE_ENTITIES"))
					{
						i++;
						continue;
					}

					actualColumns.Add(new ColumnInfo
					{
						Name = columnName,
						Type = (Type)row[typeCol],
						SqlColumnIndex = i
					});

					///////////////////////////////////////////////////////////////////////////////

					i++;
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			return actualColumns;
		}

        protected List<ColumnInfo> SqlGetAllColumnsList()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            var actualColumns = new List<ColumnInfo>();

            ///////////////////////////////////////////////////////////////////////////////

            using (var cmd = new OleDbCommand("SELECT TOP 1 * FROM " + _tableName, _connection))
            using (var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
            {
                var table = reader.GetSchemaTable();
                var nameCol = table.Columns["ColumnName"];
                var typeCol = table.Columns["DataType"];

                ///////////////////////////////////////////////////////////////////////////////

                var i = 0;

                foreach (var row in table.Rows.Cast<DataRow>())
                {
                    var columnName = row[nameCol].ToString();
                    
                    actualColumns.Add(new ColumnInfo
                    {
                        Name = columnName,
                        Type = (Type)row[typeCol],
                        SqlColumnIndex = i
                    });

                    ///////////////////////////////////////////////////////////////////////////////

                    i++;
                }
            }

            ///////////////////////////////////////////////////////////////////////////////

            return actualColumns;
        }

        protected ColumnInfo SqlAddColumn(string columnName, string accessType, bool isDynamic, bool updateDatabase)
		{
			ColumnInfo result = null;

			if (updateDatabase)
			{
				try
				{
					BeforeSqlQuery();

					///////////////////////////////////////////////////////////////////////////////

					try
					{
						var commandText = String.Format("ALTER TABLE [{0}] ADD COLUMN [{1}] {2}", _tableName, columnName, accessType);

						var cmd = new OleDbCommand(commandText, _connection);
						cmd.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						var hresult = -2147467259;

						if (ex.HResult == hresult)
							throw new Exception("Too many columns defined. To continue adding new columns please open this database in Access and press 'Compact and Repair'");

						throw;
					}

					///////////////////////////////////////////////////////////////////////////////

					ResetCommands();
				}
				finally
				{
					AfterSqlQuery();
				}
			}
			else
				ResetCommands();

			///////////////////////////////////////////////////////////////////////////////

			var clrType = AccessTypeToClrType(accessType);
			_table.Columns.Add(columnName, clrType);

			///////////////////////////////////////////////////////////////////////////////

			result = new ColumnInfo
			{
				Name = columnName,
				Type = clrType,
				IsDynamic = isDynamic,
				IsExtra = !isDynamic
			};

			_allColumnsList.Add(result);

			///////////////////////////////////////////////////////////////////////////////

			AdjustSqlColumnIndices();

			///////////////////////////////////////////////////////////////////////////////

			return result;
		}

		protected void SqlDropColumn(string columnName)
		{
			var commandText = String.Format("ALTER TABLE [{0}] DROP COLUMN [{1}]", _tableName, columnName);

			var cmd = new OleDbCommand(commandText, _connection);
			cmd.ExecuteNonQuery();
		}

		public int SqlCopyColumnValues(string oldColumnName, string newColumnName)
		{
			var commandText = String.Format("UPDATE [{0}] SET [{1}] = [{2}]", _tableName, newColumnName, oldColumnName);
			var cmd = new OleDbCommand(commandText, _connection);

			return cmd.ExecuteNonQuery();
		}

		protected int SqlSetColumnValue(string columnName, object value, string whereColumn, object whereValue, ExpressionType expressionType = ExpressionType.Equal)
		{
			var cmd = BuildSqlUpdateCommand(columnName, value, whereColumn, whereValue, expressionType);

			return cmd.ExecuteNonQuery();
		}

		protected OleDbCommand BuildSqlUpdateCommand(string columnName, object value, string whereColumn, object whereValue, ExpressionType expressionType)
		{
			var commandText = String.Format("UPDATE [{0}] SET [{1}] = [@{1}]", _tableName, columnName);

			///////////////////////////////////////////////////////////////////////////////

			if (whereValue != null)
			{
				if (String.IsNullOrEmpty(whereColumn))
					whereColumn = _primaryKey.Name;

				///////////////////////////////////////////////////////////////////////////////

				if (expressionType == ExpressionType.Equal)
					commandText += String.Format(" WHERE [{0}] = [@{0}_]", whereColumn);
				else if (expressionType == ExpressionType.LessThan)
					commandText += String.Format(" WHERE [{0}] < [@{0}_]", whereColumn);
				else if (expressionType == ExpressionType.GreaterThan)
					commandText += String.Format(" WHERE [{0}] > [@{0}_]", whereColumn);
				else
					throw new ArgumentException();
			}

			///////////////////////////////////////////////////////////////////////////////

			var cmd = new OleDbCommand(commandText, _connection);

			///////////////////////////////////////////////////////////////////////////////

			if (value == null)
				value = DBNull.Value;

			cmd.Parameters.AddWithValue("@" + columnName, value);

			if (whereValue != null)
				cmd.Parameters.AddWithValue("@" + whereColumn + "_", whereValue);

			///////////////////////////////////////////////////////////////////////////////

			return cmd;
		}

		protected void PrepareCommand_SelectAllColumns()
		{
			if (_selectAllColumnsCommand != null)
				return;

			///////////////////////////////////////////////////////////////////////////////

			var commandText = "SELECT [";

			///////////////////////////////////////////////////////////////////////////////

			var columns = String.Join("], [", this.GetListOfColumnsToSelect()
												  .Select(x => x.Name)
												  .ToArray());

			///////////////////////////////////////////////////////////////////////////////

			commandText += String.Format("{0}] FROM [{1}]", columns, _tableName);

			///////////////////////////////////////////////////////////////////////////////

			_selectAllColumnsCommand = new OleDbCommand(commandText, _connection);
		}

		protected void PrepareCommand_UpdateAllColumns()
		{
			if (_updateAllColumnsCommand != null)
				return;

			///////////////////////////////////////////////////////////////////////////////

			var commandText = String.Format("UPDATE [{0}] SET ", _tableName);

			///////////////////////////////////////////////////////////////////////////////

			var listOfColumnsToUpdate = this.GetListOfColumnsToUpdate();

			var allColumnsArray = listOfColumnsToUpdate.Select(x => String.Format("[{0}] = [@{0}]", x.Name))
													   .ToArray();

			var columns = String.Join(", ", allColumnsArray);

			///////////////////////////////////////////////////////////////////////////////

			commandText += String.Format("{0} WHERE `{1}` = @{1}", columns, _primaryKey.Name);

			///////////////////////////////////////////////////////////////////////////////

			_updateAllColumnsCommand = new OleDbCommand(commandText, _connection);

			///////////////////////////////////////////////////////////////////////////////

			AddCommandParameters(_updateAllColumnsCommand, listOfColumnsToUpdate);

			_updateAllColumnsCommand.Parameters.AddWithValue(_primaryKey.Name, GetDefaultValue(_primaryKey.Type));
		}

		protected void PrepareCommand_Delete()
		{
			if (_deleteCommand != null)
				return;

			///////////////////////////////////////////////////////////////////////////////

			var commandText = String.Format("DELETE FROM [{0}] WHERE ED_ENC_NUM = @{1}", _tableName, _primaryKey.Name);

			///////////////////////////////////////////////////////////////////////////////

			_deleteCommand = new OleDbCommand(commandText, _connection);

			_deleteCommand.Parameters.AddWithValue(_primaryKey.Name, GetDefaultValue(_primaryKey.Type));
		}

		protected void AddCommandParameters(OleDbCommand command, List<ColumnInfo> columns)
		{
			foreach (var col in columns)
			{
				command.Parameters.AddWithValue(col.Name, GetDefaultValue(col.Type));
			}
		}

		protected void FillCommandParameters_Update(List<ColumnInfo> listOfColumnsToUpdate, OleDbParameterCollection parameterCollection, DataRow row)
		{
			var i = 0;

			foreach (var col in listOfColumnsToUpdate)
			{
				parameterCollection[i].Value = row[col.Name];

				i++;
			}

			///////////////////////////////////////////////////////////////////////////////

			parameterCollection[i].Value = row[_primaryKey.Name];
		}

		protected List<ColumnInfo> GetListOfColumnsToSelect()
		{
			return _allColumnsList.ToList();
		}

		protected List<ColumnInfo> GetListOfColumnsToUpdate()
		{
			return _allColumnsList.Where(x => x.Name != _primaryKey.Name)
								  .ToList();
		}

		#endregion

		#region External data

		protected OleDbCommand PrepareExternalCommand_SelectAllColumns(OleDbConnection connection, string tableName, string orderByColumns)
		{
			var commandText = "SELECT [";

			///////////////////////////////////////////////////////////////////////////////

			var columns = String.Join("], [", this.GetListOfColumnsToSelect()
				.Select(x => x.Name)
				.ToArray());

			///////////////////////////////////////////////////////////////////////////////

			commandText += $"{columns}] FROM [{tableName}]";

			if (!String.IsNullOrEmpty(orderByColumns))
				commandText += $" ORDER BY {orderByColumns}";

			///////////////////////////////////////////////////////////////////////////////

			return new OleDbCommand(commandText, connection);
		}

		#endregion

		#region Implementation: reflection

		protected List<ColumnInfo> AdjustListOfDynamicColumnsToActualColumnsList(List<ColumnInfo> dynamicColumnsList)
		{
			var deletedColumnsList = new List<ColumnInfo>();

			///////////////////////////////////////////////////////////////////////////////

			for (var i = 0; i < dynamicColumnsList.Count; i++)
			{
				var dynamicColumn = dynamicColumnsList[i];
				var actualColumn = _allColumnsList.FirstOrDefault(x => String.Equals(x.Name, dynamicColumn.Name, StringComparison.InvariantCultureIgnoreCase));
				if (actualColumn != null)
				{
					actualColumn.IsDynamic = true;
					actualColumn.DynamicColumnID = dynamicColumn.DynamicColumnID;
				}
				else
				{
					deletedColumnsList.Add(dynamicColumn);

					dynamicColumnsList.RemoveAt(i);
					i--;
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			return deletedColumnsList;
		}

		protected void AdjustDataTableColumnsToActualColumnsList(DataTable table)
		{
			if (_initialColumnsList == null)
			{


				_initialColumnsList = table.Columns.Cast<DataColumn>()
											.ToList();
			}
            
            ///////////////////////////////////////////////////////////////////////////////

            table.PrimaryKey = null;
			table.Columns.Clear();

			var tableType = _table.GetType();

			///////////////////////////////////////////////////////////////////////////////

			for (var i = 0; i < _allColumnsList.Count; i++)
			{
				var actualColumn = _allColumnsList[i];

				///////////////////////////////////////////////////////////////////////////////

				FieldInfo privateField = null;

				///////////////////////////////////////////////////////////////////////////////

				var dataColumn = _initialColumnsList.FirstOrDefault(x => String.Equals(x.ColumnName, actualColumn.Name, StringComparison.InvariantCultureIgnoreCase));
				if (dataColumn != null)
				{
					privateField = tableType.GetField("column" + actualColumn.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
				}
				else
				{
					if (!actualColumn.IsDynamic)
						actualColumn.IsExtra = true;
				}

				///////////////////////////////////////////////////////////////////////////////

				dataColumn = new DataColumn(actualColumn.Name, actualColumn.Type);
				table.Columns.Add(dataColumn);

				dataColumn.SetOrdinal(i);

				///////////////////////////////////////////////////////////////////////////////

				if (privateField != null)
					privateField.SetValue(table, dataColumn);
			}
		}

		protected object GetDefaultValue(Type type)
		{
			if (type.IsValueType)
				return Activator.CreateInstance(type);

			return null;
		}

		#endregion

		#region Implementation: type conversion

		protected string DynamicColumnTypeToAccessType(DynamicColumnType type)
		{
			switch (type)
			{
				case DynamicColumnType.FreeText:
				case DynamicColumnType.Category:
					return "MEMO";

				case DynamicColumnType.Numeric:
					return "DOUBLE";

				case DynamicColumnType.DateTime:
					return "DATETIME";
			}

			///////////////////////////////////////////////////////////////////////////////

			throw new Exception("Unsupported column type: " + type);
		}

		protected DynamicColumnType ClrTypeToDynamicColumnType(Type clrType)
		{
			if (clrType == typeof(string))
				return DynamicColumnType.FreeText;

			if (clrType == typeof(decimal) || clrType == typeof(double))
				return DynamicColumnType.Numeric;

			if (clrType == typeof(DateTime))
				return DynamicColumnType.DateTime;

			///////////////////////////////////////////////////////////////////////////////

			throw new Exception("Unsupported column type " + clrType);
		}

		public static Type ColumnTypeToClrType(DynamicColumnType type)
		{
			switch (type)
			{
				case DynamicColumnType.FreeText:
				case DynamicColumnType.Category:
					return typeof(string);

				case DynamicColumnType.Numeric:
					return typeof(double);

				case DynamicColumnType.DateTime:
					return typeof(DateTime);
			}

			///////////////////////////////////////////////////////////////////////////////

			throw new Exception("Unsupported column type " + type);
		}

		protected Type AccessTypeToClrType(string accessType)
		{
			switch (accessType)
			{
				case "MEMO":
					return typeof(string);

				case "INTEGER":
					return typeof(int);

				case "DOUBLE":
					return typeof(decimal);

				case "DATETIME":
					return typeof(DateTime);
			}

			///////////////////////////////////////////////////////////////////////////////

			throw new Exception("Unsupported column type: " + accessType);
		}

		protected string ClrTypeToAccessType(Type type)
		{
			if (type == typeof(string))
				return "MEMO";
			else if (type == typeof(int))
				return "INTEGER";
			else if (type == typeof(decimal) || type == typeof(double))
				return "DOUBLE";
			else if (type == typeof(DateTime))
				return "DATETIME";

			///////////////////////////////////////////////////////////////////////////////

			throw new Exception("Unsupported column type: " + type);
		}

		#endregion

		#region Implementation: IDisposable

		public void Dispose()
		{
			if (_selectAllColumnsCommand != null)
			{
				_selectAllColumnsCommand.Dispose();
				_selectAllColumnsCommand = null;
			}

			///////////////////////////////////////////////////////////////////////////////

			if (_updateAllColumnsCommand != null)
			{
				_updateAllColumnsCommand.Dispose();
				_updateAllColumnsCommand = null;
			}
		}

		#endregion
	}

	[DebuggerDisplay("Name = {Name}, Type = {Type")]
	public class ColumnInfo
	{
		#region Fields

		public string Name { get; set; }
		public Type Type { get; set; }
		public int SqlColumnIndex { get; set; }

		public bool IsExtra { get; set; }
		public bool IsDynamic { get; set; }
		public int DynamicColumnID { get; set; }
        #endregion

		#region Operations

		public ColumnInfo Clone()
		{
			var ci = new ColumnInfo
			{
				Name = this.Name,
				Type = this.Type,
				SqlColumnIndex = this.SqlColumnIndex,
				IsDynamic = this.IsDynamic,
				IsExtra = this.IsExtra,
				DynamicColumnID = this.DynamicColumnID,
            };

			return ci;
		}

		#endregion
	}

	public class ColumnInfoEqualityComparer : IEqualityComparer<ColumnInfo>
	{
		#region Implementation: IEqualityComparer<ColumnInfo>

		public bool Equals(ColumnInfo x, ColumnInfo y)
		{
			return String.Equals(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase);
		}

		public int GetHashCode(ColumnInfo obj)
		{
			return obj.Name.GetHashCode();
		}

		#endregion
	}
}