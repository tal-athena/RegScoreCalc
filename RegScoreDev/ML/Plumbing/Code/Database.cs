using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

using Helpers;

using SQLite;

namespace Plumbing.Code
{
	public class Database
	{
		#region Fields

		protected readonly string _mdbFilePath;
		protected readonly Parameters _parameters;

		protected Logger _logger;

		#endregion

		#region Ctors

		public Database(string mdbFilePath, Parameters parameters, Logger logger)
		{
			_mdbFilePath = mdbFilePath;
			_parameters = parameters;

			_logger = logger;
		}

		#endregion

		#region Operations

		public void ConvertMdbToSqlite(string trainSqliteName, string testSqliteName, int dynamicColumnID, string noteColumnName = "NOTE_TEXT")
		{
			bool invalidPassword;
			using (var input = new OleDbConnection(ConnectionStringHelper.GetConnectionString(_mdbFilePath, _parameters.password, out invalidPassword)))
			{
				input.Open();

				if (_parameters.divideCategories)
					DeleteUncategorized(input);

				using (var train = new SQLiteConnection(trainSqliteName))
				{
					train.CreateTable<Documents>();

					using (var test = new SQLiteConnection(testSqliteName))
					{
						test.CreateTable<Documents>();

						int count;

						if (_parameters.divideCategories)
							count = Convert_Divide(input, test, train, _parameters, noteColumnName);
						else if (_parameters.scoreAll)
							count = Convert_ScoreAll(input, test, train, _parameters, noteColumnName);
						else
							count = Convert_ScoreUncategorized(input, test, train, _parameters, noteColumnName);

						CopyCategoriesTable(input, test, train, dynamicColumnID);

						_logger.Log(String.Format("Total rows processed: {0}", count));
					}
				}
			}
		}

