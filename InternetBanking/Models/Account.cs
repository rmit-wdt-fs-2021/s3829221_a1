using System.Collections.Generic;

namespace Models
{
    public class Account
    {

        // Properties
        public int AccountNumber { get; set; }
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
