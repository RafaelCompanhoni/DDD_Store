using AutoMapper;
using LuaBijoux.Web.Infrastructure.Mappers.Users;

namespace LuaBijoux.Web.Infrastructure.Mappers
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<UsersDomainToViewModelMappingProfile>();
                x.AddProfile<UsersViewModelToDomainMappingProfile>();
            });
        }
    }
}