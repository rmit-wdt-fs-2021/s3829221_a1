using System;
using SimpleHashing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Managers;
using ClassLibrary;

namespace Client
{
    class Menu
    {

        private readonly CustomerManager _customerManager;
        private readonly LoginManager _loginManager;
        private Customer _customer;
        private Account _account;


        public Menu(string connectionString)
        {
            _customerManager = new CustomerManager(connectionString);
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
                Console.Write(
@"Select a service
==========================
1. Deposit
2. Withdraw
3. Transfer
4. My statement
5. Return to customer page

Enter an option: ");

                var option = InputUtilities.EnterOption(1, 5);

                switch (option)
                {
                    case 1:
                        Console.Clear();

                        break;
                    case 2:
                        Console.Clear();

                        break;
                    case 3:
                        Console.Clear();

                        break;
                    case 4:
                        Console.Clear();

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
    }
}
