﻿namespace Services.Interfaces;

public interface ICancellationTokenProvider : IDisposable
{
    CancellationToken Token { get; }
    
    void Cancel();
}