namespace Services.Logger;

public class Logger : ILogger
{
    private readonly IEnumerable<ILoggerProvider> _loggers;

    public Logger(IEnumerable<ILoggerProvider> loggers)
    {
        _loggers = loggers;
    }
    
    public async Task LogAsync(LogLevel logLevel, string message)
    {
        var logMessage = GenerateLogMessage(logLevel, message);

        foreach (var logger in _loggers)
        {
            await logger.LogMessageAsync(logMessage);
        }
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

    public async ValueTask DisposeAsync()
    {
        var disposableLoggers = _loggers.Where(logger => logger is IAsyncDisposable);

        foreach (var logger in disposableLoggers)
        {
            await (logger as IAsyncDisposable)!.DisposeAsync();
        }
    }
}