using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;

using Helpers;

namespace ExportDocuments.Code
{
	public class Exporter : IDisposable
	{
		#region Internal types

		protected enum ColumnClassType
		{
			#region Constants

			Category = 0,
			FreeText,
			Numeric,
			DateTime,
			None

			#endregion
		}

		[DebuggerDisplay("CategoryClassID = {CategoryClassID}, className = {ClassName}, type = {Type}")]
		protected class ColumnClassInfo
		{
			#region Fields

			public int CategoryClassID { get; set; }
			public string ClassName { get; set; }
			public ColumnClassType Type { get; set; }

			#endregion
		}

		[DebuggerDisplay("CategoryID = {CategoryID}, categoryClassID = {CategoryClassID}, categoryName = {CategoryName}")]
		protected class CategoryInfo
		{
			#region Fields

			public int CategoryID { get; set; }
			public int CategoryClassID { get; set; }
			public string CategoryName { get; set; }

			#endregion
		}

		[DebuggerDisplay("DocumentID = {DocumentID}, values count = {Documents_ValuesList.Count}")]
		protected class DocumentInfo
		{
			#region Ctors

			public DocumentInfo()
			{
				this.Documents_ValuesList = new List<object>();
			}

			#endregion

			#region Fields

			public double DocumentID { get; set; }

			public List<object> Documents_ValuesList { get; set; }

			public int? DocumentColumnValues_ColumnID { get; set; }
			public string DocumentColumnValues_TextValue { get; set; }
			public double? DocumentColumnValues_NumericValue { get; set; }
			public DateTime? DocumentColumnValues_DateTimeValue { get; set; }

			public int? CategoryClasses_CategoryClassID { get; set; }
			public string CategoryClasses_ClassName { get; set; }
			public ColumnClassType? CategoryClasses_Type { get; set; }

			public int? DocumentCategory_CategoryID { get; set; }

			#endregion
		}

		[DebuggerDisplay("Name = {Name}, type = {Type}, needAdd = {NeedAdd}, needDelete = {NeedDelete}")]
		protected class ColumnInfo
		{
			#region Fields

			public string Name { get; set; }
			public bool NeedAdd { get; set; }
			public bool NeedDelete { get; set; }
			public OleDbType Type { get; set; }

			#endregion
		}

		protected class ColumnDataRowComparer : IComparer<DataRow>
		{
			#region Implementation: IComparer<DataRow>

			public int Compare(DataRow x, DataRow y)
			{
				var xValue = Convert.ToInt32(x.ItemArray[6]);
				var yValue = Convert.ToInt32(y.ItemArray[6]);

				///////////////////////////////////////////////////////////////////////////////

				if (xValue > yValue)
					return 1;

				if (xValue < yValue)
					return -1;

				///////////////////////////////////////////////////////////////////////////////

				return 0;
			}

			#endregion
		}

		#endregion

		#region Constants

		protected const string _documentsTableName = "Documents";
		protected const string _documentsPrimaryKeyName = "ED_ENC_NUM";

		protected Dictionary<OleDbType, string> _accessTypeMappings;

		#endregion

		#region Fields

		protected bool _deleteDocuments;

		protected readonly bool _useTransaction;

		protected readonly BackgroundWorker _worker;

		protected OleDbCommand _insertDocumentCommand;

		protected OleDbTransaction _transaction;

		protected int _totalDocumentsCount;

		#endregion

		#region Properties

		public bool ReportWarnings { get; set; }

		#endregion

		#region Ctors

		public Exporter(bool useTransaction, bool deleteDocuments, bool reportWarnings, BackgroundWorker worker)
		{
			_useTransaction = useTransaction;
			_deleteDocuments = deleteDocuments;
			this.ReportWarnings = reportWarnings;

			_worker = worker;

			FillAccessTypeMappings();
		}

		#endregion

		#region Operations

