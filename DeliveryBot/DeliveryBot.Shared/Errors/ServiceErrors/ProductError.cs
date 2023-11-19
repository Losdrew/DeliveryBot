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

    public static ProductError GetCompanyProductsError = new()
    {
        Header = "Get company products error",
        ErrorMessage = "Error when getting company products",
        Code = 2
    };

    public static ProductError ProductEditError = new()
    {
        Header = "Product edit error",
        ErrorMessage = "Error when editing product",
        Code = 3
    };

    public static ProductError ProductNotFound = new()
    {
        Header = "Product not found",
        ErrorMessage = "The requested product was not found",
        Code = 3
    };
}