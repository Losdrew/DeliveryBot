using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Robot;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Robot;

public class UpdateRobotCommand : UpdateRobotCommandDto, IRequest<ServiceResponse>
{
    public class UpdateRobotCommandHandler : ExtendedBaseHandler<UpdateRobotCommand, ServiceResponse>
    {
        public UpdateRobotCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<UpdateRobotCommandHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse> Handle(UpdateRobotCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Update robot error");
                return ServiceResponseBuilder.Failure(RobotError.RobotEditError);
            }
        }

        protected override async Task<ServiceResponse> UnsafeHandleAsync(UpdateRobotCommand request,
            CancellationToken cancellationToken)
        {
            var robotToUpdate =
                Context.Robots.FirstOrDefault(r => r.DeviceId != null && r.DeviceId.Equals(request.DeviceId));

            if (robotToUpdate == null)
            {
                return ServiceResponseBuilder.Failure(RobotError.RobotNotFound);
            }

            Mapper.Map(request, robotToUpdate);
            await Context.SaveChangesAsync(cancellationToken);

            return ServiceResponseBuilder.Success();
        }
    }
}