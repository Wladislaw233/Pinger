using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Serilog;
using Services;
using Services.Interfaces;
using Services.Pingers;

namespace Pinger.Tests;

public class PingServiceTests
{
    /*private readonly IServiceProvider _serviceProvider;
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
        
        _serviceProvider = services.BuildServiceProvider();
        
        _logger = _serviceProvider.GetRequiredService<ILogger>();
    }

    [Fact]
    public async Task HttpPinger_Ping_Success()
    {
        //Arrange
        
        var configService = _serviceProvider.GetRequiredService<IConfigService>();
        var config = configService.GetConfig();
        var pingResults = new List<PingResult>();
        
        //Act

        var tasks = config.HttpConfigs.Select(httpConfig => Task.Run(async () =>
        {
            var httpPinger = new HttpPinger(httpConfig);
            
            var pingResult = await httpPinger.Ping();
            
            pingResults.Add(pingResult);
            
            _logger.Information(pingResult.ToString());
        })).ToList();

        await Task.WhenAll(tasks);
        
        //Assert
        
        Assert.DoesNotContain(pingResults, result => !result.Status);
    }
    
    [Fact]
    public async Task IcmpPinger_Ping_Success()
    {
        //Arrange
        
        var configService = _serviceProvider.GetRequiredService<IConfigService>();
        var config = configService.GetConfig();
        var pingResults = new List<PingResult>();
        
        //Act

        var tasks = config.IcmpConfigs.Select(icmpConfig => Task.Run(async () =>
        {
            var icmpPinger = new IcmpPinger(icmpConfig);
            
            var pingResult = await icmpPinger.Ping();
            
            pingResults.Add(pingResult);
            
            _logger.Information(pingResult.ToString());
        })).ToList();

        await Task.WhenAll(tasks);
        
        //Assert
        
        Assert.DoesNotContain(pingResults, result => !result.Status);
    }

    [Theory]
    [InlineData("x", 1)]
    [InlineData("y", 1)]
    [InlineData("z", 1)]
    [InlineData("6", 1)]
    public async Task TcpPinger_Ping_Success(string x, int y)
    {
        //Arrange
        
        var configService = _serviceProvider.GetRequiredService<IConfigService>();
        var config = configService.GetConfig();
        var pingResults = new List<PingResult>();
        
        //Act

        var tasks = config.TcpConfigs.Select(tcpConfig => Task.Run(async () =>
        {
            var tcpPinger = new TcpPinger(tcpConfig);
            
            var pingResult = await tcpPinger.Ping();
            
            pingResults.Add(pingResult);
            
            _logger.Information(pingResult.ToString());
        })).ToList();

        await Task.WhenAll(tasks);
        
        //Assert
        Assert.DoesNotContain(pingResults, result => !result.Status);
    }*/
}