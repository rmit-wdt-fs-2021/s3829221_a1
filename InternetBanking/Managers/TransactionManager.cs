using System;
using System.Collections.Generic;
using System.Linq;
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
            var connection = _connectionString.CreateConnection();

            // Create command
            var command = connection.CreateCommand();
            command.CommandText = "select * from [Transaction] where AccountNumber = @accountNumber";
            command.Parameters.AddWithValue("accountNumber", accountNumber);

            // Get table from database
            var transactionTable = command.GetDataTable();

            // Construct transaction objects
            return transactionTable.Select().Select(x => new Transaction
            {
                TransactionID = (int)x["TransactionID"],
                TransactionType = char.Parse((string)x["TransactionType"]),
                AccountNumber = (int)x["AccountNumber"],
                DestinationAccountNumber = x["DestinationAccountNumber"] == DBNull.Value ? 0 : (int)x["DestinationAccountNumber"],
                Amount = (decimal)x["Amount"],
                Comment = (string)x["Comment"],
                TransactionTimeUtc = (DateTime)x["TransactionTimeUtc"]
            }).ToList();
        }


        public void InsertTransaction(Transaction transaction)
        {

            // Connect to database
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            // Create command
            var command = connection.CreateCommand();

            // Parameterised SQL - insert transaction data into table
            command.CommandText = "insert into [Transaction] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, Comment, TransactionTimeUtc)" +
                "values (@transactionType, @accountNumber, @destinationAccountNumber, @amount, @comment, @transactionTimeUtc)";
            command.Parameters.AddWithValue("transactionType", transaction.TransactionType != '\0' ? transaction.TransactionType : 'D');
            command.Parameters.AddWithValue("accountNumber", transaction.AccountNumber);
            command.Parameters.AddWithValue("destinationAccountNumber", transaction.DestinationAccountNumber != 0 ? transaction.DestinationAccountNumber : DBNull.Value);
            command.Parameters.AddWithValue("amount", transaction.Amount != '\0' ? Math.Round(transaction.Amount, 2) : AccountManager.Accounts[transaction.AccountNumber].Balance);
            command.Parameters.AddWithValue("comment", (object)transaction.Comment ?? DBNull.Value);
            command.Parameters.AddWithValue("transactionTimeUtc", transaction.TransactionTimeUtc);

            command.ExecuteNonQuery();
        }
    }
}
