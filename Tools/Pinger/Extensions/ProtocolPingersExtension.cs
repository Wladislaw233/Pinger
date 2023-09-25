using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.ProtocolsConfig;
using Services;
using Services.Interfaces;
using Services.Pingers;

namespace Pinger.Extensions;

public static class ProtocolPingersExtension
{
    public static void AddProtocolPingers(this IServiceCollection services)
    {
        services.AddTransient<IPinger, HttpPinger>();
        services.AddTransient<IPinger, TcpPinger>();
        services.AddTransient<IPinger, IcmpPinger>();
    }
}