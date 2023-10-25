using Models;

namespace Services.Interfaces;

public interface IPinger
{
    ProtocolConfig Config { get; }
    
    Task<PingResult> Ping();
}