using System.Collections.Generic;

namespace Models
{
    public class Account
    {

        // Properties
        public int AccountNumber { get; set; }
        public char AccountType { get; set; }
        public int CustomerID { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }


        public Account()
        {
            // Sort transactions by time in descending order
            Transactions.Sort();
        }
    }
}
