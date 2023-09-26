using Models;
using Services.Interfaces;
using Services.Logger;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

namespace Services;

public class PingService : IPingService
{
    private readonly ILogger _logger;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    private readonly IPingerFactory _pingerFactory;

    public PingService(ILogger logger, ICancellationTokenProvider cancellationTokenProvider,
        IPingerFactory pingerFactory)
    {
        _logger = logger;
        _cancellationTokenProvider = cancellationTokenProvider;
        _pingerFactory = pingerFactory;
    }

    public async Task StartPingers()
    {
        var configPingers = _pingerFactory.GetConfigPingers();

        foreach (var configPinger in configPingers)
        {
            PingAndLogResult(configPinger.Key, configPinger.Value);
        }

        await Cancel();
    }

    private async Task PingAndLogResult(ProtocolConfig config, IPinger pinger)
    {
        while (true)
        {
            _cancellationTokenProvider.Token.ThrowIfCancellationRequested();

            PingResult pingResult;

            try
            {
                pingResult = await pinger.Ping();
            }
            catch (Exception e)
            {
                await _logger.LogAsync(LogLevel.Error,$"Pinger: {pinger.GetType()}, {config}.\n{e}");
                continue;
            }

            await _logger.LogAsync(LogLevel.Information,pingResult.ToString());

            await Task.Delay(config.PingInterval);
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private async Task Cancel()
    {
        Console.WriteLine("Press any key for stop pingers.");

        await Task.Run(() => Console.ReadKey(true));

        Console.WriteLine("Pingers is shutting down...");

        _cancellationTokenProvider.Cancel();
    }
}