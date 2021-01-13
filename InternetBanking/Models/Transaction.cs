using System;
using Utilities;

namespace Models
{
    public class Transaction
    {

        // Properties
        public int TransactionID { get; }
        public char TransactionType { get; }
        public Account Account { get; }
        public Account DestinationAccount { get; }
        public double Amount { get; }
        public string Comment { get; }
        public DateTime TransactionTimeUtc { get; }


        public Transaction(char transactionType, Account account, double amount, string comment)
        {
            TransactionID = account.CreateTransactionID();
            TransactionType = transactionType;
            Account = account;
            Amount = amount;
            Comment = comment;
            TransactionTimeUtc = DateTime.UtcNow;
        }


        public Transaction(char transactionType, Account account, double amount, string comment, DateTime transactionTimeUtc) :
            this(transactionType, account, amount, comment)
        {
            TransactionTimeUtc = transactionTimeUtc;
        }


        public Transaction(char transactionType, Account account, Account destinationAccount, double amount, string comment) : 
            this(transactionType, account, amount, comment)
        {
            DestinationAccount = destinationAccount;
        }


        public Transaction(int transactionID, char transactionType, Account account, Account destinationAccount, double amount, string comment, DateTime transactionTimeUtc) :
            this(transactionType, account, destinationAccount, amount, comment)
        {
            TransactionID = transactionID;
            TransactionTimeUtc = transactionTimeUtc;
        }
    }
}
