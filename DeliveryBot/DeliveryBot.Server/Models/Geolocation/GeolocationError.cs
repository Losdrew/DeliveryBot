namespace DeliveryBot.Server.Models.Geolocation;

public class GeolocationError
{
    public GeolocationErrorResponse? Error { get; set; }
}

public class GeolocationErrorResponse
{
    public string? Code { get; set; }
    public string? Message { get; set; }
}
