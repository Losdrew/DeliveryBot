namespace DeliveryBot.Shared.Dto.Delivery;

public class CreateDeliveryCommandDto
{
    public DateTime ShippedDateTime { get; set; }
    public Guid? RobotId { get; set; }
    public Guid? OrderId { get; set; }
}