using DeliveryBot.Shared.Dto.Address;

namespace DeliveryBot.Shared.Dto.Order;

public class OrderDto
{
    public DateTime PlacedDateTime { get; set; }
    public AddressDto OrderAddress { get; set; }
    public ICollection<OrderProductDto> OrderProducts { get; set; }
}