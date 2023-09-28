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

    public Configs GetConfigs()
    {
        var configs = _configuration.Get<Configs>();
        return configs ?? throw new InvalidOperationException($"Configs not found.");
    }
}