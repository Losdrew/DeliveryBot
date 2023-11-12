using DeliveryBot.Shared.Dto.Address;
using DeliveryBot.Shared.Dto.CompanyEmployee;

namespace DeliveryBot.Shared.Dto.Company;

public class EditCompanyCommandDto : CompanyDto
{
    public ICollection<EditableAddressDto>? CompanyAddresses { get; set; }
    public ICollection<EditableCompanyEmployeeDto>? CompanyEmployees { get; set; }
}