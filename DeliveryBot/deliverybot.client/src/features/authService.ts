import axios from "axios";
import apiClient from "../config/apiClient";
import { AuthResultDto, SignInCommand } from "../interfaces/account";

const signIn = async (
  request: SignInCommand
): Promise<AuthResultDto> => {
  try {
    const response = await apiClient.post<AuthResultDto>(
      '/api/Account/sign-in',
      request
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

const signUpCustomer = async (
  request: SignInCommand
): Promise<AuthResultDto> => {
  try {
    const response = await apiClient.post<AuthResultDto>(
      '/api/Account/customer/create',
      request
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

const signUpCompanyEmployee = async (
  request: SignInCommand
): Promise<AuthResultDto> => {
  try {
    const response = await apiClient.post<AuthResultDto>(
      '/api/Account/company-employee/create',
      request
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

const signUpManager = async (
  request: SignInCommand
): Promise<AuthResultDto> => {
  try {
    const response = await apiClient.post<AuthResultDto>(
      '/api/Account/manager/create',
      request
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

const signUpAdministrator = async (
  request: SignInCommand
): Promise<AuthResultDto> => {
  try {
    const response = await apiClient.post<AuthResultDto>(
      '/api/Account/administrator/create',
      request
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

const authService = {
  signIn,
  signUpCustomer,
  signUpCompanyEmployee,
  signUpManager,
  signUpAdministrator
};

export default authService;