using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Company;
using DeliveryBot.Shared.Dto.Order;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBot.Server.Features.Order;

public class GetOwnOrdersQuery : IRequest<ServiceResponse<ICollection<OrderInfoDto>>>
{
    public class GetOwnOrdersQueryHandler : ExtendedBaseHandler<GetOwnOrdersQuery, ServiceResponse<ICollection<OrderInfoDto>>>
    {
        public GetOwnOrdersQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<GetOwnOrdersQueryHandler> logger) 
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<ICollection<OrderInfoDto>>> Handle(GetOwnOrdersQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get own orders error");
                return ServiceResponseBuilder.Failure<ICollection<OrderInfoDto>>(OrderError.GetOwnOrdersError);
            }
        }

        protected override async Task<ServiceResponse<ICollection<OrderInfoDto>>> UnsafeHandleAsync(GetOwnOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var customer = await Context.Customers.FindAsync(userId);

            if (!isUserIdValid || customer == null)
            {
                return ServiceResponseBuilder.Failure<ICollection<OrderInfoDto>>(UserError.InvalidAuthorization);
            }

            var order = Context.Orders
                .Include(o => o.OrderProducts)
                .Include(o => o.OrderAddress)
                .Where(o => o.CustomerId == customer.Id);

            if (!order.Any())
            {
                return ServiceResponseBuilder.Failure<ICollection<OrderInfoDto>>(OrderError.OrderNotFound);
            }

            var result = Mapper.Map<ICollection<OrderInfoDto>>(order);
            return ServiceResponseBuilder.Success(result);
        }
    }
}