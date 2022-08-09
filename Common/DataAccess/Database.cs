using Dapper;
namespace Common.DataAccess;

public class Database
{
    private readonly DbContext _context;

    public Database(DbContext context)
    {
        _context = context;
    }

    public void CreateDatabase(string dbName)
    {
        var query = "SELECT datname FROM pg_database WHERE datname = @Name";
        using var connection = _context.CreateRootConnection();
        var records = connection.Query(query, new { Name = dbName });
        if (!records.Any())
            connection.Execute($"CREATE DATABASE {dbName}");
    }
}
