namespace EntryLog.Entities.Entities;

public class Location
{
    public string Latitude { get; set; } = "";
    public string Longitude { get; set; } = "";
    public string? ApproximateAddress { get; set; }
    public string IpAddress { get; set; } = "";
}