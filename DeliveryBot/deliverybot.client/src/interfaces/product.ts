export interface CompanyProductInfoDto {
  name?: string | undefined;
  price?: number;
  description?: string | undefined;
  weightG?: number;
  volumeCm3?: number;
  id?: string;
  companyId?: string;
}

export interface CreateProductCommand {
  name?: string | undefined;
  price?: number;
  description?: string | undefined;
  weightG?: number;
  volumeCm3?: number;
}

export interface EditProductCommand {
  name?: string | undefined;
  price?: number;
  description?: string | undefined;
  weightG?: number;
  volumeCm3?: number;
  id?: string;
}