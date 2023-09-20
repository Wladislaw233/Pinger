using Services.Interfaces;

namespace Services;

public class Logger : ILogger
{
    private readonly string _logFilePath;
    private readonly SemaphoreSlim _semaphoreSlim = new(1,1);
    public Logger(string logFilePath)
    {
        if (string.IsNullOrWhiteSpace(logFilePath))
            throw new ArgumentNullException(nameof(logFilePath), "Error creating logger.");
       
        _logFilePath = logFilePath;
    }
    public async Task LogInfoAsync(string message)
    {
        await LogAsync("INF", message);
    }

    public async Task LogWarningAsync(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        await LogAsync("WRG", message);
        Console.ResetColor();
    }
    
    public async Task LogErrorAsync(string message)
    {
        await LogAsync("ERR", message);
    }

    private async Task LogAsync(string logLevel, string message)
    {
        try
        {
            var fileInfo = new FileInfo(_logFilePath);

            if (!fileInfo.Exists)
            {
                fileInfo.Create();
            }

            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

            await using var streamWriter = File.AppendText(_logFilePath);

            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {message}";

            Console.WriteLine(logEntry);

            await streamWriter.WriteLineAsync(logEntry).ConfigureAwait(false);
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
}