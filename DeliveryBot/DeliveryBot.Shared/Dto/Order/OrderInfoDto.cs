using DeliveryBot.Shared.Helpers;

namespace DeliveryBot.Shared.Dto.Order;

public class OrderInfoDto : OrderDto
{
    public Guid Id { get; set; }    
    public Guid CustomerId { get; set; }    
    public OrderStatuses OrderStatus { get; set; }
}