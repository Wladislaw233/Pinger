using System.Net.Sockets;
using Models;
using Models.ProtocolConfigs;
using Services.Interfaces;

namespace Services.Pingers;

public class TcpPinger : IPinger
{
    public string Protocol => "TCP";
    
    private readonly TcpConfig _tcpConfig;

    public TcpPinger(TcpConfig tcpConfig)
    {
        _tcpConfig = tcpConfig;
    }

    public async Task<PingResult> Ping()
    {
        using var tcpClient = new TcpClient();
        {
            await tcpClient.ConnectAsync(_tcpConfig.HostUrl, _tcpConfig.Port);

            return new PingResult()
            {
                HostUrl = $"{_tcpConfig.HostUrl}:{_tcpConfig.Port}",
                Status = true,
                Protocol = Protocol
            };
        }
    }
}