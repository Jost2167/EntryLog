using System.Text.Json.Serialization;

namespace EntryLog.Business.DTOs;

internal class ImageBbResponseDTO
{
    [JsonPropertyName("data")]
    public Data Data { get; set; } = default!;

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }
}

internal class Data
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonPropertyName("url_viewer")]
    public string UrlViewer { get; set; } = "";

    [JsonPropertyName("url")]
    public string Url { get; set; } = "";

    [JsonPropertyName("display_url")]
    public string DisplayUrl { get; set; } = "";

    [JsonPropertyName("width")]
    public string Width { get; set; } = "";

    [JsonPropertyName("height")]
    public string Height { get; set; } = "";

    [JsonPropertyName("size")]
    public string Size { get; set; } = "";

    [JsonPropertyName("time")]
    public string Time { get; set; } = "";

    [JsonPropertyName("expiration")]
    public string Expiration { get; set; } = "";

    [JsonPropertyName("image")]
    public MediaFile Image { get; set; } = default!;

    [JsonPropertyName("thumb")]
    public MediaFile Thumb { get; set; } = default!;

    [JsonPropertyName("medium")]
    public MediaFile Medium { get; set; } = default!;

    [JsonPropertyName("delete_url")]
    public string DeleteUrl { get; set; } = "";
}

internal class MediaFile
{
    [JsonPropertyName("filename")]
    public string Filename { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("mime")]
    public string Mime { get; set; } = "";

    [JsonPropertyName("extension")]
    public string Extension { get; set; } = "";

    [JsonPropertyName("url")]
    public string Url { get; set; } = ""; 
}


