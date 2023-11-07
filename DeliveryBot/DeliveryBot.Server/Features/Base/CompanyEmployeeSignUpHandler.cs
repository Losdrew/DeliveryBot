using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Account;
using DeliveryBot.Server.Models.Account;
using DeliveryBot.Server.Services;
using DeliveryBot.Shared.Dto.Account;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Base;

public abstract class CompanyEmployeeSignUpHandler<TCommand, TResponse> : BaseHandler<TCommand, TResponse>
    where TCommand : CreateCompanyEmployeeCommandDto, IRequest<TResponse>
    where TResponse : ServiceResponse<SignUpResultDto>, new()
{
    protected readonly IMediator Mediator;
    protected readonly ApplicationDbContext Context;
    protected readonly ITokenGenerator TokenGenerator;

    protected CompanyEmployeeSignUpHandler(IMediator mediator, ApplicationDbContext context, ITokenGenerator tokenGenerator,
        ILogger<CompanyEmployeeSignUpHandler<TCommand, TResponse>> logger)
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
            var companyEmployee = await CreateCompanyEmployee(request, createIdentityResponse);
            var token = await TokenGenerator.GenerateAsync(createIdentityResponse.Result.IdentityUser);
                
            var result = new SignUpResultDto
            {
                UserId = companyEmployee.Id,
                Bearer = token
            };

            return (TResponse)ServiceResponseBuilder.Success(result);
        }

        return (TResponse)createIdentityResponse.MapErrorResult<SignUpResultDto>();
    }

    protected abstract string GetRole();

    protected async Task<CompanyEmployee> CreateCompanyEmployee(TCommand request,
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