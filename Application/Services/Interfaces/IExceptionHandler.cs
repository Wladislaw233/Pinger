namespace Services.Interfaces;

public interface IExceptionHandler
{
    void UnhandledExceptionHandler(UnhandledExceptionEventArgs e);
}