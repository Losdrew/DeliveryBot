using AutoMapper;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Delivery;
using DeliveryBot.Shared.Dto.Delivery;

namespace DeliveryBot.Server.Mapper;

public class DeliveryProfile : Profile
{
    public DeliveryProfile()
    {
        CreateMap<CreateDeliveryCommand, Delivery>();
        CreateMap<Delivery, DeliveryInfoDto>();
    }
}