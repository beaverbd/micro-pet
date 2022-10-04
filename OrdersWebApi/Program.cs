using Common.Configs;
using Common.DataAccess;
using Common.Exceptions;
using Common.Extensions;
using Common.Infrastructure;
using Common.Logging;
using Common.MessageBroker.Products;
using Connectors;
using Connectors.Products;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using Orders.App.ExternalData;
using Orders.App.HostedServices;
using Orders.App.Migrations;
using Orders.App.Repositories;
using Orders.App.Repositories.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration.Get<ConfigBase>();
Log.Logger = LoggingSetup.CreateLogger(builder.Configuration, config.Service.Name);
builder.Host.UseSerilog();
Log.Information($"Starting {config.Service.Name}..");

// ConfigureServices

builder.Services.AddConvey()
    .AddRabbitMq()
    .AddEventHandlers()
    .AddInMemoryEventDispatcher();
builder.Services.Configure<ConfigBase>(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCommonServices();
builder.Services.AddServiceSwagger(typeof(Program));
builder.Services.AddServiceHealthChecks(config);
builder.Services.AddMigrations(config.Database, typeof(MigrationBase));
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddProductsApi();
builder.Services.AddHostedService<FetchProductsHostedService>();
builder.Services.AddSingleton<IProductsRepository, ProductsRepository>();

var app = builder.Build();
app.MigrateDatabase("ordersdb");

// Configure


app.UseRequestExceptionsHandler();
app.UseServiceSerilogRequestLogging();
app.UseServiceSwagger();
app.UseRouting()
    .UseEndpoints(config => config.UseServiceHealthChecksRouting());
app.UseServiceHealthChecks();

app.UseConvey()
    .UseRabbitMq()
    .SubscribeEvent<ProductCreated>();
app.MapGet("api/v1/orders", async (IOrdersRepository repository) => await repository.GetAllAsync());
app.MapGet("api/v1/orders/products", async (IProductsApi products) => await products.GetProducts());

app.Run();
Log.Information($"Started {config.Service.Name}!");