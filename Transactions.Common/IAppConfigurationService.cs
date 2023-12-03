using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Common
{
    public interface IAppConfigurationService
    {
        string GetConnectionString(string section);
    }
}
