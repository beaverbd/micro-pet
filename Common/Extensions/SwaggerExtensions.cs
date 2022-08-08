using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddServiceSwagger(this IServiceCollection services, Type executingAssemblyType)
    {
        services.AddSwaggerGen(opts =>
        {
            var xmlFile = $"{executingAssemblyType.Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            opts.IncludeXmlComments(xmlPath);
        });
        return services;
    }

    public static IApplicationBuilder UseServiceSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}

