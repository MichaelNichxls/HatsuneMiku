using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using System;
using System.Threading.Tasks;

namespace HatsuneMiku.SlashCommands;

// Source generator? lol
// Rename to XCommandModule
// Relocate?
//[SlashCommandGroup]
[SlashRequireGuild]
public class BonkSlashCommand : ApplicationCommandModule
{
    private static DateTimeOffset TimeoutUntil => DateTimeOffset.UtcNow.AddSeconds(10);

    // Suppress "Make static"
    // Relocate link
    //[SlashCommandPermissions]
    [SlashCommand("bonk", "Bonks the horny")]
    [SlashCooldown(3, 60, SlashCooldownBucketType.User)]
    public async Task Bonk(InteractionContext ctx, [Option("user", "User to bonk")] DiscordUser user)
    {
        DiscordMember member = await ctx.Guild.GetMemberAsync(user.Id).ConfigureAwait(false);

        try
        {
            await member.TimeoutAsync(TimeoutUntil, "Horny").ConfigureAwait(false);
        }
        finally
        {
            // Throws if a role is mentioned instead of a user
            await ctx.CreateResponseAsync($"Bonk, {member.Mention}", new DiscordEmbedBuilder().WithImageUrl("https://i.imgur.com/w2rfJnr.png")).ConfigureAwait(false);
        }
    }
}