namespace Services.Logger

{
    public interface ILogger : IAsyncDisposable
    {
        /// <summary>
        /// Logs the message with the dedicated logging level.
        /// </summary>
        /// <param name="logLevel">Desired logging level.</param>
        /// <param name="message">logged message.</param>
        /// <returns></returns>
        Task LogAsync(LogLevel logLevel, string message);
    }
}