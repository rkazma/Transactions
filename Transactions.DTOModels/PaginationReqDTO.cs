namespace Transactions.DTOModels
{
    public class PaginationReqDTO
    {
        public int Start { get; set; } = 0;
        public int Limit { get; set; } = 100;
    }
}
