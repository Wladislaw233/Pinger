using Services.Interfaces;
using Services.Logger;

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
        var pingers = _pingerFactory.GetPingers();

        var pingersTasks = pingers.Select(PingAndLogResult).ToList();
        
        Cancel();

        await Task.WhenAll(pingersTasks);
    }

    private async Task PingAndLogResult(IPinger pinger)
    {
        while (true)
        {
            if (_cancellationTokenProvider.Token.IsCancellationRequested)
            {
                Console.WriteLine($"Pinger with config {pinger.Config} is shutting down.");
                break;
            }
            
            try
            {
                var pingResult = await pinger.Ping();
                await _logger.LogAsync(LogLevel.Information,pingResult.ToString());
            }
            catch (Exception e)
            {
                await _logger.LogAsync(LogLevel.Error, $"Pinger: {pinger.GetType()}, {pinger.Config}.\n{e}");
            }
            finally
            {
                await Task.Delay(pinger.Config.PingInterval);
            }
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