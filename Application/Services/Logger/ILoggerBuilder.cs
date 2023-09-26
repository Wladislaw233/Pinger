namespace Services.Logger;

public interface ILoggerBuilder
{
    void LogToConsole();

    void LogToFile();
    
    void LogToFile(string logFilePath);
    
    void LogToFile(string logFilePath, string logFileName);

    Logger Build();
}