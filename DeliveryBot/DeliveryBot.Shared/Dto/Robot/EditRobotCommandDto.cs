namespace DeliveryBot.Shared.Dto.Robot;

public class EditRobotCommandDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal WeightCapacityG { get; set; }
    public decimal VolumeCapacityCm3 { get; set; }
}