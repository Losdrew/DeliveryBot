namespace DeliveryBot.Shared.Dto.Geolocation;

public class GetRouteQueryDto
{
    public LocationDto FirstPoint { get; set; }
    public LocationDto SecondPoint { get; set; }
}