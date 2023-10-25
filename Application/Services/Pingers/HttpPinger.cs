using Models;
using Models.ProtocolsConfig;
using Services.Interfaces;

namespace Services.Pingers;

public class HttpPinger : IPinger
{
    private readonly HttpConfig _httpConfig;

    public HttpPinger(HttpConfig httpConfig)
    {
        _httpConfig = httpConfig ?? throw new ArgumentNullException(nameof(httpConfig));
    }
    
    public ProtocolConfig Config => _httpConfig;
    
    public async Task<PingResult> Ping()
    {
        using var httpClient = new HttpClient();

        var message = await httpClient.GetAsync(_httpConfig.HostUrl);
        
        var status = (int)message.StatusCode == _httpConfig.StatusCode;

        return new PingResult()
        {
            HostUrl = _httpConfig.HostUrl,
            Protocol = "HTTP",
            Status = status
        };
    }
}