using AutoMapper;
using DeliveryBot.Db.Models;
using DeliveryBot.Shared.Dto.Address;

namespace DeliveryBot.Server.Mapper;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Address, AddressDto>();
        CreateMap<AddressDto, Address>();
        CreateMap<Address, EditableAddressDto>();
        CreateMap<EditableAddressDto, Address>()
            .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
    }
}