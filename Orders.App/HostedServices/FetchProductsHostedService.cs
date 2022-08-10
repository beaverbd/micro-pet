using Connectors.Products;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        var products = await productsApi.GetProducts();
        Log.Information($"Took {products.Count} products");
        Log.Information("Finished fetching products last updates..");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

