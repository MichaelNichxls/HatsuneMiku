using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands
{
    //[Category]
    [RequireGuild]
    public class BonkCommand : BaseCommandModule
    {
        // Make configurable
        public static DateTimeOffset TimeoutUntil => DateTimeOffset.UtcNow.AddSeconds(10);

        // Relocate link
        // Disallow timeout if member in question is in vc
        [Command("bonk")]
        [Description("Bonks the horny")]
        public async Task Bonk(CommandContext ctx, DiscordMember member)
        {
            try
            {
                await member.TimeoutAsync(TimeoutUntil, "Horny").ConfigureAwait(false);
            }
            finally
            {
                await ctx.Channel.SendMessageAsync($"Bonk, {member.Mention}", new DiscordEmbedBuilder().WithImageUrl("https://i.imgur.com/w2rfJnr.png")).ConfigureAwait(false);
            }
        }
    }
}