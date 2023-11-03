namespace DeliveryBot.Shared.Dto.Error;

public class ErrorDto
{
    public List<ValidationErrorDto> ValidationErrors { get; set; }
    public List<ServiceErrorDto> ServiceErrors { get; set; }
}