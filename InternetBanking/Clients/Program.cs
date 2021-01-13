using System;
using Services;
using Microsoft.Extensions.Configuration;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

            // Import connection string from JSON file
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = configuration["ConnectionString"];

            CustomersWebService.ReadAndSaveCustomer(connectionString);
            LoginsWebService.ReadAndSaveLogin(connectionString);

        }
    }
}
