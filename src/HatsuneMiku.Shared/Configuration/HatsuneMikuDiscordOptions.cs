using DSharpPlus;
using System.ComponentModel.DataAnnotations;

namespace HatsuneMiku.Shared.Configuration;

public sealed class HatsuneMikuDiscordOptions
{
    public required string Token { get; init; }
    [EnumDataType(typeof(DiscordIntents[]))]
    public required DiscordIntents[] Intents { get; init; }
    public required string[] CommandPrefixes { get; init; }
}