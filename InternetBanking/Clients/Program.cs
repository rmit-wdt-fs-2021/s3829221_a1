using Models;
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

            var container = new Container();

            // Read data from JSON file and save on database
            AsyncCustomerWebService.ReadAndSaveCustomer(connectionString);
            LoginsWebService.ReadAndSaveLogin(connectionString);

            new Menu(connectionString).Run();
        }
    }
}
