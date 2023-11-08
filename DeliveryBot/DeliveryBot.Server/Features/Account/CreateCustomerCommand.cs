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

public class CreateCustomerCommand : CreateCustomerCommandDto, IRequest<ServiceResponse<AuthResultDto>>
{
    public class CreateCustomerCommandHandler : 
        SignUpHandler<CreateCustomerCommand, ServiceResponse<AuthResultDto>>
    {
        public CreateCustomerCommandHandler(IMediator mediator, ApplicationDbContext context,
            ITokenGenerator tokenGenerator, ILogger<CreateCustomerCommandHandler> logger)
            : base(mediator, context, tokenGenerator, logger)
        {
        }

        public override async Task<ServiceResponse<AuthResultDto>> Handle(CreateCustomerCommand request, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Customer creation error");
                return ServiceResponseBuilder.Failure<AuthResultDto>(AccountError.AccountCreateError);
            }
        }

        protected override string GetRole()
        {
            return Roles.Customer;
        }

        protected override async Task<User> CreateUserAsync(CreateCustomerCommand request,
            ServiceResponse<CreateIdentityUserResult> createIdentityResponse)
        {
            var customer = new Customer
            {
                Id = Guid.Parse(createIdentityResponse.Result.IdentityUser.Id),
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber
            };

            Context.Customers.Add(customer);
            await Context.SaveChangesAsync();

            return customer;
        }
    }
}