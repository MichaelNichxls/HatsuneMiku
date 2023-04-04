using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands.Attributes;

// Class?
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class RequireVoiceStateAttribute : CheckBaseAttribute
{
    public override async Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
    {
        if (help)
            return true;

        if (ctx.Member is { VoiceState: not null and { Channel: not null } })
            return true;

        await ctx.RespondAsync("You must be in a voice channel").ConfigureAwait(false);
        return false;
    }
}