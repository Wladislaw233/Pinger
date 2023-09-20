using Services.Interfaces;

namespace Services;

public class CancellationTokenProvider : ICancellationTokenProvider
{
    private readonly CancellationTokenSource  _cancellationTokenProvider = new();

    public CancellationToken Token => _cancellationTokenProvider.Token;

    public void Cancel()
    {
        _cancellationTokenProvider.Cancel();
    }

    public void Dispose()
    {
        _cancellationTokenProvider.Dispose();
    }
}