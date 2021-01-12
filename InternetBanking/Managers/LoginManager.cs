using System.Collections.Generic;
using System.Linq;
using Models;
using ClassLibrary;

namespace Managers
{
    public class LoginManager
    {

        private readonly string _connectionString;
        public Dictionary<string, Login> Logins { get; }


        public LoginManager(string connectionString)
        {
            _connectionString = connectionString;

            // Create connection
            var connection = _connectionString.CreateConnection();

            // Create command
            var command = connection.CreateCommand();
            command.CommandText = "select * from Login";

            // Get table from database
            var loginTable = command.GetDataTable();

            // Construct Login objects
            foreach (var x in loginTable.Select())
            {
                var loginID = (string)x["LoginID"];
                var customerID = (int)x["CustomerID"];
                var customer = CustomerManager.Customers[customerID];

                var login = new Login(loginID, customer, (string)x["PasswordHash"]);

                Logins.Add(loginID, login);
            }
        }


        public void InsertLogin(Login login)
        {
            // Connect to database
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            // Create command
            var command = connection.CreateCommand();

            // Parameterised SQL - insert login data into table
            command.CommandText = "insert into Login (LoginID, CustomerID, PasswordHash) values (@loginID, @customerID, @passwordHash)";
            command.Parameters.AddWithValue("loginID", login.LoginID);
            command.Parameters.AddWithValue("customerID", login.Customer.CustomerID);
            command.Parameters.AddWithValue("passwordHash", login.PasswordHash);

            command.ExecuteNonQuery();
        }

    }
}
