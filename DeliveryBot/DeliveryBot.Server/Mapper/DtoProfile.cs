using AutoMapper;
using DeliveryBot.Shared.Dto.Error;
using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Server.Mapper;

public class DtoProfiles : Profile
{
    public DtoProfiles()
    {
        CreateMap<ValidationError, ValidationErrorDto>();
        CreateMap<ServiceError, ServiceErrorDto>();
        CreateMap<Error, ErrorDto>()
            .ForMember(e => e.ServiceErrors, otp => otp.MapFrom(src => src.ServiceErrors))
            .ForMember(e => e.ValidationErrors, otp => otp.MapFrom(src => src.ValidationErrors));
    }
}