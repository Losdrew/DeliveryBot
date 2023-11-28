﻿using DeliveryBot.Shared.Dto.Address;
using DeliveryBot.Shared.Dto.Robot;
using DeliveryBot.Shared.ServiceResponseHandling;

namespace DeliveryBot.Server.Services;

public interface IGeolocationService
{
    public Task<ServiceResponse<LocationDto>> GetAddressLocationAsync(AddressDto address);
}