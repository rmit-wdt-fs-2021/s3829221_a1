using System.Collections.Generic;
using Models;
using ClassLibrary;

namespace Managers
{
    public class LoginManager
    {

        private readonly string _connectionString;


        public LoginManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Instantiate()
        { 
            // Create connection
            using var connection = _connectionString.CreateConnection();

            // Create command
            var command = connection.CreateCommand();
            command.CommandText = "select * from Login";

            // Get table from database
            var loginTable = command.GetDataTable();

            // Construct Login objects
            foreach (var x in loginTable.Select())
            {
                var login = new Login
                {
                    LoginID = (string)x["LoginID"],
                    CustomerID = (int)x["CustomerID"],
                    PasswordHash = (string)x["PasswordHash"]
                };

                Container.Logins.Add(login.LoginID, login);
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
            command.Parameters.AddWithValue("customerID", login.CustomerID);
            command.Parameters.AddWithValue("passwordHash", login.PasswordHash);

            command.ExecuteNonQuery();
        }

    }
}
