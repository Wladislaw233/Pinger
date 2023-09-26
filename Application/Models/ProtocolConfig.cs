namespace Models;

public class ProtocolConfig
{
    public string HostUrl { get; set; }
    public int PingInterval { get; set; }

    public override string ToString()
    {
        return $"Url: {HostUrl}, ping interval: {PingInterval}";
    }
}