using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.ServiceErrors;

public class AccountError : ServiceError
{
    public static AccountError AccountCreateError = new()
    {
        Header = "Account creation error",
        ErrorMessage = "Error when creating account",
        Code = 1
    };
}