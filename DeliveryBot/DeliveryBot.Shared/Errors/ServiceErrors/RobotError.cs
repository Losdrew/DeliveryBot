using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.Errors.ServiceErrors;

public class RobotError : ServiceError
{
    public static RobotError RobotCreateError = new()
    {
        Header = "Robot creation error",
        ErrorMessage = "Error when creating robot",
        Code = 1
    };
}