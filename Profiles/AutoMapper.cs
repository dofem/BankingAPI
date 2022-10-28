using AutoMapper;
using BankingAPI.DTO;
using BankingAPI.Model;

namespace BankingAPI.Profiles
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<RegisterAccountModel, Account>();
            CreateMap<UpdateAccountModel, Account>();
            CreateMap<GetAllAccountsModel,Account>();
        }
    }
}
