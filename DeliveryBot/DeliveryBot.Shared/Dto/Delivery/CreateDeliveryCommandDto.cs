namespace DeliveryBot.Shared.Dto.Delivery;

public class CreateDeliveryCommandDto
{
    public Guid? RobotId { get; set; }
    public Guid? OrderId { get; set; }
}