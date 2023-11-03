namespace DeliveryBot.Shared.Dto.Error;

public class ServiceErrorDto
{
    public string Header { get; set; }
    public string ErrorMessage { get; set; }
    public int Code { get; set; }
}