using DeliveryBot.Server.Features.Account;
using DeliveryBot.Server.Features.Company;
using DeliveryBot.Server.Features.Order;
using DeliveryBot.Server.Features.Product;
using DeliveryBot.Shared.Validators.Account;
using DeliveryBot.Shared.Validators.Company;
using DeliveryBot.Shared.Validators.Order;
using DeliveryBot.Shared.Validators.Product;
using FluentValidation;

namespace DeliveryBot.Server.BuildInjections;

internal static class ValidatorInjection
{
    internal static void AddValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<CreateIdentityUserCommand>, CreateIdentityUserCommandValidator>();
        services.AddTransient<IValidator<CreateCompanyEmployeeCommand>, CreateCompanyEmployeeCommandValidator>();
        services.AddTransient<IValidator<CreateCustomerCommand>, CreateCustomerCommandValidator>();
        services.AddTransient<IValidator<CreateCompanyCommand>, CreateCompanyCommandValidator>();
        services.AddTransient<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
        services.AddTransient<IValidator<EditCompanyCommand>, EditCompanyCommandValidator>();
        services.AddTransient<IValidator<EditProductCommand>, EditProductCommandValidator>();
        services.AddTransient<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
    }
}