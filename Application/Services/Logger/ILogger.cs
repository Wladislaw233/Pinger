namespace Services.Logger

{
    public interface ILogger
    {
        Task LogAsync(LogLevel logLevel, string message);
    }
}