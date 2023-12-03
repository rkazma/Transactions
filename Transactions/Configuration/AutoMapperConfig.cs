using AutoMapper;
using Transactions.Domain.Models;
using Transactions.DTOModels;

namespace Transactions.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<TransactionObj, TransactionDTO>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<TransactionDTO, TransactionObj>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