		public void ConvertSqliteToCSV(string testSqliteName, string csvName)
		{
			using (var test = new SQLiteConnection(testSqliteName))
			{
				using (var writer = new StreamWriter(new FileStream(csvName, FileMode.Create)))
				{
					if (_parameters.version == PlumbingToolVersion.Version2)
						writer.WriteLine(_parameters.dynamicColumnID.ToString());

					var query = test.Table<Documents>();
					foreach (var row in query)
					{
						writer.WriteLine("{0},{1}", row.ED_ENC_NUM.ToString("F0"), row.Score);
					}
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

		protected int Convert_Divide(OleDbConnection input, SQLiteConnection test, SQLiteConnection train, Parameters parameters, string noteColumnName)
		{
			if (parameters.positiveCategories == null || parameters.positiveCategories.Length == 0)
				throw new ArgumentException("No positive categories");

			///////////////////////////////////////////////////////////////////////////////

			var totalCount = 0;
			var excludedCount = 0;

			///////////////////////////////////////////////////////////////////////////////

			string cmdText = "SELECT ED_ENC_NUM, " + noteColumnName + ", Category FROM DOCUMENTS";

			///////////////////////////////////////////////////////////////////////////////

			var positiveCategories = new HashSet<int>(parameters.positiveCategories);
			var excludedCategories = new HashSet<int>(parameters.excludedCategories);

			var positiveDocuments = new List<Documents>();
			var negativeDocuments = new List<Documents>();

			var command = new OleDbCommand(cmdText, input);

			var random = new Random(DateTime.Now.Millisecond);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					var categoryColumnType = GetCategoryColumnType(reader);

					while (reader.Read())
					{
						var row = new Documents
								  {
									  ED_ENC_NUM = reader.GetDouble(0),
									  NOTE_TEXT = reader.IsDBNull(1) ? "" : reader.GetString(1),
									  Category = GetInt32ValueInvariant(reader, 2, categoryColumnType)
								  };


						///////////////////////////////////////////////////////////////////////////////

						if (!excludedCategories.Contains(row.Category.Value))
						{
							if (positiveCategories.Contains(row.Category.Value))
								InsertRandom(positiveDocuments, row, random);
							else
								InsertRandom(negativeDocuments, row, random);
						}
						else
							excludedCount++;

						///////////////////////////////////////////////////////////////////////////////

						totalCount++;
					}
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			var takePositiveDocsCount = (int) ((positiveDocuments.Count / 100d) * parameters.validationPercentage);
			var takeNegativeDocsCount = (int) ((negativeDocuments.Count / 100d) * parameters.validationPercentage);

			foreach (var row in positiveDocuments.Take(takePositiveDocsCount))
			{
				test.Insert(row);
			}

			foreach (var row in negativeDocuments.Take(takeNegativeDocsCount))
			{
				test.Insert(row);
			}

			///////////////////////////////////////////////////////////////////////////////

			foreach (var row in positiveDocuments.Skip(takePositiveDocsCount))
			{
				row.Score = 100;

				train.Insert(row);
			}

			foreach (var row in negativeDocuments.Skip(takeNegativeDocsCount))
			{
				row.Score = -100;

				train.Insert(row);
			}

			///////////////////////////////////////////////////////////////////////////////

			_logger.Log("Excluded documents count: " + excludedCount);

			_logger.Log("TEST positive documents count: " + takePositiveDocsCount);
			_logger.Log("TEST negative documents count: " + takeNegativeDocsCount);

			_logger.Log("TRAIN positive documents count: " + (positiveDocuments.Count - takePositiveDocsCount));
			_logger.Log("TRAIN negative documents count: " + (negativeDocuments.Count - takeNegativeDocsCount));

			///////////////////////////////////////////////////////////////////////////////

			AssertDocumentsCount(test, train);

			///////////////////////////////////////////////////////////////////////////////

			return totalCount;
		}

		protected int Convert_ScoreAll(OleDbConnection input, SQLiteConnection test, SQLiteConnection train, Parameters parameters, string noteColumnName)
		{
			var totalCount = 0;

			var trainPositiveCount = 0;
			var trainNegativeCount = 0;
			var excludedCount = 0;

			var cmdText = "SELECT ED_ENC_NUM, " + noteColumnName + ", Category FROM DOCUMENTS";

			///////////////////////////////////////////////////////////////////////////////

			var positiveCategories = new HashSet<int>(parameters.positiveCategories);
			var excludedCategories = new HashSet<int>(parameters.excludedCategories);

			var command = new OleDbCommand(cmdText, input);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					var categoryColumnType = GetCategoryColumnType(reader);

					while (reader.Read())
					{
						try
						{
							double d1 = reader.GetDouble(0);
							string s1 = reader.GetString(1);
							int? c1 = GetInt32ValueInvariant(reader, 2, categoryColumnType);

							var row = new Documents
							{
								ED_ENC_NUM = d1,
								NOTE_TEXT = s1,
								Category = c1
							};


							///////////////////////////////////////////////////////////////////////////////

							if (row.Category.HasValue && excludedCategories.Contains(row.Category.Value))
							{
								excludedCount++;
								totalCount++;

								continue;
							}

							///////////////////////////////////////////////////////////////////////////////

							row.Score = 0;
							test.Insert(row);

							///////////////////////////////////////////////////////////////////////////////

							if (row.Category != null)
							{
								if (positiveCategories.Contains(row.Category.Value))
								{
									row.Score = 100;

									trainPositiveCount++;
								}
								else
								{
									row.Score = -100;


									trainNegativeCount++;
								}

								train.Insert(row);
							}

							///////////////////////////////////////////////////////////////////////////////

							totalCount++;
						}
						catch (Exception e)
						{
							Console.WriteLine(e);
							continue;
						}
					}
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			_logger.Log("Excluded documents count: " + excludedCount);

			_logger.Log("TEST documents count: " + totalCount);

			_logger.Log("TRAIN positive documents count: " + trainPositiveCount);
			_logger.Log("TRAIN negative documents count: " + trainNegativeCount);

			///////////////////////////////////////////////////////////////////////////////

			AssertDocumentsCount(test, train);

			///////////////////////////////////////////////////////////////////////////////

			return totalCount;
		}

		protected int Convert_ScoreUncategorized(OleDbConnection input, SQLiteConnection test, SQLiteConnection train, Parameters parameters, string noteColumnName)
		{
			var totalCount = 0;

			var testCount = 0;
			var trainPositiveCount = 0;
			var trainNegativeCount = 0;
			var excludedCount = 0;

			var cmdText = "SELECT ED_ENC_NUM, " + noteColumnName + ", Category FROM DOCUMENTS";

			///////////////////////////////////////////////////////////////////////////////

			var positiveCategories = new HashSet<int>(parameters.positiveCategories);
			var excludedCategories = new HashSet<int>(parameters.excludedCategories);

			var command = new OleDbCommand(cmdText, input);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					var categoryColumnType = GetCategoryColumnType(reader);

					while (reader.Read())
					{
						var row = new Documents
								  {
									  ED_ENC_NUM = reader.GetDouble(0),
									  NOTE_TEXT = reader.GetString(1),
									  Category = GetInt32ValueInvariant(reader, 2, categoryColumnType)
								  };


						///////////////////////////////////////////////////////////////////////////////

						if (row.Category != null)
						{
							if (!excludedCategories.Contains(row.Category.Value))
							{
								if (positiveCategories.Contains(row.Category.Value))
								{
									row.Score = 100;

									trainPositiveCount++;
								}
								else
								{
									row.Score = -100;

									trainNegativeCount++;
								}

								train.Insert(row);
							}
							else
								excludedCount++;
						}
						else
						{
							row.Score = 0;
							test.Insert(row);

							testCount++;
						}

						///////////////////////////////////////////////////////////////////////////////

						totalCount++;
					}
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			_logger.Log("Excluded documents count: " + excludedCount);

			_logger.Log("TEST documents count: " + testCount);

			_logger.Log("TRAIN positive documents count: " + trainPositiveCount);
			_logger.Log("TRAIN negative documents count: " + trainNegativeCount);

			///////////////////////////////////////////////////////////////////////////////

			AssertDocumentsCount(test, train);

			///////////////////////////////////////////////////////////////////////////////

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
