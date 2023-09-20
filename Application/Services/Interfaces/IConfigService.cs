using Models;

namespace Services.Interfaces;

public interface IConfigService
{
    T GetConfig<T>(string configName) where T : ProtocolConfig;
}