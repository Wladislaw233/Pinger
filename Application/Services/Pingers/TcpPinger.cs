using System.Net.Sockets;
using Models;
using Models.ProtocolsConfig;
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
        var status = false;

        try
        {
            using var tcpClient = new TcpClient();

            await tcpClient.ConnectAsync(_tcpConfig.HostUrl, _tcpConfig.Port);

            status = true;
        }
        catch (SocketException)
        {
            //ignored.
        }

        return new PingResult
        {
            HostUrl = $"{_tcpConfig.HostUrl}:{_tcpConfig.Port}",
            Status = status,
            Protocol = Protocol
        };
    }
}