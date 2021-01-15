using System.Collections.Generic;
using Builders;
using ClassLibrary;
using Managers;
using System.Linq;
using System;

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

            Console.Clear();
            Console.WriteLine("A deposit of {0:C} has been made to your account {1}", amount, accountNumber);
            Console.WriteLine();

            return transaction;
        }


        public List<Transaction> Withdraw(int accountNumber, int destinationAccountNumber, decimal amount, string comment)
        {
            var transactionList = new List<Transaction>();

            // Count the number of transactions where service charge may apply (withdrwal & transfer-out)
            var result = from transaction in Transactions
                         where transaction.TransactionType == 'W' || transaction.TransactionType == 'T' && transaction.DestinationAccountNumber != 0
                         select transaction;
            var count = result.Count();

            var serviceFee = count < 4 ? 0 : (decimal)0.1;

            if (AccountType == 'S' && Balance - amount - serviceFee < 0)
            {
                Console.WriteLine("Withdrawal request: ");
                Console.WriteLine("Withdrawl amount: {0:C}", amount);
                Console.WriteLine("Service charge:   {0:C}", serviceFee);
                Console.WriteLine();
                throw new MinBalanceBreachException(0);
            }
                
            else if (AccountType == 'C' && Balance - amount - serviceFee < 200)
            {
                Console.WriteLine("Withdrawal request: ");
                Console.WriteLine("Withdrawl amount: {0:C}", amount);
                Console.WriteLine("Service charge:   {0:C}", serviceFee);
                Console.WriteLine();
                throw new MinBalanceBreachException(200);
            }
                
            else
            {
                Balance -= amount;

                var withdrawalBuilder = new WithdrawalBuilder();
                var withdrawalDirector = new Director();

                withdrawalDirector.SetTransactionBuilder(withdrawalBuilder);
                withdrawalDirector.ConstructTransaction(accountNumber, destinationAccountNumber, amount, comment);

                var withdrawal = withdrawalDirector.GetTransaction();
                Transactions.Add(withdrawal);
                transactionList.Add(withdrawal);

                Console.Clear();
                Console.WriteLine("A withdrawal of {0:C} has been made from your account {1}.", amount, accountNumber);
                Console.WriteLine();

                // If service charge applies
                if (serviceFee != 0)
                {
                    Balance -= serviceFee;

                    var serviceChargeBuilder = new ServiceChargeBuilder();
                    var serviceChargeDirector = new Director();

                    serviceChargeDirector.SetTransactionBuilder(serviceChargeBuilder);
                    serviceChargeDirector.ConstructTransaction(accountNumber, 0, serviceFee, null);

                    var serviceCharge = serviceChargeDirector.GetTransaction();
                    Transactions.Add(serviceCharge);
                    transactionList.Add(serviceCharge);

                    Console.WriteLine("A service charge of {0} has been applied.", serviceFee);
                    Console.WriteLine();
                }

                return transactionList;
            }
        }


        public List<Transaction> Transfer(int accountNumber, int destinationAccountNumber, decimal amount, string comment)
        {
            var transactionList = new List<Transaction>();

            // Count the number of transactions where service charge may apply (withdrwal & transfer-out)
            var result = from transaction in Transactions
                         where transaction.TransactionType == 'W' || transaction.TransactionType == 'T' && transaction.DestinationAccountNumber != 0
                         select transaction;
            var count = result.Count();

            var serviceFee = count < 4 ? 0 : (decimal)0.2;

            if (!Container.Accounts.ContainsKey(destinationAccountNumber))
            {
                Console.Clear();
                Console.Write("Transfer rejected: ");
                throw new ObjectNotFoundException("account");
            }

            else
            {
                if (AccountType == 'S' && Balance - amount - serviceFee < 0)
                {
                    Console.WriteLine("Transfer request: ");
                    Console.WriteLine("Transfer amount: {0:C}", amount);
                    Console.WriteLine("Service charge:  {0:C}", serviceFee);
                    Console.WriteLine();
                    throw new MinBalanceBreachException(0);
                }

                else if (AccountType == 'C' && Balance - amount - serviceFee < 200)
                {
                    Console.WriteLine("Transfer request: ");
                    Console.WriteLine("Transfer amount: {0:C}", amount);
                    Console.WriteLine("Service charge:  {0:C}", serviceFee);
                    Console.WriteLine();
                    throw new MinBalanceBreachException(200);
                }

                else
                {
                    // Debit this account
                    Balance -= amount;

                    var transferOutBuilder = new TransferBuilder();
                    var director = new Director();

                    director.SetTransactionBuilder(transferOutBuilder);
                    director.ConstructTransaction(accountNumber, destinationAccountNumber, amount, comment);

                    var transferOut = director.GetTransaction();
                    Transactions.Add(transferOut);
                    transactionList.Add(transferOut);

                    // Credit destination account
                    var destinationAccount = Container.Accounts[destinationAccountNumber];
                    destinationAccount.Balance += amount;

                    var transferInBuilder = new TransferBuilder();
                    var director2 = new Director();

                    director2.SetTransactionBuilder(transferInBuilder);
                    director2.ConstructTransaction(destinationAccountNumber, 0, amount, comment);

                    var transferIn = director2.GetTransaction();
                    Transactions.Add(transferIn);
                    transactionList.Add(transferIn);

                    Console.Clear();
                    Console.WriteLine("A transfer of {0:C} has been made from account {1} to account {2}.", amount, accountNumber, destinationAccountNumber);
                    Console.WriteLine();

                    // If service charge applies
                    if (serviceFee != 0)
                    {
                        Balance -= serviceFee;

                        var serviceChargeBuilder = new ServiceChargeBuilder();
                        var serviceChargeDirector = new Director();

                        serviceChargeDirector.SetTransactionBuilder(serviceChargeBuilder);
                        serviceChargeDirector.ConstructTransaction(accountNumber, 0, serviceFee, null);

                        var serviceCharge = serviceChargeDirector.GetTransaction();
                        Transactions.Add(serviceCharge);
                        transactionList.Add(serviceCharge);

                        Console.WriteLine("A service charge of {0} has been applied.", serviceFee);
                        Console.WriteLine();
                    }

                    return transactionList;
                } 
            }
        }
    }
}
