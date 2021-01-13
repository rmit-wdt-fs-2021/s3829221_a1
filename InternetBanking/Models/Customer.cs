using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Customer
    {

        // Properties
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public Dictionary<int, Account> Accounts { get; }


        public Customer(int customerID, string name, string address, string city, string postCode, Dictionary<int, Account> accounts)
        {
            CustomerID = customerID;
            Name = name;
            Address = address;
            City = city;
            PostCode = postCode;
            Accounts = accounts;
        }
    }
}
