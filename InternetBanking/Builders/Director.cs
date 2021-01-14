using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Builders
{
    class Director
    {
        private TransactionBuilder _transactionBuilder;


        // Construct the builder object for given concrete builder
        public void SetTransactionBuilder(TransactionBuilder transactionBuilder)
        {
            _transactionBuilder = transactionBuilder;
        }


        // Call methods required for building the transaction
        public void ConstructTransaction(int accountNumber, int destinationAccountNumber, decimal amount, string comment)
        {
            _transactionBuilder.CreateTransaction();
            _transactionBuilder.BuildType();
            _transactionBuilder.BuildTime();
            _transactionBuilder.BuildOthers(accountNumber, destinationAccountNumber, amount, comment);
        }


        // Return the transaction after creation
        public Transaction GetTransaction()
        {
            return _transactionBuilder.GetTransaction();
        }
    }
}
