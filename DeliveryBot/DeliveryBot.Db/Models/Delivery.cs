namespace DeliveryBot.Db.Models;

public class Delivery : Entity
{
    public DateTime ShippedDateTime { get; set; }
    public DateTime DeliveredDateTime { get; set; }

    public Guid? RobotId { get; set; }
    public Robot? Robot { get; set; }

    public Guid? CompanyEmployeeId { get; set; }
    public CompanyEmployee? CompanyEmployee { get; set; }
    
    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }
}