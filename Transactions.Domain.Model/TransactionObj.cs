namespace Transactions.Domain.Models
{
    public class TransactionObj
    {
        public long TransactionId { get; set; }
        public long CustomerId { get; set; }
        public long AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}