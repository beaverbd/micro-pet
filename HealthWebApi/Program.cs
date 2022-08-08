using Common.Extensions;
using Common.Infrastructure;
using HealthWebApi.ServicesHealth;

var builder = WebApplication.CreateBuilder(args);

// ConfigureServices

builder.Services.AddCommonServices();
builder.Services.AddServiceSwagger(typeof(Program));
builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure

app.UseRouting();
app.UseEndpoints(config => config.MapHealthChecksUI(c =>
{
    c.ResourcesPath = "/health/ui/resources";
}));
app.UseServiceSwagger();
app.MapGet("", () => "Hi from health");

app.Run();
