using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Common.MessageBroker.Products;

[Message(exchange:"products")]
public class ProductCreated : IEvent
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public float AvailableAmount { get; set; }

    public ProductCreated(Guid id, string name, decimal price, string? description, float availableAmount)
    {
        Id = id;
        Name = name;
        Price = price;
        Description = description;
        AvailableAmount = availableAmount;
    }
}
