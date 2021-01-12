using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Account
    {

        // Properties
        public int AccountNumber { get; }
        public char AccountType { get; }
        public Customer Customer { get; }
        public double Balance { get; }
        public List<Transaction> Transactions { get; }


        public Account(int accountNumber, char accountType, Customer customer, double initialAmount, DateTime openTimeUtc)
        {
            AccountNumber = accountNumber;
            AccountType = accountType;
            Customer = customer;
            Balance += initialAmount;
            Transactions = new List<Transaction>();

            // Record initial deposit
            Transaction initialDeposit = new Transaction('D', this, initialAmount, "Initial deposit", openTimeUtc);
            Transactions.Add(initialDeposit);
        }
    }
}
