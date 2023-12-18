import axios from "axios";
import apiClient from "../config/apiClient";

const getRoute = async (
  point1: number[],
  point2: number[]
) => {
  try {
    const coordinates = point1.join(',') + ';' + point2.join(',');
    const response = await apiClient.get(
      'api/Geolocation/route/' + coordinates
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

const geolocationService = {
  getRoute
};

export default geolocationService;