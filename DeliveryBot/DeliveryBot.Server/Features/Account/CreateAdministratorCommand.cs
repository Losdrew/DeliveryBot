using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Server.Models.Account;
using DeliveryBot.Server.Services;
using DeliveryBot.Shared.Dto.Account;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.Helpers;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Account;

public class CreateAdministratorCommand : CreateCompanyEmployeeCommandDto, IRequest<ServiceResponse<AuthResultDto>>
{
    public class CreateAdministratorCommandHandler :
        SignUpHandler<CreateAdministratorCommand, ServiceResponse<AuthResultDto>>
    {
        public CreateAdministratorCommandHandler(IMediator mediator, ApplicationDbContext context, 
            ITokenGenerator tokenGenerator, ILogger<CreateAdministratorCommandHandler> logger)
            : base(mediator, context, tokenGenerator, logger)
        {
        }

        public override async Task<ServiceResponse<AuthResultDto>> Handle(CreateAdministratorCommand request, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Administrator creation error");
                return ServiceResponseBuilder.Failure<AuthResultDto>(AccountError.AccountCreateError);
            }
        }

        protected override string GetRole()
        {
            return Roles.Administrator;
        }

        protected override async Task<User> CreateUserAsync(CreateAdministratorCommand request,
            ServiceResponse<CreateIdentityUserResult> createIdentityResponse)
        {
            var companyEmployee = new Db.Models.CompanyEmployee
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