import axios from "axios";
import apiClient from "../config/apiClient";
import { CompanyPreviewDto, CreateCompanyCommand, EditCompanyCommand, OwnCompanyInfoDto } from "../interfaces/company";
import { CompanyEmployeeAccountDto } from "../interfaces/account";
import { AddressDto } from "../interfaces/address";

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

const getOwnCompany = async (
  bearerToken : string
): Promise<OwnCompanyInfoDto> => {
  try {
    const headers = {
      'Authorization': 'Bearer ' + bearerToken
    };
    const response = await apiClient.get<OwnCompanyInfoDto>(
      '/api/Company/user-company',
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

const createCompany = async (
  name: string,
  description: string,
  websiteUrl: string,
  companyAddresses: AddressDto[],
  companyEmployees: CompanyEmployeeAccountDto[],
  bearerToken : string
): Promise<OwnCompanyInfoDto> => {
  try {
    const request: CreateCompanyCommand = { 
      name,
      description, 
      websiteUrl, 
      companyAddresses, 
      companyEmployees 
    };
    const headers = {
      'Authorization': 'Bearer ' + bearerToken
    };
    const response = await apiClient.post<OwnCompanyInfoDto>(
      '/api/Company/create',
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

const editCompany = async (
  bearerToken : string,
  name?: string,
  description?: string,
  websiteUrl?: string,
  companyAddresses?: AddressDto[],
  companyEmployees?: CompanyEmployeeAccountDto[],
): Promise<OwnCompanyInfoDto> => {
  try {
    const addresses: AddressDto[] | undefined = companyAddresses?.map((address) => ({
      ...address,
      id: address.isNew ? undefined : address.id,
    }));
    const request: EditCompanyCommand = { 
      name,
      description, 
      websiteUrl, 
      companyAddresses : addresses, 
      companyEmployees 
    };
    const headers = {
      'Authorization': 'Bearer ' + bearerToken
    };
    const response = await apiClient.post<OwnCompanyInfoDto>(
      '/api/Company/edit',
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

const companyService = {
  getCompanies,
  getOwnCompany,
  createCompany,
  editCompany
};

export default companyService;