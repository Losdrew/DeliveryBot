﻿using AutoMapper;
using DeliveryBot.Db.Models;
using DeliveryBot.Shared.Dto.Account;
using DeliveryBot.Shared.Dto.CompanyEmployee;

namespace DeliveryBot.Server.Mapper;

public class CompanyEmployeeProfile : Profile
{
    public CompanyEmployeeProfile()
    {
        CreateMap<CompanyEmployee, CompanyEmployeeAccountDto>();
        CreateMap<CompanyEmployeeAccountDto, CompanyEmployee>();
        CreateMap<CompanyEmployee, CompanyEmployeeDto>();
        CreateMap<CompanyEmployeeDto, CompanyEmployee>();
        CreateMap<CompanyEmployee, CompanyEmployeeInfoDto>();
        CreateMap<EditableCompanyEmployeeDto, CompanyEmployee>()
            .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
    }
}