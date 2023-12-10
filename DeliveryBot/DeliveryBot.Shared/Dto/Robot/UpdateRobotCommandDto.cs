using DeliveryBot.Shared.Converters;
using DeliveryBot.Shared.Dto.Geolocation;
using DeliveryBot.Shared.Helpers;
using System.Text.Json.Serialization;

namespace DeliveryBot.Shared.Dto.Robot;

public class UpdateRobotCommandDto
{
    public string? DeviceId { get; set; }
    public RobotStatus Status { get; set; }
    public int BatteryCharge { get; set; }
    [JsonConverter(typeof(BoolConverter))]
    public bool IsCargoLidOpen { get; set; }
    public LocationDto? Location { get; set; }
}