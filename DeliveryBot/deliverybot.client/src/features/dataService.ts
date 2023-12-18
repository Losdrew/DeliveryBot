import axios from "axios";
import apiClient from "../config/apiClient";

const getExportedData = async (
  bearerToken: string
) => {
  try {
    const headers = {
      'Authorization': 'Bearer ' + bearerToken,
    };
    const response = await apiClient.get(
      '/api/Data/export-data',
      { headers, responseType: 'arraybuffer' }
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

const dataService = {
  getExportedData
};

export default dataService;