using Microsoft.Extensions.Configuration;
using Models;
using Services.Interfaces;

namespace Services;

public class ConfigService : IConfigService
{
    private readonly IConfiguration _configuration;
    
    public ConfigService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public List<T> GetConfigs<T>(string configName) where T : ProtocolConfig
    {
        var configs = _configuration.GetSection(configName).Get<List<T>>();
        return configs ?? throw new InvalidOperationException($"{typeof(T)} not found.");
    }
}