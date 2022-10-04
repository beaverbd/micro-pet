using Connectors.Products;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.App.ExternalData;
using Orders.App.ExternalData.Entities;
using Serilog;

namespace Orders.App.HostedServices;

public class FetchProductsHostedService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FetchProductsHostedService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Log.Information("Start fetching products last updates..");
        using var scope = _serviceScopeFactory.CreateScope();
        var productsApi = scope.ServiceProvider.GetRequiredService<IProductsApi>();
        var productsRepository = scope.ServiceProvider.GetRequiredService<IProductsRepository>();
        var products = await productsApi.GetProducts();
        Log.Information($"Store {products.Count} products to storage");
        productsRepository.AddRange(products.Select(product => new Product
        {
            Id = product.Id,
            Description = product.Description,
            Name = product.Name,
            Price = product.Price
        }));
        Log.Information("Finished fetching products current state.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

