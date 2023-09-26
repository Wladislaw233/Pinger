using Models;

namespace Services.Interfaces;

public interface IConfigService
{
    List<T> GetConfigs<T>(string configName) where T : ProtocolConfig;
}