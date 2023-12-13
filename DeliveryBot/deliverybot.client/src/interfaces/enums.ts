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