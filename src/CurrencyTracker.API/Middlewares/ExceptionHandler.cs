using System.Net;
using System.Net.Mime;
using CurrencyTracker.Application.Wrappers;

namespace CurrencyTracker.API.Middlewares;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the system. Request path:{requestPath}", context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }

    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;


        var statusCode = exception switch
        {
            KeyNotFoundException => (int)HttpStatusCode.NotFound, // 404
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401
            ArgumentException => (int)HttpStatusCode.BadRequest, // 400
            _ => (int)HttpStatusCode.InternalServerError, // 500

        };
        var responseMessage = statusCode == 500 ? "An unexpected error occured in the system." : exception.Message;

        var response = ApiResponse<object>.Fail(responseMessage);

        return context.Response.WriteAsJsonAsync(response);
    }
}
