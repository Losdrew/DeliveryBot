namespace DeliveryBot.Db.Models;

public class Delivery : Entity
{
    public DateTime ShippedDateTime { get; set; }
    public DateTime DeliveredDateTime { get; set; }

    public Robot? Robot { get; set; }
    public CompanyEmployee? CompanyEmployee { get; set; }
    public Order? Order { get; set; }
}