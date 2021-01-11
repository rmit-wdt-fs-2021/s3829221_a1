using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Transaction
    {

        // Properties
        public int TransactionID { get; }
        public char TransactionType { get; }
        public Account PayerAccount { get; }
        public Account PayeeAccount { get; }
        public double Amount { get; }
        public string Comment { get; }
        public DateTime TransactionTimeUtc { get; }
    }
}
