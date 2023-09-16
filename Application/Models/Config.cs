namespace Models;

public class Config
{
    public HttpConfig Http { get; set; }
    public IcmpConfig Icmp { get; set; }
    public TcpConfig Tcp { get; set; }
}