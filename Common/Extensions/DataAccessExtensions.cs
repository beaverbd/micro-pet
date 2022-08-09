using Common.Configs;
using FluentMigrator.Runner;

namespace Common.Extensions;

public static class DataAccessExtensions
{
    public static IServiceCollection AddMigrations(this IServiceCollection services, DatabaseConfig databaseConfig, Type assemblyType)
    {
        return services.AddLogging(x => x.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(x => x.AddPostgres()
                .WithGlobalConnectionString(databaseConfig.ConnectionString)
                .ScanIn(assemblyType.Assembly));
    }
}
