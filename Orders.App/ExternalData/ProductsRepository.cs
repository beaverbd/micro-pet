using Common.ExternalDataAccess;
using Orders.App.ExternalData.Entities;

namespace Orders.App.ExternalData;

public class ProductsRepository : ExternalDataStorage<Product>, IProductsRepository
{

}