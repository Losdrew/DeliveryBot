using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.Errors.ServiceErrors;

public class UserError : ServiceError
{
    public static UserError InvalidAuthorization = new()
    {
        Header = "Invalid authorization",
        ErrorMessage = "Invalid authorization",
        Code = 1
    };

    public static UserError UserNotFound = new()
    {
        Header = "User not found",
        ErrorMessage = "User not found",
        Code = 2
    };
}