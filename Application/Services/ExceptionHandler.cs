using System;
using Services.Interfaces;

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
            await _logger.LogErrorAsync($"Something went wrong. \n {exception}");
        else
            await _logger.LogErrorAsync($"Something went wrong. Non-Exception object: \n{e.ExceptionObject}");
        
        if(!_cancellationTokenProvider.Token.IsCancellationRequested)
            _cancellationTokenProvider.Cancel();
    }
}