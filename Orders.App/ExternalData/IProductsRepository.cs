using Common.ExternalDataAccess.Interfaces;
using Orders.App.ExternalData.Entities;
namespace Orders.App.ExternalData;

public interface IProductsRepository : IExternalDataStorage<Product>
{
}