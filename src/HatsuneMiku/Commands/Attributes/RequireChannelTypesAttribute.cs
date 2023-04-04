using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class RequireChannelTypesAttribute : CheckBaseAttribute
{
    public IEnumerable<ChannelType> ChannelTypes { get; }

    public RequireChannelTypesAttribute(params ChannelType[] channelTypes) =>
        ChannelTypes = channelTypes;

    public override async Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
    {
        if (help)
            return true;

        throw new NotImplementedException();
    }
}