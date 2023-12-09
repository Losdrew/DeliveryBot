using DeliveryBot.Shared.Dto.Geolocation;
using DeliveryBot.Shared.Helpers;

namespace DeliveryBot.Shared.Dto.Robot;

public class RobotDto
{
    public string? Name { get; set; }
    public RobotStatus Status { get; set; }
    public LocationDto? Location { get; set; }
    public int BatteryCharge { get; set; }
    public decimal WeightCapacityG { get; set; }
    public decimal VolumeCapacityCm3 { get; set; }
    public bool IsCargoLidOpen { get; set; }
    public string? DeviceId { get; set; }
    public Guid? CompanyId { get; set; }
}