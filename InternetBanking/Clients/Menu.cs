using System;
using SimpleHashing;
using System.Collections.Generic;
using System.Linq;
using Models;
using Managers;
using ClassLibrary;

namespace Client
{
    class Menu
    {

        private readonly CustomerManager _customerManager;
        private readonly AccountManager _accountManager;
        private readonly TransactionManager _transactionManager;
        private readonly LoginManager _loginManager;
        private Customer _customer;
        private Account _account;


        public Menu(string connectionString)
        {
            _customerManager = new CustomerManager(connectionString);
            _accountManager = new AccountManager(connectionString);
            _transactionManager = new TransactionManager(connectionString);
            _loginManager = new LoginManager(connectionString);
        }


        public void Run()
        {
            while (true)
            {
                Console.Write(
@"Welcome to MCBA Internet Bank
=============================
1. Login
2. Quit

Enter an option: ");

                var option = InputUtilities.EnterOption(1, 2);

                switch (option)
                {
                    case 1:
                        Console.Clear();
                        Login();
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Thanks for using MCBA Internet Bank, good bye :)");
                        return;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }


        public void Login()
        {
            Console.Write("Login ID: ");
            var loginID = Console.ReadLine();
            Console.WriteLine();

            // Verify login ID / Correct login ID
            if (_loginManager.Logins.ContainsKey(loginID))
            {
                var passwordHash = _loginManager.Logins[loginID].PasswordHash;
                var customerID = _loginManager.Logins[loginID].CustomerID;
                var customer = _customerManager.Customers[customerID];

                // Mask password input
                Console.Write("Password: ");
                var pwd = InputUtilities.EnterPwd();
                Console.Clear();

                // Verify password
                var isCorrect = PBKDF2.Verify(passwordHash, pwd);

                // Correct password / after login
                if (isCorrect)
                {
                    _customer = customer;

                    while (true)
                    {
                        Console.WriteLine($"Hello, Ms/Mr. {_customer.Name}!");
                        Console.WriteLine();
                        Console.Write(
    @"  Customer Page
=================
1. Select account
2. Log out

Enter an option: ");

                        var option = InputUtilities.EnterOption(1, 2);

                        switch (option)
                        {
                            case 1:
                                Console.Clear();
                                SelectAccount();
                                break;
                            case 2:
                                Console.Clear();

                                // Remove the current operating customer
                                _customer = null;

                                Console.WriteLine("You have logged out successfully.");
                                Console.WriteLine();
                                return;
                            default:
                                throw new InvalidOperationException();
                        } 
                    }
                }

                // Incorrect password
                else
                {
                    Console.Clear();
                    Console.WriteLine("Incorrect password, please try to log in again.");
                    Console.WriteLine();
                    return;
                }
            }

            // Login ID does not exist
            else
            {
                Console.Clear();
                Console.WriteLine("Login ID does not exist, please try to log in again.");
                Console.WriteLine();
                return;
            }
        }


        public void SelectAccount()
        {
            while (true)
            {
                Console.WriteLine("You own the following account(s): ");
                foreach (var account in _customer.Accounts)
                {
                    Console.WriteLine(account.AccountNumber);
                }

                Console.WriteLine();
                Console.Write("Select an account: ");

                var accountNumber = Console.ReadLine();
                Console.Clear();
                
                // Verify account number using LINQ
                var result = from account in _customer.Accounts
                             where account.AccountNumber.ToString().Equals(accountNumber)
                             select account;

                // Correct account number
                if (result.Any())
                {
                    _account = result.First();
                    SelectTransaction();
                    return;
                }

                // Incorrect account number
                else
                {
                    Console.Clear();
                    Console.WriteLine("This account number does not exist.");
                    Console.WriteLine();
                    continue;
                }
            }
            
        }


        public void SelectTransaction()
        {
            while (true)
            {
                Console.WriteLine(
@"Account Infomation
=============================
Account Number: {0}
Account Type:   {1}
Balance:        {2:C}",
_account.AccountNumber, _account.AccountType == 'S' ? "Saving" : "Checking", _account.Balance);

                Console.WriteLine();

                Console.Write(
@"         Services
==========================
1. Deposit
2. Withdrawal
3. Transfer
4. My statement
5. Return to customer page

Enter an option: ");

                var option = InputUtilities.EnterOption(1, 5);

                switch (option)
                {
                    case 1:
                        Console.Clear();
                        Deposit();
                        break;
                    case 2:
                        Console.Clear();
                        Withdraw();
                        break;
                    case 3:
                        Console.Clear();
                        Transfer();
                        break;
                    case 4:
                        Console.Clear();
                        DisplayStatement();
                        break;
                    case 5:
                        Console.Clear();
                        // Remove current operating account
                        _account = null;
                        return;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }


        public void Deposit()
        {
            Console.Write("Specify the amount of deposit: ");
            var amount = InputUtilities.EnterPositiveNum();

            // Create transaction object
            var transaction = _account.Deposit(_account.AccountNumber, 0, amount, null);

            // Insert transaction into database
            _transactionManager.InsertTransaction(transaction);

            // Update Account table
            _accountManager.UpdateAccount(_account.AccountNumber, amount);

            Console.Clear();
            Console.WriteLine("A deposit of {0:C} has been made to your account {1}", amount, _account.AccountNumber);
            Console.WriteLine();
        }


        public void Withdraw()
        {
            // Count the number of transactions where service charge applies
            var result = from transaction in _account.Transactions
                         where transaction.TransactionType == 'W' || transaction.TransactionType == 'T' && transaction.Amount < 0
                         select transaction;
            var count = result.Count();

            var serviceFee = count < 4 ? 0 : (decimal)0.1;

            while (true)
            {
                Console.Write("Specify the amount of withdrawal: ");
                var amount = InputUtilities.EnterPositiveNum();

                try
                {
                    // Withdraw from account
                    var withdraw = _account.Withdraw(_account.AccountNumber, 0, amount, null);

                    // Insert withdrawal into database
                    _transactionManager.InsertTransaction(withdraw);

                    Console.Clear();
                    Console.WriteLine("A withdrawal of {0:C} has been made from your account {1}", amount, _account.AccountNumber);

                    // Charge service fee if free transactions run out
                    if (serviceFee != 0)
                    {
                        var serviceCharge = _account.ServiceCharge(_account.AccountNumber, 0, serviceFee, null);
                        _transactionManager.InsertTransaction(serviceCharge);

                        Console.WriteLine("A service charge of {0:C} has been applied.", serviceFee);
                    }

                    _accountManager.UpdateAccount(_account.AccountNumber, -(amount + serviceFee));
                    Console.WriteLine();
                    return;
                }
                catch (MinBalanceBreachException)
                {
                    continue;
                }
            }
        }


        public void Transfer()
        {
            // Count the number of transactions where service charge applies
            var result = from transaction in _account.Transactions
                         where transaction.TransactionType == 'W' || transaction.TransactionType == 'T' && transaction.Amount < 0
                         select transaction;
            var count = result.Count();

            var serviceFee = count < 4 ? 0 : (decimal)0.1;

            while (true)
            {
                Console.Write("Specify the destination account: ");
                var destinationAccount = Console.ReadLine();
                Console.WriteLine();

                if (!int.TryParse(destinationAccount, out int destAcc))
                {
                    Console.Clear();
                    Console.WriteLine("Account number only consists digits, please enter again.");
                    Console.WriteLine();
                    continue;
                }

                Console.Write("Specify the amount of transfer: ");
                var amount = InputUtilities.EnterPositiveNum();
                Console.WriteLine();

                Console.Write("Leave a message (press ENTER to skip): ");
                var comment = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    // Transfer out from account
                    var transferOut = _account.TransferOut(_account.AccountNumber, int.Parse(destinationAccount), amount, comment);
                }
                catch (ObjectNotFoundException)
                {

                }
                catch (MinBalanceBreachException)
                {

                }
            }
        }


        public void DisplayStatement()
        {

        }
    }
}
