using Common.Configs;
using Common.Extensions;
using Common.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ConfigureServices

var config = builder.Configuration.Get<ConfigBase>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCommonServices();
builder.Services.AddServiceSwagger(typeof(Program));
builder.Services.AddServiceHealthChecks(config);

var app = builder.Build();

// Configure

app.UseServiceSwagger();
app.UseRouting()
    .UseEndpoints(config => config.UseServiceHealthChecksRouting());
app.UseServiceHealthChecks();

app.MapGet("", () => "Hi from products");

app.Run();
