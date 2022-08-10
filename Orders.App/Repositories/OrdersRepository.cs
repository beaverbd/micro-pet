using Common.DataAccess;
using Dapper;
using Orders.App.Entities;
using Orders.App.Repositories.Interfaces;

namespace Orders.App.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly DbContext _context;

    public OrdersRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<Order>("SELECT * FROM Orders");
        return result.ToList();
    }
}
