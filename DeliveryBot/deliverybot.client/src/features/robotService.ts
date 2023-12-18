import axios from "axios";
import apiClient from "../config/apiClient";
import { RobotInfoDto } from "../interfaces/robot";

const getOwnCompanyRobots = async (
  bearerToken: string
): Promise<RobotInfoDto[]> => {
  try {
    const headers = {
      'Authorization': 'Bearer ' + bearerToken
    };
    const response = await apiClient.get<RobotInfoDto[]>(
      'api/Robot/own-company-robots',
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

const robotService = {
  getOwnCompanyRobots
};

export default robotService;