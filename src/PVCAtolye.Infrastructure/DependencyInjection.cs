using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PVCAtolye.Application.Customers;
using PVCAtolye.Application.Identity;
using PVCAtolye.Application.Settings;
using PVCAtolye.Infrastructure.Customers;
using PVCAtolye.Infrastructure.Identity;
using PVCAtolye.Infrastructure.Persistence;
using PVCAtolye.Infrastructure.Seed;
using PVCAtolye.Infrastructure.Settings;

namespace PVCAtolye.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<AdminSeedOptions>(configuration.GetSection(AdminSeedOptions.SectionName));
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ISettingsService, SettingsService>();
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}
