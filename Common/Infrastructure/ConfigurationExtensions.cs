using Common.Providers;
using Common.Providers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTime, DateTimeProvider>();
        return services;
    }
}

