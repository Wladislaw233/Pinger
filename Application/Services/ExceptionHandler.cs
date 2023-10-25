using Services.Interfaces;
using Services.Logger;

namespace Services;

public class ExceptionHandler : IExceptionHandler
{
    private readonly ILogger _logger;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;

    public ExceptionHandler(ILogger logger, ICancellationTokenProvider cancellationTokenProvider)
    {
        _logger = logger;
        _cancellationTokenProvider = cancellationTokenProvider;
    }
    
    public void UnhandledExceptionHandler(UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
            _logger.LogAsync(LogLevel.Error,$"Something went wrong. \n {exception}").GetAwaiter().GetResult();
        else
            _logger.LogAsync(LogLevel.Error,$"Something went wrong. Non-Exception object: \n{e.ExceptionObject}").GetAwaiter().GetResult();
        
        if(_cancellationTokenProvider is { IsDisposed: false, Token.IsCancellationRequested: false })
            _cancellationTokenProvider.Cancel();
    }
}