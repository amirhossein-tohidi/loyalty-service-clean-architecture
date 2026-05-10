using FluentValidation;
using Loyalty.Api.Common;
using Loyalty.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Loyalty.Api.Middlewares;

public class GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ErrorResponse response;
        int statusCode;

        switch (exception)
        {
            case ValidationException validationException:
                response = new ErrorResponse
                {
                    Message = "Validation failed.",
                    Errors = validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(x => x.ErrorMessage).ToArray()),
                    TraceId = httpContext.TraceIdentifier
                };
                statusCode = StatusCodes.Status400BadRequest;
                break;

            case DomainValidationException domainException:
                response = new ErrorResponse
                {
                    Message = domainException.Message,
                    TraceId = httpContext.TraceIdentifier
                };
                statusCode = StatusCodes.Status400BadRequest;
                break;

            case ArgumentException argumentException:
                response = new ErrorResponse
                {
                    Message = argumentException.Message,
                    TraceId = httpContext.TraceIdentifier
                };
                statusCode = StatusCodes.Status400BadRequest;
                break;
            default:
                logger.LogError(
                    exception,
                    "{Middleware} Exception occurred: {Message}",
                    nameof(GlobalExceptionMiddleware),
                    exception.Message);

                response = new ErrorResponse
                {
                    Message = "An unexpected error occurred.",
                    TraceId = httpContext.TraceIdentifier
                };
                statusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);
        return true;
    }
}