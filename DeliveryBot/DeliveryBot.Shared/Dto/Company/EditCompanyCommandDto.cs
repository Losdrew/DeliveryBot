using DeliveryBot.Shared.Dto.Address;
using DeliveryBot.Shared.Dto.CompanyEmployee;

namespace DeliveryBot.Shared.Dto.Company;

public class EditCompanyCommandDto : CompanyDto
{
    public ICollection<AddressDto>? CompanyAddresses { get; set; }
    public ICollection<CompanyEmployeeDto>? CompanyEmployees { get; set; }
}