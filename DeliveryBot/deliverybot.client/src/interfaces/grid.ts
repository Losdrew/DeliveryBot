import { GridValidRowModel } from "@mui/x-data-grid/models";
import { AddressDto } from "./address";
import { CompanyEmployeeInfoDto } from "./companyemployee";
import { CompanyProductInfoDto } from "./product";

export interface GridCompanyAddress extends AddressDto, GridValidRowModel { }
export interface GridCompanyEmployee extends CompanyEmployeeInfoDto, GridValidRowModel { }
export interface GridCompanyProduct extends CompanyProductInfoDto, GridValidRowModel { }