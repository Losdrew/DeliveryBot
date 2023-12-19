using DeliveryBot.Shared.Dto.Address;

namespace DeliveryBot.Shared.Dto.Order;

public class CreateOrderCommandDto
{
    public AddressDto OrderAddress { get; set; }
    public ICollection<OrderProductDto> OrderProducts { get; set; }
}