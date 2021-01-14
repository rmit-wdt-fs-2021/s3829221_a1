using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public abstract class TransactionBuilder
    {
        protected Transaction transaction;


        public Transaction GetTransaction()
        {
            return transaction;
        }


        public void CreateTransaction()
        {
            transaction = new Transaction();
        }


        public abstract void BuildType();

    }
}
