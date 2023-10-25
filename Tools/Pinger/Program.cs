using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pinger.Extensions;
using Services.Interfaces;
using Services.Logger;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

namespace Pinger;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();

        using var serviceScope = host.Services.CreateScope();

        var services = serviceScope.ServiceProvider;

        var exceptionHandlerService = services.GetRequiredService<IExceptionHandler>();
        
        // Add global exception handler.
        AppDomain.CurrentDomain.UnhandledException += (_,e) => exceptionHandlerService.UnhandledExceptionHandler(e);
        
        var pingService = services.GetRequiredService<IPingService>();
        
        // Running host and pingers.
        host.RunAsync();
        
        await pingService.StartPingers();

        await host.StopAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseLogger(builder =>
            {
                builder.LogToConsole()
                    .LogToFile(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."), "ping.log");
            })
            .ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."))
                    .AddJsonFile("appsettings.json", false, true);
            })
            .ConfigureServices((_, services) =>
            {
                services.AddServices();
            });
    }
}