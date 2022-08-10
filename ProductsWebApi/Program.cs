using Common.Configs;
using Common.DataAccess;
using Common.Extensions;
using Common.Infrastructure;
using Common.Logging;
using Products.App.Migrations;
using Products.App.Repositories;
using Products.App.Repositories.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = LoggingSetup.CreateLogger(builder.Configuration);
builder.Host.UseSerilog();
var config = builder.Configuration.Get<ConfigBase>();
Log.Information($"Starting {config.Service.Name}..");
// ConfigureServices


builder.Services.Configure<ConfigBase>(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCommonServices();
builder.Services.AddServiceSwagger(typeof(Program));
builder.Services.AddServiceHealthChecks(config);
builder.Services.AddMigrations(config.Database, typeof(MigrationBase));
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();

var app = builder.Build();
app.MigrateDatabase("productsdb");

// Configure

app.UseServiceSwagger();
app.UseRouting()
    .UseEndpoints(config => config.UseServiceHealthChecksRouting());
app.UseServiceHealthChecks();

app.MapGet("api/v1/", async (IProductsRepository repository) => await repository.GetAllAsync());

app.Run();
Log.Information($"Started {config.Service.Name}!");
