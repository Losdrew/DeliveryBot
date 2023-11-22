using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.Errors.ServiceErrors;

public class RobotError : ServiceError
{
    public static RobotError RobotCreateError = new()
    {
        Header = "Create robot error",
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

    public static RobotError GetDeliveryRobotError = new()
    {
        Header = "Get delivery robot error",
        ErrorMessage = "Error when getting a robot for this delivery",
        Code = 5
    };

    public static RobotError RobotEditError = new()
    {
        Header = "Edit robot error",
        ErrorMessage = "Error when editing robot",
        Code = 6
    };
}