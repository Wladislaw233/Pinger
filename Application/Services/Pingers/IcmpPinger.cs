using System.Net.NetworkInformation;
using Models;
using Models.ProtocolsConfig;
using Services.Interfaces;

namespace Services.Pingers;

public class IcmpPinger : IPinger
{
    private readonly IcmpConfig _icmpConfig;

    public IcmpPinger(IcmpConfig icmpConfig)
    {
        _icmpConfig = icmpConfig ?? throw new ArgumentNullException(nameof(icmpConfig));
    }
    
    public ProtocolConfig Config => _icmpConfig;
    
    public async Task<PingResult> Ping()
    {
        using var ping = new Ping();
        
        var reply = await ping.SendPingAsync(_icmpConfig.HostUrl, _icmpConfig.Timeout);

        var status = reply.Status == IPStatus.Success;
        
        return new PingResult()
        {
            HostUrl = _icmpConfig.HostUrl,
            Protocol = "ICMP",
            Status = status
        };
    }
}