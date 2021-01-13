using System.Collections.Generic;
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
            var AccountTable = command.GetDataTable();

            var customer = CustomerManager.Customers[customerID];

            var transactionManager = new TransactionManager(_connectionString);

            // Construct Account objects
            foreach (var x in AccountTable.Select())
            {
                var accountNumber = (int)x["AccountNumber"];
                var accountType = (char)x["AccountType"];
                var balance = (double)x["Balance"];
                var transactions = transactionManager.GetTransactions(accountNumber);

                var account = new Account(accountNumber, accountType, customer, balance, transactions);

                Accounts.Add(accountNumber, account);
            }
            
            return Accounts;
        }


        public void InsertAccount(Account account)
        {

            // Connect to database
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            // Create command
            var command = connection.CreateCommand();

            // Parameterised SQL - insert account data into table
            command.CommandText = "insert into Account (AccountNumber, AccountType, CustomerID, Balance) values (@accountNumber, @accountType, @customerID, @balance)";
            command.Parameters.AddWithValue("accountNumber", account.AccountNumber);
            command.Parameters.AddWithValue("accountType", account.AccountType);
            command.Parameters.AddWithValue("customerID", account.Customer.CustomerID);
            command.Parameters.AddWithValue("balance", account.Balance);

            command.ExecuteNonQuery();
        }
    }
}
