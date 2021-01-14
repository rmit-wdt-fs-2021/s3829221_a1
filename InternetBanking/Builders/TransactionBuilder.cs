using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Builders
{
    public abstract class TransactionBuilder
    {
        protected Transaction Transaction;


        public Transaction GetTransaction()
        {
            return Transaction;
        }


        public void CreateTransaction()
        {
            Transaction = new Transaction();
        }


        public abstract void BuildType();

        public void BuildTime()
        {
            Transaction.TransactionTimeUtc = DateTime.UtcNow;
        }

        public void BuildOthers(int accountNumber, int destinationAccountNumber, decimal amount, string comment)
        {
            Transaction.AccountNumber = accountNumber;
            Transaction.DestinationAccountNumber = destinationAccountNumber;
            Transaction.Amount = amount;
            Transaction.Comment = comment;
        }
    }
}
