using System.Net.NetworkInformation;
using System.Net.Sockets;
using Models;
using Serilog;
using Services.Interfaces;
// ReSharper disable FunctionNeverReturns

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
        _config ??= _configService.GetConfig();

        var tasks = new List<Task>
        {
            PingIcmp(),
            PingHttp(),
            PingTcp()
        };

        await Task.WhenAll(tasks);
    }

    public async Task PingIcmp()
    {
        _config ??= _configService.GetConfig();

        var tasks = _config.IcmpConfigs.Select(icmpConfig => Task.Run(async () =>
            {
                while (true)
                {
                    var ping = new Ping();
                    var reply = await ping.SendPingAsync(icmpConfig.HostUrl, icmpConfig.Timeout);
                    
                    GenerateAndLogPingResult("ICMP", icmpConfig.HostUrl, reply.Status == IPStatus.Success);
                    
                    await Task.Delay(icmpConfig.Period);
                }
            }))
            .ToList();

        await Task.WhenAll(tasks);
    }

    public async Task PingHttp()
    {
       _config ??= _configService.GetConfig();

       var tasks = _config.HttpConfigs.Select(httpConfig => Task.Run(async () =>
       {
           while (true)
           {
               using (var httpClient = new HttpClient())
               {
                   var message = await httpClient.GetAsync(httpConfig.HostUrl);

                   GenerateAndLogPingResult("HTTP", httpConfig.HostUrl,
                       (int)message.StatusCode == httpConfig.StatusCode);
               }
               await Task.Delay(httpConfig.Period);
           }
       })).ToList();
       
       await Task.WhenAll(tasks);
    }

    public async Task PingTcp()
    {
        _config ??= _configService.GetConfig();
        
        var tasks = _config.TcpConfigs.Select(tcpConfig => Task.Run(async () =>
        {
            while (true)
            {
                using var tcpClient = new TcpClient();
                {
                    await tcpClient.ConnectAsync(tcpConfig.HostUrl, tcpConfig.Port);

                    GenerateAndLogPingResult("TCP", tcpConfig.HostUrl, true);
                }
                await Task.Delay(tcpConfig.Period);
            }
        })).ToList();
       
        await Task.WhenAll(tasks);
    }

    private void GenerateAndLogPingResult(string protocol, string hostUrl, bool status)
    {
        var result = new PingResult()
        {
            Protocol = protocol,
            HostUrl = hostUrl,
            Status = status
        };
        
        _logger.Information(result.ToString());
    }
}