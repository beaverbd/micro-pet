using Common.Configs;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;
namespace Common.Extensions;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddServiceHealthChecks<TConfigs>(this IServiceCollection services, TConfigs config)
    where TConfigs : ConfigBase
    {
        return services.AddHealthChecks()
            .AddAsyncCheck("service", () => Task.FromResult(HealthCheckResult.Healthy()))
            .AddNpgSql(config.Database.ConnectionString);
    }

    public static IEndpointConventionBuilder UseServiceHealthChecksRouting(this IEndpointRouteBuilder builder)
    {
        return builder.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }

    public static IApplicationBuilder UseServiceHealthChecks(this IApplicationBuilder app)
    {
        return app.UseHealthChecks("/health");
    }
}
