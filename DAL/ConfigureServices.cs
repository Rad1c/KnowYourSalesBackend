using DAL.IRepositories;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DAL;

public static class ConfigureServices
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IShopRepository, ShopRepository>();
        services.AddScoped<ICommerceRepository, CommerceRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IReferenteDataRepository, ReferenteDataRepository>();

        return services;
    }
}
