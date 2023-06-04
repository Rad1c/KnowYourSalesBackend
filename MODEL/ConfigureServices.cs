using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MODEL.Entities;

namespace MODEL;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")!
        ), ServiceLifetime.Scoped);

        services.AddScoped<QueryContext>();

        return services;
    }
}
