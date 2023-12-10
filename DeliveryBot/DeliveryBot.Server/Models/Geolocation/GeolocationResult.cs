namespace DeliveryBot.Server.Models.Geolocation;

public class GeolocationResult
{
    public List<GeolocationAddress>? Data { get; set; }
}

public class GeolocationAddress
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Label { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Number { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public double Confidence { get; set; }
    public string Region { get; set; }
    public string RegionCode { get; set; }
    public string AdministrativeArea { get; set; }
    public string Neighbourhood { get; set; }
    public string Country { get; set; }
    public string CountryCode { get; set; }
    public string MapUrl { get; set; }
}
