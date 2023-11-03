namespace DeliveryBot.Server.BuildInjections;

internal static class CorsInjection
{
    internal static string PolicyName => "policy";

    internal static void AddSetCors(this IServiceCollection services)
    {
        services.AddCors(corsOptions =>
        {
            corsOptions.AddPolicy(PolicyName,
                policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }
}