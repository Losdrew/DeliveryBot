import axios from "axios";
import apiClient from "../config/apiClient";
import { CreateOrderCommand, OrderInfoDto } from "../interfaces/order";

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

const orderService = {
  createOrder
};

export default orderService;