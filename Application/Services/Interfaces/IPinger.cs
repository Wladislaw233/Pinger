using Models;

namespace Services.Interfaces;

public interface IPinger
{
    string Protocol { get; }
    Task<PingResult> Ping();
}