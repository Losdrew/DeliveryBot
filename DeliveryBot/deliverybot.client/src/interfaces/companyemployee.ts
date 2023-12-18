export interface CompanyEmployeeDto {
  id?: string;
  email?: string | undefined;
  firstName?: string | undefined;
  lastName?: string | undefined;
}

export interface CompanyEmployeeInfoDto extends CompanyEmployeeDto {
  id?: string;
}

export interface EditableCompanyEmployeeDto extends CompanyEmployeeDto {
  password?: string | undefined;
}

export interface EditCompanyEmployeeCommand extends EditableCompanyEmployeeDto {}