using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Robot;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Robot;

public class GetOwnCompanyRobotsQuery : IRequest<ServiceResponse<ICollection<RobotInfoDto>>>
{
    public class GetOwnCompanyRobotsQueryHandler : ExtendedBaseHandler<GetOwnCompanyRobotsQuery, ServiceResponse<ICollection<RobotInfoDto>>>
    {
        public GetOwnCompanyRobotsQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<GetOwnCompanyRobotsQueryHandler> logger) 
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<ICollection<RobotInfoDto>>> Handle(GetOwnCompanyRobotsQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get own company robots error");
                return ServiceResponseBuilder.Failure<ICollection<RobotInfoDto>>(RobotError.GetOwnCompanyRobotsError);
            }
        }

        protected override async Task<ServiceResponse<ICollection<RobotInfoDto>>> UnsafeHandleAsync(GetOwnCompanyRobotsQuery request,
            CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var companyEmployee = await Context.FindAsync<CompanyEmployee>(userId);

            if (!isUserIdValid || companyEmployee == null)
            {
                return ServiceResponseBuilder.Failure<ICollection<RobotInfoDto>>(UserError.InvalidAuthorization);
            }

            var robots = Context.Robots.Where(r => r.CompanyId == companyEmployee.CompanyId);
            var result = Mapper.Map<ICollection<RobotInfoDto>>(robots);
            return ServiceResponseBuilder.Success(result);
        }
    }
}