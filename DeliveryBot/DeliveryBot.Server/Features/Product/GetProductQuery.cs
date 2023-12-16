using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Product;
using DeliveryBot.Shared.Dto.Robot;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

public class GetProductQuery : IRequest<ServiceResponse<ProductDto>>
{
    public Guid ProductId { get; set; }

    public class GetProductQueryHandler : ExtendedBaseHandler<GetProductQuery, ServiceResponse<ProductDto>>
    {
        public GetProductQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<GetProductQueryHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<ProductDto>> Handle(GetProductQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get product error");
                return ServiceResponseBuilder.Failure<ProductDto>(ProductError.GetProductError);
            }
        }

        protected override async Task<ServiceResponse<ProductDto>> UnsafeHandleAsync(GetProductQuery request,
            CancellationToken cancellationToken)
        {
            var product = await Context.Products.FindAsync(request.ProductId);

            if (product == null)
            {
                return ServiceResponseBuilder.Failure<ProductDto>(ProductError.ProductNotFound);
            }

            var result = Mapper.Map<ProductDto>(product);
            return ServiceResponseBuilder.Success(result);
        }
    }
}