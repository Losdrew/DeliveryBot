using DeliveryBot.Server.Models.Account;
using DeliveryBot.Shared.Dto.Account;
using DeliveryBot.Shared.Errors.Base;
using DeliveryBot.Shared.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DeliveryBot.Server.Features;

public class CreateIdentityUserCommand : CredentialsDto, IRequest<ServiceResponse<CreateIdentityUserResult>>
{
    public string Role { get; set; }

    public class CreateIdentityUserCommandHandler
        : IRequestHandler<CreateIdentityUserCommand, ServiceResponse<CreateIdentityUserResult>>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<CreateIdentityUserCommandHandler> _logger;

        public CreateIdentityUserCommandHandler(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<CreateIdentityUserCommandHandler> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<ServiceResponse<CreateIdentityUserResult>> Handle(CreateIdentityUserCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "User creation error");
                return ServiceResponseBuilder.Failure<CreateIdentityUserResult>(AccountError.IdentityCreateError);
            }
        }

        private async Task<ServiceResponse<CreateIdentityUserResult>> UnsafeHandleAsync(CreateIdentityUserCommand request,
            CancellationToken cancellationToken)
        {
            var identityUser = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Email
            };

            var createResult = await _userManager.CreateAsync(identityUser, request.Password);

            if (createResult.Succeeded)
            {
                await AssureRoleCreatedAsync(request.Role);
                await _userManager.AddToRoleAsync(identityUser, request.Role);

                var result = new CreateIdentityUserResult
                {
                    IdentityUser = identityUser
                };

                return ServiceResponseBuilder.Success(result);
            }

            var errors = createResult.Errors.Select(e => new ServiceError
            {
                Header = "UserError",
                ErrorMessage = e.Description,
                Code = int.Parse(e.Code)
            }).ToList();

            return ServiceResponseBuilder.Failure<CreateIdentityUserResult>(errors);
        }

        private async Task AssureRoleCreatedAsync(string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}