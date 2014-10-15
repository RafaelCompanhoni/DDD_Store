using System;
using AutoMapper;
using LuaBijoux.Web.Areas.Admin.Models;

namespace LuaBijoux.Web.Infrastructure.Mappers
{
    public class ModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ModelToDomainMappings"; }
        }

        protected override void Configure()
        {
           // Mapper.CreateMap<UserViewModel, User>()
           //    .ForMember(d => d.Birthdate, o => o.MapFrom(u => Convert.ToDateTime(u.Birthdate)));
        }
    }
}