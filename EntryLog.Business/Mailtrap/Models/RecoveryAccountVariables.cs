using System.Text.Json.Serialization;

namespace EntryLog.Business.Mailtrap.Models;

public class RecoveryAccountVariables
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    
    [JsonPropertyName("url")]
    public string Url { get; set; } = "";
}