namespace DeliveryBot.Db.Models;

public class OrderProduct : Entity
{
    public int Quantity { get; set; }

    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }

    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }
}