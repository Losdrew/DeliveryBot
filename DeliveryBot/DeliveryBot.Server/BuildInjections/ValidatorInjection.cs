using DeliveryBot.Server.Features;
using DeliveryBot.Shared.Validators.Account;
using FluentValidation;

namespace DeliveryBot.Server.BuildInjections;

internal static class ValidatorInjection
{
    internal static void AddValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<CreateIdentityUserCommand>, CreateIdentityUserCommandValidator>();
    }
}