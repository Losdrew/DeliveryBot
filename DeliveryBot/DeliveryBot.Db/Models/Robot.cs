﻿using System.ComponentModel.DataAnnotations.Schema;
using Point = NetTopologySuite.Geometries.Point;

namespace DeliveryBot.Db.Models;

public class Robot : Entity
{
    public string? Name { get; set; }
    public RobotStatus Status { get; set; }
    public int BatteryCharge { get; set; }
    public decimal WeightCapacityG { get; set; }
    public decimal VolumeCapacityCm3 { get; set; }
    public bool IsCargoLidOpen { get; set; }
    public string? DeviceId { get; set; }

    [Column(TypeName="geometry (point)")]
    public Point? Location { get; set; }

    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }
}

public enum RobotStatus
{
    Inactive,
    Idle,
    WaitingForCargo,
    Delivering,
    ReadyForPickup,
    Returning,
    Maintenance,
    Danger
}