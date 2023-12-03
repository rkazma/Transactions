namespace Transactions.DTOModels
{
    public class TransactionDTO
    {
        public long TransactionId { get; set; }
        public long CustomerId { get; set; }
        public long AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}