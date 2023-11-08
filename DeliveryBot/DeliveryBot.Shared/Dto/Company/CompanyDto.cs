using DeliveryBot.Shared.Dto.Account;
using DeliveryBot.Shared.Dto.Address;

namespace DeliveryBot.Shared.Dto.Company;

public class CompanyDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string? WebsiteUrl { get; set; }

    public ICollection<AddressDto>? CompanyAddresses { get; set; }
    public ICollection<CompanyEmployeeDto>? CompanyEmployees { get; set; }
}