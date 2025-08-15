using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TextProcessing.Api.Application;
using TextProcessing.Api.Controllers;
using NUnit.Framework;

namespace TextProcessing.Tests
{
  public class ProcessingControllerTests
  {
    private Mock<IProcessorService> _processorMock;
    private ProcessingController _controller;

    [SetUp]
    public void Setup()
    {
      _processorMock = new Mock<IProcessorService>();
      _controller = new ProcessingController(_processorMock.Object);
      var httpContext = new DefaultHttpContext();
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = httpContext
      };
    }

    [Test]
    public async Task Process_ReturnsBadRequest_WhenRequestIsNull()
    {
      var result = await _controller.Process(null);
      Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public async Task Process_ReturnsBadRequest_WhenInputIsNull()
    {
      var result = await _controller.Process(new ProcessRequest { Input = null });
      Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public void Health_ReturnsOk()
    {
      var result = _controller.Health();
      Assert.IsInstanceOf<OkObjectResult>(result);
    }
  }
}