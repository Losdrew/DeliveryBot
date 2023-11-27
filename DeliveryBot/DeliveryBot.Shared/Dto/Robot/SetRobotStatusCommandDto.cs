using DeliveryBot.Shared.Helpers;

namespace DeliveryBot.Shared.Dto.Robot;

public class SetRobotStatusCommandDto
{
    public Guid Id { get; set; }
    public RobotStatus NewStatus { get; set; }
}