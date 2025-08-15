
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TextProcessing.Api.Infrastructure;

namespace TextProcessing.Tests
{
  public class ProcessorServiceTests
  {
    [Test]
    public async Task ProcessStringAsync_ReturnsExpectedOutput()
    {
      var service = new ProcessorService();
      var input = "aabbc";
      var expectedPrefix = "a2b2c1/";

      var result = "";
      var expectedResult = "a2b2c1/YWFiYmM=";
      await foreach (var ch in service.ProcessStringAsync(input, CancellationToken.None))
      {
        result += ch;
      }
      //validate prefix has the counter and order expected
      Assert.IsTrue(result.StartsWith(expectedPrefix));

      var base64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
      //validate the base64 encoded string is at the end
      Assert.IsTrue(result.EndsWith(base64));

      //validate ther entire result is as expected
      Assert.That(result, Is.EqualTo(expectedResult));
    }
  }
}