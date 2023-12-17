import { GetDeliveryRobotQueryDto } from "./robot";

export interface DeliveryInfoDto {
  shippedDateTime?: Date | undefined;
  deliveredDateTime?: Date | undefined;
  robotId?: string | undefined;
  orderId?: string | undefined;
  id?: string;
}

export interface DeliveryFullInfo extends DeliveryInfoDto {
  robot: GetDeliveryRobotQueryDto
}

export interface CreateDeliveryCommand {
  shippedDateTime?: Date;
  robotId?: string | undefined;
  orderId?: string | undefined;
}

