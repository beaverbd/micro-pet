using Common.DataAccess;
using Common.Providers;
using Common.Providers.Interfaces;

namespace Common.Infrastructure;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTime, DateTimeProvider>();
        services.AddScoped<DbContext>();
        services.AddSingleton<Database>();
        return services;
    }
}
