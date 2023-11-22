using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Delivery;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBot.Server.Features.Delivery;

public class GetDeliveryQuery : IRequest<ServiceResponse<DeliveryInfoDto>>
{
    public Guid OrderId { get; set; }

    public class GetDeliveryQueryHandler : ExtendedBaseHandler<GetDeliveryQuery, ServiceResponse<DeliveryInfoDto>>
    {
        public GetDeliveryQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<GetDeliveryQueryHandler> logger) 
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<DeliveryInfoDto>> Handle(GetDeliveryQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get delivery error");
                return ServiceResponseBuilder.Failure<DeliveryInfoDto>(DeliveryError.GetDeliveryError);
            }
        }

        protected override async Task<ServiceResponse<DeliveryInfoDto>> UnsafeHandleAsync(GetDeliveryQuery request,
            CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var customer = await Context.Customers.FindAsync(userId);

            if (!isUserIdValid || customer == null)
            {
                return ServiceResponseBuilder.Failure<DeliveryInfoDto>(UserError.InvalidAuthorization);
            }

            var delivery = await Context.Deliveries
                .FirstOrDefaultAsync(d => d.OrderId == request.OrderId, cancellationToken);

            if (delivery == null)
            {
                return ServiceResponseBuilder.Failure<DeliveryInfoDto>(DeliveryError.OrderDeliveryNotFound);
            }

            var result = Mapper.Map<DeliveryInfoDto>(delivery);
            return ServiceResponseBuilder.Success(result);
        }
    }
}