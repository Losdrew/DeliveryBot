using DeliveryBot.Shared.Dto.Address;
using DeliveryBot.Shared.Dto.Geolocation;
using DeliveryBot.Shared.ServiceResponseHandling;

namespace DeliveryBot.Server.Services;

public interface IGeolocationService
{
    public Task<ServiceResponse<LocationDto>> GetAddressLocationAsync(AddressDto address);
    public Task<ServiceResponse<RoutesDto>> GetRoutesAsync(LocationDto firstPoint, LocationDto secondPoint);
}