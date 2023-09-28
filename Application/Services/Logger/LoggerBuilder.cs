namespace Services.Logger;

public class LoggerBuilder : ILoggerBuilder
{
    private ILoggerProvider? _fileLogger;
    
    private ILoggerProvider? _consoleLogger;
    
    private const string FileName = "logs.log";

    private readonly string _filePath = Directory.GetCurrentDirectory();
    
    public ILoggerBuilder LogToConsole()
    {
        _consoleLogger = new LoggerConsoleProvider();
        
        return this;
    }
    
    public ILoggerBuilder LogToFile()
    {
        var pathToFile = Path.Combine(_filePath, FileName);
        
        _fileLogger = new LoggerFileProvider(pathToFile);
        
        return this;
    }
    
    public ILoggerBuilder LogToFile(string filePath)
    {
        ValidateFilePath(filePath);
        
        var pathToFile = Path.Combine(filePath, FileName);
        
        _fileLogger = new LoggerFileProvider(pathToFile);
        
        return this;
    }

    public ILoggerBuilder LogToFile(string filePath, string fileName)
    {
        ValidateFilePath(filePath);
        ValidateFileName(fileName);
        
        var pathToFile = Path.Combine(filePath, fileName);

        _fileLogger = new LoggerFileProvider(pathToFile);
        
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

    public IEnumerable<ILoggerProvider> Build()
    {
        var loggers = new List<ILoggerProvider>();
        
        var consoleLogger = _consoleLogger;
        
        if(consoleLogger != null)
            loggers.Add(consoleLogger);

        _consoleLogger = null;
        
        var fileLogger = _fileLogger;
        
        if(fileLogger != null)
            loggers.Add(fileLogger);

        _fileLogger = null;
        
        return loggers;
    }
}