using Services.Interfaces;

namespace Services;

public class CancellationTokenProvider : ICancellationTokenProvider
{
    private readonly CancellationTokenSource  _cancellationTokenProvider = new();

    public bool IsDisposed { get; set; } 
    
    public CancellationToken Token => _cancellationTokenProvider.Token;

    public void Cancel()
    {
        _cancellationTokenProvider.Cancel();
    }

    public void Dispose()
    {
        IsDisposed = true;
        _cancellationTokenProvider.Dispose();
    }
}