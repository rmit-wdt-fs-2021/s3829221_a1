using Microsoft.Data.SqlClient;
using System.Data;

namespace ClassLibrary
{
    public static class SqlUtilities
    {

        public static SqlConnection CreateConnection(this string connectionString) =>
            new SqlConnection(connectionString);


        public static DataTable GetDataTable(this string connectionString, string query)
        {
            // Create connection to the database
            using var connection = new SqlConnection(connectionString);

            // Create command
            var cmd = connection.CreateCommand();
            cmd.CommandText = query;

            // Create table and fill data into it
            var table = new DataTable();
            new SqlDataAdapter(cmd).Fill(table);

            return table;
        }
    }
}
