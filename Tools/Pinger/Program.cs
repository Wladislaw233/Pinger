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

        AppDomain.CurrentDomain.UnhandledException += ExceptionHandler;

        var cancellationTokenProvider = services.GetRequiredService<ICancellationTokenProvider>();

        var token = cancellationTokenProvider.Token;

        host.RunAsync(token);

        var pingService = services.GetRequiredService<IPingService>();

        await pingService.StartPingers();

        return;

        async void ExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            await exceptionHandlerService.UnhandledExceptionHandler(e);
        }
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
            .ConfigureServices((_, services) => { services.AddServices(); });
    }
}