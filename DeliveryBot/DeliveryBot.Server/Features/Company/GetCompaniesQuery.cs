using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Company;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Company;

public class GetCompaniesQuery : IRequest<ServiceResponse<ICollection<CompanyPreviewDto>>>
{
    public class GetCompaniesQueryHandler : 
        ExtendedBaseHandler<GetCompaniesQuery, ServiceResponse<ICollection<CompanyPreviewDto>>>
    {
        public GetCompaniesQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<GetCompaniesQueryHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<ICollection<CompanyPreviewDto>>> Handle(GetCompaniesQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get companies error");
                return ServiceResponseBuilder.Failure<ICollection<CompanyPreviewDto>>(CompanyError.GetCompaniesError);
            }
        }

        protected override async Task<ServiceResponse<ICollection<CompanyPreviewDto>>> UnsafeHandleAsync(
            GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companies = Context.Companies.ToList();
            var result = Mapper.Map<ICollection<CompanyPreviewDto>>(companies);
            return ServiceResponseBuilder.Success(result);
        }
    }
}