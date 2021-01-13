using System.Collections.Generic;
using Models;
using ClassLibrary;
using System;

namespace Managers
{
    public class CustomerManager
    {

        private readonly string _connectionString;
        public static Dictionary<int, Customer> Customers { get; }


        public CustomerManager(string connectionString)
        {
            _connectionString = connectionString;

            // Create connection
            var connection = _connectionString.CreateConnection();

            // Create command
            var command = connection.CreateCommand();
            command.CommandText = "select * from Customer";

            // Get table from database
            var customerTable = command.GetDataTable();

            var accountManager = new AccountManager(_connectionString);

            // Construct Customer objects
            foreach (var x in customerTable.Select())
            {
                var customerID = (int)x["CustomerID"];
                var name = (string)x["Name"];
                var address = (string)x["Address"];
                var city = (string)x["City"];
                var postCode = (string)x["PostCode"];
                var accounts = accountManager.getAccounts(customerID);

                var customer = new Customer(customerID, name, address, city, postCode, accounts);

                Customers.Add(customerID, customer);
            }
        }


        public void InsertCustomer(Customer customer)
        {

            // Connect to database
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            // Create command
            var command = connection.CreateCommand();

            // Parameterised SQL - insert customer data into table
            command.CommandText = "insert into Customer (CustomerID, Name, Address, City, PostCode) values (@customerID, @name, @address, @city, @postCode)";
            command.Parameters.AddWithValue("CustomerID", customer.CustomerID);
            command.Parameters.AddWithValue("Name", customer.Name);
            command.Parameters.AddWithValue("Address", (object)customer.Address ?? DBNull.Value);
            command.Parameters.AddWithValue("City", (object)customer.City ?? DBNull.Value);
            command.Parameters.AddWithValue("PostCode", (object)customer.PostCode ?? DBNull.Value);

            command.ExecuteNonQuery();
        }
    }
}
