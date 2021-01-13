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


        public void InsertTransaction(Transaction transaction)
        {

            // Connect to database
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            // Create command
            var command = connection.CreateCommand();

            // Parameterised SQL - insert transaction data into table
            command.CommandText = "insert into Transaction (TransactionID, TransactionType, AccountNumber, DestinationAccountNumber, Amount, Comment, TransactionTimeUtc)" +
                "values (@transactionID, @transactionType, @accountNumber, @destinationAccountNumber, @amount, @comment, @transactionTimeUtc)";
            command.Parameters.AddWithValue("transactionID", transaction.TransactionID);
            command.Parameters.AddWithValue("transactionType", transaction.TransactionType);
            command.Parameters.AddWithValue("accountNumber", transaction.Account.AccountNumber);
            command.Parameters.AddWithValue("destinationAccountNumber", (object)transaction.DestinationAccount.AccountNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("amount", transaction.Amount);
            command.Parameters.AddWithValue("comment", (object)transaction.Comment ?? DBNull.Value);
            command.Parameters.AddWithValue("transactionTimeUtc", transaction.TransactionTimeUtc);

            command.ExecuteNonQuery();
        }
    }
}
