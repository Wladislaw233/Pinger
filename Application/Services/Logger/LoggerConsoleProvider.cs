namespace Services.Logger;

public class LoggerConsoleProvider : ILoggerProvider
{
    public Task LogMessageAsync(string logMessage)
    {
        Console.WriteLine(logMessage);
        return Task.CompletedTask;
    }
}