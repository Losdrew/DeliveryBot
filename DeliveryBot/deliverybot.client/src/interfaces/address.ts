export interface AddressDto {
  addressLine1?: string | undefined;
  addressLine2?: string | undefined;
  addressLine3?: string | undefined;
  addressLine4?: string | undefined;
  townCity?: string | undefined;
  region?: string | undefined;
  country?: string | undefined;
  postcode?: string | undefined;
}

export interface EditableAddressDto extends AddressDto {
  id?: string;
}