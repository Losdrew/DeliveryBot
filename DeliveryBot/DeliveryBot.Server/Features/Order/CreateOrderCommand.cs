using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Order;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Order;

public class CreateOrderCommand : CreateOrderCommandDto, IRequest<ServiceResponse<OrderInfoDto>>
{
    public class CreateOrderCommandHandler :
        ExtendedBaseHandler<CreateOrderCommand, ServiceResponse<OrderInfoDto>>
    {
        public CreateOrderCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<CreateOrderCommandHandler> logger, IMediator mediator)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<OrderInfoDto>> Handle(CreateOrderCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Order creation error");
                return ServiceResponseBuilder.Failure<OrderInfoDto>(OrderError.OrderCreateError);
            }
        }

        protected override async Task<ServiceResponse<OrderInfoDto>> UnsafeHandleAsync(CreateOrderCommand request,
            CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var customer = await Context.Customers.FindAsync(userId);

            if (!isUserIdValid || customer == null)
            {
                return ServiceResponseBuilder.Failure<OrderInfoDto>(UserError.InvalidAuthorization);
            }

            var orderProducts = Mapper.Map<ICollection<OrderProduct>>(request.OrderProducts);

            var newOrder = Mapper.Map<Db.Models.Order>(request);
            newOrder.Customer = customer;
            newOrder.OrderProducts = orderProducts;
            Context.Add(newOrder);

            var orderProductIds = orderProducts.Select(op => op.ProductId).ToHashSet();
            var products = Context.Products.Where(p => orderProductIds.Contains(p.Id));

            if (!products.Any())
            {
                return ServiceResponseBuilder.Failure<OrderInfoDto>(ProductError.ProductNotFound);
            }

            foreach (var product in products)
            {
                product.OrderProducts = orderProducts;
            }

            await Context.SaveChangesAsync(cancellationToken);

            var result = Mapper.Map<OrderInfoDto>(newOrder);
            result.CustomerId = customer.Id;

            return ServiceResponseBuilder.Success(result);
        }
    }
}