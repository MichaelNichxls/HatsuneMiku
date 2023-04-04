namespace HatsuneMiku.Shared.Configuration;

public sealed class HatsuneMikuLavalinkConnectionEndpointOptions
{
    public required int Port { get; init; }
    public required string Address { get; init; }
}