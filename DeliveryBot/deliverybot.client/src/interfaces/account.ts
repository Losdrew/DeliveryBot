export interface AuthResultDto {
  userId?: string;
  bearer?: string | undefined;
  role?: string;
}

export interface CompanyEmployeeAccountDto {
  email?: string | undefined;
  password?: string | undefined;
  firstName?: string | undefined;
  lastName?: string | undefined;
}

export interface SignInCommand {
  email?: string | undefined;
  password?: string | undefined;
}

export interface CreateCustomerCommand  {
  email?: string | undefined;
  password?: string | undefined;
  firstName?: string | undefined;
  lastName?: string | undefined;
  phoneNumber?: string | undefined;
}

export interface CreateCompanyEmployeeCommand extends CompanyEmployeeAccountDto {}

export interface CreateAdministratorCommand extends CreateCompanyEmployeeCommand {}

export interface CreateManagerCommand extends CreateCompanyEmployeeCommand {}