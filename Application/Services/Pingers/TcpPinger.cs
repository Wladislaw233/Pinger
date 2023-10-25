using System.Net.Sockets;
using Models;
using Models.ProtocolsConfig;
using Services.Interfaces;

namespace Services.Pingers;

public class TcpPinger : IPinger
{
    private readonly TcpConfig _tcpConfig;
    
    public TcpPinger(TcpConfig tcpConfig)
    {
        _tcpConfig = tcpConfig ?? throw new ArgumentNullException(nameof(tcpConfig));
    }
    
    public ProtocolConfig Config => _tcpConfig;
    
    public async Task<PingResult> Ping()
    {
        using var tcpClient = new TcpClient();

        await tcpClient.ConnectAsync(_tcpConfig.HostUrl, _tcpConfig.Port);
        
        return new PingResult
        {
            HostUrl = $"{_tcpConfig.HostUrl}:{_tcpConfig.Port}",
            Protocol = "TCP",
            Status = true
        };
    }
}