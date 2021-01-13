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
                DateFormatString = "dd/mm/yyyy"
            });

            // Insert data stored in JSON into database
            var accountManager = new AccountManager(connectionString);
            var transactionManager = new TransactionManager(connectionString);
            foreach (var customer in customers)
            {
                customerManager.InsertCustomer(customer);

                foreach (var account in customer.Accounts)
                {
                    // Set account's customer
                    account.Value.Customer.CustomerID = customer.CustomerID;
                    accountManager.InsertAccount(account.Value);

                    foreach (var transaction in account.Value.Transactions)
                    {

                        // Set transaction's account number
                        transaction.Account.AccountNumber = account.Value.AccountNumber;
                        transactionManager.InsertTransaction(transaction);
                    }
                }
            }

        }
    }
}
