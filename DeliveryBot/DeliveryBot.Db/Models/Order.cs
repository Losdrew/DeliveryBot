namespace DeliveryBot.Db.Models;

public class Order : Entity
{
    public DateTime PlacedDateTime { get; set; }

    public Customer? Customer { get; set; }
    public Address? OrderAddress { get; set; }
    public ICollection<OrderProduct>? OrderProducts { get; set; }
}