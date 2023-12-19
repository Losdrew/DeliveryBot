import { AddressDto } from "./address";
import { DeliveryFullInfo } from "./delivery";
import { OrderStatus } from "./enums";
import { ProductDto } from "./product";

export interface OrderInfoDto {
  placedDateTime?: Date;
  orderAddress?: AddressDto;
  orderProducts?: OrderProductDto[] | undefined;
  id?: string;
  customerId?: string;
  orderStatus?: OrderStatus;
}

export interface OrderFullInfo {
  placedDateTime?: Date;
  orderAddress?: AddressDto;
  products?: ProductDto[] | undefined;
  id?: string;
  customerId?: string;
  orderStatus?: OrderStatus;
  delivery?: DeliveryFullInfo
}

export interface OrderProductDto {
  quantity?: number;
  productId?: string;
}

export interface CreateOrderCommand {
  orderAddress?: AddressDto;
  orderProducts?: OrderProductDto[] | undefined;
}

export interface CancelOwnOrderCommand {
  orderId?: string;
}