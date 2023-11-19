﻿using AutoMapper;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Order;
using DeliveryBot.Shared.Dto.Order;

namespace DeliveryBot.Server.Mapper;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CreateOrderCommand, Order>()
            .ForMember(o => o.OrderProducts, opt => opt.Ignore());

        CreateMap<Order, OrderInfoDto>();
        CreateMap<OrderProductDto, OrderProduct>();
        CreateMap<OrderProduct, OrderProductDto>();
    }
}