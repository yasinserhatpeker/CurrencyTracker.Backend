using System.Net;
using CurrencyTracker.Application.Wrappers;

namespace CurrencyTracker.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {  
        try
        {
        await _next(context);
            
        }
        catch(Exception ex)
        {
            await HandleExceptionAsync(context,ex);
        }

    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType ="application/json";
        var response = ApiResponse<object>.Fail(exception.Message);

        context.Response.StatusCode = exception switch
        {
            KeyNotFoundException => (int)HttpStatusCode.NotFound, // 404
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401
            ArgumentException => (int)HttpStatusCode.BadRequest, // 400
            _ =>(int)HttpStatusCode.InternalServerError, // 500

        };

        return context.Response.WriteAsJsonAsync(response);  
    }
}
