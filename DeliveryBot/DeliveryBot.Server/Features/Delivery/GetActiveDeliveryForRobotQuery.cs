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

public class GetActiveDeliveryForRobotQuery : IRequest<ServiceResponse<DeliveryInfoDto>>
{
    public string DeviceId { get; set; }

    public class GetActiveDeliveryForRobotQueryHandler 
        : ExtendedBaseHandler<GetActiveDeliveryForRobotQuery, ServiceResponse<DeliveryInfoDto>>
    {
        public GetActiveDeliveryForRobotQueryHandler(ApplicationDbContext context, IMapper mapper,
            IHttpContextAccessor contextAccessor, ILogger<GetActiveDeliveryForRobotQueryHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<DeliveryInfoDto>> Handle(
            GetActiveDeliveryForRobotQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get active delivery for robot error");
                return ServiceResponseBuilder.Failure<DeliveryInfoDto>(DeliveryError.GetDeliveryError);
            }
        }

        protected override async Task<ServiceResponse<DeliveryInfoDto>> UnsafeHandleAsync(
            GetActiveDeliveryForRobotQuery request, CancellationToken cancellationToken)
        {
            var delivery = await Context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.Robot)
                .FirstOrDefaultAsync(d => d.Robot.DeviceId.Equals(request.DeviceId), cancellationToken);

            if (delivery == null || delivery.Order.Status != OrderStatus.Delivering)
            {
                return ServiceResponseBuilder.Failure<DeliveryInfoDto>(DeliveryError.OrderDeliveryNotFound);
            }

            var result = Mapper.Map<DeliveryInfoDto>(delivery);
            return ServiceResponseBuilder.Success(result);
        }
    }
}