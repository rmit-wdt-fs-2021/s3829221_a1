using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Login
    {

        //Properties
        public string LoginID { get; }
        public Customer Customer { get; }
        public string PasswordHash { get; set; }
    }
}
