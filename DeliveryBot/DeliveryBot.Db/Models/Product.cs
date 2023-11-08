namespace DeliveryBot.Db.Models;

public class Product : Entity
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public decimal WeightG { get; set; }
    public decimal VolumeCm3 { get; set; }

    public ICollection<OrderProduct>? OrderProducts { get; set; }
}