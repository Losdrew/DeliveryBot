using DeliveryBot.Server.Mediator.Pipeline;
using DeliveryBot.Server.Services;
using MediatR;

namespace DeliveryBot.Server.BuildInjections;

internal static class ServicesInjection
{
    internal static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<ITokenGenerator, JwtTokenGenerator>();
        services.AddTransient<IValidationService, ValidationService>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient<IGeolocationService, GeolocationService>();
    }
}