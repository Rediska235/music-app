﻿using MusicApp.SongService.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace MusicApp.SongService.Web.Middlewares;

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

        if (exception is NotYourSongException)
        {
            code = HttpStatusCode.BadRequest;
        }
        else if (exception is SongNotFoundException)
        {
            code = HttpStatusCode.NotFound;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        var result = JsonSerializer.Serialize(new { error = exception.Message });

        return context.Response.WriteAsync(result);
    }
}
