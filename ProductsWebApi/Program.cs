using Common.Configs;
using Common.DataAccess;
using Common.Exceptions;
using Common.Extensions;
using Common.Infrastructure;
using Common.Logging;
using Common.MessageBroker;
using Common.MessageBroker.Products;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.MessageBrokers.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Products.App.Migrations;
using Products.App.Repositories;
using Products.App.Repositories.Interfaces;
using ProductsWebApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration.Get<ConfigBase>();
Log.Logger = LoggingSetup.CreateLogger(builder.Configuration, config.Service.Name);
builder.Host.UseSerilog();
Log.Information($"Starting {config.Service.Name}..");

// ConfigureServices

builder.Services.AddConvey()
    .AddRabbitMq();
builder.Services.Configure<ConfigBase>(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCommonServices();
builder.Services.AddServiceSwagger(typeof(Program));
builder.Services.AddServiceHealthChecks(config);
builder.Services.AddMigrations(config.Database, typeof(MigrationBase));
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddSingleton<IApplicationBusPublisher, ApplicationBusPublisher>();

var app = builder.Build();
app.MigrateDatabase("productsdb");

// Configure

app.UseRequestExceptionsHandler();
app.UseServiceSerilogRequestLogging();
app.UseServiceSwagger();
app.UseRouting()
    .UseEndpoints(config => config.UseServiceHealthChecksRouting());
app.UseServiceHealthChecks();

app.MapGet("api/v1/products", async (IProductsRepository repository) => await repository.GetAllAsync());
app.MapPost("api/v1/products",
    async ([FromBody] CreateProductRequestModel request, IProductsRepository repository, IApplicationBusPublisher publisher) =>
    {
        await repository.CreateAsync(request.Name, request.Price, request.Description, request.AvailableAmount);
        var product = await repository.GetAllAsync();
        var currentProduct = product.First(x => x.Name == request.Name);
        await publisher.PublishMessageAsync(new ProductCreated(currentProduct.Id, currentProduct.Name, currentProduct.Price,
            currentProduct.Description, currentProduct.AvailableAmount));
    });
        

app.Run();
Log.Information($"Started {config.Service.Name}!");
