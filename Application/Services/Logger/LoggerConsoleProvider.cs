namespace Services.Logger;

public class LoggerConsoleProvider : ILoggerProvider
{
    public async Task LogMessageAsync(string logMessage)
    {
        Console.WriteLine(logMessage);
        await Task.CompletedTask;
    }
}