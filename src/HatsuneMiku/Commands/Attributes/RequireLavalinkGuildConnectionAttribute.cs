using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using HatsuneMiku.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class RequireLavalinkGuildConnectionAttribute : CheckBaseAttribute
{
    [RequireVoiceState]
    [RequireLavalinkNodeConnection]
    public override async Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
    {
        if (help)
            return true;

        if (!await this.ExecutePreCheckAsync(ctx, help).ConfigureAwait(false))
            return false;

        DiscordVoiceState voiceState            = ctx.Member!.VoiceState;
        LavalinkExtension lavalink              = ctx.Client.GetLavalink();
        LavalinkNodeConnection nodeConnection   = lavalink.ConnectedNodes.Values.First();
        LavalinkGuildConnection guildConnection = nodeConnection.GetGuildConnection(voiceState.Guild);

        if (guildConnection is not null)
            return true;

        await ctx.RespondAsync("There must be an established Lavalink connection to a voice channel").ConfigureAwait(false);
        return false;
    }
}