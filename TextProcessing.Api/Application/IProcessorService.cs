namespace TextProcessing.Api.Application
{
  public interface IProcessorService
  {
    IAsyncEnumerable<char> ProcessStringAsync(string input, CancellationToken cancellationToken);
  }
}
