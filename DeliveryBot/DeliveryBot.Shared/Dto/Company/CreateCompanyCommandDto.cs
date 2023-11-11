using DeliveryBot.Shared.Dto.Account;
using DeliveryBot.Shared.Dto.Address;

namespace DeliveryBot.Shared.Dto.Company;

public class CreateCompanyCommandDto : CompanyDto
{
    public ICollection<AddressDto>? CompanyAddresses { get; set; }
    public ICollection<CompanyEmployeeAccountDto>? CompanyEmployees { get; set; }
}