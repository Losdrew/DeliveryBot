using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Robot;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var robot = Context.Robots.FirstOrDefault(r => r.DeviceId != null && r.DeviceId.Equals(request.DeviceId));

            if (robot == null)
            {
                return ServiceResponseBuilder.Failure(RobotError.RobotNotFound);
            }

            Mapper.Map(request, robot);

            var delivery = Context.Deliveries
                .Include(d => d.Order)
                .FirstOrDefault(d => d.RobotId == robot.Id);

            if (delivery == null)
            {
                return ServiceResponseBuilder.Failure(DeliveryError.OrderDeliveryNotFound);
            }

            delivery.Order.Status = robot.Status switch
            {
                RobotStatus.Delivering => OrderStatus.Delivering,
                RobotStatus.ReadyForPickup => OrderStatus.PickupAvailable,
                RobotStatus.Returning => OrderStatus.Delivered,
                _ => delivery.Order.Status
            };

            await Context.SaveChangesAsync(cancellationToken);
            return ServiceResponseBuilder.Success();
        }
    }
}