namespace DeliveryBot.Shared.Errors.Base;

public class Error
{
    public List<ValidationError> ValidationErrors { get; set; } = new();
    public List<ServiceError> ServiceErrors { get; set; } = new();

    public Error(List<ValidationError>? validationErrors = null, List<ServiceError>? serviceErrors = null)
    {
        ValidationErrors = validationErrors ?? ValidationErrors;
        ServiceErrors = serviceErrors ?? ServiceErrors;
    }
}