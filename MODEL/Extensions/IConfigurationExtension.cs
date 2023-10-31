using Microsoft.Extensions.Configuration;

namespace MODEL.Extensions;

public static class IConfigurationExtension
{
    public static string GetPostgresConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("DefaultConnection")!;
    }
}
