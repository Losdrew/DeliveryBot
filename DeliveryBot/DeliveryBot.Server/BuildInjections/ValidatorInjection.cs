using DeliveryBot.Server.Features.Account;
using DeliveryBot.Server.Features.Company;
using DeliveryBot.Server.Features.Product;
using DeliveryBot.Shared.Validators.Account;
using DeliveryBot.Shared.Validators.Company;
using DeliveryBot.Shared.Validators.Product;
using FluentValidation;

namespace DeliveryBot.Server.BuildInjections;

internal static class ValidatorInjection
{
    internal static void AddValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<CreateIdentityUserCommand>, CredentialsValidator>();
        services.AddTransient<IValidator<CreateCompanyEmployeeCommand>, CompanyEmployeeValidator>();
        services.AddTransient<IValidator<CreateCustomerCommand>, CreateCustomerCommandValidator>();
        services.AddTransient<IValidator<CreateCompanyCommand>, CompanyValidator>();
        services.AddTransient<IValidator<CreateProductCommand>, ProductValidator>();
    }
}