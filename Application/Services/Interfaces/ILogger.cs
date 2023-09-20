namespace Services.Interfaces;

public interface ILogger
{
    Task LogWarningAsync(string message);
    
    Task LogInfoAsync(string message);

    Task LogErrorAsync(string message);
}