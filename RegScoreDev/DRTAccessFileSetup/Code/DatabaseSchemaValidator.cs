using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DRTAccessFileSetup.Code
{
	public class DatabaseSchemaValidator
	{
		#region Constants

		protected const string _schemasFolderName = "Schemas";
		protected const string _schemasFilePattern = "*.xml";

		#endregion

		#region Fields

		protected List<TableInfo> _tables;

		#endregion

		#region Ctors
		public DatabaseSchemaValidator()
		{
			_tables = new List<TableInfo>();
		}

		#endregion

		#region Operations

		public void DumpSchemaXml(string connectionString, List<string> tableNames)
		{
			var connection = new OleDbConnection(connectionString);

			connection.Open();

			try
			{
				var tableValidator = new TableSchemaValidator(connection);

				var tables = new List<TableInfo>();

				///////////////////////////////////////////////////////////////////////////////

				foreach (var table in tableNames)
				{
					var xml = tableValidator.GetTableSchemaXml(table);

					tables.Add(new TableInfo
					           {
						           TableName = table,
						           ProperSchemaXml = xml
					           });
				}

				///////////////////////////////////////////////////////////////////////////////

				SaveSchemas(tables);
			}
			finally
			{
				connection.Close();
			}
		}

		public List<string> GetSchemaDifferences(string connectionString)
		{
			var connection = new OleDbConnection(connectionString);

			connection.Open();

			///////////////////////////////////////////////////////////////////////////////

			try
			{
				LoadSchemas();

				var tableValidator = new TableSchemaValidator(connection);

				///////////////////////////////////////////////////////////////////////////////

				foreach (var table in _tables)
				{
					table.Differences = tableValidator.GetSchemaDifferences(table.TableName, table.ProperSchemaXml);
					if (table.Differences.Any())
						table.Differences.Add("");
				}
			}
			finally
			{
				connection.Close();
			}

			///////////////////////////////////////////////////////////////////////////////

			return _tables.SelectMany(x => x.Differences)
			              .ToList();
		}

		#endregion

		#region Implementation

		protected void LoadSchemas()
		{
			string sqlFolderPath = GetSqlFolderPath();
			if (Directory.Exists(sqlFolderPath))
			{
				var di = new DirectoryInfo(sqlFolderPath);
				var files = di.GetFiles(_schemasFilePattern, SearchOption.TopDirectoryOnly);

				///////////////////////////////////////////////////////////////////////////////

				foreach (var file in files)
				{
					var xml = File.ReadAllText(file.FullName);

					///////////////////////////////////////////////////////////////////////////////

					_tables.Add(new TableInfo
					            {
						            TableName = Path.GetFileNameWithoutExtension(file.Name),
						            ProperSchemaXml = xml
					            });
				}
			}
		}

		protected void SaveSchemas(List<TableInfo> tables)
		{
			var sqlFolderPath = GetSqlFolderPath();
			if (!Directory.Exists(sqlFolderPath))
				Directory.CreateDirectory(sqlFolderPath);

			///////////////////////////////////////////////////////////////////////////////

			foreach (var table in tables)
			{
				var filePath = Path.Combine(sqlFolderPath, table.TableName + ".xml");

				File.WriteAllText(filePath, table.ProperSchemaXml);
			}
		}

		protected string GetSqlFolderPath()
		{
			string sqlFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
			                                                                  .Location), _schemasFolderName);
			return sqlFolderPath;
		}

		#endregion
	}
}
