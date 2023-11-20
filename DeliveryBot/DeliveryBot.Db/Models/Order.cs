﻿namespace DeliveryBot.Db.Models;

public class Order : Entity
{
    public DateTime PlacedDateTime { get; set; }
    public OrderStatuses Status { get; set; }

    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public Guid? OrderAddressId { get; set; }
    public Address? OrderAddress { get; set; }

    public ICollection<OrderProduct>? OrderProducts { get; set; }
}

public enum OrderStatuses
{
    Pending,
    Delivering,
    PickupAvailable,
    Delivered,
    Cancelled
}