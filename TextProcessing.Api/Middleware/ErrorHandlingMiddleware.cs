using System.Net;

namespace TextProcessing.Api.Middleware
{
  public class ErrorHandlingMiddleware
  {
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (OperationCanceledException)
      {
        // swallow cancellations (client aborted) to avoid noisy logs
        context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
      }
      catch (Exception ex)
      {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";
        var payload = System.Text.Json.JsonSerializer.Serialize(new { error = ex.Message });
        await context.Response.WriteAsync(payload);
      }
    }
  }
}
