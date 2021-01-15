using System.Collections.Generic;
using Models;
using ClassLibrary;
using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Managers
{
    public class CustomerManager
    {

        private readonly string _connectionString;


        public CustomerManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Instantiate()
        { 
            // Create connection
            var connection = _connectionString.CreateConnection();

            // Create command
            var command = connection.CreateCommand();
            command.CommandText = "select * from Customer";

            //Get table from database
            var customerTable = command.GetDataTable();

            var accountManager = new AccountManager(_connectionString);

            // Construct Customer objects
            foreach (var x in customerTable.Select())
            {
                var customer = new Customer
                {
                    CustomerID = (int)x["CustomerID"],
                    Name = (string)x["Name"],
                    Address = x["Address"] == DBNull.Value ? null : (string)x["Address"],
                    City = x["City"] == DBNull.Value ? null : (string)x["City"],
                    PostCode = x["PostCode"] == DBNull.Value ? null : (string)x["PostCode"],
                    Accounts = accountManager.getAccounts((int)x["CustomerID"])
                };
                Container.Customers.Add(customer.CustomerID, customer);
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
