using System.Collections.Generic;
using Models;
using ClassLibrary;

namespace Managers
{
    public class AccountManager
    {

        private readonly string _connectionString;


        public AccountManager(string connectionString)
        {
            _connectionString = connectionString;
        }


        public List<Account> getAccounts(int customerID)
        {
            // Create connection
            var connection = _connectionString.CreateConnection();

            // Create command
            var command = connection.CreateCommand();
            command.CommandText = "select * from Account where CustomerID = @customerID";
            command.Parameters.AddWithValue("customerID", customerID);

            // Get table from database
            var accountTable = command.GetDataTable();

            var accountList = new List<Account>();

            var transactionManager = new TransactionManager(_connectionString);

            // Construct Account objects
            foreach (var x in accountTable.Select())
            {
                var account = new Account
                {
                    AccountNumber = (int)x["AccountNumber"],
                    AccountType = char.Parse((string)x["AccountType"]),
                    CustomerID = (int)x["CustomerID"],
                    Balance = (decimal)x["Balance"],
                    Transactions = transactionManager.GetTransactions((int)x["AccountNumber"])
                };
                Container.Accounts.Add(account.AccountNumber, account);
                accountList.Add(account);
            }

            return accountList;
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
            command.Parameters.AddWithValue("customerID", account.CustomerID);
            command.Parameters.AddWithValue("balance", account.Balance);

            command.ExecuteNonQuery();
        }


        public void UpdateAccount(int accountNumber, decimal amount)
        {
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "update Account set Balance += @amount where AccountNumber = @accountNumber";
            command.Parameters.AddWithValue("amount", amount);
            command.Parameters.AddWithValue("accountNumber", accountNumber);

            command.ExecuteNonQuery();
        }
    }
}
