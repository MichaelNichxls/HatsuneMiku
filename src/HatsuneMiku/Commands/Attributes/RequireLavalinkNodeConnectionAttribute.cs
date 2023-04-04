using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Lavalink;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class RequireLavalinkNodeConnectionAttribute : CheckBaseAttribute
{
    public override async Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
    {
        if (help)
            return true;

        LavalinkExtension lavalink = ctx.Client.GetLavalink();

        if (lavalink.ConnectedNodes.Any())
            return true;

        await ctx.RespondAsync("There must be an established connection to a Lavalink node").ConfigureAwait(false);
        return false;
    }
}