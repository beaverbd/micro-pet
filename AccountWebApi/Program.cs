using System.Reflection;
using Common.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ConfigureServices

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCommonServices();
builder.Services.AddSwaggerGen(opts =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opts.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure

app.MapGet("api/v1/", () => "Hi from account");
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
