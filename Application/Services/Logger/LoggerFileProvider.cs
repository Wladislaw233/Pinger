using System.Text;

namespace Services.Logger;

public class LoggerFileProvider : ILoggerProvider, IAsyncDisposable
{
    private const int BufferSize = 1024;

    private readonly StringBuilder _messageBuffer = new();
    
    private readonly SemaphoreSlim _semaphoreSlim = new(1,1);
    
    private readonly string _pathToFile;
    
    public LoggerFileProvider(string pathToFile)
    {
        _pathToFile = pathToFile;
    }

    public async Task LogMessageAsync(string logMessage)
    {
        _messageBuffer.AppendLine(logMessage);

        if (_messageBuffer.Length >= BufferSize)
        {
            await FlushAsync();
            _messageBuffer.Clear();
        }
    }
    
    private async Task FlushAsync()
    {
        try
        {
            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

            await using var fileStream = new FileStream(_pathToFile, FileMode.Append, FileAccess.Write, FileShare.Read);
            
            await using var streamWriter = new StreamWriter(fileStream);
            
            await streamWriter.WriteAsync(_messageBuffer.ToString()).ConfigureAwait(false);
        }
        catch (Exception exc)
        {
            Console.WriteLine($"An error occurred while writing to the log. {exc}");
            throw;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await FlushAsync();
        
        _semaphoreSlim.Dispose();
    }
}