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

export const OrderStatusLabels: Record<OrderStatus, string> = {
  [OrderStatus.Pending]: "Pending",
  [OrderStatus.Delivering]: "Delivering",
  [OrderStatus.PickupAvailable]: "Pickup Available",
  [OrderStatus.Delivered]: "Delivered",
  [OrderStatus.Cancelled]: "Cancelled",
};

export const OrderStatusColors: Record<OrderStatus, string> = {
  [OrderStatus.Pending]: "#DE5DF1",
  [OrderStatus.Delivering]: "#9C9405",
  [OrderStatus.PickupAvailable]: "#9631F5",
  [OrderStatus.Delivered]: "#34C42F",
  [OrderStatus.Cancelled]: "#F54C64",
};

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

export const RobotStatusLabels: Record<RobotStatus, string> = {
  [RobotStatus.Inactive]: "Inactive",
  [RobotStatus.Idle]: "Idle",
  [RobotStatus.WaitingForCargo]: "Waiting For Cargo",
  [RobotStatus.Delivering]: "Delivering",
  [RobotStatus.ReadyForPickup]: "ReadyForPickup",
  [RobotStatus.Returning]: "Returning",
  [RobotStatus.Maintenance]: "Maintenance",
  [RobotStatus.Danger]: "Danger",
};

export const RobotStatusColors: Record<RobotStatus, string> = {
  [RobotStatus.Inactive]: "#44474D",
  [RobotStatus.Idle]: "#C77a1C",
  [RobotStatus.WaitingForCargo]: "#DE5DF1",
  [RobotStatus.Delivering]: "#9C9405",
  [RobotStatus.ReadyForPickup]: "#9631F5",
  [RobotStatus.Returning]: "#34C42F",
  [RobotStatus.Maintenance]: "#2355A6",
  [RobotStatus.Danger]: "#F54C64",
};