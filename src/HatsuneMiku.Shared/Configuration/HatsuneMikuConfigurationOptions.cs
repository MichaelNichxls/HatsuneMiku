namespace HatsuneMiku.Shared.Configuration;

public sealed class HatsuneMikuConfigurationOptions
{
    public const string SectionKey = "HatsuneMiku";

    // Some probably don't have to be required
    // Do same with Db entities
    // Add more validation
    public required HatsuneMikuDiscordOptions Discord { get; init; }
    public required HatsuneMikuLavalinkOptions Lavalink { get; init; }
    public required HatsuneMikuPersistenceOptions Persistence { get; init; }
    public required HatsuneMikuLoggingOptions Logging { get; init; }
}