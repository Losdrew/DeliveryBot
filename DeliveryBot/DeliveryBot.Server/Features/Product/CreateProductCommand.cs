using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Product;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Product;

public class CreateProductCommand : ProductDto, IRequest<ServiceResponse<CompanyProductInfoDto>>
{
    public class CreateProductCommandHandler :
        ExtendedBaseHandler<CreateProductCommand, ServiceResponse<CompanyProductInfoDto>>
    {
        public CreateProductCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<CreateProductCommandHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<CompanyProductInfoDto>> Handle(CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Product creation error");
                return ServiceResponseBuilder.Failure<CompanyProductInfoDto>(ProductError.ProductCreateError);
            }
        }

        protected override async Task<ServiceResponse<CompanyProductInfoDto>> UnsafeHandleAsync(
            CreateProductCommand request, CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var manager = await Context.CompanyEmployees.FindAsync(userId);

            if (!isUserIdValid || manager == null)
            {
                return ServiceResponseBuilder.Failure<CompanyProductInfoDto>(UserError.InvalidAuthorization);
            }

            var product = Mapper.Map<Db.Models.Product>(request);
            product.CompanyId = manager.CompanyId;

            Context.Add(product);
            await Context.SaveChangesAsync(cancellationToken);

            var result = Mapper.Map<CompanyProductInfoDto>(product);
            return ServiceResponseBuilder.Success(result);
        }
    }
}