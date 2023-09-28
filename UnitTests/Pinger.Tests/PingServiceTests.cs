using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.ProtocolsConfig;
using Services;
using Services.Interfaces;
using Services.Logger;
using Services.Pingers;

namespace Pinger.Tests;

public class PingServiceTests
{
    /*private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    
    public PingServiceTests()
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
        
        services.AddSingleton<IConfiguration>(configuration);
        services.AddScoped<IConfigService, ConfigService>();

        ILoggerBuilder loggerBuilder = new LoggerBuilder();
        loggerBuilder.LogToConsole();
        loggerBuilder.LogToFile(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."), "ping.log");
        var logger = loggerBuilder.Build();
        
        services.AddSingleton<ILogger>(logger);
        services.AddTransient<IPinger, HttpPinger>();
        services.AddTransient<IPinger, TcpPinger>();
        services.AddTransient<IPinger, IcmpPinger>();
        
        _serviceProvider = services.BuildServiceProvider();
        
        _logger = _serviceProvider.GetRequiredService<ILogger>();
    }

    [Fact]
    public async Task HttpPinger_Ping_Success()
    {
        await PingerTest<HttpConfig>(typeof(HttpPinger), "Http");
    }
    
    [Fact]
    public async Task TspPinger_Ping_Success()
    {
        await PingerTest<TcpConfig>(typeof(TcpPinger), "Tcp");
    }
    
    [Fact]
    public async Task IcmpPinger_Ping_Success()
    {
        await PingerTest<IcmpConfig>(typeof(IcmpPinger), "Icmp");
    }
    
    private async Task PingerTest<T>(Type pingerType, string configName) where T : ProtocolConfig
    {
        //Arrange
        var configService = _serviceProvider.GetRequiredService<IConfigService>();
        var configs = configService.GetConfigs();
        
        var pinger = _serviceProvider.GetRequiredService<IEnumerable<IPinger>>().FirstOrDefault(pinger => pinger.GetType() == pingerType);
        var pingResults = new List<PingResult>();
        
        //Act
        if (pinger != null)
        {
            foreach (var config in configs.Http)
            {
                pinger.SetConfig(config);
                var result = await pinger.Ping();
                pingResults.Add(result);
                await _logger.LogAsync(LogLevel.Information, result.ToString());
            }
        }
        
        //Assert
        Assert.True(pinger != null && pingResults.All(result => result.Status));
    }*/
}