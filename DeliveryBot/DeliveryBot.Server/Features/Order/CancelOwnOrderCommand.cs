using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Order;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBot.Server.Features.Order;

public class CancelOwnOrderCommand : IRequest<ServiceResponse<OrderInfoDto>>
{
    public Guid OrderId { get; set; }

    public class CancelOwnOrderCommandHandler :
        ExtendedBaseHandler<CancelOwnOrderCommand, ServiceResponse<OrderInfoDto>>
    {
        public CancelOwnOrderCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<CancelOwnOrderCommandHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<OrderInfoDto>> Handle(CancelOwnOrderCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Cancel order error");
                return ServiceResponseBuilder.Failure<OrderInfoDto>(OrderError.OrderCancelError);
            }
        }

        protected override async Task<ServiceResponse<OrderInfoDto>> UnsafeHandleAsync(CancelOwnOrderCommand request,
            CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var customer = await Context.Customers.FindAsync(userId);

            if (!isUserIdValid || customer == null)
            {
                return ServiceResponseBuilder.Failure<OrderInfoDto>(UserError.InvalidAuthorization);
            }

            var orderToCancel = await Context.Orders
                .Include(o => o.OrderProducts)
                .Include(o => o.OrderAddress)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (orderToCancel == null)
            {
                return ServiceResponseBuilder.Failure<OrderInfoDto>(OrderError.OrderNotFound);
            }

            if (orderToCancel.CustomerId != customer.Id)
            {
                return ServiceResponseBuilder.Failure<OrderInfoDto>(UserError.ForbiddenAccess);
            }

            orderToCancel.Status = OrderStatus.Cancelled;

            var result = Mapper.Map<OrderInfoDto>(orderToCancel);
            return ServiceResponseBuilder.Success(result);
        }
    }
}