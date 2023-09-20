namespace Services.Interfaces;

public interface IExceptionHandler
{
    Task UnhandledExceptionHandler(UnhandledExceptionEventArgs e);
}