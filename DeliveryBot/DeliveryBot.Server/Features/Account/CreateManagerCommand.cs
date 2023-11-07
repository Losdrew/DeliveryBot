using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Server.Services;
using DeliveryBot.Shared.Dto.Account;
using DeliveryBot.Shared.Helpers;
using DeliveryBot.Shared.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Account;

public class CreateManagerCommand : CreateCompanyEmployeeCommandDto, IRequest<ServiceResponse<SignUpResultDto>>
{
    public class CreateManagerCommandHandler : 
        CompanyEmployeeSignUpHandler<CreateManagerCommand, ServiceResponse<SignUpResultDto>>
    {
        public CreateManagerCommandHandler(IMediator mediator, ApplicationDbContext context,
            ITokenGenerator tokenGenerator, ILogger<CreateManagerCommandHandler> logger)
            : base(mediator, context, tokenGenerator, logger)
        {
        }

        public override async Task<ServiceResponse<SignUpResultDto>> Handle(CreateManagerCommand request, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Manager creation error");
                return ServiceResponseBuilder.Failure<SignUpResultDto>(AccountError.AccountCreateError);
            }
        }

        protected override string GetRole()
        {
            return Roles.Manager;
        }
    }
}