using System.Collections.Generic;

namespace Models
{
    class Container
    {
        public static Dictionary<int, Customer> Customers { get; set; }
        public static Dictionary<int, Account> Accounts { get; set; }
        public static Dictionary<string, Login> Logins { get; set; }


        public Container()
        {
            Customers = new Dictionary<int, Customer>();
            Accounts = new Dictionary<int, Account>();
            Logins = new Dictionary<string, Login>();
        }
    }
}
