import axios from "axios";
import apiClient from "../config/apiClient";
import { AuthResultDto, CreateAdministratorCommand, CreateCompanyEmployeeCommand, CreateCustomerCommand, CreateManagerCommand, SignInCommand } from "../interfaces/account";

const signIn = async (
  email : string,
  password : string
): Promise<AuthResultDto> => {
  try {
    const request: SignInCommand = { email, password };
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
  email : string,
  password : string,
  firstName : string,
  lastName : string
): Promise<AuthResultDto> => {
  try {
    const request: CreateCustomerCommand = { 
      email, 
      password, 
      firstName, 
      lastName 
    };
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
  email : string,
  password : string,
  firstName : string,
  lastName : string
): Promise<AuthResultDto> => {
  try {
    const request: CreateCompanyEmployeeCommand = { 
      email, 
      password, 
      firstName, 
      lastName 
    };
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
  email : string,
  password : string,
  firstName : string,
  lastName : string
): Promise<AuthResultDto> => {
  try {
    const request: CreateManagerCommand = { 
      email, 
      password, 
      firstName, 
      lastName 
    };
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
  email : string,
  password : string,
  firstName : string,
  lastName : string
): Promise<AuthResultDto> => {
  try {
    const request: CreateAdministratorCommand = { 
      email, 
      password, 
      firstName, 
      lastName 
    };
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