namespace Services.Logger;

public class Logger : ILogger
{
    private readonly string _logFilePath;
    
    private readonly bool _logToConsole;
    
    private readonly bool _logToFile;
    
    private readonly SemaphoreSlim _semaphoreSlim = new(1,1);
    public Logger(string logFilePath, bool logToConsole, bool logToFile)
    {
        _logToConsole = logToConsole;
        _logToFile = logToFile;
        _logFilePath = logFilePath;
    }

    public async Task LogAsync(LogLevel logLevel, string message)
    {
        var logMessage = GenerateLogMessage(message, logLevel);

        if (_logToFile)
        {
            await LogToFileAsync(logMessage);
        }

        if (_logToConsole)
        {
            LogToConsole(logMessage, logLevel);
        }
    }

    private static string GenerateLogMessage(string message, LogLevel logLevel)
    {
        var logLevelReduction = GetLogLevelReduction(logLevel);
        return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevelReduction}] {message}";
    }

    private static string GetLogLevelReduction(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Error => "ERR",
            LogLevel.Information => "INF",
            LogLevel.Warning => "WRG",
            _ => "UNK"
        };
    }
    
    private async Task LogToFileAsync(string logMessage)
    {
        try
        {
            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

            await using var fileStream = new FileStream(_logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            
            await using var streamWriter = new StreamWriter(fileStream);
            
            await streamWriter.WriteLineAsync(logMessage).ConfigureAwait(false);
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

    private void LogToConsole(string logMessage, LogLevel logLevel)
    {
        Console.ForegroundColor = logLevel switch
        {
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Warning => ConsoleColor.Yellow,
            _ => Console.ForegroundColor
        };
        
        Console.WriteLine(logMessage);
        
        Console.ResetColor();
    }
}