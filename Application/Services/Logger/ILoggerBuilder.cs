namespace Services.Logger;

public interface ILoggerBuilder
{
    /// <summary>
    /// Adds logging to the console.
    /// </summary>
    /// <returns>ILoggerBuilder</returns>
    ILoggerBuilder LogToConsole();

    /// <summary>
    /// Adds logging to a file located in the root folder of the project and called "logs.log".
    /// </summary>
    /// <returns>ILoggerBuilder</returns>
    ILoggerBuilder LogToFile();
    
    /// <summary>
    /// Adds logging to a file located along the passed path and called "logs.log".
    /// </summary>
    /// <param name="filePath">Path to the location of the log file.</param>
    /// <returns>ILoggerBuilder</returns>
    ILoggerBuilder LogToFile(string filePath);

    /// <summary>
    /// Adds logging to a file located at the passed path with the passed name and extension.
    /// </summary>
    /// <param name="filePath">Path to the location of the log file.</param>
    /// <param name="fileName">Name of the log file with file extension.</param>
    /// <returns>ILoggerBuilder</returns>
    ILoggerBuilder LogToFile(string filePath, string fileName);

    /// <summary>
    /// Returns the finished logger.
    /// </summary>
    /// <returns>Logger</returns>
    IEnumerable<ILoggerProvider> Build();
}