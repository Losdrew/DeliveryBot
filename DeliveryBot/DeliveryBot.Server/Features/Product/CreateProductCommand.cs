using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Company;
using DeliveryBot.Shared.Dto.Product;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Product;

public class CreateProductCommand : ProductDto, IRequest<ServiceResponse<CompanyProductInfoDto>>
{
    public class CreateProductCommandHandler :
        BaseHandler<CreateProductCommand, ServiceResponse<CompanyProductInfoDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<CreateProductCommandHandler> logger, IMediator mediator)
            : base(logger)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
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

        protected override async Task<ServiceResponse<CompanyProductInfoDto>> UnsafeHandleAsync(CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var isUserIdValid = _contextAccessor.TryGetUserId(out var userId);
            var manager = await _context.FindAsync<CompanyEmployee>(userId);

            if (!isUserIdValid || manager == null)
            {
                return ServiceResponseBuilder.Failure<CompanyProductInfoDto>(UserError.InvalidAuthorization);
            }

            var product = _mapper.Map<Db.Models.Product>(request);
            product.CompanyId = manager.CompanyId;

            _context.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<CompanyProductInfoDto>(product);
            return ServiceResponseBuilder.Success(result);
        }
    }
}