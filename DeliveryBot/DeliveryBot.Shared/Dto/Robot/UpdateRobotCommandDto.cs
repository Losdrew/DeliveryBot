using DeliveryBot.Shared.Dto.Geolocation;
using DeliveryBot.Shared.Helpers;

namespace DeliveryBot.Shared.Dto.Robot;

public class UpdateRobotCommandDto
{
    public string? DeviceId { get; set; }
    public RobotStatus Status { get; set; }
    public int BatteryCharge { get; set; }
    public bool HasCargo { get; set; }
    public LocationDto? Location { get; set; }
}