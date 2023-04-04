namespace HatsuneMiku.Shared.Configuration;

public sealed class HatsuneMikuLavalinkOptions
{
    public required HatsuneMikuLavalinkConnectionEndpointOptions ConnectionEndpoint { get; init; }
    public required string Password { get; init; }
}