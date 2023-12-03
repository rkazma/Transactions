using Transactions.Common;
using Transactions.Domain;
using Microsoft.Extensions.Configuration;

namespace Transactions.DataAccess
{
    public class EnterpriseRepository
    {
        private string enterpriseConnectionString;
        private IAppConfigurationService _appConfigService;
        protected IConfiguration configuration;
        public EnterpriseRepository(IConfiguration configuration, IAppConfigurationService appConfigurationService)
        {
            this.configuration = configuration;
            _appConfigService = appConfigurationService;
            this.enterpriseConnectionString = _appConfigService.GetConnectionString(Constants.AccountsDbConnection);
        }
        public DapperUtils GetDapper()
        {
            return new DapperUtils(enterpriseConnectionString);
        }
    }
}
