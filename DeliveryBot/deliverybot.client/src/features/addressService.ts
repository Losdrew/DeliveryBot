import { AddressDto } from "../interfaces/address";

const getFullAddress = (address: AddressDto) => {
  const fullAddress: string[] = [];

  fullAddress.push(`${address.addressLine1} `);
  fullAddress.push(`${address.addressLine2} `);

  if (address.addressLine3 && address.addressLine3.trim() !== '') {
    fullAddress.push(`${address.addressLine3} `);
  }

  if (address.addressLine4 && address.addressLine4.trim() !== '') {
    fullAddress.push(`${address.addressLine4} `);
  }

  fullAddress.push(`, ${address.townCity}`);
  fullAddress.push(`, ${address.region}`);
  fullAddress.push(`, ${address.country}`);

  return fullAddress.join('');
}

const addressService = {
  getFullAddress
};

export default addressService;