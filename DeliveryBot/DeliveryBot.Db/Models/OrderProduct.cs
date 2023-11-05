namespace DeliveryBot.Db.Models;

public class OrderProduct : Entity
{
    public int Quantity { get; set; }

    public Product? Product { get; set; }
    public Order? Order { get; set; }
}