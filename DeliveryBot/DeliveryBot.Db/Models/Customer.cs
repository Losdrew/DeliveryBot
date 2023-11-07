namespace DeliveryBot.Db.Models;

public class Customer : Entity
{
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }

    public Address? CustomerAddress { get; set; }
}