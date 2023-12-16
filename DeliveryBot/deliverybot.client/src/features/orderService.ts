import axios from "axios";
import apiClient from "../config/apiClient";
import { CreateOrderCommand, OrderInfoDto, CancelOwnOrderCommand } from "../interfaces/order";

const createOrder = async (
  request: CreateOrderCommand,
  bearerToken: string,
): Promise<OrderInfoDto> => {
  try {
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
): Promise<OrderInfoDto[]> => {
  try {
    const headers = {
      'Authorization': 'Bearer ' + bearerToken
    };
    const response = await apiClient.get<OrderInfoDto[]>(
      '/api/Order/user-orders',
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

const cancelOrder = async (
  request: CancelOwnOrderCommand,
  bearerToken: string,
): Promise<boolean> => {
  try {
    const headers = {
      'Authorization': 'Bearer ' + bearerToken
    };
    await apiClient.post(
      '/api/Order/user-orders',
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