using System;
using AutoMapper;
using LuaBijoux.Core.DomainModels.Identity;
using LuaBijoux.Web.Areas.Admin.Models;

namespace LuaBijoux.Web.Infrastructure.Mappers.Users
{
    public class UsersViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<CreateUserVM, AppUser>()
                .ForMember(d => d.UserName, o => o.MapFrom(u => u.Email.ToLower()))
                .ForMember(d => d.Email, o => o.MapFrom(u => u.Email.ToLower()))
                .ForMember(d => d.Cpf, o => o.MapFrom(u => u.Cpf.Replace(".", "").Replace("-", "")))
                .ForMember(d => d.Birthdate, o => o.MapFrom(u => Convert.ToDateTime(u.Birthdate)));

            Mapper.CreateMap<EditUserVM, AppUser>()
                .ForMember(d => d.UserName, o => o.MapFrom(u => u.Email.ToLower()))
                .ForMember(d => d.Email, o => o.MapFrom(u => u.Email.ToLower()))
                .ForMember(d => d.Cpf, o => o.MapFrom(u => u.Cpf.Replace(".", "").Replace("-", "")))
                .ForMember(d => d.Birthdate, o => o.MapFrom(u => Convert.ToDateTime(u.Birthdate)));
        }
    }
}