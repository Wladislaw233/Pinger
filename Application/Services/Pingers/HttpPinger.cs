using Models;
using Models.ProtocolsConfig;
using Services.Interfaces;

namespace Services.Pingers;

public class HttpPinger : IPinger
{
    public string ConfigName => "HttpConfig";
    
    private HttpConfig? _httpConfig;

    public void SetConfig<T>(T config) where T : ProtocolConfig
    {
        if (config is HttpConfig httpConfig)
            _httpConfig = httpConfig;
        else
            throw new ArgumentException($"Parameter is not a type HttpConfig.", nameof(config));
    }
    
    public async Task<PingResult> Ping()
    {
        if (_httpConfig == null)
            throw new InvalidOperationException($"{nameof(_httpConfig)} is null.");
        
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