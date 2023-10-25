using System.Collections.ObjectModel;
using Models;
using Services.Interfaces;
using Services.Pingers;

namespace Services;

public class PingerFactory : IPingerFactory
{
    private readonly IConfigService _configService;
    
    private readonly Configs _configs = new();

    private readonly List<IPinger> _pingers = new();

    public PingerFactory(IConfigService configService)
    {
        _configService = configService;
    }

    public IEnumerable<IPinger> GetPingers()
    {
        _configService.SetConfigs(_configs);

        _pingers.AddRange(_configs.Tcp.Select(config => (IPinger)new TcpPinger(config)));
        
        _pingers.AddRange(_configs.Http.Select(config => (IPinger)new HttpPinger(config)));
        
        _pingers.AddRange(_configs.Icmp.Select(config => (IPinger)new IcmpPinger(config)));

        return _pingers;
    }
}