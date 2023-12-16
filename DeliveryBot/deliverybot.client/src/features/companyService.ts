import axios from "axios";
import apiClient from "../config/apiClient";
import { CompanyPreviewDto } from "../interfaces/company";

const getCompanies = async (
): Promise<CompanyPreviewDto[]> => {
  try {
    const response = await apiClient.get<CompanyPreviewDto[]>(
      '/api/Company/companies'
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

const companyService = {
  getCompanies
};

export default companyService;