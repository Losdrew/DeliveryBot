using AutoMapper;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Robot;
using DeliveryBot.Shared.Dto.Robot;
using NetTopologySuite.Geometries;

namespace DeliveryBot.Server.Mapper;

public class RobotProfile : Profile
{
    public RobotProfile()
    {
        CreateMap<CreateRobotCommand, Robot>();
        CreateMap<EditRobotCommand, Robot>();
        CreateMap<Robot, RobotInfoDto>()
            .ForMember(dest => dest.Location, opt => 
                opt.MapFrom(src => src.Location == null ? null : new LocationDto
                {
                    X = src.Location.X, 
                    Y = src.Location.Y
                }));

        CreateMap<Robot, GetDeliveryRobotQueryDto>()
            .ForMember(dest => dest.Location, opt => 
                opt.MapFrom(src => src.Location == null ? null : new LocationDto
                {
                    X = src.Location.X, 
                    Y = src.Location.Y
                }));

        CreateMap<UpdateRobotCommand, Robot>()
            .ForMember(r => r.Location, opt => 
                opt.MapFrom(src => src.Location == null ? null : 
                    new Point(new Coordinate(src.Location.X, src.Location.Y))));
    }
}