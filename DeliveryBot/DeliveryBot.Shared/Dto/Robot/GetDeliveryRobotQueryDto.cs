using DeliveryBot.Shared.Dto.Geolocation;
using DeliveryBot.Shared.Helpers;

namespace DeliveryBot.Shared.Dto.Robot;

public class GetDeliveryRobotQueryDto
{
    public string? Name { get; set; }
    public RobotStatus Status { get; set; }
    public LocationDto? Location { get; set; }
}