using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Models;
using Managers;

namespace Services
{
    public static class CustomersWebService
    {

        public static void ReadAndSaveCustomer(string connectionString)
        {

            // Check if any customer exists in the table
            var customerManager = new CustomerManager(connectionString);
            if (CustomerManager.Customers.Any())
                return;

            // Contact web service and read JSON file
            using var client = new HttpClient();
            var json = client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/").Result;

            // Deserialise JSON file into objects
            var customers = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy hh:mm:ss tt"
            });

            // Insert data stored in JSON into database
            var accountManager = new AccountManager(connectionString);
            var transactionManager = new TransactionManager(connectionString);
            foreach (var customer in customers)
            {
                customerManager.InsertCustomer(customer);
                CustomerManager.Customers.Add(customer.CustomerID, customer);

                foreach (var account in customer.Accounts)
                {
                    // Set account's customer ID
                    account.CustomerID = customer.CustomerID;
                    accountManager.InsertAccount(account);
                    AccountManager.Accounts.Add(account.AccountNumber, account);

                    foreach (var transaction in account.Transactions)
                    {

                        // Set transaction's account number
                        transaction.AccountNumber = account.AccountNumber;
                        transactionManager.InsertTransaction(transaction);
                    }
                }
            }

        }
    }
}
