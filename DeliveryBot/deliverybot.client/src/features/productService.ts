import axios from "axios";
import apiClient from "../config/apiClient";
import { CompanyProductInfoDto } from "../interfaces/product";

const getCompanyProducts = async (
  companyId: string
): Promise<CompanyProductInfoDto[]> => {
  try {
    const response = await apiClient.get<CompanyProductInfoDto[]>(
      'api/Product/company-products?companyId=' + companyId
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

const productService = {
  getCompanyProducts
};

export default productService;