using Microsoft.Data.SqlClient;
using System.Data;

namespace ClassLibrary
{
    public static class SqlUtilities
    {

        public static SqlConnection CreateConnection(this string connectionString) =>
            new SqlConnection(connectionString);

        
        public static DataTable GetDataTable(this SqlCommand query)
        {
            var table = new DataTable();
            new SqlDataAdapter(query).Fill(table);

            return table;
        }
    }
}
