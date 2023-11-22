using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.Errors.ServiceErrors;

public class DeliveryError : ServiceError
{
    public static DeliveryError DeliveryCreateError = new()
    {
        Header = "Delivery creation error",
        ErrorMessage = "Error when creating delivery",
        Code = 1
    };

    public static DeliveryError DeliveryOrderError = new()
    {
        Header = "Delivery creation error",
        ErrorMessage = "Can't create delivery for this order",
        Code = 2
    };
}