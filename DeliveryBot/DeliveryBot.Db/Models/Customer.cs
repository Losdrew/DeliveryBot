namespace DeliveryBot.Db.Models;

public class Customer : User
{
    public string? PhoneNumber { get; set; }

    public Address? CustomerAddress { get; set; }
}