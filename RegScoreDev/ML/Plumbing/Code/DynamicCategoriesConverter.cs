using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

using Helpers;

namespace Plumbing.Code
{
	internal class DynamicCategoriesConverter
	{
		private readonly Logger _logger;

		public DynamicCategoriesConverter(Logger logger)
		{
			_logger = logger;
		}

		public void Convert(string inputFilePath, Parameters parameters)
		{
			if (parameters.dynamicPositiveCategories == null)
				parameters.dynamicPositiveCategories = new string[] { };

			if (parameters.dynamicExcludedCategories == null)
				parameters.dynamicExcludedCategories = new string[] { };

			bool invalidPassword;
			using (var connection = new OleDbConnection(ConnectionStringHelper.GetConnectionString(inputFilePath, parameters.password, out invalidPassword)))
			{
				connection.Open();

				var cmd = connection.CreateCommand();
				cmd.CommandText = "SELECT ID, Title FROM DynamicColumnCategories WHERE DynamicColumnID = @ID";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@ID", parameters.dynamicColumnID);

				var categoriesTable = new DataTable();
				var adapter = new OleDbDataAdapter(cmd);
				adapter.Fill(categoriesTable);

				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@categoryID", 0);
				cmd.Parameters.AddWithValue("@categoryTitle", "");

				var totalRowsUpdated = 0;

				var positiveCategories = new List<int>();
				var excludedCategories = new List<int>();

				var cmdClearCategoryColumn = connection.CreateCommand();
				cmdClearCategoryColumn.CommandText = "UPDATE Documents SET Category = NULL";
				cmdClearCategoryColumn.ExecuteNonQuery();

				foreach (var row in categoriesTable.Rows.Cast<DataRow>())
				{
					var categoryID = (int) row[0];
					var categoryTitle = (string) row[1];

					if (parameters.dynamicPositiveCategories.Any(x => x == categoryTitle))
						positiveCategories.Add(categoryID);
					else if (parameters.dynamicExcludedCategories.Any(x => x == categoryTitle))
						excludedCategories.Add(categoryID);

					///////////////////////////////////////////////////////////////////////////////

					cmd.CommandText = "UPDATE Documents SET Category = @categoryID WHERE [" + parameters.dynamicColumnTitle + "] = @categoryTitle";
					cmd.Parameters[0].Value = categoryID;
					cmd.Parameters[1].Value = categoryTitle;

					totalRowsUpdated += cmd.ExecuteNonQuery();
				}

				///////////////////////////////////////////////////////////////////////////////

				parameters.positiveCategories = positiveCategories.ToArray();
				parameters.excludedCategories = excludedCategories.ToArray();

				_logger.Log("Categories converted for " + totalRowsUpdated + " document(s)");
			}
		}
	}
}