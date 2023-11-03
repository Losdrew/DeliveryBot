namespace DeliveryBot.Shared.Errors.Base;

public class ValidationError : IDisplayError
{
    public string FieldCode { get; set; }
    public string ErrorMessage { get; set; }
}