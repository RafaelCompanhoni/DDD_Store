using AutoMapper;
using LuaBijoux.Core.DomainModels.Identity;
using LuaBijoux.Web.Areas.Admin.Models.Users;

namespace LuaBijoux.Web.Infrastructure.Mappers.Users
{
    public class UsersDomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<AppUser, CreateUserVM>();
            Mapper.CreateMap<AppUser, EditUserVM>()
                .ForMember(d => d.EmailConfirmation, o => o.MapFrom(u => u.Email));
        }
    }
}