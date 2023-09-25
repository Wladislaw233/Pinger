namespace Services.Interfaces;

public interface ICancellationTokenProvider : IDisposable
{
    CancellationToken Token { get; }

    bool IsDisposed { get; set; }

    void Cancel();
}