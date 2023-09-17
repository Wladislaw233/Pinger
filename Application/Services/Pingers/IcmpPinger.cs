using System.Net.NetworkInformation;
using Models;
using Models.ProtocolConfigs;
using Services.Interfaces;

namespace Services.Pingers;

public class IcmpPinger : IPinger
{
    public string Protocol => "ICMP";

    private readonly IcmpConfig _icmpConfig;

    public IcmpPinger(IcmpConfig icmpConfig)
    {
        _icmpConfig = icmpConfig;
    }

    public async Task<PingResult> Ping()
    {
        var ping = new Ping();
        var reply = await ping.SendPingAsync(_icmpConfig.HostUrl, _icmpConfig.Timeout);

        return new PingResult()
        {
            HostUrl = _icmpConfig.HostUrl,
            Protocol = Protocol,
            Status = reply.Status == IPStatus.Success
        };
    }
}