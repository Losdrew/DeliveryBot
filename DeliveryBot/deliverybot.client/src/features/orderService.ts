import axios from "axios";
import apiClient from "../config/apiClient";
import { AddressDto } from "../interfaces/address";
import { OrderStatus } from "../interfaces/enums";
import { CancelOwnOrderCommand, CreateOrderCommand, OrderFullInfo, OrderInfoDto, OrderProductDto } from "../interfaces/order";
import deliveryService from "./deliveryService";
import productService from "./productService";

const createOrder = async (
  orderAddress: AddressDto,
  orderProducts: OrderProductDto[],
  bearerToken: string,
): Promise<OrderInfoDto> => {
  try {
    const request: CreateOrderCommand = { 
      orderAddress,
      orderProducts
    }
    const headers = {
      'Authorization': 'Bearer ' + bearerToken
    };
    const response = await apiClient.post<OrderInfoDto>(
      '/api/Order/create',
      request,
      { headers }
    );
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      throw new Error(error.response?.data.message);
    } else {
      throw new Error("Unknown error occurred.");
    }
  }
};

const getOwnOrders = async (
  bearerToken: string,
): Promise<OrderFullInfo[]> => {
  try {
    const headers = {
      'Authorization': 'Bearer ' + bearerToken
    };
    const ordersResponse = await apiClient.get<OrderInfoDto[]>(
      '/api/Order/user-orders',
      { headers }
    );

    const ordersFullInfo = [];

    for (const order of ordersResponse.data) {
      const productDetails = [];

      for (const orderProduct of order.orderProducts || []) {
        const productDto = await productService.getProduct(orderProduct.productId);
        productDetails.push(productDto);
      }

      const orderFullInfo : OrderFullInfo = {
        ...order,
        products: productDetails
      };

      if (order.orderStatus !== OrderStatus.Pending && 
          order.orderStatus !== OrderStatus.Cancelled
      ) {
        const delivery = await deliveryService.getDelivery(order.id, bearerToken);
        orderFullInfo.delivery = delivery;
      }

      order.placedDateTime = new Date(Date.parse(order.placedDateTime?.toString()));

      ordersFullInfo.push(orderFullInfo);
    }

    return ordersFullInfo;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      throw new Error(error.response?.data.message);
    } else {
      throw new Error("Unknown error occurred.");
    }
  }
};

const cancelOrder = async (
  orderId: string,
  bearerToken: string,
): Promise<boolean> => {
  try {
    const request: CancelOwnOrderCommand = { orderId };
    const headers = {
      'Authorization': 'Bearer ' + bearerToken
    };
    await apiClient.post(
      '/api/Order/cancel',
      request,
      { headers }
    );
    return true;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      throw new Error(error.response?.data.message);
    } else {
      throw new Error("Unknown error occurred.");
    }
  }
};

const orderService = {
  createOrder,
  getOwnOrders,
  cancelOrder
};

export default orderService;