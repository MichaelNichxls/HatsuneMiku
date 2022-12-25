using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using System;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands.SlashCommands
{
    // Relocate?
    //[SlashCommandGroup]
    [SlashRequireGuild]
    public class BonkSlashCommand : ApplicationCommandModule
    {
        public static DateTimeOffset TimeoutUntil => BonkCommand.TimeoutUntil;

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
                await ctx.CreateResponseAsync($"Bonk, {member.Mention}", new DiscordEmbedBuilder().WithImageUrl("https://i.imgur.com/w2rfJnr.png")).ConfigureAwait(false);
            }
        }
    }
}