using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Services.Logger;

public static class LoggerExtension
{
    public static IHostBuilder UseLogger(this IHostBuilder hostBuilder, Action<ILoggerBuilder> configureBuilder)
    {
        ILoggerBuilder builder = new LoggerBuilder();
        configureBuilder(builder);
        var loggers = builder.Build();
        
        hostBuilder.ConfigureServices((_, services) =>
        {
            foreach (var logger in loggers)
            {
                services.AddSingleton(logger);
            }
            
            services.AddSingleton<ILogger, Logger>();
        });

        return hostBuilder;
    }
}