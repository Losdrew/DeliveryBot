using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Robot;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBot.Server.Features.Robot;

public class GetDeliveryRobotQuery : IRequest<ServiceResponse<GetDeliveryRobotQueryDto>>
{
    public Guid DeliveryId { get; set; }

    public class GetDeliveryRobotQueryHandler : 
        ExtendedBaseHandler<GetDeliveryRobotQuery, ServiceResponse<GetDeliveryRobotQueryDto>>
    {
        public GetDeliveryRobotQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<GetDeliveryRobotQueryHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<GetDeliveryRobotQueryDto>> Handle(GetDeliveryRobotQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get delivery robot error");
                return ServiceResponseBuilder.Failure<GetDeliveryRobotQueryDto>(RobotError.GetDeliveryRobotError);
            }
        }

        protected override async Task<ServiceResponse<GetDeliveryRobotQueryDto>> UnsafeHandleAsync(
            GetDeliveryRobotQuery request, CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var customer = await Context.Customers.FindAsync(userId);

            if (!isUserIdValid || customer == null)
            {
                return ServiceResponseBuilder.Failure<GetDeliveryRobotQueryDto>(UserError.InvalidAuthorization);
            }

            var delivery = await Context.Deliveries
                .Include(d => d.Robot)
                .FirstOrDefaultAsync(d => d.Id == request.DeliveryId, cancellationToken);

            if (delivery == null)
            {
                return ServiceResponseBuilder.Failure<GetDeliveryRobotQueryDto>(DeliveryError.OrderDeliveryNotFound);
            }

            var robot = delivery.Robot;

            if (robot == null)
            {
                return ServiceResponseBuilder.Failure<GetDeliveryRobotQueryDto>(RobotError.RobotNotFound);
            }

            var result = Mapper.Map<GetDeliveryRobotQueryDto>(robot);
            return ServiceResponseBuilder.Success(result);
        }
    }
}