using Models;

namespace Services.Interfaces;

public interface IPingerFactory
{
    Dictionary<ProtocolConfig, IPinger> GetConfigPingers();
}