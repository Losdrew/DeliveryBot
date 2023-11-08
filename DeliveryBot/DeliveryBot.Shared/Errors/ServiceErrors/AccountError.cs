using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.Errors.ServiceErrors;

public class AccountError : ServiceError
{
    public static AccountError AccountCreateError = new()
    {
        Header = "Account creation error",
        ErrorMessage = "Error when creating account",
        Code = 1
    };

    public static AccountError LoginServiceError = new()
    {
        Header = "Login error",
        ErrorMessage = "Error when performing login",
        Code = 2
    };

    public static AccountError LoginValidationError = new()
    {
        Header = "Login error",
        ErrorMessage = "Email or password is not valid",
        Code = 3
    };
}