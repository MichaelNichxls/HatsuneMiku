using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using System;
using System.Threading.Tasks;

// Move
namespace HatsuneMiku.SlashCommands;

[SlashRequireGuild]
public sealed class BonkSlashCommand : ApplicationCommandModule
{
    public static DateTimeOffset MemberTimeout => DateTimeOffset.UtcNow.AddSeconds(10);

    // Suppress "Make static"
    // Relocate link
    //[SlashCommandPermissions]
    [SlashCommand("bonk", "Bonks the horny")]
    [SlashCooldown(3, 60, SlashCooldownBucketType.User)]
    public async Task BonkAsync(InteractionContext ctx, [Option("user", "User to bonk")] DiscordUser user)
    {
        DiscordMember member = await ctx.Guild.GetMemberAsync(user.Id).ConfigureAwait(false);

        try
        {
            await member.TimeoutAsync(MemberTimeout, "Horny").ConfigureAwait(false);
            await ctx.CreateResponseAsync($"Bonk, {member.Mention}").ConfigureAwait(false); // UHhhhhh
        }
        finally
        {
            await ctx.Channel.SendMessageAsync("https://i.imgur.com/w2rfJnr.png").ConfigureAwait(false);
        }
    }
}