using System;
using System.Collections.Generic;
using ClassLibrary;
using Models;

namespace Managers
{
    class TransactionManager
    {

        private readonly string _connectionString;


        public TransactionManager(string connectionString)
        {
            _connectionString = connectionString;
        }


        public List<Transaction> GetTransactions(int accountNumber)
        {
            // Create connection
            using var connection = _connectionString.CreateConnection();

            // Create command
            var command = connection.CreateCommand();
            command.CommandText = "select * from Transaction where AccountNumber = @accountNumber";
            command.Parameters.AddWithValue("accountNumber", accountNumber);

            // Get table from database
            var transactionTable = command.GetDataTable();

            var account = AccountManager.Accounts[accountNumber];

            // Construct transaction objects
            foreach (var x in transactionTable.Select())
            {
                var transactionID = (int)x["TransactionID"];
                var transactionType = (char)x["TransactionType"];
                var destinationAccount = AccountManager.Accounts[(int)x["DestinationAccountNumber"]];
                var amount = (double)x["Amount"];
                var comment = (string)x["Comment"];
                var transactionTimeUtc = (DateTime)x["TransactionTimeUtc"];

                var transaction = new Transaction(transactionID, transactionType, account, destinationAccount, amount, comment, transactionTimeUtc);

                account.Transactions.Add(transaction);
            }

            return account.Transactions;
        }
    }
}
