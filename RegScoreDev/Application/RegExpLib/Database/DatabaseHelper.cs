using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

using Helpers;

namespace RegExpLib.Database
{
	public static class DatabaseHelper
	{
		#region Static operations

		public static OleDbConnection CreateConnection(string databasePath, string password)
		{
			bool invalidPassword;
			return new OleDbConnection(ConnectionStringHelper.GetConnectionString(databasePath, password, out invalidPassword));
		}

		public static IEnumerable<IDataRecord> AsDataRecordEnumerable(IEnumerable<DataRow> rowCollection)
		{
			foreach (var row in rowCollection)
			{
				if (row.RowState != DataRowState.Deleted)
					yield return new DataRowAdapter(row);
			}
		}

		public static IEnumerable<IDataRecord> GetDataRecords(OleDbConnection connection, string query)
		{
			var command = new OleDbCommand(query, connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader != null)
				{
					foreach (IDataRecord record in reader)
					{
						yield return record;
					}
				}
				else
					throw new Exception("Failed to read rows");
			}
		}

		public static IEnumerable<DataRow> GetDataRows(OleDbConnection connection, string query)
		{
			var table = new DataTable();

			var adapter = new OleDbDataAdapter(query, connection);
			adapter.Fill(table);

			return table.AsEnumerable()
						.Cast<DataRow>();
		}

		public static long GetRowsCount(OleDbConnection connection, string tableName, string where = null)
		{
			var cmdText = String.Format("SELECT COUNT (*) FROM {0}", tableName);
			if (!String.IsNullOrEmpty(where))
				cmdText += " WHERE " + where;

			var command = new OleDbCommand(cmdText, connection);

			return Convert.ToInt64(command.ExecuteScalar());
		}

        public static int GetNoteColumnCount(OleDbConnection connection, string tableName, string colBaseName = "NOTE_TEXT")
        {
            int count = 0;

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM " + tableName;
            var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);


            DataTable table = reader.GetSchemaTable();
            var nameCol = table.Columns["ColumnName"];


            foreach (DataRow row in table.Rows)
            {
                if (row[nameCol].ToString().Contains(colBaseName))
                    count++;
                Console.WriteLine(row[nameCol]);
            }

            return count;
        }

        public static string GetNoteText(OleDbConnection connection, double documentID, int index = 0)
        {
            
            var cmd = connection.CreateCommand();

            if (index == 0)
                cmd.CommandText = "SELECT NOTE_TEXT FROM Documents WHERE ED_ENC_NUM = " + documentID;
            else
                cmd.CommandText = "SELECT NOTE_TEXT" + index + " FROM Documents WHERE ED_ENC_NUM = " + documentID;


            var result = cmd.ExecuteScalar();           

            return result.ToString();
        }

        public static T GetValue<T>(DataRow row, string columnName) where T : class
		{
			var result = default(T);

			try
			{
				if (row.Table.Columns.Contains(columnName))
				{
					if (!row.IsNull(columnName))
						result = (T)row[columnName];
					else
						return null;
				}
			}
			catch { }

			return result;
		}

		public static void SetValue<T>(DataRow row, string columnName, T value)
		{
			if (row.Table.Columns.Contains(columnName))
			{
				if (value != null)
					row[columnName] = value;
				else
					row[columnName] = DBNull.Value;
			}
		}

		public static int GetInt32ValueInvariant(IDataRecord record, int columnIndex)
		{
			if (record.IsDBNull(columnIndex))
				return 0;

			///////////////////////////////////////////////////////////////////////////////

			try
			{
				return record.GetInt32(columnIndex);
			}
			catch { }

			try
			{
				return Convert.ToInt32(record.GetDouble(columnIndex));
			}
			catch { }

			return 0;
		}

		#endregion
	}
}
