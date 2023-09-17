using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Services;
using Services.Interfaces;

namespace PingService.Tests;

public class PingServiceTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    
    public PingServiceTests()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.ping.log"),
                rollingInterval: RollingInterval.Day)
            .CreateLogger();
        
        var services = new ServiceCollection();
        services.AddSingleton<IConfigService>(_ =>
        {
            const string configFileName = "test.configuration.json";

            return new ConfigService(AppDomain.CurrentDomain.BaseDirectory, configFileName);
        });
        services.AddSingleton<ILogger>(_ => Log.Logger);
        services.AddTransient<IPingService, Services.PingService>();
        
        _serviceProvider = services.BuildServiceProvider();
        
        _logger = _serviceProvider.GetRequiredService<ILogger>();
    }
}