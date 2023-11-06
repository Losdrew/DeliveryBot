using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.ServiceErrors;

public class AccountError : ServiceError
{
    public static AccountError IdentityCreateError = new()
    {
        Header = "Account creation error",
        ErrorMessage = "Error when creating account",
        Code = 1
    };

    public static AccountError UserCreateError = new()
    {
        Header = "Account creation error",
        ErrorMessage = "Error when creating account",
        Code = 2
    };
}