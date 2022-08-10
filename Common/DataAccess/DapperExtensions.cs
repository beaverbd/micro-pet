using System.Data;
using SerilogTimings;

namespace Dapper
{
    public static class DapperExtensions
    {
        public static async Task<IEnumerable<TEntity>> QueryWithMetricAsync<TEntity>(this IDbConnection? connection, string query, object param = null)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            using var timing = Operation.Time("SQL query {query}", query);
            var result = await connection.QueryAsync<TEntity>(query, param);
            return result;
        }
    }
}
