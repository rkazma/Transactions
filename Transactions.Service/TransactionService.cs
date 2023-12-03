using Common.Contracts;
using Transactions.DataAccess.Contracts;
using Transactions.Domain.Models;
using Transactions.Service.Contracts;

namespace Transactions.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<TransactionObj>> GetTransactions(long customerId, long accountId, int start, int limit)
        {
            return await _transactionRepository.GetTransactions(customerId, accountId, start, limit);
        }

        public async Task<int> CreateTransaction(EventMessage transaction)
        {
            return await _transactionRepository.CreateTransaction(transaction);
        }
    }
}