namespace DeliveryBot.Shared.Dto.Account;

public class CreateCompanyEmployeeCommandDto : CompanyEmployeeAccountDto
{
    public Guid? CompanyId { get; set; }
}