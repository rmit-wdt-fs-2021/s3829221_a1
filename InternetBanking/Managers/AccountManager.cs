using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using ClassLibrary;

namespace Managers
{
    public class AccountManager
    {

        private readonly string _connectionString;
        public static Dictionary<int, Account> Accounts { get; }


        public AccountManager(string connectionString)
        {
            _connectionString = connectionString;
        }


        public Dictionary<int, Account> getAccounts(int customerID)
        {
            // Create connection
            using var connection = _connectionString.CreateConnection();

            // Create command
            var command = connection.CreateCommand();
            command.CommandText = "select * from Account where CustomerID = @customerID";
            command.Parameters.AddWithValue("customerID", customerID);

            // Get table from database
            var transactionTable = command.GetDataTable();


        }
    }
}
