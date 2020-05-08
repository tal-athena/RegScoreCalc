using System;
using System.Data.OleDb;

namespace Helpers
{
	public class ConnectionStringHelper
	{
		public static string GetConnectionStringWithoutPassword(string strFilePath)
		{
			bool invalidPassword;
			return GetConnectionString(strFilePath, null, out invalidPassword);
		}

		public static string GetConnectionString(string strFilePath, string password, out bool invalidPassword)
		{
			const string ace12ProviderString = "Microsoft.ACE.OLEDB.12.0";
			const string ace15ProviderString = "Microsoft.ACE.OLEDB.15.0";

			//const string connectionFormatString = "Provider={0};Data Source=\"{1}\";Jet OLEDB:Database Password=111aaa___;";

			var connectionFormatString = "Provider={0};Data Source=\"{1}\";";

			if (!String.IsNullOrEmpty(password))
				connectionFormatString += $"Jet OLEDB:Database Password={password};";

			///////////////////////////////////////////////////////////////////////////////

			var connectionString = String.Format(connectionFormatString, ace12ProviderString, strFilePath);
			if (TryConnectToDatabase(connectionString, out invalidPassword))
				return connectionString;

			if (invalidPassword)
				return String.Empty;

			connectionString = String.Format(connectionFormatString, ace15ProviderString, strFilePath);
			if (TryConnectToDatabase(connectionString, out invalidPassword))
				return connectionString;

			///////////////////////////////////////////////////////////////////////////////

			return String.Empty;
		}

		private static bool TryConnectToDatabase(string connectionString, out bool invalidPassword)
		{
			var result = false;

			invalidPassword = false;

			try
			{
				using (var connection = new OleDbConnection(connectionString))
				{
					connection.Open();

					result = true;
				}
			}
			catch (Exception ex)
			{
				if (ex.HResult == -2147217843 || ex.Message.IndexOf("password", StringComparison.InvariantCultureIgnoreCase) != -1)
					invalidPassword = true;
			}

			return result;
		}
	}
}
