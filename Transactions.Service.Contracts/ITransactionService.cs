using Common.Contracts;
using Transactions.Domain.Models;

namespace Transactions.Service.Contracts
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionObj>> GetTransactions(long customerId, long accountId, int start, int limit);
        Task<int> CreateTransaction(EventMessage transaction);

    }
}