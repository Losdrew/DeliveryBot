using DeliveryBot.Server.Services;

namespace DeliveryBot.Server.BuildInjections;

internal static class ServicesInjection
{
    internal static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<ITokenGenerator, JwtTokenGenerator>();
    }
}