namespace HealthWebApi.ServicesHealth;

public static class ServicesHealthExtensions
{
    public static IHealthChecksBuilder AddOtherServicesHealth(this IHealthChecksBuilder builder)
    {
        return builder;
    }
}
