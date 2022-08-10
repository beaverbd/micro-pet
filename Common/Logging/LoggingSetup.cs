using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Common.Logging;

public static class LoggingSetup
{
    public static Logger CreateLogger(IConfiguration configuration)
    {
        return new LoggerConfiguration()
            .MinimumLevel.Override("Mircosoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }
}
