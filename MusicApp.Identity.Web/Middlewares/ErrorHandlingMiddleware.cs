using MusicApp.Identity.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace MusicApp.Identity.Web.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;

        if(exception is AlreadyExistsException)
        {
            code = HttpStatusCode.BadRequest;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        var errors = exception.Message.Split(';');
        var result = JsonSerializer.Serialize(new { errors = errors });

        return context.Response.WriteAsync(result);
    }
}
