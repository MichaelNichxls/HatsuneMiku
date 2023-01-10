using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using HatsuneMiku.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands;

public class MikuGifCommand : BaseCommandModule
{
    private readonly IEnumerable<string> _gifs;

    public MikuGifCommand(IDictionary<ImageType, IEnumerable<string>> images) =>
        _gifs = images[ImageType.Animated];

    [Command("mikugif"), Aliases("mikugiphy", "mikutenor")]
    [Description("")]
    public async Task MikuGif(CommandContext ctx) =>
        await ctx.Channel.SendMessageAsync(new DiscordEmbedBuilder().WithImageUrl(_gifs.ElementAt(new Random().Next(_gifs.Count())))).ConfigureAwait(false);
}