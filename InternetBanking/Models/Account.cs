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


        public Account(int accountNumber, char accountType, Customer customer, double balance, List<Transaction> transactions)
        {
            AccountNumber = accountNumber;
            AccountType = accountType;
            Customer = customer;
            Balance = balance;
            Transactions = transactions;
        }
    }
}
