using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.Errors.ServiceErrors;

public class OrderError : ServiceError
{
    public static OrderError OrderCreateError = new()
    {
        Header = "Order creation error",
        ErrorMessage = "Error when creating order",
        Code = 1
    };
