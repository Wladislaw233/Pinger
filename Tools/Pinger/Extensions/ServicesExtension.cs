using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Interfaces;

namespace Pinger.Extensions;

public static class ServicesExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IExceptionHandler, ExceptionHandler>();
        services.AddSingleton<ILogger>(new Logger(Path.Combine(Directory.GetCurrentDirectory(), "ping.log")));
        services.AddSingleton<ICancellationTokenProvider, CancellationTokenProvider>();
        services.AddScoped<IPingService, PingService>();
        services.AddScoped<IConfigService, ConfigService>();
    }
}