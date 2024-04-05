using Infrastructure.Middlewares.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Middlewares
{
    public class ExceptionsHandlingMiddleware(ILogger<ExceptionsHandlingMiddleware> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            string logErrorMessage = exception switch
            {
                BadRequestException => $"Bad request. {exception.Message} {exception.StackTrace}",
                ArgumentException => $"Bad request. {exception.Message} {exception.StackTrace}",
                ForbiddenException => $"Access denied. {exception.Message} {exception.StackTrace}",
                NotFoundException => $"Resource not found. {exception.Message} {exception.StackTrace}",
                _ => $"Something went wrong. {exception.Message} {exception.StackTrace}"
            };

            string responseErrorMessage = exception switch
            {
                BadRequestException => $"{exception.Message}",
                ArgumentException => $"{exception.Message}",
                ForbiddenException => $"{exception.Message}",
                NotFoundException => $"{exception.Message}",
                _ => $"Something went wrong."
            };

            int statusCode = exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                ForbiddenException => StatusCodes.Status403Forbidden,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            logger.LogError(logErrorMessage);
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(responseErrorMessage, cancellationToken);
            return true;
        }
    }
}
