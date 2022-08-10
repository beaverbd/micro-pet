using Orders.App.Entities;

namespace Orders.App.Repositories.Interfaces;

public interface IOrdersRepository
{
    Task<List<Order>> GetAllAsync();
}

