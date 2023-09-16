using System.Net.NetworkInformation;
using System.Net.Sockets;
using Models;
using Serilog;
using Services.Interfaces;

namespace Services;

public class PingService : IPingService
{
    private readonly IConfigService _configService;
    private readonly ILogger _logger;
    private Config? _config;
    
    public PingService(IConfigService configService, ILogger logger)
    {
        _configService = configService;
        _logger = logger;
    }

    public async Task StartPingTests()
    {
        while (true)
        {
            _config = _configService.GetConfig();

            var icmpPingResult = await PingIcmp();

            var httpPingResult = await PingHttp();
            
            var tcpPingResult = await PingTcp();
            
            _logger.Information(icmpPingResult.ToString());
            
            _logger.Information(httpPingResult.ToString());
            
            _logger.Information(tcpPingResult.ToString());
            
            await Task.Delay(60000);
        }
    }

    private async Task<PingResult> PingIcmp()
    {
        var ping = new Ping();
        var reply = await ping.SendPingAsync(_config.Icmp.Host, _config.Icmp.Timeout);

        return new PingResult()
        {
            Host = _config.Icmp.Host,
            Status = reply.Status == IPStatus.Success ? "OK": "FAILED"
        };
    }

    private async Task<PingResult> PingHttp()
    {
        using var httpClient = new HttpClient();
        
        var message = await httpClient.GetAsync(_config.Http.Url);
        
        return new PingResult()
        {
            Host = _config.Http.Url,
            Status = (int)message.StatusCode == _config.Http.StatusCode ? "OK" : "FAILED"
        };
    }

    private async Task<PingResult> PingTcp()
    {
        using var tcpClient = new TcpClient();

        var isConnected = false;
        
        try
        {
            await tcpClient.ConnectAsync(_config.Tcp.Host, _config.Tcp.Port);
            isConnected = true;
        }
        catch (Exception e)
        {
            // ignored
        }

        return new PingResult()
        {
            Host = $"{_config.Tcp.Host}:{_config.Tcp.Port}",
            Status = isConnected ? "OK" : "FAILED"
        };
    }
}