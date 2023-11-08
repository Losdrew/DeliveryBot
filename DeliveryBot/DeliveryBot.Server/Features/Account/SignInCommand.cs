using DeliveryBot.Server.Features.Base;
using DeliveryBot.Server.Services;
using DeliveryBot.Shared.Dto.Account;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DeliveryBot.Server.Features.Account;

public class SignInCommand : CredentialsDto, IRequest<ServiceResponse<AuthResultDto>>
{
    public class SignInCommandHandler : BaseHandler<SignInCommand, ServiceResponse<AuthResultDto>>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;

        public SignInCommandHandler(UserManager<IdentityUser> userManager, ITokenGenerator tokenGenerator,
            ILogger<SignInCommandHandler> logger) 
            : base(logger)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public override async Task<ServiceResponse<AuthResultDto>> Handle(SignInCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError("Sign in error", ex);
                return ServiceResponseBuilder.Failure<AuthResultDto>(AccountError.LoginServiceError);
            }
        }

        protected override async Task<ServiceResponse<AuthResultDto>> UnsafeHandleAsync(SignInCommand request,
            CancellationToken cancellationToken)
        {
            var identityUser = await _userManager.FindByEmailAsync(request.Email);

            if (identityUser is null)
            {
                return ServiceResponseBuilder.Failure<AuthResultDto>(AccountError.LoginValidationError);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(identityUser, request.Password);

            if (!isPasswordValid)
            {
                return ServiceResponseBuilder.Failure<AuthResultDto>(AccountError.LoginValidationError);
            }

            var token = await _tokenGenerator.GenerateAsync(identityUser);
            var result = new AuthResultDto
            {
                UserId = Guid.Parse(identityUser.Id),
                Bearer = token
            };

            return ServiceResponseBuilder.Success(result);
        }
    }
}