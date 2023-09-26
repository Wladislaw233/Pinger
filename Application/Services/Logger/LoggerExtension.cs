using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Services.Logger;

public static class LoggerExtension
{
    public static IHostBuilder UseLogger(this IHostBuilder hostBuilder, Action<ILoggerBuilder> configureBuilder)
    {
        ILoggerBuilder builder = new LoggerBuilder();
        configureBuilder(builder);
        var logger = builder.Build();

        hostBuilder.ConfigureServices((_, services) => services.AddSingleton<ILogger>(logger));

        return hostBuilder;
    }
}