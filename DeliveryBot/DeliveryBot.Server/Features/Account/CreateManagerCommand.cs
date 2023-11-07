using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Server.Models.Account;
using DeliveryBot.Server.Services;
using DeliveryBot.Shared.Dto.Account;
using DeliveryBot.Shared.Helpers;
using DeliveryBot.Shared.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Account;

public class CreateManagerCommand : CreateCompanyEmployeeCommandDto, IRequest<ServiceResponse<AuthResultDto>>
{
    public class CreateManagerCommandHandler : 
        SignUpHandler<CreateManagerCommand, ServiceResponse<AuthResultDto>>
    {
        public CreateManagerCommandHandler(IMediator mediator, ApplicationDbContext context,
            ITokenGenerator tokenGenerator, ILogger<CreateManagerCommandHandler> logger)
            : base(mediator, context, tokenGenerator, logger)
        {
        }

        public override async Task<ServiceResponse<AuthResultDto>> Handle(CreateManagerCommand request, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Manager creation error");
                return ServiceResponseBuilder.Failure<AuthResultDto>(AccountError.AccountCreateError);
            }
        }

        protected override string GetRole()
        {
            return Roles.Manager;
        }

        protected override async Task<User> CreateUserAsync(CreateManagerCommand request,
            ServiceResponse<CreateIdentityUserResult> createIdentityResponse)
        {
            var companyEmployee = new CompanyEmployee
            {
                Id = Guid.Parse(createIdentityResponse.Result.IdentityUser.Id),
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CompanyId = request.CompanyId
            };

            Context.CompanyEmployees.Add(companyEmployee);
            await Context.SaveChangesAsync();

            return companyEmployee;
        }
    }
}