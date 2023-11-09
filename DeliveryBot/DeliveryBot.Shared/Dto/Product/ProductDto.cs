namespace DeliveryBot.Shared.Dto.Product;

public class ProductDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public decimal WeightG { get; set; }
    public decimal VolumeCm3 { get; set; }
}