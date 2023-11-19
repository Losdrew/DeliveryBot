using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBot.Server.Features.Product;

public class DeleteProductCommand : IRequest<ServiceResponse>
{
    public Guid ProductId { get; set; }

    public class DeleteProductCommandHandler :
        ExtendedBaseHandler<DeleteProductCommand, ServiceResponse>
    {
        public DeleteProductCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<DeleteProductCommandHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse> Handle(DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Edit product error");
                return ServiceResponseBuilder.Failure(ProductError.ProductEditError);
            }
        }

        protected override async Task<ServiceResponse> UnsafeHandleAsync(DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            var productToDelete = Context.Products.Where(p => p.Id == request.ProductId);
            var isDeleted = await productToDelete.ExecuteDeleteAsync(cancellationToken);
            
            if (isDeleted == 0)
            {
                return ServiceResponseBuilder.Failure(ProductError.ProductNotFound);
            }

            return ServiceResponseBuilder.Success();
        }
    }
}