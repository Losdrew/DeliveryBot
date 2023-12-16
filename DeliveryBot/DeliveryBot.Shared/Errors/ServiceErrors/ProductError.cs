using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.Errors.ServiceErrors;

public class ProductError : ServiceError
{
    public static ProductError ProductCreateError = new()
    {
        Header = "Create product error",
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
        Header = "Edit product error",
        ErrorMessage = "Error when editing product",
        Code = 3
    };

    public static ProductError ProductNotFound = new()
    {
        Header = "Product not found",
        ErrorMessage = "The requested product was not found",
        Code = 4
    };

    public static ProductError ProductDeleteError = new()
    {
        Header = "Delete product error",
        ErrorMessage = "Error when deleting product",
        Code = 5
    };

    public static ProductError GetProductError = new()
    {
        Header = "Get product error",
        ErrorMessage = "Error when getting product",
        Code = 6
    };
}