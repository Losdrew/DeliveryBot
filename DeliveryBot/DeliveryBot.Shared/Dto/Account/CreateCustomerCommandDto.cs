namespace DeliveryBot.Shared.Dto.Account;

public class CreateCustomerCommandDto : CredentialsDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
}