using Models;

namespace Services.Interfaces;

public interface IPinger
{
    IPinger SetConfig<T>(T config) where T : ProtocolConfig;
    
    Task<PingResult> Ping();
}