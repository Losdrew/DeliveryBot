namespace DeliveryBot.Db.Models;

public class CompanyEmployee : User
{
    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }
}