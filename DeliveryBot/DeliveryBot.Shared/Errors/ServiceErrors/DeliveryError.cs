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

    public static DeliveryError OrderDeliveryNotFound = new()
    {
        Header = "Delivery not found",
        ErrorMessage = "Delivery for this order not found",
        Code = 3
    };

    public static DeliveryError GetDeliveryError = new()
    {
        Header = "Get delivery error",
        ErrorMessage = "Error when getting delivery",
        Code = 4
    };
}