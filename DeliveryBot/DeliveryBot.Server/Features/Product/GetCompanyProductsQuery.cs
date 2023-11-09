﻿using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Product;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Product;

public class GetCompanyProductsQuery : IRequest<ServiceResponse<ICollection<ProductDto>>>
{
    public Guid CompanyId { get; set; }

    public class GetCompanyProductsQueryHandler : ExtendedBaseHandler<GetCompanyProductsQuery, ServiceResponse<ICollection<ProductDto>>>
    {
        public GetCompanyProductsQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper,
            ILogger<GetCompanyProductsQueryHandler> logger) 
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<ICollection<ProductDto>>> Handle(GetCompanyProductsQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get company products error");
                return ServiceResponseBuilder.Failure<ICollection<ProductDto>>(ProductError.GetCompanyProductsError);
            }
        }

        protected override async Task<ServiceResponse<ICollection<ProductDto>>> UnsafeHandleAsync(GetCompanyProductsQuery request,
            CancellationToken cancellationToken)
        {
            var products = Context.Products.Where(p => p.CompanyId == request.CompanyId);
            var result = Mapper.Map<ICollection<ProductDto>>(products);
            return ServiceResponseBuilder.Success(result);
        }
    }
}