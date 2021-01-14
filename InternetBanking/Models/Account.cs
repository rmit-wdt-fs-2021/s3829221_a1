using System.Collections.Generic;
using Builders;
using ClassLibrary;
using Managers;

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


        public Transaction Deposit(int accountNumber, int destinationAccountNumber, decimal amount, string comment)
        {
            Balance += amount;

            var depositBuilder = new DepositBuilder();
            var director = new Director();

            director.SetTransactionBuilder(depositBuilder);
            director.ConstructTransaction(accountNumber, destinationAccountNumber, amount, comment);

            var transaction = director.GetTransaction();
            Transactions.Add(transaction);

            return transaction;
        }


        public Transaction Withdraw(int accountNumber, int destinationAccountNumber, decimal amount, string comment)
        {
            if (AccountType == 'S' && Balance - amount < 0)
                throw new MinBalanceBreachException(0);

            else if (AccountType == 'C' && Balance - amount < 200)
                throw new MinBalanceBreachException(200);

            else
            {
                Balance -= amount;

                var withdrawalBuilder = new WithdrawalBuilder();
                var director = new Director();

                director.SetTransactionBuilder(withdrawalBuilder);
                director.ConstructTransaction(accountNumber, destinationAccountNumber, amount, comment);

                var transaction = director.GetTransaction();
                Transactions.Add(transaction);

                return transaction;
            }
        }


        public Transaction TransferOut(int accountNumber, int destinationAccountNumber, decimal amount, string comment)
        {
            if (!AccountManager.Accounts.ContainsKey(destinationAccountNumber) && destinationAccountNumber != 0)
            {
                throw new ObjectNotFoundException("account");
            }

            else
            {
                if (AccountType == 'S' && Balance - amount < 0)
                    throw new MinBalanceBreachException(0);

                else if (AccountType == 'C' && Balance - amount < 200)
                    throw new MinBalanceBreachException(200);

                else
                {
                    Balance -= amount;

                    var transferBuilder = new TransferBuilder();
                    var director = new Director();

                    director.SetTransactionBuilder(transferBuilder);
                    director.ConstructTransaction(accountNumber, destinationAccountNumber, amount, comment);

                    var transaction = director.GetTransaction();
                    Transactions.Add(transaction);

                    return transaction;
                } 
            }
        }


        public Transaction TransferIn(int accountNumber, int destinationAccountNumber, decimal amount, string comment)
        {
            Balance += amount;

            var transferBuilder = new TransferBuilder();
            var director = new Director();

            director.SetTransactionBuilder(transferBuilder);
            director.ConstructTransaction(accountNumber, destinationAccountNumber, amount, comment);

            var transaction = director.GetTransaction();
            Transactions.Add(transaction);

            return transaction;
        }


        public Transaction ServiceCharge(int accountNumber, int destinationAccountNumber, decimal amount, string comment)
        {
            Balance += amount;

            var serviceChargeBuilder = new ServiceChargeBuilder();
            var director = new Director();

            director.SetTransactionBuilder(serviceChargeBuilder);
            director.ConstructTransaction(accountNumber, destinationAccountNumber, amount, comment);

            var transaction = director.GetTransaction();
            Transactions.Add(transaction);

            return transaction;
        }
    }
}
