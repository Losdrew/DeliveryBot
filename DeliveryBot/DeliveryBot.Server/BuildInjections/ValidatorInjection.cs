using DeliveryBot.Server.Features.Account;
using DeliveryBot.Shared.Validators.Account;
using FluentValidation;

namespace DeliveryBot.Server.BuildInjections;

internal static class ValidatorInjection
{
    internal static void AddValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<CreateIdentityUserCommand>, CreateIdentityUserCommandValidator>();
        services.AddTransient<IValidator<CreateCompanyEmployeeCommand>, CreateCompanyEmployeeCommandValidator>();
        services.AddTransient<IValidator<CreateCustomerCommand>, CreateCustomerCommandValidator>();
    }
}