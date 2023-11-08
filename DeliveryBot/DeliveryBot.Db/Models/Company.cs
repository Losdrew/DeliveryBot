namespace DeliveryBot.Db.Models;

public class Company : Entity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? WebsiteUrl { get; set; }

    public ICollection<Address>? CompanyAddresses { get; set; }
    public ICollection<CompanyEmployee>? CompanyEmployees { get; set; }
}