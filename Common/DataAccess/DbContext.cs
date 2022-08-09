
using System.Data;
using Common.Configs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Common.DataAccess
{
    public class DbContext
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRoot;

        public DbContext(IConfiguration configuration)
        {
            _connectionString = configuration.Get<ConfigBase>().Database.ConnectionString;
            _connectionStringRoot = configuration.Get<ConfigBase>().Database.ConnectionStringRoot;
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
        public IDbConnection CreateRootConnection() => new NpgsqlConnection(_connectionStringRoot);
    }
}
