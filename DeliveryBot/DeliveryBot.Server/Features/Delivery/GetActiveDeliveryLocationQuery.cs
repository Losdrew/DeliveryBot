using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Server.Services;
using DeliveryBot.Shared.Dto.Address;
using DeliveryBot.Shared.Dto.Delivery;
using DeliveryBot.Shared.Dto.Robot;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBot.Server.Features.Delivery;

public class GetActiveDeliveryLocationQuery : IRequest<ServiceResponse<LocationDto>>
{
    public string DeviceId { get; set; }

    public class GetActiveDeliveryLocationQueryHandler 
        : ExtendedBaseHandler<GetActiveDeliveryLocationQuery, ServiceResponse<LocationDto>>
    {
        private readonly IGeolocationService _geolocationService;

        public GetActiveDeliveryLocationQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<GetActiveDeliveryLocationQueryHandler> logger, IGeolocationService geolocationService)
            : base(context, contextAccessor, mapper, logger)
        {
            _geolocationService = geolocationService;
        }

        public override async Task<ServiceResponse<LocationDto>> Handle(
            GetActiveDeliveryLocationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get active delivery location error");
                return ServiceResponseBuilder.Failure<LocationDto>(DeliveryError.GetDeliveryError);
            }
        }

        protected override async Task<ServiceResponse<LocationDto>> UnsafeHandleAsync(
            GetActiveDeliveryLocationQuery request, CancellationToken cancellationToken)
        {
            var delivery = await Context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.Order.OrderAddress)
                .Include(d => d.Robot)
                .FirstOrDefaultAsync(d => d.Robot.DeviceId.Equals(request.DeviceId), cancellationToken);

            if (delivery == null || delivery.Order.Status != OrderStatus.Delivering)
            {
                return ServiceResponseBuilder.Failure<LocationDto>(DeliveryError.OrderDeliveryNotFound);
            }

            var address = Mapper.Map<AddressDto>(delivery.Order.OrderAddress);

            var location = await _geolocationService.GetAddressLocationAsync(address);

            if (!location.IsSuccess)
            {
                return ServiceResponseBuilder.Failure<LocationDto>(location.Error);
            }

            return ServiceResponseBuilder.Success(location.Result);
        }
    }
}