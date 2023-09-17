using Models;
using Models.ProtocolConfigs;
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

        return new PingResult()
        {
            HostUrl = _httpConfig.HostUrl,
            Protocol = Protocol,
            Status = (int)message.StatusCode == 200
        };
    }
}