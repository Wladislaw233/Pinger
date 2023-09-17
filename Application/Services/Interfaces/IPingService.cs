using Models;

namespace Services.Interfaces;

public interface IPingService
{
    Task StartPingTests();

    Task PingIcmp();
    
    Task PingHttp();
    
    Task PingTcp();
}