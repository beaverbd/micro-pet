using System.Net;
using System.Text.Json;
using Common.Configs;
using Common.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace Common.Exceptions
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly IOptions<ConfigBase> _config;

        public ExceptionHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env, IOptions<ConfigBase> config)
        {
            _next = next;
            _env = env;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ServiceException ex)
            {
                await HandleServiceException(context, ex);
                LogException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
                LogException(context, ex);
            }
        }

        private async Task HandleServiceException(HttpContext context, ServiceException exception)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = (int)exception.StatusCode;
                context.Response.ContentType = "application/json";
            }

            var body = context.Response.Body;
            var response = new ExceptionResponse
            {
                Code = (int)exception.Type,
                Message = exception.Message,
                Args = exception.Args
            };
            if (!_env.IsProduction())
                response.Dev = CreateDevExceptionDetails(exception);
            await JsonSerializer.SerializeAsync(body, response);
            await body.FlushAsync();
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
            }

            var body = context.Response.Body;
            var response = new ExceptionResponse
            {
                Code = (int)ExceptionType.Unknown,
                Message = exception.Message
            };
            if (!_env.IsProduction())
                response.Dev = CreateDevExceptionDetails(exception);
            await JsonSerializer.SerializeAsync(body, response);
            await body.FlushAsync();
        }

        private DevExceptionDetails CreateDevExceptionDetails(Exception exception)
        {
            return new DevExceptionDetails
            {
                Message = exception.Message,
                AdditionalInformation = exception.InnerException != null
                ? (exception.InnerException.Message + " -- " + exception.InnerException.StackTrace)
                : exception.StackTrace
            };
        }

        private void LogException(HttpContext context, Exception exception)
        {
            Log.Error(exception, "An exception of type {ExceptionType} has occurred while processing request. {StackTrace}",
                exception.GetType().Name, exception.StackTrace);
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestExceptionsHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
