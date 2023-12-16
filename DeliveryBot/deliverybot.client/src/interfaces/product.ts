export interface ProductDto {
  name?: string | undefined;
  price?: number;
  description?: string | undefined;
  weightG?: number;
  volumeCm3?: number;
}

export interface CompanyProductInfoDto extends ProductDto {
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

export interface CartProduct {
  id: string;
  name: string;
  description: string;
  price: number;
}