using BLL.IServices;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL;

public static class ConfigureServices
{
    public static IServiceCollection AddBLLServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IShopService, ShopService>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<ICommerceService, CommerceService>();

        return services;
    }
}
