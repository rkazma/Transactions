using Common.Contracts;
using Transactions.Domain.Models;

namespace Transactions.DataAccess.Contracts
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TransactionObj>> GetTransactions(long customerId, long acountId, int start, int limit);
        Task<int> CreateTransaction(EventMessage transaction);
    }
}