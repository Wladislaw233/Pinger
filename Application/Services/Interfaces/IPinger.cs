using Models;

namespace Services.Interfaces;

public interface IPinger
{
    void SetConfig<T>(T config) where T : ProtocolConfig;
    
    Task<PingResult> Ping();
}