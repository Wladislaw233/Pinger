using Models;
using Models.ProtocolsConfig;
using Services.Interfaces;
using Services.Logger;
using Services.Pingers;

namespace Services;

public class PingerFactory : IPingerFactory
{
    private readonly IConfigService _configService;

    private readonly ILogger _logger;

    public PingerFactory(IConfigService configService, ILogger logger)
    {
        _configService = configService;
        _logger = logger;
    }

    public Dictionary<ProtocolConfig, IPinger> GetConfigPingers()
    {
        var configPingers = new Dictionary<ProtocolConfig, IPinger>();
        
        foreach (PingerEnum pingerEnum in Enum.GetValues(typeof(PingerEnum)))
        {
            var addedConfigPingers = pingerEnum switch
            {
                PingerEnum.Http => CreateConfigPingers<HttpConfig>("Http", pingerEnum),
                PingerEnum.Icmp => CreateConfigPingers<IcmpConfig>("Icmp", pingerEnum),
                PingerEnum.Tcp => CreateConfigPingers<TcpConfig>("Tcp", pingerEnum),
                _ => null
            };

            if (addedConfigPingers != null)
            {
                configPingers = configPingers
                    .Concat(addedConfigPingers)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            else
            {
                _logger.LogAsync(LogLevel.Warning, "Perhaps you forgot to connect one of the pingers in the factory.");
            }
        }
        
        return configPingers;
    }
    
    private Dictionary<ProtocolConfig, IPinger> CreateConfigPingers<T>(string configName, PingerEnum pingerEnum) where T : ProtocolConfig
    {
        var configPingers = new Dictionary<ProtocolConfig, IPinger>();
        
        var configs = _configService.GetConfigs<T>(configName);

        foreach (var config in configs)
        {
            var pinger = CreatePinger(pingerEnum);
            
            pinger.SetConfig(config);
            
            configPingers.Add(config, pinger);
        }

        return configPingers;
    }
    
    private static IPinger CreatePinger(PingerEnum pingerEnum)
    {
        return pingerEnum switch
        {
            PingerEnum.Tcp => new TcpPinger(),
            PingerEnum.Icmp => new IcmpPinger(),
            PingerEnum.Http => new HttpPinger(),
            _ => throw new ArgumentOutOfRangeException(nameof(pingerEnum))
        };
    }
}