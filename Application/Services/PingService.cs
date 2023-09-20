using Models;
using Models.ProtocolsConfig;
using Services.Interfaces;
using Services.Pingers;

namespace Services;

public class PingService : IPingService
{
    private readonly ILogger _logger;
    private readonly IEnumerable<IPinger> _pingers;
    private readonly IConfigService _configService;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;

    public PingService(ILogger logger, IEnumerable<IPinger> pingers, IConfigService configService,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        _logger = logger;
        _pingers = pingers;
        _configService = configService;
        _cancellationTokenProvider = cancellationTokenProvider;
    }

    public async Task StartPingersTests()
    {
        foreach (var pinger in _pingers)
        {
            var pingerTask = pinger switch
            {
                HttpPinger => PingAndLogResult<HttpConfig>(pinger, "HttpConfig"),
                IcmpPinger => PingAndLogResult<IcmpConfig>(pinger, "IcmpConfig"),
                TcpPinger => PingAndLogResult<TcpConfig>(pinger, "TcpConfig"),
                _ => null
            };

            if (pingerTask == null)
            {
                await _logger.LogWarningAsync("Perhaps we forgot to connect one of the pinger in the PingerService service.");
            }
        }
        
        await Cancel();
    }

    private async Task PingAndLogResult<T>(IPinger pinger, string configName) where T : ProtocolConfig
    {
        try
        {
            var config = _configService.GetConfig<T>(configName);

            while (true)
            {
                _cancellationTokenProvider.Token.ThrowIfCancellationRequested();

                pinger.SetConfig(config);

                var pingResult = await pinger.Ping();

                await _logger.LogInfoAsync(pingResult.ToString());

                await Task.Delay(config.PingInterval);
            }
        }
        catch (Exception exc)
        {
            await _logger.LogErrorAsync(exc.ToString());
            _cancellationTokenProvider.Cancel();
        }
    }

    private async Task Cancel()
    {
        Console.WriteLine("Press any key for stop pingers.");

        await Task.Run(() => Console.ReadKey(true));

        Console.WriteLine("Pingers is shutting down...");

        _cancellationTokenProvider.Cancel();
    }
}