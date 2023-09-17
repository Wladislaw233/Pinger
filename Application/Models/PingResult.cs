namespace Models;

public class PingResult
{
    private readonly DateTime _dateTimeInspection = DateTime.Now;
    public string Protocol { get; init; }
    public string HostUrl { get; init; }
    public bool Status { get; init; }

    public override string ToString()
    {
        var status = Status ? "OK" : "FAILED";
        return $"{Protocol} {_dateTimeInspection:G} {HostUrl} {status}";
    }
}