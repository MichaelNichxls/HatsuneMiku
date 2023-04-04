using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands;

[RequireGuild]
//[Category("Miscellaneous")]
public sealed class BonkCommand : BaseCommandModule
{
    // Make configurable
    // Suppress "Make static"
    // Change visibility
    public static DateTimeOffset MemberTimeout => DateTimeOffset.UtcNow.AddSeconds(10);

    // Localize command name and description
    // Relocate link or use file
    // Disallow timeout if member in question is in vc
    // Add permissions
    [Command]
    [Description("Bonks the horny")]
    [Cooldown(3, 60, CooldownBucketType.User)]
    public async Task BonkAsync(CommandContext ctx, DiscordMember member)
    {
        try
        {
            await member.TimeoutAsync(MemberTimeout, "Horny").ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync($"Bonk, {member.Mention}").ConfigureAwait(false);
        }
        finally
        {
            await ctx.Channel.SendMessageAsync("https://i.imgur.com/w2rfJnr.png").ConfigureAwait(false);
        }
    }
}