using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.Span;
using Serilog.Events;

namespace Common.Logging;

public static class LoggingSetup
{
    public static Logger CreateLogger(IConfiguration configuration, string serviceName)
    {
        return new LoggerConfiguration()
            .MinimumLevel.Override("Mircosoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithSpan()
            .Enrich.WithProperty("service", serviceName)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }

    public static IApplicationBuilder UseServiceSerilogRequestLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.IncludeQueryInRequestPath = true;
            options.EnrichDiagnosticContext = EnrichFromRequest;
            options.GetLevel = GetLevel;
        });
        return app;
    }

    private static void EnrichFromRequest(IDiagnosticContext context, HttpContext httpContext)
    {
        var request = httpContext.Request;

        // Set all the common properties available for every request
        context.Set("Host", request.Host);
        context.Set("Protocol", request.Protocol);
        context.Set("Scheme", request.Scheme);

        if (request.QueryString.HasValue)
        {
            context.Set("QueryString", request.QueryString.Value);
        }

        context.Set("ContentType", httpContext.Response.ContentType);

        var endpoint = httpContext.GetEndpoint();
        if (endpoint != null)
        {
            context.Set("EndpointName", endpoint.DisplayName);
        }
    }

    private static LogEventLevel GetLevel(HttpContext context, double _, Exception ex)
    {
        return ex != null
            ? LogEventLevel.Error
            : context.Response.StatusCode > 499
                ? LogEventLevel.Error
                : IsHealthCheckEndpoint(context)
                    ? LogEventLevel.Verbose
                    : LogEventLevel.Information;
    }
    private static bool IsHealthCheckEndpoint(HttpContext ctx)
    {
        var endpoint = ctx.GetEndpoint();
        if (endpoint != null)
        {
            return string.Equals(
                endpoint.DisplayName,
                "Health checks",
                StringComparison.Ordinal);
        }
        return false;
    }
}
