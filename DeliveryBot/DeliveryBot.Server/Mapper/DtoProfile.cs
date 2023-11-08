using AutoMapper;
using DeliveryBot.Shared.Dto.Error;
using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Server.Mapper;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<ValidationError, ValidationErrorDto>();
        CreateMap<ServiceError, ServiceErrorDto>();
        CreateMap<Error, ErrorDto>()
            .ForMember(e => e.ServiceErrors, otp => otp.MapFrom(src => src.ServiceErrors))
            .ForMember(e => e.ValidationErrors, otp => otp.MapFrom(src => src.ValidationErrors));
    }
}