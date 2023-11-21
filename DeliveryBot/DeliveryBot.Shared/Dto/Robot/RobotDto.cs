using DeliveryBot.Shared.Helpers;
using NetTopologySuite.Geometries;

namespace DeliveryBot.Shared.Dto.Robot;

public class RobotDto
{
    public string? Name { get; set; }
    public RobotStatus? Status { get; set; }
    public Point? Location { get; set; }
    public int BatteryCharge { get; set; }
    public bool HasCargo { get; set; }
    public decimal WeightCapacityG { get; set; }
    public decimal VolumeCapacityCm3 { get; set; }
    public Guid? CompanyId { get; set; }
}