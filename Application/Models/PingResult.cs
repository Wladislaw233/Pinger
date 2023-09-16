namespace Models;

public class PingResult
{
    public DateTime DateTimeInspection = DateTime.Now;
    public string Host { get; init; }
    public string Status { get; init; }

    public override string ToString()
    {
        return $"{DateTimeInspection:G} {Host} {Status}";
    }
}