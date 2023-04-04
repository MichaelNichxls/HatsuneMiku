using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HatsuneMiku.Extensions;

public static class CheckBaseAttributeExtensions
{
    public static async Task<bool> ExecutePreCheckAsync(this CheckBaseAttribute source, CommandContext ctx, bool help)
    {
        var checks = source
            .GetType()
            .GetMethod(nameof(CheckBaseAttribute.ExecuteCheckAsync), new[] { typeof(CommandContext), typeof(bool) })!
            .GetCustomAttributes<CheckBaseAttribute>();

        foreach (CheckBaseAttribute check in checks)
        {
            if (ctx.Command!.ExecutionChecks.Contains(check))
                continue;

            if (!await check.ExecuteCheckAsync(ctx, help).ConfigureAwait(false))
                return false;
        }

        return true;
    }
}