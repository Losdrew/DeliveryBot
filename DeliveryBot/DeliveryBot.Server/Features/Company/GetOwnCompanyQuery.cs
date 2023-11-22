using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Company;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBot.Server.Features.Company;

public class GetOwnCompanyQuery : IRequest<ServiceResponse<OwnCompanyInfoDto>>
{
    public class GetOwnCompanyQueryHandler : ExtendedBaseHandler<GetOwnCompanyQuery, ServiceResponse<OwnCompanyInfoDto>>
    {
        public GetOwnCompanyQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<GetOwnCompanyQueryHandler> logger) 
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<OwnCompanyInfoDto>> Handle(GetOwnCompanyQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get own company error");
                return ServiceResponseBuilder.Failure<OwnCompanyInfoDto>(CompanyError.GetOwnCompanyError);
            }
        }

        protected override async Task<ServiceResponse<OwnCompanyInfoDto>> UnsafeHandleAsync(GetOwnCompanyQuery request,
            CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var manager = await Context.CompanyEmployees.FindAsync(userId);

            if (!isUserIdValid || manager == null)
            {
                return ServiceResponseBuilder.Failure<OwnCompanyInfoDto>(UserError.InvalidAuthorization);
            }

            var company = await Context.Companies
                .Include(c => c.CompanyEmployees)
                .Include(c => c.CompanyAddresses)
                .FirstOrDefaultAsync(c => c.Id == manager.CompanyId, cancellationToken);

            if (company == null)
            {
                return ServiceResponseBuilder.Failure<OwnCompanyInfoDto>(CompanyError.CompanyNotFound);
            }

            var result = Mapper.Map<OwnCompanyInfoDto>(company);
            return ServiceResponseBuilder.Success(result);
        }
    }
}