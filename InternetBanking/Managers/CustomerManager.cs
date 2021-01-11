using System.Collections.Generic;
using System.Linq;
using Models;
using ClassLibrary;

namespace Managers
{
    public class CustomerManager
    {

        private readonly string _connectionString;
        public Dictionary<int, Customer> Customers { get; }


        public CustomerManager(string connectionString)
        {
            _connectionString = connectionString;

            // Using class library - connect to database, execute query, return table and disconnect
            var customerTable = _connectionString.GetDataTable("select * from Customer");

            // Construct Customer objects

        }


        public void InsertCustomer(Customer customer)
        {

            // Connect to database
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            // Create command
            var command = connection.CreateCommand();

            // Parameterised SQL
            command.CommandText = "insert into Customer (CustomerID, Name, Address, City, PostCode) values (@customerID, @name, @address, @city, @postCode)";
            command.Parameters.AddWithValue("CustomerID", customer.CustomerID);
            command.Parameters.AddWithValue("Name", customer.Name);
            command.Parameters.AddWithValue("Address", customer.Address);
            command.Parameters.AddWithValue("City", customer.City);
            command.Parameters.AddWithValue("PostCode", customer.PostCode);

            command.ExecuteNonQuery();
        }
    }
}
