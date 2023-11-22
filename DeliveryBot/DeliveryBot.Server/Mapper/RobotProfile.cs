using AutoMapper;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Robot;
using DeliveryBot.Shared.Dto.Robot;

namespace DeliveryBot.Server.Mapper;

public class RobotProfile : Profile
{
    public RobotProfile()
    {
        CreateMap<CreateRobotCommand, Robot>();
        CreateMap<Robot, RobotInfoDto>();
    }
}