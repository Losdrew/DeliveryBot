namespace DeliveryBot.Db.Models;

public class Address : Entity
{
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? AddressLine4 { get; set; }
    public string? TownCity { get; set; }
    public string? Region { get; set; }
    public string? Country { get; set; }
    public string? Postcode { get; set; }
}