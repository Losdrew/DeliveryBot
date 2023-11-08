﻿using AutoMapper;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Company;
using DeliveryBot.Shared.Dto.Account;
using DeliveryBot.Shared.Dto.Address;
using DeliveryBot.Shared.Dto.Company;

namespace DeliveryBot.Server.Mapper;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<CreateCompanyCommand, Company>()
            .ForMember(e => e.CompanyEmployees, opt => opt.Ignore());
        CreateMap<Company, OwnCompanyInfoDto>();
        CreateMap<OwnCompanyInfoDto, Company>();

        CreateMap<CompanyEmployee, CompanyEmployeeDto>();
        CreateMap<CompanyEmployeeDto, CompanyEmployee>();

        CreateMap<Address, AddressDto>();
        CreateMap<AddressDto, Address>();
    }
}