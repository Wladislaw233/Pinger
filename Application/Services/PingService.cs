﻿using Models;
using Serilog;
using Services.Interfaces;
using Services.Pingers;

// ReSharper disable FunctionNeverReturns

namespace Services;

public class PingService : IPingService
{
    private readonly IConfigService _configService;
    private readonly ILogger _logger;
    private Config? _config;

    public PingService(IConfigService configService, ILogger logger)
    {
        _configService = configService;
        _logger = logger;
    }

    public async Task StartPingersTests()
    {
        _config = _configService.GetConfig();

        var tasks = new List<Task>
        {
            PingHttp(),
            PingIcmp(),
            PingTcp()
        };

        await Task.WhenAll(tasks);
    }

    private async Task PingHttp()
    {
        if (_config == null)
            return;

        var tasks = _config.HttpConfigs.Select(async httpConfig =>
        {
            var httpPinger = new HttpPinger(httpConfig);

            while (true)
            {
                await PingAndLogResult(httpPinger);
                await Task.Delay(httpConfig.PingInterval);
            }
        });

        await Task.WhenAll(tasks);
    }

    private async Task PingIcmp()
    {
        if (_config == null)
            return;

        var tasks = _config.IcmpConfigs.Select(async icmpConfig =>
        {
            var icmpPinger = new IcmpPinger(icmpConfig);

            while (true)
            {
                await PingAndLogResult(icmpPinger);
                await Task.Delay(icmpConfig.PingInterval);
            }
        });

        await Task.WhenAll(tasks);
    }

    private async Task PingTcp()
    {
        if (_config == null)
            return;

        var tasks = _config.TcpConfigs.Select(async tcpConfig =>
        {
            var tcpPinger = new TcpPinger(tcpConfig);
            while (true)
            {
                await PingAndLogResult(tcpPinger);
                await Task.Delay(tcpConfig.PingInterval);
            }
        });

        await Task.WhenAll(tasks);
    }

    private async Task PingAndLogResult<T>(T pinger) where T : IPinger
    {
        try
        {
            var pingResult = await pinger.Ping();
            _logger.Information(pingResult.ToString());
        }
        catch (Exception e)
        {
            _logger.Error(e, "Something went wrong.");
            throw;
        }
    }
}