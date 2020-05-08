using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegExpMerge
{
    public static class Helpers
    {
        public static string GetConnectionString(string strFilePath)
        {
            string format = strFilePath.EndsWith("mdb") ? "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"{0}\"" : "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"{0}\"";
            //string format = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"{0}\"";


            // Try to make a new connection
            OleDbConnection connection;
            connection = new OleDbConnection();
            connection.ConnectionString = string.Format(format, strFilePath);
            bool success = true;
            try
            {
                connection.Open();
            }
            catch
            {
                success = false;
            }

            connection.Dispose();
            connection = null;
            GC.Collect();

            if (!success) // If ACE fails try JET
            {
                format = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"{0}\"";
            }

            return string.Format(format, strFilePath);
        }
    }
}
