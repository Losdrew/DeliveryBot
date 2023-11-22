using DeliveryBot.Shared.Helpers;
using NetTopologySuite.Geometries;

namespace DeliveryBot.Shared.Dto.Robot;

public class GetDeliveryRobotQueryDto
{
    public string? Name { get; set; }
    public RobotStatus Status { get; set; }
    public Point? Location { get; set; }
}