using AutoMapper;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Company;
using DeliveryBot.Shared.Dto.Company;

namespace DeliveryBot.Server.Mapper;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<CreateCompanyCommand, Company>()
            .ForMember(c => c.CompanyEmployees, opt => opt.Ignore());
        
        CreateMap<EditCompanyCommand, Company>()
            .ForMember(c => c.CompanyEmployees, opt => opt.Ignore())
            .ForMember(c => c.CompanyAddresses, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

        CreateMap<Company, CompanyDto>();
        CreateMap<CompanyDto, Company>();
        CreateMap<Company, OwnCompanyInfoDto>();
        CreateMap<OwnCompanyInfoDto, Company>();
        CreateMap<Company, CompanyPreviewDto>();
    }
}