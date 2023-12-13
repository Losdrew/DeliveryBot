import { CompanyEmployeeAccountDto } from "./account";
import { AddressDto, EditableAddressDto } from "./address";
import { CompanyEmployeeDto, EditableCompanyEmployeeDto } from "./companyemployee";

export interface OwnCompanyInfoDto {
  name?: string | undefined;
  description?: string | undefined;
  websiteUrl?: string | undefined;
  id?: string;
  companyAddresses?: AddressDto[] | undefined;
  companyEmployees?: CompanyEmployeeDto[] | undefined;
}

export interface CompanyPreviewDto {
  name?: string | undefined;
  description?: string | undefined;
}

export interface CreateCompanyCommand {
  name?: string | undefined;
  description?: string | undefined;
  websiteUrl?: string | undefined;
  companyAddresses?: AddressDto[] | undefined;
  companyEmployees?: CompanyEmployeeAccountDto[] | undefined;
}

export interface EditCompanyCommand {
  name?: string | undefined;
  description?: string | undefined;
  websiteUrl?: string | undefined;
  companyAddresses?: EditableAddressDto[] | undefined;
  companyEmployees?: EditableCompanyEmployeeDto[] | undefined;
}