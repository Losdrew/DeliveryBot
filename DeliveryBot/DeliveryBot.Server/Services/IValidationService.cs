using DeliveryBot.Shared.ServiceResponseHandling;

namespace DeliveryBot.Server.Services;

public interface IValidationService
{
    public Task<ServiceResponse> ValidateAsync<T>(T item);
}