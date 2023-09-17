namespace Models;

public class Config
{
    public IEnumerable<HttpConfig> HttpConfigs { get; set; }
    public IEnumerable<IcmpConfig> IcmpConfigs { get; set; }
    public IEnumerable<TcpConfig> TcpConfigs { get; set; }
}