		public int ExportDocuments(string inputDatabasePath, string outputDatabasePath, string password)
		{
			var totalRowsCopied = 0;

			///////////////////////////////////////////////////////////////////////////////

			const string defaultErrorMessage = "Cannot open input database. Please make sure that you are running application with correct bitness";
			const string passwordErrorMessage = "Invalid password";

			bool invalidPassword;

			var inputConnectionString = ConnectionStringHelper.GetConnectionString(inputDatabasePath, password, out invalidPassword);
			if (invalidPassword)
				throw new Exception(passwordErrorMessage);

			if (String.IsNullOrEmpty(inputConnectionString))
				throw new Exception(defaultErrorMessage);

			var outputConnectionString = ConnectionStringHelper.GetConnectionString(outputDatabasePath, password, out invalidPassword);
			if (invalidPassword)
				throw new Exception(passwordErrorMessage);

			if (String.IsNullOrEmpty(inputConnectionString))
				throw new Exception(defaultErrorMessage);

			///////////////////////////////////////////////////////////////////////////////

			using (var inputConnection = new OleDbConnection(inputConnectionString))
			using (var outputConnection = new OleDbConnection(outputConnectionString))
			{
				inputConnection.Open();
				outputConnection.Open();

				///////////////////////////////////////////////////////////////////////////////

				try
				{
					if (_useTransaction)
						_transaction = outputConnection.BeginTransaction();

					///////////////////////////////////////////////////////////////////////////////

					totalRowsCopied = CopyDocuments(inputConnection, outputConnection);

					///////////////////////////////////////////////////////////////////////////////

					if (_transaction != null)
						_transaction.Commit();
				}
				finally
				{
					var transaction = _transaction;
					_transaction = null;

					///////////////////////////////////////////////////////////////////////////////

					if (transaction != null)
						transaction.Dispose();
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			return totalRowsCopied;
		}

		#endregion

		#region Implementation

		protected int CopyDocuments(OleDbConnection inputConnection, OleDbConnection outputConnection)
		{
			CheckIfDocumentsTableExist(outputConnection);

			///////////////////////////////////////////////////////////////////////////////

			var inputDocumentColumns = GetTableColumnNames(inputConnection, _documentsTableName);
			var outputDocumentColumns = GetTableColumnNames(outputConnection, _documentsTableName);

			///////////////////////////////////////////////////////////////////////////////

			var categoryColumn = inputDocumentColumns.FirstOrDefault(x => String.Compare(x.Name, "category", StringComparison.InvariantCultureIgnoreCase) == 0);
			if (categoryColumn == null)
				throw new Exception("Cannot find input Category column");

			categoryColumn.Type = OleDbType.VarWChar;

			///////////////////////////////////////////////////////////////////////////////

			var columnClasses = GetColumnClasses(inputConnection);
			var dynamicColumns = ColumnClassesToColumns(columnClasses);

			AddDynamicColumnsToInputColumnsList(inputDocumentColumns, dynamicColumns);

			///////////////////////////////////////////////////////////////////////////////

			CompareColumnLists(inputDocumentColumns, outputDocumentColumns);

			///////////////////////////////////////////////////////////////////////////////

			DeleteIndex(outputConnection);

			///////////////////////////////////////////////////////////////////////////////

			var columnsToDelete = inputDocumentColumns.Where(x => x.NeedDelete).ToList();
			if (columnsToDelete.Any())
				DeleteColumns(outputConnection, _documentsTableName, columnsToDelete);
			
			var columnsToAdd = inputDocumentColumns.Where(x => x.NeedAdd).ToList();
			if (columnsToAdd.Any())
				AddColumns(outputConnection, _documentsTableName, columnsToAdd);

			///////////////////////////////////////////////////////////////////////////////

			inputDocumentColumns = inputDocumentColumns.Except(dynamicColumns).ToList();

			///////////////////////////////////////////////////////////////////////////////

			return CopyDocumentRows(inputConnection, outputConnection, inputDocumentColumns, columnClasses, dynamicColumns);
		}

		protected int CopyDocumentRows(OleDbConnection inputConnection, OleDbConnection outputConnection, List<ColumnInfo> inputDocumentColumns, List<ColumnClassInfo> columnClasses, List<ColumnInfo> dynamicColumns)
		{
			var totalRowsInserted = 0;

			///////////////////////////////////////////////////////////////////////////////

			var documentsCountCommand = BuildCommand_Select_DocumentsCount(inputConnection);
			_totalDocumentsCount = (int) documentsCountCommand.ExecuteScalar();

			///////////////////////////////////////////////////////////////////////////////

			var categories = GetCategories(inputConnection);

			///////////////////////////////////////////////////////////////////////////////

			var idColumnIndex = inputDocumentColumns.FindIndex(x => x.Name == _documentsPrimaryKeyName);
			if (idColumnIndex == -1)
				throw new Exception(String.Format("Cannot find {0} column", _documentsPrimaryKeyName));

			///////////////////////////////////////////////////////////////////////////////

			var categoryColumnIndex = inputDocumentColumns.FindIndex(x => String.Compare(x.Name, "category", StringComparison.InvariantCultureIgnoreCase) == 0);

			///////////////////////////////////////////////////////////////////////////////

			using (var selectDocumentsCommand = BuildCommand_Select_Documents(inputConnection, inputDocumentColumns))
			using (var reader = selectDocumentsCommand.ExecuteReader())
			{
				if (!reader.HasRows)
					return 0;

				///////////////////////////////////////////////////////////////////////////////

				if (_deleteDocuments)
					DeleteDocuments(outputConnection);

				///////////////////////////////////////////////////////////////////////////////

				CreateIndex(outputConnection);

				///////////////////////////////////////////////////////////////////////////////

				var documentsList = new List<DocumentInfo>();

				var lastDocumentID = -1d;

				///////////////////////////////////////////////////////////////////////////////

				var staticColumnsCount = inputDocumentColumns.Count;
				AddDynamicColumnsToInputColumnsList(inputDocumentColumns, dynamicColumns);

				///////////////////////////////////////////////////////////////////////////////

				_worker.ReportProgress(-1, "Copying rows...");

				///////////////////////////////////////////////////////////////////////////////

				while (reader.Read())
				{
					var doc = new DocumentInfo
					          {
						          DocumentID = reader.GetDouble(idColumnIndex)
					          };

					///////////////////////////////////////////////////////////////////////////////

					int i;

					///////////////////////////////////////////////////////////////////////////////

					for (i = 0; i < staticColumnsCount; i++)
					{
						doc.Documents_ValuesList.Add(reader.GetValue(i));
					}

					///////////////////////////////////////////////////////////////////////////////

					var staticCategoryName = ReadAsString(reader, i++);
					if (String.IsNullOrEmpty(staticCategoryName))
						doc.Documents_ValuesList[categoryColumnIndex] = DBNull.Value;
					else
						doc.Documents_ValuesList[categoryColumnIndex] = staticCategoryName;

					///////////////////////////////////////////////////////////////////////////////

					doc.DocumentColumnValues_ColumnID = ReadAsInt(reader, i++);
					doc.DocumentColumnValues_TextValue = ReadAsString(reader, i++);
					doc.DocumentColumnValues_NumericValue = ReadAsDouble(reader, i++);
					doc.DocumentColumnValues_DateTimeValue = ReadAsDateTime(reader, i++);

					doc.CategoryClasses_CategoryClassID = ReadAsInt(reader, i++);
					doc.CategoryClasses_ClassName = ReadAsString(reader, i++);

					doc.CategoryClasses_Type = (ColumnClassType?) ReadAsInt(reader, i++);

					doc.DocumentCategory_CategoryID = ReadAsInt(reader, i);

					///////////////////////////////////////////////////////////////////////////////

					if (lastDocumentID > 0)
					{
						if (doc.DocumentID != lastDocumentID)
						{
							InsertAsSingleRow(outputConnection, inputDocumentColumns, documentsList, columnClasses, categories);

							documentsList.Clear();

							///////////////////////////////////////////////////////////////////////////////

							totalRowsInserted++;

							///////////////////////////////////////////////////////////////////////////////

							if (totalRowsInserted % 200 == 0)
							{
								if (_worker.CancellationPending)
									break;

								///////////////////////////////////////////////////////////////////////////////

								int progressPercentage = (int) (totalRowsInserted / (double) _totalDocumentsCount * 100d);

								_worker.ReportProgress(progressPercentage, totalRowsInserted);
							}
						}
					}

					///////////////////////////////////////////////////////////////////////////////

					documentsList.Add(doc);

					///////////////////////////////////////////////////////////////////////////////

					lastDocumentID = doc.DocumentID;
				}

				///////////////////////////////////////////////////////////////////////////////

				if (documentsList.Any())
				{
					InsertAsSingleRow(outputConnection, inputDocumentColumns, documentsList, columnClasses, categories);

					///////////////////////////////////////////////////////////////////////////////

					totalRowsInserted++;
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			_worker.ReportProgress(100, totalRowsInserted);

			///////////////////////////////////////////////////////////////////////////////

			return totalRowsInserted;
		}

		protected void DeleteDocuments(OleDbConnection outputConnection)
		{
			_worker.ReportProgress(-1, String.Format("Deleting rows from {0} table...", _documentsTableName));

			///////////////////////////////////////////////////////////////////////////////

			var deleteCommand = BuildCommand_Delete_Documents(outputConnection);
			var deletedDocumentsCount = deleteCommand.ExecuteNonQuery();

			///////////////////////////////////////////////////////////////////////////////

			_worker.ReportProgress(-1, String.Format("Total rows deleted:  {0}", deletedDocumentsCount));
		}

		protected void InsertAsSingleRow(OleDbConnection outputConnection, List<ColumnInfo> columnsList, List<DocumentInfo> documentsList, List<ColumnClassInfo> columnClasses, List<CategoryInfo> categories)
		{
			var doc = AggregateDocumentRows(documentsList, columnClasses, categories);

			///////////////////////////////////////////////////////////////////////////////

			var command = BuildCommand_Insert_Document(outputConnection, columnsList);

			for (var i = 0; i < columnsList.Count; i++)
			{
				command.Parameters[i].Value = doc.Documents_ValuesList[i];
			}

			///////////////////////////////////////////////////////////////////////////////

			var count = command.ExecuteNonQuery();
			if (count != 1)
				throw new Exception(String.Format("Failed to insert row with '{0} = {1}", _documentsPrimaryKeyName, doc.DocumentID.ToString("N0")));
		}

		protected DocumentInfo AggregateDocumentRows(List<DocumentInfo> documentsList, List<ColumnClassInfo> columnClasses, List<CategoryInfo> dynamicCategories)
		{
			var result = documentsList.First();

			///////////////////////////////////////////////////////////////////////////////

			var join = from doc in documentsList
					   join cat in dynamicCategories
				           on doc.DocumentCategory_CategoryID equals cat.CategoryID
				           into tmp
			           from t in tmp.DefaultIfEmpty()
			           select new { doc, cat = t };

			///////////////////////////////////////////////////////////////////////////////

			var startIndex = result.Documents_ValuesList.Count;

			for (var index = 0; index < columnClasses.Count; index++)
			{
				result.Documents_ValuesList.Add(DBNull.Value);
			}

			///////////////////////////////////////////////////////////////////////////////

			foreach (var j in join)
			{
				int columnClassIndex;

				///////////////////////////////////////////////////////////////////////////////

				if (j.cat != null)
				{
					columnClassIndex = startIndex + columnClasses.FindIndex(x => x.CategoryClassID == j.cat.CategoryClassID);

					result.Documents_ValuesList[columnClassIndex] = j.cat.CategoryName;
				}

				///////////////////////////////////////////////////////////////////////////////

				columnClassIndex = startIndex + columnClasses.FindIndex(x => x.CategoryClassID == j.doc.CategoryClasses_CategoryClassID);

				///////////////////////////////////////////////////////////////////////////////

				switch (j.doc.CategoryClasses_Type)
				{
					case ColumnClassType.FreeText:
						if (j.doc.DocumentColumnValues_TextValue != null)
							result.Documents_ValuesList[columnClassIndex] = j.doc.DocumentColumnValues_TextValue;
						break;

					case ColumnClassType.Numeric:
						if (j.doc.DocumentColumnValues_NumericValue != null)
							result.Documents_ValuesList[columnClassIndex] = j.doc.DocumentColumnValues_NumericValue;
						break;

					case ColumnClassType.DateTime:
						if (j.doc.DocumentColumnValues_DateTimeValue != null)
							result.Documents_ValuesList[columnClassIndex] = j.doc.DocumentColumnValues_DateTimeValue;
						break;

					default:
						if (j.doc.DocumentColumnValues_ColumnID != null && this.ReportWarnings)
							_worker.ReportProgress(-1, "Document with ID = " + j.doc.DocumentID.ToString("N0") + "  has values that does not match any dynamic column. Dynamic column ID = " + j.doc.DocumentColumnValues_ColumnID);

						break;
					//throw new ArgumentOutOfRangeException(String.Format("Invalid column type: {0}", j.doc.CategoryClasses_Type));
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			return result;
		}

		protected List<ColumnClassInfo> GetColumnClasses(OleDbConnection inputConnection)
		{
			var tableClasses = new DataTable();

			using (var cmd = BuildCommand_Select_CategoryClasses(inputConnection))
			using (var dataAdapter = new OleDbDataAdapter(cmd))
			{
				dataAdapter.Fill(tableClasses);

				///////////////////////////////////////////////////////////////////////////////

				return tableClasses.Rows.Cast<DataRow>()
				                   .Select(x => new ColumnClassInfo
				                                {
					                                CategoryClassID = Convert.ToInt32(x[0]),
					                                ClassName = x[1] as string,
					                                Type = (ColumnClassType) Convert.ToInt32(x[2])
				                                })
				                   .ToList();
			}
		}

		protected List<ColumnInfo> ColumnClassesToColumns(List<ColumnClassInfo> classes)
		{
			var columns = new List<ColumnInfo>();

			///////////////////////////////////////////////////////////////////////////////

			foreach (var classInfo in classes)
			{
				switch (classInfo.Type)
				{
					case ColumnClassType.Category:
					case ColumnClassType.FreeText:
						columns.Add(new ColumnInfo { Name = classInfo.ClassName, Type = OleDbType.VarWChar });
						break;

					case ColumnClassType.Numeric:
						columns.Add(new ColumnInfo { Name = classInfo.ClassName, Type = OleDbType.Double });
						break;

					case ColumnClassType.DateTime:
						columns.Add(new ColumnInfo { Name = classInfo.ClassName, Type = OleDbType.Date });
						break;

					default:
						throw new ArgumentOutOfRangeException(String.Format("Invalid column type: {0}", classInfo.Type.ToString()));
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			return columns;
		}

		protected List<CategoryInfo> GetCategories(OleDbConnection inputConnection)
		{
			var tableCategories = new DataTable();

			using (var selectCategories = BuildCommand_Select_CategoryClassCategory(inputConnection))
			using (var dataAdapter = new OleDbDataAdapter(selectCategories))
			{
				dataAdapter.Fill(tableCategories);

				///////////////////////////////////////////////////////////////////////////////

				return tableCategories.Rows.Cast<DataRow>()
				                      .Select(x => new CategoryInfo
				                                   {
					                                   CategoryID = Convert.ToInt32(x[0]), CategoryClassID = Convert.ToInt32(x[1]), CategoryName = x[2] as string
				                                   })
				                      .ToList();
			}
		}

		protected void CompareColumnLists(List<ColumnInfo> inputDocumentColumns, List<ColumnInfo> outputDocumentColumns)
		{
			var join = (from i in inputDocumentColumns
			            join o in outputDocumentColumns on i.Name.ToLower() equals o.Name.ToLower() into tmp
			            from t in tmp.DefaultIfEmpty()
			            select new
			                   {
				                   InputColumn = i,
								   OutputColumn = t
			                   }).ToList();

			///////////////////////////////////////////////////////////////////////////////

			foreach (var j in join)
			{
				if (j.OutputColumn == null)
				{
					j.InputColumn.NeedAdd = true;
					continue;
				}

				///////////////////////////////////////////////////////////////////////////////

				if (j.InputColumn.Type != j.OutputColumn.Type)
				{
					j.InputColumn.NeedAdd = true;
					j.InputColumn.NeedDelete = true;
				}
			}
		}

		protected void DeleteColumns(OleDbConnection outputConnection, string tableName, List<ColumnInfo> columnsToDelete)
		{
			var commandPrefix = "ALTER TABLE [" + tableName + "] ";

			using (var cmd = new OleDbCommand("", outputConnection, _transaction))
			{
				foreach (var col in columnsToDelete)
				{
					var commandText = commandPrefix + "DROP COLUMN [" + col.Name + "] ";
					cmd.CommandText = commandText;

					cmd.ExecuteNonQuery();
				}
			}
		}

		protected void AddColumns(OleDbConnection outputConnection, string tableName, List<ColumnInfo> columnsToAdd)
		{
			_worker.ReportProgress(-1, "Adjusting columns...");

			///////////////////////////////////////////////////////////////////////////////

			var commandPrefix = "ALTER TABLE [" + tableName + "] ";

			using (var cmd = new OleDbCommand("", outputConnection, _transaction))
			{
				foreach (var col in columnsToAdd)
				{
					var commandText = commandPrefix + "ADD COLUMN [" + col.Name + "] " + MapAccessType(col.Type);
					cmd.CommandText = commandText;

					cmd.ExecuteNonQuery();
				}
			}
		}

		protected List<ColumnInfo> GetTableColumnNames(OleDbConnection connection, string tableName)
		{
			var args = new object[] { null, null, tableName, null };

			var schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, args);

			var columns = schema.Rows.Cast<DataRow>()
			                    .ToList();

			///////////////////////////////////////////////////////////////////////////////

			try
			{
				var comparer = new ColumnDataRowComparer();

				return columns.OrderBy(x => x, comparer)
				              .Select(x => new ColumnInfo
				                           {
					                           Name = x.ItemArray[3] as string, Type = (OleDbType) Convert.ToInt32(x.ItemArray[11])
				                           })
				              .ToList();
			}
			catch
			{
				return columns.Select(x => new ColumnInfo
				                           {
					                           Name = x.ItemArray[3] as string, Type = (OleDbType) Convert.ToInt32(x.ItemArray[11])
				                           })
				              .ToList();
			}
		}

		protected void AddDynamicColumnsToInputColumnsList(List<ColumnInfo> inputStaticDocumentColumns, List<ColumnInfo> dynamicColumnsList)
		{
			var duplicates = (from inputCol in inputStaticDocumentColumns
			                  join dynCol in dynamicColumnsList
				                  on inputCol.Name.ToLower() equals dynCol.Name.ToLower()
			                  select new { InputColumn = inputCol, DynamicColumn = dynCol })
				.ToList();

			///////////////////////////////////////////////////////////////////////////////

			if (duplicates.Any())
			{
				var suffix = 2;

				foreach (var d in duplicates)
				{
					d.DynamicColumn.Name += "_" + suffix;

					suffix++;
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			inputStaticDocumentColumns.AddRange(dynamicColumnsList);

		}

		#endregion

		#region Implementation: commands

		protected OleDbCommand BuildCommand_Select_CategoryClassCategory(OleDbConnection inputConnection)
		{
			var commandText = "SELECT [CategoryID], [CategoryClassID], [CategoryName] FROM [CategoryClassCategory]";

			return new OleDbCommand(commandText, inputConnection);
		}

		protected OleDbCommand BuildCommand_Select_CategoryClasses(OleDbConnection inputConnection)
		{
			var commandText = "SELECT [CategoryClassID], [ClassName], [Type] FROM [CategoryClasses]";

			return new OleDbCommand(commandText, inputConnection);
		}

		protected OleDbCommand BuildCommand_Select_DocumentsCount(OleDbConnection inputConnection)
		{
			var commandText = "SELECT COUNT(*) FROM " + _documentsTableName;

			return new OleDbCommand(commandText, inputConnection);
		}

		protected OleDbCommand BuildCommand_Select_Documents(OleDbConnection inputConnection, List<ColumnInfo> inputColumns)
		{
			var columnsList = String.Join(", ", inputColumns.Select(x => _documentsTableName + ".[" + x.Name + "]"));

			var commandText = Properties.Resources.Query.Replace("%placeholder%", columnsList);

			return new OleDbCommand(commandText, inputConnection);
		}

		protected OleDbCommand BuildCommand_Delete_Documents(OleDbConnection outputConnection)
		{
			var commandText = "DELETE FROM " + _documentsTableName;

			return new OleDbCommand(commandText, outputConnection, _transaction);
		}

		protected OleDbCommand BuildCommand_Insert_Document(OleDbConnection outputConnection, List<ColumnInfo> columnsToCopy)
		{
			if (_insertDocumentCommand != null)
				return _insertDocumentCommand;

			///////////////////////////////////////////////////////////////////////////////

			_insertDocumentCommand = new OleDbCommand("", outputConnection, _transaction);

			///////////////////////////////////////////////////////////////////////////////

			var commandText = String.Format("INSERT INTO {0} (", _documentsTableName);

			var columnNames = columnsToCopy.Select(x => x.Name)
										   .ToList();

			///////////////////////////////////////////////////////////////////////////////

			var columns = "[" + String.Join("], [", columnNames) + "]";
			var parameters = "";
			for (var i = 0; i < columnNames.Count; i++)
			{
				if (parameters.Length > 0)
					parameters += ", ";

				parameters += "?";
			}

			commandText += String.Format("{0}) VALUES ({1})", columns, parameters);

			_insertDocumentCommand.CommandText = commandText;

			///////////////////////////////////////////////////////////////////////////////

			foreach (var col in columnsToCopy)
			{
				_insertDocumentCommand.Parameters.Add(col.Name, col.Type);
			}

			///////////////////////////////////////////////////////////////////////////////

			return _insertDocumentCommand;
		}

		#endregion

		#region Implementation: create table

		protected void CheckIfDocumentsTableExist(OleDbConnection connection)
		{
			var exists = connection.GetSchema("Tables", new string[4] { null, null, _documentsTableName, "TABLE" }).Rows.Count > 0;
			if (exists)
				return;

			///////////////////////////////////////////////////////////////////////////////

			_worker.ReportProgress(-1, "Creating Documents table...");

			///////////////////////////////////////////////////////////////////////////////

			var commandText = String.Format("CREATE TABLE {0} (ED_ENC_NUM DOUBLE)", _documentsTableName);

			var cmd = new OleDbCommand(commandText, connection, _transaction);
			cmd.ExecuteNonQuery();

			///////////////////////////////////////////////////////////////////////////////

			_deleteDocuments = false;
		}

		#endregion

		#region Implementation: index

		protected void DeleteIndex(OleDbConnection connection)
		{
			var indexName = GetIndexName(connection);
			if (!String.IsNullOrEmpty(indexName))
			{
				_worker.ReportProgress(-1, "Deleting index...");

				DropIndex(connection, indexName);
			}
		}

		protected string GetIndexName(OleDbConnection connection)
		{
			var table = connection.GetSchema("Indexes");
			var indexRow = table.Rows
								.Cast<DataRow>()
								.FirstOrDefault(x => x.Field<string>("TABLE_NAME") == _documentsTableName && x.Field<string>("COLUMN_NAME") == _documentsPrimaryKeyName);

			if (indexRow != null)
				return (string) indexRow["INDEX_NAME"];

			return String.Empty;
		}

		protected void DropIndex(OleDbConnection connection, string indexName)
		{
			var query = string.Format("DROP INDEX {0} ON Documents", indexName);

			var cmd = new OleDbCommand(query, connection, _transaction);
			cmd.ExecuteNonQuery();
		}

		protected void CreateIndex(OleDbConnection connection)
		{
			_worker.ReportProgress(-1, "Creating index...");

			///////////////////////////////////////////////////////////////////////////////

			var query = "CREATE INDEX INDEX_DOCUMENTS ON " + _documentsTableName +  " (" + _documentsPrimaryKeyName + ")";

			var cmd = new OleDbCommand(query, connection, _transaction);
			cmd.ExecuteNonQuery();
		}

		#endregion

		#region Helpers

		protected string ReadAsString(OleDbDataReader reader, int index)
		{
			return !reader.IsDBNull(index) ? reader.GetString(index) : null;
		}

		protected int? ReadAsInt(OleDbDataReader reader, int index)
		{
			return !reader.IsDBNull(index) ? reader.GetInt32(index) : (int?) null;
		}

		protected double? ReadAsDouble(OleDbDataReader reader, int index)
		{
			return !reader.IsDBNull(index) ? reader.GetDouble(index) : (double?) null;
		}

		protected DateTime? ReadAsDateTime(OleDbDataReader reader, int index)
		{
			return !reader.IsDBNull(index) ? reader.GetDateTime(index) : (DateTime?) null;
		}

		protected string MapAccessType(OleDbType dbType)
		{
			string value;
			if (!_accessTypeMappings.TryGetValue(dbType, out value))
				throw new Exception("Unsupported column type " + dbType);

			///////////////////////////////////////////////////////////////////////////////

			return value;
		}

		protected void FillAccessTypeMappings()
		{
			_accessTypeMappings = new Dictionary<OleDbType, string>();

			_accessTypeMappings[OleDbType.WChar] = "MEMO";
			_accessTypeMappings[OleDbType.VarWChar] = "TEXT(255)";
			_accessTypeMappings[OleDbType.LongVarWChar] = "MEMO";

			_accessTypeMappings[OleDbType.UnsignedTinyInt] = "BIT";
			_accessTypeMappings[OleDbType.SmallInt] = "INTEGER";
			_accessTypeMappings[OleDbType.Integer] = "INTEGER";

			_accessTypeMappings[OleDbType.Numeric] = "DOUBLE";
			_accessTypeMappings[OleDbType.Single] = "DOUBLE";
			_accessTypeMappings[OleDbType.Double] = "DOUBLE";

			_accessTypeMappings[OleDbType.Boolean] = "YESNO";

			_accessTypeMappings[OleDbType.Date] = "DATETIME";
		}

		#endregion

		#region Implementation: IDisposable

		public void Dispose()
		{
			if (_insertDocumentCommand != null)
			{
				var command = _insertDocumentCommand;
				_insertDocumentCommand = null;

				command.Dispose();
			}

			///////////////////////////////////////////////////////////////////////////////

			if (_transaction != null)
			{
				var transaction = _transaction;
				_transaction = null;

				transaction.Dispose();
			}
		}

		#endregion
	}
}