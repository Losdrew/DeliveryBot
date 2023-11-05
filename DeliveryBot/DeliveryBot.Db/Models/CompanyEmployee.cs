﻿namespace DeliveryBot.Db.Models;

public class CompanyEmployee : Entity
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public Company? Company { get; set; }
}