namespace Services.Logger;

public class LoggerBuilder : ILoggerBuilder
{
    private string _logFilePath = AppDomain.CurrentDomain.BaseDirectory;
    
    private string _logFileName = "logs.log";
    
    private bool _logToConsole;

    private bool _logToFile;
    
    public void LogToConsole()
    {
        _logToConsole = true;
    }

    public void LogToFile()
    {
        _logToFile = true;
    }
    
    public void LogToFile(string logFilePath)
    {
        if (string.IsNullOrWhiteSpace(logFilePath))
            throw new ArgumentNullException(nameof(logFilePath));
        
        _logToFile = true;
        
        _logFilePath = logFilePath;
    }

    public void LogToFile(string logFilePath, string logFileName)
    {
        if (string.IsNullOrWhiteSpace(logFilePath))
            throw new ArgumentNullException(logFilePath);
        
        if (string.IsNullOrWhiteSpace(logFileName))
            throw new ArgumentNullException(nameof(logFileName));
        
        _logToFile = true;
        _logFilePath = logFilePath;
        _logFileName = logFileName;
    }

    public Logger Build()
    {
        return new Logger(Path.Combine(_logFilePath, _logFileName), _logToConsole, _logToFile);
    }
}