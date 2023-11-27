namespace DeliveryBot.Shared.Helpers;

public enum RobotStatus
{
    Inactive,
    Idle,
    WaitingForCargo,
    Delivering,
    ReadyForPickup,
    Returning,
    Maintenance,
    Danger
}