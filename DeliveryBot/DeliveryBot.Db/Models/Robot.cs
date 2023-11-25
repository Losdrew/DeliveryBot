using System.ComponentModel.DataAnnotations.Schema;
using Point = NetTopologySuite.Geometries.Point;

namespace DeliveryBot.Db.Models;

public class Robot : Entity
{
    public string? Name { get; set; }
    public RobotStatus Status { get; set; }
    public int BatteryCharge { get; set; }
    public bool HasCargo { get; set; }
    public decimal WeightCapacityG { get; set; }
    public decimal VolumeCapacityCm3 { get; set; }
    public string? DeviceId { get; set; }

    [Column(TypeName="geometry (point)")]
    public Point? Location { get; set; }

    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }
}

public enum RobotStatus
{
    Inactive,
    Idle,
    Delivering,
    Returning,
    Charging,
    Danger
}