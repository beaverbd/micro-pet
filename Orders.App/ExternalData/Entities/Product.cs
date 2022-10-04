using Common.ExternalDataAccess;

namespace Orders.App.ExternalData.Entities;

public class Product : ExternalDataItemBase
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? Description { get; set; }
}