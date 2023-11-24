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

public class CreateDeliveryCommand : CreateDeliveryCommandDto, IRequest<ServiceResponse<DeliveryInfoDto>>
{
    public class CreateDeliveryCommandHandler :
        ExtendedBaseHandler<CreateDeliveryCommand, ServiceResponse<DeliveryInfoDto>>
    {
        public CreateDeliveryCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<CreateDeliveryCommandHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<DeliveryInfoDto>> Handle(CreateDeliveryCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Delivery creation error");
                return ServiceResponseBuilder.Failure<DeliveryInfoDto>(DeliveryError.DeliveryCreateError);
            }
        }

        protected override async Task<ServiceResponse<DeliveryInfoDto>> UnsafeHandleAsync(CreateDeliveryCommand request,
            CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var companyEmployee = await Context.CompanyEmployees.FindAsync(userId);

            if (!isUserIdValid || companyEmployee == null)
            {
                return ServiceResponseBuilder.Failure<DeliveryInfoDto>(UserError.InvalidAuthorization);
            }

            var newDelivery = Mapper.Map<Db.Models.Delivery>(request);

            var order = await Context.Orders
                .Include(d => d.OrderProducts)
                .FirstOrDefaultAsync(d => d.Id == newDelivery.OrderId, cancellationToken);

            var robot = await Context.FindAsync<Db.Models.Robot>(newDelivery.RobotId);

            var validationResult = await ValidateDelivery(order, robot);

            if (!validationResult.IsSuccess)
            {
                return (ServiceResponse<DeliveryInfoDto>)validationResult;
            }

            robot.Status = RobotStatus.Delivering;
            order.Status = OrderStatus.Delivering;
            newDelivery.CompanyEmployee = companyEmployee;
            Context.Add(newDelivery);

            await Context.SaveChangesAsync(cancellationToken);

            var result = Mapper.Map<DeliveryInfoDto>(newDelivery);
            return ServiceResponseBuilder.Success(result);
        }

        private async Task<ServiceResponse> ValidateDelivery(Db.Models.Order? order, Db.Models.Robot? robot)
        {
            if (order == null || order.OrderProducts == null)
            {
                return ServiceResponseBuilder.Failure<DeliveryInfoDto>(OrderError.OrderNotFound);
            }

            if (order.Status != OrderStatus.Pending)
            {
                return ServiceResponseBuilder.Failure<DeliveryInfoDto>(DeliveryError.DeliveryOrderError);
            }

            if (robot == null)
            {
                return ServiceResponseBuilder.Failure<DeliveryInfoDto>(RobotError.RobotNotFound);
            }

            decimal totalWeight = 0;
            decimal totalVolume = 0;

            foreach (var orderProduct in order.OrderProducts)
            {
                var product = await Context.Products.FindAsync(orderProduct.ProductId);
                totalWeight += product.WeightG * orderProduct.Quantity;
                totalVolume += product.VolumeCm3 * orderProduct.Quantity;
            }

            if (robot.Status != RobotStatus.Idle || robot.WeightCapacityG < totalWeight ||
                robot.VolumeCapacityCm3 < totalVolume)
            {
                return ServiceResponseBuilder.Failure<DeliveryInfoDto>(RobotError.RobotUnavailableError);
            }

            return ServiceResponseBuilder.Success();
        }
    }
}