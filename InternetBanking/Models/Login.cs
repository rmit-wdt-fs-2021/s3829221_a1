
namespace Models
{
    public class Login
    {

        //Properties
        public string LoginID { get; }
        public Customer Customer { get; }
        public string PasswordHash { get; set; }


        public Login(string loginID, Customer customer, string passwordHash)
        {
            LoginID = loginID;
            Customer = customer;
            PasswordHash = passwordHash;
        }
    }
}
