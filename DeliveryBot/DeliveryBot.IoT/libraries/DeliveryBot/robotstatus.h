#pragma once

enum class RobotStatus
{
  Inactive,
  Idle,
  WaitingForCargo,
  Delivering,
  ReadyForPickup,
  Returning,
  Maintenance,
  Danger
};