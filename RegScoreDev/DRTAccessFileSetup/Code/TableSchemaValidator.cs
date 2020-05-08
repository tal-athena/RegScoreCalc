using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace DRTAccessFileSetup.Code
{
	public class TableSchemaValidator
	{
		#region Fields

		protected OleDbConnection _connection;

		#endregion

		#region Ctors
		public TableSchemaValidator(OleDbConnection connection)
		{
			_connection = connection;
		}

		#endregion

		#region Operations

		public string GetTableSchemaXml(string tableName)
		{
			try
			{
				var columnsList = SqlGetActualColumnsList(tableName);

				return SerializeColumns(columnsList);
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}

			return String.Empty;
		}

		public List<string> GetSchemaDifferences(string testTableName, string properTableSchemaXml)
		{
			var errors = new List<string>();

			try
			{
				var testColumnsList = SqlGetActualColumnsList(testTableName);
				var properColumnsList = DeserializeColumns(properTableSchemaXml);

				///////////////////////////////////////////////////////////////////////////////

				foreach (var properColumn in properColumnsList)
				{
					var testColumn = testColumnsList.FirstOrDefault(x => String.Compare(x.Name, properColumn.Name, StringComparison.InvariantCultureIgnoreCase) == 0);
					if (testColumn != null)
					{
						var differences = properColumn.GetColumnDifferences(testColumn);
						if (differences.Any())
						{
							errors.AddRange(differences);
							errors.Add("");
						}
					}
					else
					{
						errors.Add(String.Format("Column '{0}' of type '{1}' does not exist", properColumn.Name, properColumn.Type));
					}
				}

			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}

			return errors;
		}

		#endregion

		#region Implementation

		protected List<ColumnInfo> SqlGetActualColumnsList(string tableName)
		{
			var actualColumns = new List<ColumnInfo>();

			///////////////////////////////////////////////////////////////////////////////

			using (var cmd = new OleDbCommand("SELECT TOP 1 * FROM " + tableName, _connection))
			using (var reader = cmd.ExecuteReader(CommandBehavior.KeyInfo))
			{
				var table = reader.GetSchemaTable();

				///////////////////////////////////////////////////////////////////////////////

				var colName = table.Columns["ColumnName"];
				var colType = table.Columns["DataType"];
				var colIsKey = table.Columns["IsKey"];
				var colIsLong = table.Columns["IsLong"];
				var colNumericScale = table.Columns["NumericScale"];
				var colColumnSize = table.Columns["ColumnSize"];
				var colNumericPrecision = table.Columns["NumericPrecision"];
				var colIsUnique = table.Columns["IsUnique"];
				var colAllowDBNull = table.Columns["AllowDBNull"];
				var colIsAutoIncrement = table.Columns["IsAutoIncrement"];

				///////////////////////////////////////////////////////////////////////////////

				foreach (var row in table.Rows.Cast<DataRow>())
				{
					actualColumns.Add(new ColumnInfo
					                  {
						                  Name = Convert.ToString(row[colName]),
						                  Type = Convert.ToString(row[colType]),
						                  IsKey = Convert.ToString(row[colIsKey]),
						                  IsLong = Convert.ToString(row[colIsLong]),
						                  NumericScale = Convert.ToString(row[colNumericScale]),
						                  ColumnSize = Convert.ToString(row[colColumnSize]),
						                  NumericPrecision = Convert.ToString(row[colNumericPrecision]),
						                  IsUnique = Convert.ToString(row[colIsUnique]),
						                  AllowDBNull = Convert.ToString(row[colAllowDBNull]),
						                  IsAutoIncrement = Convert.ToString(row[colIsAutoIncrement])
					                  });
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			return actualColumns;
		}

		public string SerializeColumns(List<ColumnInfo> columnsList)
		{
			var writer = new StringWriter();

			var serializer = CreateSerializer();
			serializer.Serialize(writer, columnsList);

			return writer.GetStringBuilder().ToString();
		}

		public List<ColumnInfo> DeserializeColumns(string xml)
		{
			var reader = new StringReader(xml);

			var serializer = CreateSerializer();
			var columnsList = (List<ColumnInfo>)serializer.Deserialize(reader);

			return columnsList;
		}

		protected XmlSerializer CreateSerializer()
		{
			return new XmlSerializer(typeof(List<ColumnInfo>));
		}

		#endregion
	}
}
