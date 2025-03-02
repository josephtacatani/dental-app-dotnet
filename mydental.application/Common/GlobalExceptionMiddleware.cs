using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using mydental.application.Common;
using mydental.application.DTO;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ServiceResult<object>.Error("Internal Server Error", new List<string> { ex.Message });
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        // Handle Unauthorized (401) response
        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            context.Response.ContentType = "application/json";
            var response = new UnauthorizedResponseDto();
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        // Handle Forbidden (403) response
        if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
        {
            context.Response.ContentType = "application/json";
            var response = ServiceResult<object>.Forbidden("Forbidden", new List<string> { "You do not have permission to access this resource." });
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
