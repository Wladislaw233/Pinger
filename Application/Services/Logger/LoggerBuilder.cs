namespace Services.Logger;

public class LoggerBuilder : ILoggerBuilder
{
    private Logger _logger = new();

    private const string FileName = "logs.log";

    private readonly string _filePath = Directory.GetCurrentDirectory();
    
    public ILoggerBuilder LogToConsole()
    {
        _logger.LoggingToConsole = true;
        
        return this;
    }
    
    public ILoggerBuilder LogToFile()
    {
        _logger.LoggingToFile = true;

        _logger.PathToFile = Path.Combine(_filePath, FileName);
        
        return this;
    }
    
    public ILoggerBuilder LogToFile(string filePath)
    {
        ValidateFilePath(filePath);
        
        _logger.LoggingToFile = true;

        _logger.PathToFile = Path.Combine(filePath, FileName);
        
        return this;
    }

    public ILoggerBuilder LogToFile(string filePath, string fileName)
    {
        ValidateFilePath(filePath);
        ValidateFileName(fileName);
        
        _logger.LoggingToFile = true;

        _logger.PathToFile = Path.Combine(filePath, fileName);
        
        return this;
    }

    private static void ValidateFilePath(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || filePath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
        {
            throw new ArgumentException("Invalid directory path.");
        }
        
        if (!Directory.Exists(filePath))
        {
            throw new DirectoryNotFoundException("The file path does not exist.");
        }
    }

    private static void ValidateFileName(string fileName)
    {
        if (string.IsNullOrEmpty(fileName) || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new ArgumentException("Invalid file name.");
        }

        var fileExtension = Path.GetExtension(fileName);
        
        if (string.IsNullOrEmpty(fileExtension))
        {
            throw new ArgumentException("Invalid file extension.");
        }
    }

    public Logger Build()
    {
        var logger = _logger;

        _logger = new Logger();

        return logger;
    }
}