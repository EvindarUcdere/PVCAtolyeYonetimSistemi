using System.Net;
using System.Text.Json;
using PVCAtolye.Application.Common.Models;

namespace PVCAtolye.Api.Middleware;

public sealed partial class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            LogUnhandledApiException(logger, exception);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponse.Fail<object>("Beklenmeyen bir hata olustu.", "Islem tamamlanamadi.");
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
        }
    }

    [LoggerMessage(EventId = 1000, Level = LogLevel.Error, Message = "Unhandled API exception")]
    private static partial void LogUnhandledApiException(ILogger logger, Exception exception);
}
