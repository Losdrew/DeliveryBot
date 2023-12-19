import { RobotStatus } from "./enums";
import { LocationDto } from "./geolocation";

export interface RobotInfoDto {
  name?: string | undefined;
  status?: RobotStatus;
  location?: LocationDto;
  batteryCharge?: number;
  weightCapacityG?: number;
  volumeCapacityCm3?: number;
  isCargoLidOpen?: boolean;
  deviceId?: string | undefined;
  companyId?: string | undefined;
  id?: string;
}

export interface GetDeliveryRobotQueryDto {
  name?: string | undefined;
  status?: RobotStatus;
  location?: LocationDto;
}

export interface CreateRobotCommand {
  name?: string | undefined;
  weightCapacityG?: number;
  volumeCapacityCm3?: number;
  deviceId?: string | undefined;
}

export interface EditRobotCommand {
  id?: string;
  name?: string | undefined;
  weightCapacityG?: number;
  volumeCapacityCm3?: number;
  deviceId?: string | undefined;
}

export interface SetRobotStatusCommand {
  id?: string;
  newStatus?: RobotStatus;
}

export interface ToggleRobotCargoLidCommand {
  robotId?: string;
}
