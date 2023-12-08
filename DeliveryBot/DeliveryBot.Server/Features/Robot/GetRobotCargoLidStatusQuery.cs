using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.Helpers;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Robot;

public class GetRobotCargoLidStatusQuery : IRequest<ServiceResponse<string>>
{
    public string? DeviceId { get; set; }

    public class GetRobotCargoLidStatusQueryHandler : 
        ExtendedBaseHandler<GetRobotCargoLidStatusQuery, ServiceResponse<string>>
    {
        public GetRobotCargoLidStatusQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<GetRobotCargoLidStatusQueryHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<string>> Handle(GetRobotCargoLidStatusQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get robot cargo lid status error");
                return ServiceResponseBuilder.Failure<string>(RobotError.GetDeliveryRobotError);
            }
        }

        protected override async Task<ServiceResponse<string>> UnsafeHandleAsync(GetRobotCargoLidStatusQuery request,
            CancellationToken cancellationToken)
        {
            var robot = Context.Robots.FirstOrDefault(r => r.DeviceId != null && r.DeviceId.Equals(request.DeviceId));

            if (robot == null)
            {
                return ServiceResponseBuilder.Failure<string>(RobotError.RobotNotFound);
            }

            var lidStatus = robot.IsCargoLidOpen ? CargoLidStatus.LidOpen : CargoLidStatus.LidClosed;
            return ServiceResponseBuilder.Success(lidStatus);
        }
    }
}