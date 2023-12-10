using DeliveryBot.Server.Models.Geolocation;
using DeliveryBot.Shared.Dto.Address;
using DeliveryBot.Shared.Dto.Geolocation;
using DeliveryBot.Shared.Errors.Base;
using DeliveryBot.Shared.ServiceResponseHandling;
using Newtonsoft.Json;
using System.Text;

namespace DeliveryBot.Server.Services;

public class GeolocationService : IGeolocationService
{
    private const string PositionStackBaseUrl = "http://api.positionstack.com/v1/forward";
    private const string MapboxBaseUrl = "https://api.mapbox.com/directions/v5/mapbox/walking";

    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public GeolocationService(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    public async Task<ServiceResponse<LocationDto>> GetAddressLocationAsync(AddressDto address)
    {
        var accessToken = _configuration.GetRequiredSection("PositionStackAccessToken").Value;
        var fullAddress = GetFullAddress(address);
        var endpoint = $"{PositionStackBaseUrl}?access_key={accessToken}&query={fullAddress}";

        var httpResponseMessage = await _httpClient.GetAsync(endpoint);
        var json = await httpResponseMessage.Content.ReadAsStringAsync();

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            return HandleError(json).MapErrorResult<LocationDto>();
        }

        var result = JsonConvert.DeserializeObject<GeolocationResult>(json);
        var addressResult = result.Data.First();
        var location = new LocationDto
        {
            X = addressResult.Longitude,
            Y = addressResult.Latitude
        };

        return ServiceResponseBuilder.Success(location);
    }

    public async Task<ServiceResponse<RoutesDto>> GetRoutesAsync(LocationDto firstPoint, LocationDto secondPoint)
    {
        var accessToken = _configuration.GetRequiredSection("MapBoxAccessToken").Value;
        var location = $"{firstPoint.X},{firstPoint.Y};{secondPoint.X},{secondPoint.Y}";
        var parameters = $"geometries=geojson&overview=simplified&steps=true&access_token={accessToken}";
        var endpoint = $"{MapboxBaseUrl}/{location}?{parameters}";

        var httpResponseMessage = await _httpClient.GetAsync(endpoint);
        var json = await httpResponseMessage.Content.ReadAsStringAsync();

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            return HandleError(json).MapErrorResult<RoutesDto>();
        }

        var result = JsonConvert.DeserializeObject<RoutesDto>(json);
        return ServiceResponseBuilder.Success(result);
    }

    public double CalculateDistance(LocationDto firstPoint, LocationDto secondPoint)
    {
        // Earth radius in kilometers
        const double earthRadius = 6371; 

        double latitude1Radian = DegreeToRadian(firstPoint.Y);
        double longitude1Radian = DegreeToRadian(firstPoint.X);
        double latitude2Radian = DegreeToRadian(secondPoint.Y);
        double longitude2Radian = DegreeToRadian(secondPoint.X);

        double deltaLatitude = latitude2Radian - latitude1Radian;
        double deltaLongitude = longitude2Radian - longitude1Radian;

        // Using Haversine formula to calculate distance
        double haversine = Math.Sin(deltaLatitude / 2) * Math.Sin(deltaLatitude / 2) +
                           Math.Cos(latitude1Radian) * Math.Cos(latitude2Radian) *
                           Math.Sin(deltaLongitude / 2) * Math.Sin(deltaLongitude / 2);

        double centralAngle = 2 * Math.Atan2(Math.Sqrt(haversine), Math.Sqrt(1 - haversine));

        double distance = earthRadius * centralAngle;
        return distance;
    }

    private double DegreeToRadian(double degree)
    {
        return degree * (Math.PI / 180.0);
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

    private ServiceResponse HandleError(string json)
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

        return ServiceResponseBuilder.Failure(error);
    }
}