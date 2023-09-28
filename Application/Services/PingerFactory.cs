using Models;
using Services.Interfaces;
using Services.Pingers;

namespace Services;

public class PingerFactory : IPingerFactory
{
    private readonly IConfigService _configService;
    
    private Configs? _configs;

    public PingerFactory(IConfigService configService)
    {
        _configService = configService;
    }

    public Dictionary<ProtocolConfig, IPinger> GetConfigPingers()
    {
        _configs = _configService.GetConfigs();

        var configPingers = new Dictionary<ProtocolConfig, IPinger>();

        return Enum.GetValues<PingerEnum>()
            .Select(CreateConfigPingers)
            .Aggregate(configPingers, (current, createdConfigPingers) => current.Concat(createdConfigPingers)
                .ToDictionary(pair => pair.Key, pair => pair.Value));
    }

    private Dictionary<ProtocolConfig, IPinger> CreateConfigPingers(PingerEnum pingerEnum)
    {
        if (_configs == null)
            throw new ArgumentNullException(nameof(_configs), "Protocol configs is null.");
        
        var dictElements = pingerEnum switch
        {
            PingerEnum.Tcp => _configs.Tcp.Select(config =>
                new KeyValuePair<ProtocolConfig, IPinger>(config, new TcpPinger().SetConfig(config))),

            PingerEnum.Icmp => _configs.Icmp.Select(config =>
                new KeyValuePair<ProtocolConfig, IPinger>(config, new IcmpPinger().SetConfig(config))),

            PingerEnum.Http => _configs.Http.Select(config =>
                new KeyValuePair<ProtocolConfig, IPinger>(config, new HttpPinger().SetConfig(config))),

            _ => throw new ArgumentOutOfRangeException(nameof(pingerEnum),
                "Perhaps you forgot to connect one of the pingers in the factory.")
        };

        return dictElements.ToDictionary(pair => pair.Key, pair => pair.Value);
    }
}