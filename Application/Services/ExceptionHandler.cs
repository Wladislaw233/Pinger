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
    
    public async Task UnhandledExceptionHandler(UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
            await _logger.LogAsync(LogLevel.Error,$"Something went wrong. \n {exception}");
        else
            await _logger.LogAsync(LogLevel.Error,$"Something went wrong. Non-Exception object: \n{e.ExceptionObject}");
        
        if(_cancellationTokenProvider is { IsDisposed: false, Token.IsCancellationRequested: false })
            _cancellationTokenProvider.Cancel();
    }
}