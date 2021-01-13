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

            // Verify login ID
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

                if (isCorrect)
                {
                    Console.WriteLine($"Hello, Ms/Mr. {customer.Name}!");
                    Console.WriteLine();

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Incorrect password, please try to log in again.");
                    Console.WriteLine();
                    return;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Login ID does not exist, please try to log in again.");
                Console.WriteLine();
                return;
            }
        }
    }
}
