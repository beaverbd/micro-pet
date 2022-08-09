using System.Reflection;
using Common.Infrastructure;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureAppConfiguration((context, configBuilder) =>
{
    configBuilder.AddJsonFile(Path.Combine($"ocelot.json"));
    configBuilder.AddJsonFile(Path.Combine($"ocelot.{context.HostingEnvironment.EnvironmentName}.json"));
});

// ConfigureServices
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCommonServices();
builder.Services.AddOcelot();
builder.Services.AddSwaggerForOcelot(builder.Configuration, swaggerSetup: options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure

app.MapGet("", () => "Hi from gateway");
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});
await app.UseOcelot();

app.Run();
