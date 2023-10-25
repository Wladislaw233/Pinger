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

    public void SetConfigs(Configs configs)
    {
        _configuration.Bind(configs);
    }
}