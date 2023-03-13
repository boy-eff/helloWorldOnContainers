﻿using System.Net;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using Words.BusinessAccess.Exceptions;
using Words.WebAPI.Models;

namespace Words.WebAPI.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (AuthorizationException ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
        catch(Exception ex)
        {
            _logger.LogError($"Something went wrong {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        switch (exception)
        {
            case AuthorizationException:
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                var message = exception.Message.IsNullOrEmpty() ? "Access denied" : exception.Message;
                await context.Response.WriteAsync(message);
                break;
            }
            case NotFoundException:
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                var message = exception.Message.IsNullOrEmpty() ? "Not found" : exception.Message;
                await context.Response.WriteAsync(message);
                break;
            }
            case ValidationException:
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var message = exception.Message.IsNullOrEmpty() ? "Validation error" : exception.Message;
                await context.Response.WriteAsync(message);
                break;
            }
            case WrongActionException:
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var message = exception.Message.IsNullOrEmpty() ? "Wrong Action" : exception.Message;
                await context.Response.WriteAsync(message);
                break;
            }
            default:
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync("Internal server error");
                break;
            }

        }
    }
}