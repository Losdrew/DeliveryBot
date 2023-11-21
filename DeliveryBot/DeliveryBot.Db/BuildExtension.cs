using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace DeliveryBot.Db;

public static class BuildExtension
{
    public static void AddDbSetup(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationString = configuration.GetRequiredSection("ConnectionString").Value;

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configurationString);
        dataSourceBuilder.MapEnum<OrderStatus>();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(dataSource, builder =>
            {
                builder.MigrationsAssembly("DeliveryBot.Db");
                builder.UseNetTopologySuite();
            }));

        services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }
}