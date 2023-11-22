using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Delivery;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBot.Server.Features.Delivery;

public class GetActiveDeliveriesQuery : IRequest<ServiceResponse<ICollection<DeliveryInfoDto>>>
{
    public class GetActiveDeliveriesQueryHandler : ExtendedBaseHandler<GetActiveDeliveriesQuery,
        ServiceResponse<ICollection<DeliveryInfoDto>>>
    {
        public GetActiveDeliveriesQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<GetActiveDeliveriesQueryHandler> logger) 
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<ICollection<DeliveryInfoDto>>> Handle(
            GetActiveDeliveriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get active deliveries error");
                return ServiceResponseBuilder.Failure<ICollection<DeliveryInfoDto>>(DeliveryError.GetDeliveriesError);
            }
        }

        protected override async Task<ServiceResponse<ICollection<DeliveryInfoDto>>> UnsafeHandleAsync(
            GetActiveDeliveriesQuery request, CancellationToken cancellationToken)
        {
            var deliveries = Context.Deliveries
                .Include(d => d.Order)
                .Where(d => d.Order.Status == OrderStatus.Delivering || d.Order.Status == OrderStatus.PickupAvailable);
            
            var result = Mapper.Map<ICollection<DeliveryInfoDto>>(deliveries);
            return ServiceResponseBuilder.Success(result);
        }
    }
}