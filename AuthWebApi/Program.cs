using Common.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ConfigureServices

builder.Services.AddCommonServices();

var app = builder.Build();

// Configure

app.MapGet("", () => "Hi from auth");

app.Run();
