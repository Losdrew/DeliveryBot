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

    public static RobotError RobotNotFound = new()
    {
        Header = "Robot not found",
        ErrorMessage = "Robot not found",
        Code = 2
    };

    public static RobotError RobotUnavailableError = new()
    {
        Header = "Robot unavailable",
        ErrorMessage = "Robot is not available right now",
        Code = 3
    };

    public static RobotError GetOwnCompanyRobotsError = new()
    {
        Header = "Get own company robots error",
        ErrorMessage = "Error when getting a list of company robots",
        Code = 4
    };
}