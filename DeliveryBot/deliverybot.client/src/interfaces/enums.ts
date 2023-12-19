export enum Roles {
  None = "",
  Administrator = "admin",
  Manager = "manager",
  CompanyEmployee = "company-employee",
  Customer = "customer"
}

export enum OrderStatus {
  Pending,
  Delivering,
  PickupAvailable,
  Delivered,
  Cancelled
}

export enum RobotStatus {
  Inactive,
  Idle,
  WaitingForCargo,
  Delivering,
  ReadyForPickup,
  Returning,
  Maintenance,
  Danger
}
