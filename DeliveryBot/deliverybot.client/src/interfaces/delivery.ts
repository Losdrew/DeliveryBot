export interface DeliveryInfoDto {
  shippedDateTime?: Date;
  deliveredDateTime?: Date | undefined;
  robotId?: string | undefined;
  orderId?: string | undefined;
  id?: string;
}

export interface CreateDeliveryCommand {
  shippedDateTime?: Date;
  robotId?: string | undefined;
  orderId?: string | undefined;
}

