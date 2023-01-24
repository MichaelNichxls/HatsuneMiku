using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands;

// Rename to XCommandModule
//[Category]
[RequireGuild]
public class BonkCommand : BaseCommandModule
{
    // Make configurable
    private static DateTimeOffset TimeoutUntil => DateTimeOffset.UtcNow.AddSeconds(10);

    // XAsync
    // Make configurable
    // Localize command name and description
    // Suppress "Make static"
    // Use title?
    // Relocate link
    // Disallow timeout if member in question is in vc
    [Command("bonk")]
    [Description("Bonks the horny")]
    [Cooldown(3, 60, CooldownBucketType.User)]
    public async Task Bonk(CommandContext ctx, DiscordMember member)
    {
        try
        {
            await member.TimeoutAsync(TimeoutUntil, "Horny").ConfigureAwait(false);
        }
        finally
        {
            await ctx.Channel.SendMessageAsync($"Bonk, {member.Mention}").ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync("https://i.imgur.com/w2rfJnr.png").ConfigureAwait(false);
        }
    }
}