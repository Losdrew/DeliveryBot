namespace DeliveryBot.Shared.Dto.Robot;

public class CreateRobotCommandDto
{
    public string? Name { get; set; }
    public decimal WeightCapacityG { get; set; }
    public decimal VolumeCapacityCm3 { get; set; }
}