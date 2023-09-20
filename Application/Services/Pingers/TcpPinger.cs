using System.Net.Sockets;
using Models;
using Models.ProtocolsConfig;
using Services.Interfaces;

namespace Services.Pingers;

public class TcpPinger : IPinger
{
    private TcpConfig? _tcpConfig;

    public void SetConfig<T>(T config) where T : ProtocolConfig
    {
        if (config is TcpConfig tcpConfig)
            _tcpConfig = tcpConfig;
        else
            throw new ArgumentException($"Parameter is not a type HttpConfig.", nameof(config));
    }
    
    public async Task<PingResult> Ping()
    {
        if (_tcpConfig == null)
            throw new ArgumentNullException($"Tcp configuration not set - {nameof(_tcpConfig)}");
        
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