using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Robot;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using DocumentFormat.OpenXml.InkML;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeliveryBot.Server.Features.Robot;

public class ToggleRobotCargoLidCommand : IRequest<ServiceResponse<RobotInfoDto>>
{
    public Guid RobotId { get; set; }

    public class ToggleRobotCargoLidCommandHandler :
        ExtendedBaseHandler<ToggleRobotCargoLidCommand, ServiceResponse<RobotInfoDto>>
    {
        public ToggleRobotCargoLidCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<ToggleRobotCargoLidCommandHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<RobotInfoDto>> Handle(ToggleRobotCargoLidCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Toggle robot cargo lid error");
                return ServiceResponseBuilder.Failure<RobotInfoDto>(RobotError.RobotEditError);
            }
        }

        protected override async Task<ServiceResponse<RobotInfoDto>> UnsafeHandleAsync(ToggleRobotCargoLidCommand request,
            CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            
            if (!isUserIdValid)
            {
                return ServiceResponseBuilder.Failure<RobotInfoDto>(UserError.InvalidAuthorization);
            }

            var companyEmployee = await Context.CompanyEmployees.FindAsync(userId);
            var customer = await Context.Customers.FindAsync(userId);

            if (companyEmployee == null && customer == null)
            {
                return ServiceResponseBuilder.Failure<RobotInfoDto>(UserError.InvalidAuthorization);
            }

            var robotToEdit = await Context.Robots.FindAsync(request.RobotId);

            if (robotToEdit == null)
            {
                return ServiceResponseBuilder.Failure<RobotInfoDto>(RobotError.RobotNotFound);
            }

            Db.Models.Delivery? delivery;

            if (companyEmployee != null)
            {
                delivery = Context.Deliveries
                    .Include(d => d.Order)
                    .FirstOrDefault(d => d.CompanyEmployeeId == companyEmployee.Id);

                if (delivery == null || delivery.Order.Status != OrderStatus.Pending || 
                    (robotToEdit.Status != RobotStatus.Idle && robotToEdit.Status != RobotStatus.WaitingForCargo))
                {
                    return ServiceResponseBuilder.Failure<RobotInfoDto>(RobotError.RobotUnavailableError);
                }

                robotToEdit.Status = RobotStatus.WaitingForCargo;
                robotToEdit.IsCargoLidOpen = !robotToEdit.IsCargoLidOpen;
            }

            if (customer != null)
            {
                delivery = Context.Deliveries
                    .Include(d => d.Order)
                    .FirstOrDefault(d => d.Order.CustomerId == customer.Id);

                if (delivery == null || delivery.Order.Status != OrderStatus.PickupAvailable ||
                    delivery.Robot.Status != RobotStatus.ReadyForPickup)
                {
                    return ServiceResponseBuilder.Failure<RobotInfoDto>(RobotError.RobotUnavailableError);
                }

                if (robotToEdit.IsCargoLidOpen)
                {
                    robotToEdit.Status = RobotStatus.Returning;
                }

                robotToEdit.IsCargoLidOpen = !robotToEdit.IsCargoLidOpen;
            }

            await Context.SaveChangesAsync(cancellationToken);

            var result = Mapper.Map<RobotInfoDto>(robotToEdit);
            return ServiceResponseBuilder.Success(result);
        }
    }
}