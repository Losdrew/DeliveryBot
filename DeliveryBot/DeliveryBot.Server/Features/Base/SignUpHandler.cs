using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Account;
using DeliveryBot.Server.Models.Account;
using DeliveryBot.Server.Services;
using DeliveryBot.Shared.Dto.Account;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Base;

public abstract class SignUpHandler<TCommand, TResponse> : BaseHandler<TCommand, TResponse>
    where TCommand : CredentialsDto, IRequest<TResponse>
    where TResponse : ServiceResponse<AuthResultDto>, new()
{
    protected readonly IMediator Mediator;
    protected readonly ApplicationDbContext Context;
    protected readonly ITokenGenerator TokenGenerator;

    protected SignUpHandler(IMediator mediator, ApplicationDbContext context, ITokenGenerator tokenGenerator,
        ILogger<SignUpHandler<TCommand, TResponse>> logger)
        : base(logger)
    {
        Mediator = mediator;
        Context = context;
        TokenGenerator = tokenGenerator;
    }

    protected override async Task<TResponse> UnsafeHandleAsync(TCommand request, CancellationToken cancellationToken)
    {
        var createIdentityUserCommand = new CreateIdentityUserCommand
        {
            Email = request.Email,
            Password = request.Password,
            Role = GetRole()
        };

        var createIdentityResponse = await Mediator.Send(createIdentityUserCommand, cancellationToken);

        if (createIdentityResponse.IsSuccess)
        {
            var user = await CreateUserAsync(request, createIdentityResponse);
            var token = await TokenGenerator.GenerateAsync(createIdentityResponse.Result.IdentityUser);

            var result = new AuthResultDto
            {
                UserId = user.Id,
                Bearer = token,
                Role = GetRole()
            };

            return (TResponse)ServiceResponseBuilder.Success(result);
        }

        return (TResponse)createIdentityResponse.MapErrorResult<AuthResultDto>();
    }

    protected abstract string GetRole();

    protected abstract Task<User> CreateUserAsync(TCommand request,
        ServiceResponse<CreateIdentityUserResult> createIdentityResponse);
}