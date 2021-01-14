using System;

namespace ClassLibrary
{
    public class MinBalanceBreachException : Exception
    {
        public MinBalanceBreachException(int minBalance)
        {
            Console.WriteLine("Transaction rejected: the minimal balance for this account is {0}.", minBalance);
            Console.WriteLine();
        }
    }
}
