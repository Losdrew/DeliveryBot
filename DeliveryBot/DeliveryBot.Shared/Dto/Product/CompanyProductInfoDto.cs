namespace DeliveryBot.Shared.Dto.Product;

public class CompanyProductInfoDto : ProductDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
}