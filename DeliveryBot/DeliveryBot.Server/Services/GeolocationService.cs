using DeliveryBot.Server.Models.Geolocation;
using DeliveryBot.Shared.Dto.Address;
using DeliveryBot.Shared.Dto.Robot;
using DeliveryBot.Shared.Errors.Base;
using DeliveryBot.Shared.ServiceResponseHandling;
using Newtonsoft.Json;
using System.Text;

namespace DeliveryBot.Server.Services;

public class GeolocationService : IGeolocationService
{
    private const string BaseUrl = "http://api.positionstack.com/";

    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public GeolocationService(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<ServiceResponse<LocationDto>> GetAddressLocationAsync(AddressDto address)
    {
        var accessKey = _configuration.GetRequiredSection("GeolocationAPIAccessKey").Value;
        var fullAddress = GetFullAddress(address);
        var endpoint = $"v1/forward?access_key={accessKey}&query={fullAddress}";

        var httpResponseMessage = await _httpClient.GetAsync(endpoint);
        var json = await httpResponseMessage.Content.ReadAsStringAsync();

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            return HandleError(json);
        }

        return HandleSuccess(json);
    }

    private string GetFullAddress(AddressDto address)
    {
        var fullAddress = new StringBuilder();

        fullAddress.Append(address.AddressLine1 + " ");
        fullAddress.Append(address.AddressLine2 + " ");

        if (!string.IsNullOrEmpty(address.AddressLine3))
        {
            fullAddress.Append(address.AddressLine3 + " ");
        }

        if (!string.IsNullOrEmpty(address.AddressLine4))
        {
            fullAddress.Append(address.AddressLine4 + " ");
        }

        fullAddress.Append(", " + address.TownCity);
        fullAddress.Append(", " + address.Region);
        fullAddress.Append(", " + address.Country);

        return fullAddress.ToString();
    }

    private ServiceResponse<LocationDto> HandleSuccess(string json)
    {
        var result = JsonConvert.DeserializeObject<GeolocationResult>(json);
        var addressResult = result.Data.First();
        var location = new LocationDto
        {
            X = addressResult.Longitude,
            Y = addressResult.Latitude
        };
        return ServiceResponseBuilder.Success(location);
    }

    private ServiceResponse<LocationDto> HandleError(string json)
    {
        var errorResult = JsonConvert.DeserializeObject<GeolocationError>(json);
        var error = new Error().ServiceErrors = new List<ServiceError>
        {
            new()
            {
                Code = 400,
                Header = errorResult?.Error?.Code,
                ErrorMessage = errorResult?.Error?.Message
            }
        };

        return ServiceResponseBuilder.Failure<LocationDto>(error);
    }
}