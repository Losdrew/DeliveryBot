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

    public static OrderError GetPendingOrdersError = new()
    {
        Header = "Get all pending orders error",
        ErrorMessage = "Error when getting a list of pending orders",
        Code = 2
    };

    public static OrderError GetOwnOrdersError = new()
    {
        Header = "Get own orders error",
        ErrorMessage = "Error when getting a list of user's orders",
        Code = 3
    };

    public static OrderError OrderNotFound = new()
    {
        Header = "Order not found",
        ErrorMessage = "Order not found",
        Code = 4
    };

    public static OrderError OrderCancelError = new()
    {
        Header = "Order cancel error",
        ErrorMessage = "Error when cancelling order",
        Code = 5
    };
}