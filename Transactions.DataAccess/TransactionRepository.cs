using Common.Contracts;
using Microsoft.Extensions.Configuration;
using Transactions.Common;
using Transactions.DataAccess.Contracts;
using Transactions.Domain;
using Transactions.Domain.Models;

namespace Transactions.DataAccess
{
    public class TransactionRepository : EnterpriseRepository, ITransactionRepository
    {
        private DapperUtils _dbUtils;
        public TransactionRepository(IConfiguration configuration, IAppConfigurationService appConfigurationService) : base(configuration, appConfigurationService)
        {
            _dbUtils = GetDapper();

        }

        public async Task<IEnumerable<TransactionObj>> GetTransactions(long customerId, long accountId, int start, int limit)
        {
            return await _dbUtils.GetListAsync<TransactionObj>(Constants.TRANSACTION_GET_LIST, new
            {
                CustomerId = customerId,
                AccountId = accountId,
                Start = start,
                Limit = limit
            });
        }

        public async Task<int> CreateTransaction(EventMessage transaction)
        {
            return await _dbUtils.ExcecuteScalarAsync<int>(Constants.TRANSACTION_CREATE, new
            {
                CustomerId = transaction.CustomerId,
                AccountId = transaction.AccountId,
                Amount = transaction.InitialCredit
            });
        }
    }
}