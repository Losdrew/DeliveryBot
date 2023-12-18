using DeliveryBot.Shared.Dto.Address;
using DeliveryBot.Shared.Dto.CompanyEmployee;

namespace DeliveryBot.Shared.Dto.Company;

public class EditCompanyCommandDto : CompanyDto
{
    public ICollection<EditableCompanyEmployeeDto>? CompanyEmployees { get; set; }
    public ICollection<AddressDto>? CompanyAddresses { get; set; }
}