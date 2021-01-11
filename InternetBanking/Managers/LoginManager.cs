using System.Collections.Generic;
using System.Linq;
using Models;
using ClassLibrary;

namespace Managers
{
    class LoginManager
    {

        private readonly string _connectionString;
        public Dictionary<string, Login> Logins { get; }


        public LoginManager(string connectionString)
        {
            _connectionString = connectionString;

            // Using class library - connect to database, execute query, return table and disconnect
            var loginTable = _connectionString.GetDataTable("select * from Login");

            var customerManager = new CustomerManager(_connectionString);

            // Construct Login objects
            foreach (var x in loginTable.Select())
            {
                var loginID = (string) x["LoginID"];
                var customerID = (int) x["CustomerID"];
                var customer = customerManager.Customers[customerID];
                var login = new Login(loginID, customer, (string) x["PasswordHash"]);
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

            // Parameterised SQL
            command.CommandText = "insert into Login (LoginID, CustomerID, PasswordHash) values (@loginID, @customerID, @passwordHash)";
            command.Parameters.AddWithValue("loginID", login.LoginID);
            command.Parameters.AddWithValue("customerID", login.Customer.CustomerID);
            command.Parameters.AddWithValue("passwordHash", login.PasswordHash);

            command.ExecuteNonQuery();
        }

    }
}
