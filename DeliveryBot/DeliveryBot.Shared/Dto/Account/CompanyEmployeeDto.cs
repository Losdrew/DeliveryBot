namespace DeliveryBot.Shared.Dto.Account;

public class CompanyEmployeeDto : CredentialsDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid? CompanyId { get; set; }
}