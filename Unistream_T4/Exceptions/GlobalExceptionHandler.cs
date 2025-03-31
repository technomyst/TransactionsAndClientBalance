using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Unistream_T4.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, exception.Message);

            var problemDetails = exception switch
            {
                NotFoundException => new ProblemDetails()
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = "NotFound",
                    Detail = exception.Message,
                    Instance = "API",
                    Status = StatusCodes.Status404NotFound
                },
                BadRequestException => new ProblemDetails()
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "BadRequest",
                    Detail = exception.Message,
                    Instance = "API",
                    Status = StatusCodes.Status400BadRequest
                },
                _ => new ProblemDetails()
                {
                    Type = "Server Error",
                    Title = "API Error",
                    Detail = $"{exception.Message} {exception.InnerException?.Message}",
                    Instance = "API",
                    Status = (int)HttpStatusCode.InternalServerError
                }
            };
                
            var response = JsonSerializer.Serialize(problemDetails);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = problemDetails.Status?? 500;

            await httpContext.Response.WriteAsync(response, cancellationToken);

            return true;

        }
    }
}
