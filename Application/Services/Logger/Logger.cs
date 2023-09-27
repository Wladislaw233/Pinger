namespace Services.Logger;

public class Logger : ILogger
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1,1);
    
    public bool LoggingToConsole { get; set; }
    
    public bool LoggingToFile { get; set; }
    
    public string PathToFile { get; set; }
    
    public async Task LogAsync(LogLevel logLevel, string message)
    {
        var logMessage = GenerateLogMessage(logLevel, message);
        
        if (LoggingToFile)
            await LogToFileAsync(logMessage);
        
        if (LoggingToConsole)
            LogToConsole(logLevel, logMessage);
            
        await Task.Delay(100);
    }

    private async Task LogToFileAsync(string logMessage)
    {
        try
        {
            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

            await using var fileStream = new FileStream(PathToFile, FileMode.Append, FileAccess.Write, FileShare.Read);
            
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
    
    private void LogToConsole(LogLevel logLevel, string logMessage)
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
    
    private static string GenerateLogMessage(LogLevel logLevel, string message)
    {
        var logLevelReduction =  logLevel switch
        {
            LogLevel.Error => "ERR",
            LogLevel.Information => "INF",
            LogLevel.Warning => "WRG",
            _ => "UNK"
        };
        
        return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevelReduction}] {message}";
    }
}