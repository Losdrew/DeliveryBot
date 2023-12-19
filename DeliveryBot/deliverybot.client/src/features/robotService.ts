import axios from "axios";
import apiClient from "../config/apiClient";
import { CreateRobotCommand, EditRobotCommand, RobotInfoDto } from "../interfaces/robot";

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

const createRobot = async (
  name: string,
  weightCapacityG: number,
  volumeCapacityCm3: number,
  deviceId: string,
  bearerToken: string
): Promise<RobotInfoDto> => {
  try {
    const request: CreateRobotCommand = { 
      name,
      weightCapacityG,
      volumeCapacityCm3,
      deviceId,
    }
    const headers = {
      'Authorization': 'Bearer ' + bearerToken
    };
    const response = await apiClient.post<RobotInfoDto>(
      'api/Robot/create',
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

const editRobot = async (
  bearerToken: string,
  id?: string,
  name?: string,
  weightCapacityG?: number,
  volumeCapacityCm3?: number,
  deviceId?: string
): Promise<RobotInfoDto> => {
  try {
    const request: EditRobotCommand = { 
      id,
      name,
      weightCapacityG,
      volumeCapacityCm3,
      deviceId,
    }
    const headers = {
      'Authorization': 'Bearer ' + bearerToken
    };
    const response = await apiClient.post<RobotInfoDto>(
      'api/Robot/edit',
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

const robotService = {
  getOwnCompanyRobots,
  createRobot,
  editRobot
};

export default robotService;