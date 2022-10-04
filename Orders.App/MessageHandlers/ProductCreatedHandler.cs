using Common.MessageBroker.Products;
using Convey.CQRS.Events;
using Orders.App.ExternalData;
using Orders.App.ExternalData.Entities;

namespace Orders.App.MessageHandlers;

public class ProductCreatedHandler : IEventHandler<ProductCreated>
{
    private readonly IProductsRepository _productsRepository;

    public ProductCreatedHandler(IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }

    public Task HandleAsync(ProductCreated @event, CancellationToken cancellationToken = new CancellationToken())
    {
        _productsRepository.Add(new Product
        {
            Id = @event.Id,
            Price = @event.Price,
            Name = @event.Name,
            Description = @event.Description
        });
        return Task.CompletedTask;
    }
}
