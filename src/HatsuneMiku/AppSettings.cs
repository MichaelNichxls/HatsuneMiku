using System.Text.Json.Serialization;

namespace HatsuneMiku;

// readonly?
public struct AppSettings
{
    // InternalsVisibleTo
    [JsonPropertyName("token")]
    public string Token { get; init; }

    [JsonPropertyName("prefix")]
    public string Prefix { get; init; }
}