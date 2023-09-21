using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.ProtocolsConfig;
using Services;
using Services.Interfaces;
using Services.Pingers;

namespace Pinger.Tests;

public class PingServiceTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    
    public PingServiceTests()
    {
        
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
        
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<ILogger>(new Logger(Path.Combine(Directory.GetCurrentDirectory(), "test.ping.log")));
        services.AddScoped<IConfigService, ConfigService>();
        
        services.AddTransient<IPinger, HttpPinger>();
        services.AddTransient<IPinger, TcpPinger>();
        services.AddTransient<IPinger, IcmpPinger>();
        
        _serviceProvider = services.BuildServiceProvider();
        
        _logger = _serviceProvider.GetRequiredService<ILogger>();
    }

    [Fact]
    public async Task HttpPinger_Ping_Success()
    {
        await PingerTest<HttpConfig>(typeof(HttpPinger), "HttpConfig");
    }
    
    [Fact]
    public async Task TspPinger_Ping_Success()
    {
        await PingerTest<TcpConfig>(typeof(TcpPinger), "TcpConfig");
    }
    
    [Fact]
    public async Task IcmpPinger_Ping_Success()
    {
        await PingerTest<IcmpConfig>(typeof(IcmpPinger), "IcmpConfig");
    }
    
    private async Task PingerTest<T>(Type pingerType, string configName) where T : ProtocolConfig
    {
        //Arrange
        var configService = _serviceProvider.GetRequiredService<IConfigService>();
        var config = configService.GetConfig<T>(configName);
        
        var pinger = _serviceProvider.GetRequiredService<IEnumerable<IPinger>>().FirstOrDefault(pinger => pinger.GetType() == pingerType);
        PingResult result = new();
        
        //Act
        if (pinger != null)
        {
            pinger.SetConfig(config);
            result = await pinger.Ping();
            await _logger.LogInfoAsync(result.ToString());
        }
        
        //Assert
        Assert.True(pinger != null && result.Status);
    }
}