using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using TextProcessing.Api.Application;


namespace TextProcessing.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProcessingController : ControllerBase
  {
    private readonly IProcessorService _processor;

    public ProcessingController(IProcessorService processor)
    {
      _processor = processor;
    }

    [HttpPost]
    public async Task<IActionResult> Process([FromBody] ProcessRequest request)
    {
      if (request is null || request.Input is null)
        return BadRequest("Body must include 'input'.");

      // Prepare for streaming
      Response.ContentType = "text/plain";
      Response.Headers["Cache-Control"] = "no-store";
      Response.Headers["X-Accel-Buffering"] = "no"; // For nginx to disable buffering

      await Response.StartAsync(HttpContext.RequestAborted);

      try
      {
        await foreach (var ch in _processor.ProcessStringAsync(request.Input, HttpContext.RequestAborted))
        {
          await Response.WriteAsync(ch.ToString(), HttpContext.RequestAborted);
          await Response.Body.FlushAsync(HttpContext.RequestAborted);
        }

        return new EmptyResult();
      }
      catch (OperationCanceledException)
      {
        // Client disconnected or cancelled — just end the response cleanly
        HttpContext.Response.HttpContext.Features.Get<IHttpResponseBodyFeature>()?.DisableBuffering();
        return new EmptyResult();
      }
    }

    [HttpGet("health")] // simple health endpoint
    public IActionResult Health() => Ok(new { status = "ok" });
  }
}
