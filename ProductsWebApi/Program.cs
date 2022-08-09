using Common.Configs;
using Common.DataAccess;
using Common.Extensions;
using Common.Infrastructure;
using Products.App.Migrations;

var builder = WebApplication.CreateBuilder(args);

// ConfigureServices

var config = builder.Configuration.Get<ConfigBase>();
builder.Services.Configure<ConfigBase>(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCommonServices();
builder.Services.AddServiceSwagger(typeof(Program));
builder.Services.AddServiceHealthChecks(config);
builder.Services.AddMigrations(config.Database, typeof(MigrationBase));

var app = builder.Build();
app.MigrateDatabase("productsdb");

// Configure

app.UseServiceSwagger();
app.UseRouting()
    .UseEndpoints(config => config.UseServiceHealthChecksRouting());
app.UseServiceHealthChecks();

app.MapGet("api/v1/", () => "Hi from products");

app.Run();
