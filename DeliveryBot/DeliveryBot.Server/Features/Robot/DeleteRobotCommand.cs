using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBot.Server.Features.Robot;

public class DeleteRobotCommand : IRequest<ServiceResponse>
{
    public Guid RobotId { get; set; }

    public class DeleteRobotCommandHandler :
        ExtendedBaseHandler<DeleteRobotCommand, ServiceResponse>
    {
        public DeleteRobotCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<DeleteRobotCommandHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse> Handle(DeleteRobotCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Delete robot error");
                return ServiceResponseBuilder.Failure(RobotError.RobotDeleteError);
            }
        }

        protected override async Task<ServiceResponse> UnsafeHandleAsync(DeleteRobotCommand request,
            CancellationToken cancellationToken)
        {
            var robotToDelete = Context.Robots.Where(p => p.Id == request.RobotId);
            var isDeleted = await robotToDelete.ExecuteDeleteAsync(cancellationToken);

            if (isDeleted == 0)
            {
                return ServiceResponseBuilder.Failure(RobotError.RobotNotFound);
            }

            return ServiceResponseBuilder.Success();
        }
    }
}