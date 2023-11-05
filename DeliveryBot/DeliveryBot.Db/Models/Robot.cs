using Point = NetTopologySuite.Geometries.Point;

namespace DeliveryBot.Db.Models;

public class Robot : Entity
{
    public string? Name { get; set; }
    public string? Status { get; set; }
    public Point? Location { get; set; }
    public int BatteryCharge { get; set; }
    public bool HasCargo { get; set; }
    public decimal WeightCapacityG { get; set; }
    public decimal VolumeCapacityCm3 { get; set; }
}