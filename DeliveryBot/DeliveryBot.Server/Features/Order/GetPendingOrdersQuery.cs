using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Order;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBot.Server.Features.Order;

public class GetPendingOrdersQuery : IRequest<ServiceResponse<ICollection<OrderInfoDto>>>
{
    public class GetPendingOrdersQueryHandler : ExtendedBaseHandler<GetPendingOrdersQuery, ServiceResponse<ICollection<OrderInfoDto>>>
    {
        public GetPendingOrdersQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<GetPendingOrdersQueryHandler> logger) 
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<ICollection<OrderInfoDto>>> Handle(GetPendingOrdersQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get all orders error");
                return ServiceResponseBuilder.Failure<ICollection<OrderInfoDto>>(OrderError.GetPendingOrdersError);
            }
        }

        protected override async Task<ServiceResponse<ICollection<OrderInfoDto>>> UnsafeHandleAsync(GetPendingOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var orders = Context.Orders
                .Include(o => o.OrderProducts)
                .Include(o => o.OrderAddress)
                .Where(o => o.Status == OrderStatuses.Pending)
                .ToList();

            var result = Mapper.Map<ICollection<OrderInfoDto>>(orders);
            return ServiceResponseBuilder.Success(result);
        }
    }
}