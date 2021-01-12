using Models;

namespace Utilities
{
    public static class BankingUtilities
    {

        public static int CreateTransactionID(this Account account)
        {
            int transactionNumber = 0;

            if (account.Transactions.Count != 0)
            {
                transactionNumber = account.Transactions.Count;
            }

            return transactionNumber;
        }
    }
}
