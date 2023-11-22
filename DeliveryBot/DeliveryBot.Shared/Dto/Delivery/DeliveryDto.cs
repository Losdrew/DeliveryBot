namespace DeliveryBot.Shared.Dto.Delivery;

public class DeliveryDto
{
    public DateTime ShippedDateTime { get; set; }
    public DateTime? DeliveredDateTime { get; set; }
    public Guid? RobotId { get; set; }
    public Guid? OrderId { get; set; }
}