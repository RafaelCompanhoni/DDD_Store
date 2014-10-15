using AutoMapper;
using LuaBijoux.Web.Areas.Admin.Models;

namespace LuaBijoux.Web.Infrastructure.Mappers
{
    public class DomainToModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToModelMappings"; }
        }

        protected override void Configure()
        {
           //  Mapper.CreateMap<User, UserViewModel>();
        }
    }
}