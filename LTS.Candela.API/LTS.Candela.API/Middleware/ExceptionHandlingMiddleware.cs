using System.Net;
using System.Text.Json;

namespace LTS.Candela.API.Middleware;

public class ExceptionHandling
{
    private readonly RequestDelegate _next;

    public ExceptionHandling(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidOperationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            var errorResponse = new { error = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var errorResponse = new { error = "An unexpected error occurred." };
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
