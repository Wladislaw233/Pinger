using Models.ProtocolsConfig;

namespace Models;

public class Configs
{
    public List<HttpConfig> Http { get; set; }
    
    public List<TcpConfig> Tcp { get; set; }
    
    public List<IcmpConfig> Icmp { get; set; }
}