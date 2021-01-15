using System.Threading.Tasks;

namespace Services
{
    public static class AsyncCustomerWebService
    {

        public static async Task ReadAndSaveCustomer(string connectionString)
        {
            await CustomersWebService.ReadAndSaveCustomer(connectionString);
        }
    }
}
