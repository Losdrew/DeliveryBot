namespace DeliveryBot.Shared.Dto.Order;

public class OrderInfoDto : OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
}