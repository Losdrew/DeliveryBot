using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Product;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Product;

public class EditProductCommand : EditProductCommandDto, IRequest<ServiceResponse<CompanyProductInfoDto>>
{
    public class EditProductCommandHandler :
        ExtendedBaseHandler<EditProductCommand, ServiceResponse<CompanyProductInfoDto>>
    {
        public EditProductCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<EditProductCommandHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<CompanyProductInfoDto>> Handle(EditProductCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Edit product error");
                return ServiceResponseBuilder.Failure<CompanyProductInfoDto>(ProductError.ProductEditError);
            }
        }

        protected override async Task<ServiceResponse<CompanyProductInfoDto>> UnsafeHandleAsync(
            EditProductCommand request, CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var manager = await Context.CompanyEmployees.FindAsync(userId);

            if (!isUserIdValid || manager == null)
            {
                return ServiceResponseBuilder.Failure<CompanyProductInfoDto>(UserError.InvalidAuthorization);
            }

            var productToEdit = await Context.Products.FindAsync(request.Id);

            if (productToEdit == null)
            {
                return ServiceResponseBuilder.Failure<CompanyProductInfoDto>(ProductError.ProductNotFound);
            }

            if (productToEdit.CompanyId != manager.CompanyId)
            {
                return ServiceResponseBuilder.Failure<CompanyProductInfoDto>(UserError.ForbiddenAccess);
            }

            Mapper.Map(request, productToEdit);
            await Context.SaveChangesAsync(cancellationToken);

            var result = Mapper.Map<CompanyProductInfoDto>(productToEdit);
            return ServiceResponseBuilder.Success(result);
        }
    }
}