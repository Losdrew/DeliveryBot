using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.Errors.ServiceErrors;

public class ProductError : ServiceError
{
    public static ProductError ProductCreateError = new()
    {
        Header = "Product creation error",
        ErrorMessage = "Error when creating product",
        Code = 1
    };
}