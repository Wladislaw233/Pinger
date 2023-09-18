using Models;
using Models.ProtocolsConfig;
using Services.Interfaces;

namespace Services.Pingers;

public class HttpPinger : IPinger
{
    public string Protocol => "HTTP";
    
    private readonly HttpConfig _httpConfig;

    public HttpPinger(HttpConfig httpConfigs)
    {
        _httpConfig = httpConfigs;
    }
    
    public async Task<PingResult> Ping()
    {
        using var httpClient = new HttpClient(); 
        
        var message = await httpClient.GetAsync(_httpConfig.HostUrl);

        var status = (int)message.StatusCode == _httpConfig.StatusCode;
        
        return new PingResult()
        {
            HostUrl = _httpConfig.HostUrl,
            Protocol = Protocol,
            Status = status
        };
    }
}