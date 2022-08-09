using FluentMigrator.Runner;
using Microsoft.Extensions.Hosting;
namespace Common.DataAccess;

public static class MigrationManager
{
    public static IHost MigrateDatabase(this IHost host, string dbName)
    {
        using var scope = host.Services.CreateScope();
        var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
        var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        try
        {
            databaseService.CreateDatabase(dbName);
            migrationService.ListMigrations();
            migrationService.MigrateUp();
        }
        catch
        {
            // TODO log 
            throw;
        }

        return host;
    }
}
