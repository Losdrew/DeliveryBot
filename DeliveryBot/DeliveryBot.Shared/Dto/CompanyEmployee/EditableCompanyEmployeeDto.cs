namespace DeliveryBot.Shared.Dto.CompanyEmployee;

public class EditableCompanyEmployeeDto : CompanyEmployeeDto
{
    public Guid Id { get; set; }
    public string? Password { get; set; }
}