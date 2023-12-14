import { AddressDto } from "./address";
import { OrderStatus } from "./enums";

export interface OrderInfoDto {
  placedDateTime?: Date;
  orderAddress?: AddressDto;
  orderProducts?: OrderProductDto[] | undefined;
  id?: string;
  customerId?: string;
  orderStatus?: OrderStatus;
}

export interface OrderProductDto {
  quantity?: number;
  productId?: string;
}

export interface CreateOrderCommand {
  placedDateTime?: Date;
  orderAddress?: AddressDto;
  orderProducts?: OrderProductDto[] | undefined;
}

export interface CancelOwnOrderCommand {
  orderId?: string;
}