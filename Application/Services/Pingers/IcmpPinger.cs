using System.Net.NetworkInformation;
using Models;
using Models.ProtocolsConfig;
using Services.Interfaces;

namespace Services.Pingers;

public class IcmpPinger : IPinger
{
    private IcmpConfig? _icmpConfig;
    
    public void SetConfig<T>(T config) where T : ProtocolConfig
    {
        if (config is IcmpConfig icmpConfig)
            _icmpConfig = icmpConfig;
        else
            throw new ArgumentException($"Parameter is not a type HttpConfig.", nameof(config));
    }
    
    public async Task<PingResult> Ping()
    {
        if (_icmpConfig == null)
            throw new InvalidOperationException($"{nameof(_icmpConfig)} is null.");
        
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