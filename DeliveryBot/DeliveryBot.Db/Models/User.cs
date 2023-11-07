namespace DeliveryBot.Db.Models;

public class User : Entity
{
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}