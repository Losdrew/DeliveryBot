import axios from "axios";
import apiClient from "../config/apiClient";
import { DeliveryFullInfo, DeliveryInfoDto } from "../interfaces/delivery";
import { GetDeliveryRobotQueryDto } from "../interfaces/robot";

const getDelivery = async (
  orderId: string,
  bearerToken: string,
): Promise<DeliveryFullInfo> => {
  try {
    const headers = {
      'Authorization': 'Bearer ' + bearerToken
    };
    const deliveryResponse = await apiClient.get<DeliveryInfoDto>(
      '/api/Delivery/order-delivery?orderId=' + orderId,
      { headers }
    );

    const deliveredDateTime = new Date(Date.parse(deliveryResponse.data.deliveredDateTime.toString()));
    const shippedDateTime = new Date(Date.parse(deliveryResponse.data.shippedDateTime.toString()));
    const delivery = { ...deliveryResponse.data, deliveredDateTime, shippedDateTime  }

    const robotResponse = await apiClient.get<GetDeliveryRobotQueryDto>(
      '/api/Robot/delivery-robot?deliveryId=' + delivery.id,
      { headers }
    );

    const deliveryFullInfo = { ...delivery, robot: robotResponse.data };
    return deliveryFullInfo;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      throw new Error(error.response?.data.message);
    } else {
      throw new Error("Unknown error occurred.");
    }
  }
};

const deliveryService = {
  getDelivery
};

export default deliveryService;