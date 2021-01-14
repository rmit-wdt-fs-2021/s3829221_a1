using System;

namespace Models
{
    public class Transaction : IComparable<Transaction>
    {

        // Properties
        public int TransactionID { get; set; }
        public char TransactionType { get; set; }
        public int AccountNumber { get; set; }
        public int DestinationAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public DateTime TransactionTimeUtc { get; set; }


        // Sort transactions by time in descending order
        public int CompareTo(Transaction transaction)
        {
            return transaction.TransactionTimeUtc.CompareTo(TransactionTimeUtc);
        }


        public override string ToString()
        {
            return String.Format("{0, 12} {1, 12} {2, 12} {3, 12} {4, 12:C} {5, 12} {6, 12}",
                TransactionID,
                TransactionType,
                AccountNumber,
                DestinationAccountNumber != 0 ? DestinationAccountNumber : "N/A",
                Amount,
                Comment ?? "N/A",
                TransactionTimeUtc.ToLocalTime());
        }
    }
}
