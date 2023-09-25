using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pinger.Extensions;
using Services;
using Services.Interfaces;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

namespace Pinger;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();

        CancellationToken token;
        
        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;
            
            var exceptionHandler = services.GetRequiredService<IExceptionHandler>();

            async void Handler(object sender, UnhandledExceptionEventArgs e)
            {
                await exceptionHandler.UnhandledExceptionHandler(e);
            }
            
            AppDomain.CurrentDomain.UnhandledException += Handler;
            
            var cancellationTokenProvider = services.GetRequiredService<ICancellationTokenProvider>();

            token = cancellationTokenProvider.Token;
            
            var pingService = services.GetRequiredService<IPingService>();
            
            pingService.StartPingersTests();
        }
        
        await host.RunAsync(token);
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((_, services) =>
            {
                services.AddProtocolPingers();
                services.AddServices();
            });
    }
}