namespace DeliveryBot.Shared.Errors.Base;

public class ServiceError : IDisplayError
{
    public string Header { get; set; }
    public string ErrorMessage { get; set; }
}