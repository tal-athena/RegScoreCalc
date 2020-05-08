using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

using Helpers;
using Newtonsoft.Json;
using SQLite;

namespace Plumbing2.Code
{
	public class Database
	{
		#region Fields

		protected readonly string _mdbFilePath;
        protected readonly string _mdbPassword;
		protected Logger _logger;
        protected Logger _progresser;

        #endregion

        #region Ctors

        public Database(string mdbFilePath, string dbPassword, Logger logger, Logger progress)
		{
			_mdbFilePath = mdbFilePath;
            _mdbPassword = dbPassword;

            _logger = logger;
            _progresser = progress;

        }

		#endregion

		#region Operations

		public void ConvertMdbToSqlite(string sqliteName, string noteColumnName = "NOTE_TEXT", List<double> filteredDocument = null)
		{
			bool invalidPassword;
			using (var input = new OleDbConnection(ConnectionStringHelper.GetConnectionString(_mdbFilePath, _mdbPassword, out invalidPassword)))
			{
				input.Open();

				using (var train = new SQLiteConnection(sqliteName))
				{
					train.CreateTable<Documents>();

                    ConvertToSqlite(input, train, noteColumnName, filteredDocument);
                }
			}
		}
        #endregion

        #region Implementation

        protected void DeleteUncategorized(OleDbConnection input)
		{
			var cmd = input.CreateCommand();
			cmd.CommandText = "DELETE FROM [Documents] WHERE [Category] IS NULL";
			cmd.ExecuteNonQuery();
		}
        protected int GetTotalCount (OleDbConnection input)
        {
            var cmdText = "SELECT COUNT(ED_ENC_NUM) FROM Documents";
            var command = new OleDbCommand(cmdText, input);
            
            int count = (int)command.ExecuteScalar();
            return count;
        }
        protected int ConvertToSqlite(OleDbConnection input, SQLiteConnection sqlite, string noteColumnName, List<double> filteredDocument)
        {
            var totalCount = GetTotalCount(input);

            var currentCount = 0;

            var cmdText = "SELECT ED_ENC_NUM, " + noteColumnName + " FROM DOCUMENTS";

            if (filteredDocument != null && filteredDocument.Any())
            {
                var idList = "(";
                foreach (var id in filteredDocument)
                {
                    if (idList == "(")
                        idList += (int)id;
                    else idList += ", " + (int)id;
                }
                idList += ")";


                cmdText += " WHERE ED_ENC_NUM IN " + idList;
            }

            var command = new OleDbCommand(cmdText, input);

            using (var reader = command.ExecuteReader())
            {                
                if (reader.HasRows)
                { 
                    while (reader.Read())
                    {
                        var row = new Documents
                        {
                            ED_ENC_NUM = reader.GetDouble(0),
                            NOTE_TEXT = reader.GetString(1).Replace("\r", ""),                            
                        };

                        sqlite.Insert(row);

                        ///////////////////////////////////////////////////////////////////////////////

                        currentCount++;
                        if (currentCount % 20 == 0)
                        {
                            _progresser.Progress(((int)((double)currentCount / totalCount * 50)).ToString());
                        }
                    }
                }
            }
            _progresser.Progress("50");
            return totalCount;
        }

        protected void InsertRandom<T>(List<T> enumerable, T value, Random random)
		{
			var index = random.Next(0, enumerable.Count + 1);

			enumerable.Insert(index, value);
		}

		protected int? GetInt32ValueInvariant(OleDbDataReader reader, int columnIndex, DbType columnType)
		{
			if (reader.IsDBNull(columnIndex))
				return null;

			///////////////////////////////////////////////////////////////////////////////

			switch (columnType)
			{
				case DbType.Int32:
					return reader.GetInt32(columnIndex);
				
				case DbType.Double:
					return Convert.ToInt32(reader.GetDouble(columnIndex));

				default:
					throw new ArgumentOutOfRangeException("columnType", columnType, null);
			}
		}

		protected static DbType GetCategoryColumnType(OleDbDataReader reader)
		{
			DbType dbType;

			var categoryColumnType = reader.GetFieldType(2);
			if (categoryColumnType == typeof (int))
				dbType = DbType.Int32;
			else if (categoryColumnType == typeof (double))
				dbType = DbType.Double;
			else
				throw new Exception("Category column has invalid type '" + categoryColumnType.Name + "'");

			return dbType;
		}

        public List<ExtractResult> GetResultFromSqlite(string sqliteFileName, string noteColumnName, int columnID)
        {
            List<ExtractResult> result = new List<ExtractResult>();

            using (var test = new SQLiteConnection(sqliteFileName))
            {                
                var query = test.Table<Documents>();
                foreach (var row in query)
                {                    
                    ExtractResult obj = new ExtractResult();
                    obj.DocumentID = row.ED_ENC_NUM;
                    obj.ColumnID = columnID;
                    obj.NoteTextColumnName = noteColumnName;
                    

                    dynamic a = JsonConvert.DeserializeObject(row.Result);
                    if (JsonConvert.SerializeObject(a["entities"]) != "{}")
                    {
                        obj.Result = JsonConvert.SerializeObject(a["entities"]);
                        result.Add(obj);
                    }
                }
            }

            return result;
        }

        protected void AssertDocumentsCount(SQLiteConnection test, SQLiteConnection train)
		{
			var testDocumentsCount = test.Table<Documents>().Count();
			var trainDocumentsCount = train.Table<Documents>().Count();

			if (testDocumentsCount == 0)
				throw new EmptyDatasetException("Resulting TEST dataset has 0 documents, aborting...");

			if (trainDocumentsCount == 0)
				throw new EmptyDatasetException("Resulting TRAIN dataset has 0 documents, aborting...");
		}

		protected void CopyCategoriesTable(OleDbConnection input, SQLiteConnection test, SQLiteConnection train, int dynamicColumnID)
		{
			test.CreateTable<Categories>();
			train.CreateTable<Categories>();

			///////////////////////////////////////////////////////////////////////////////

			var cmdText = dynamicColumnID > 0 ? "SELECT ID, Title FROM DynamicColumnCategories WHERE DynamicColumnID = " + dynamicColumnID : "SELECT ID, Category FROM Categories";
			var command = new OleDbCommand(cmdText, input);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var row = new Categories
						{
							ID = reader.GetInt32(0),
							Category = !reader.IsDBNull(1) ? reader.GetString(1) : String.Empty
						};

						test.Insert(row);
						train.Insert(row);
					}
				}
			}

		}

		#endregion
	}
}
