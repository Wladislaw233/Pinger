using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Services;
using Services.Interfaces;

namespace Pinger;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += ExceptionHandlingService.UnhandledExceptionHandler;
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ping.log"),
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        using var host = CreateHostBuilder(args).Build();
        
        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            var pingService = services.GetRequiredService<IPingService>();
            
            await pingService.StartPingTests();
        }
        
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IConfigService>(_ =>
                {
                    const string configFileName = "configuration.json";

                    return new ConfigService(AppDomain.CurrentDomain.BaseDirectory, configFileName);
                });
                services.AddSingleton<ILogger>(_ => Log.Logger);
                services.AddTransient<IPingService, PingService>();
            });
    }
}