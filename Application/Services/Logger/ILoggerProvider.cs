namespace Services.Logger;

public interface ILoggerProvider
{
    Task LogMessageAsync(string logMessage);
}