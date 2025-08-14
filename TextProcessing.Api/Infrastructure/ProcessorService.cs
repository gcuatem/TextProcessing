using System.Runtime.CompilerServices;
using System.Text;
using TextProcessing.Api.Application;

namespace TextProcessing.Api.Infrastructure
{
  public class ProcessorService : IProcessorService
  {
    public async IAsyncEnumerable<char> ProcessStringAsync(string input, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
      var grouped = input.GroupBy(c => c).OrderBy(g => g.Key);
      var sb = new StringBuilder();
      foreach (var g in grouped)
      {
        sb.Append(g.Key);
        sb.Append(g.Count());
      }
      sb.Append('/');
      sb.Append(Convert.ToBase64String(Encoding.UTF8.GetBytes(input)));

      foreach (var ch in sb.ToString())
      {
        cancellationToken.ThrowIfCancellationRequested();
        var delayMs = Random.Shared.Next(1000, 5001);
        await Task.Delay(delayMs, cancellationToken);
        yield return ch;
      }
    }
  }
}
