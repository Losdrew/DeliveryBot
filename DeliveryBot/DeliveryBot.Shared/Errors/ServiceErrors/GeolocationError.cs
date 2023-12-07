using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.Errors.ServiceErrors;

public class GeolocationError : ServiceError
{
    public static GeolocationError GetRouteError = new()
    {
        Header = "Get route error",
        ErrorMessage = "Error when route",
        Code = 1
    };
}