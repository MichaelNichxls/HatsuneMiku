using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands;

public class MikuImageCommand : BaseCommandModule
{
    private readonly IEnumerable<string> _images;

    public MikuImageCommand(ImageUrlServiceResolver images) =>
        _images = images(ImageType.Photo);

    [Command("mikuimage"), Aliases("mikuimg")]
    [Description("")]
    public async Task MikuImage(CommandContext ctx) =>
        await ctx.Channel.SendMessageAsync(new DiscordEmbedBuilder().WithImageUrl(_images.ElementAt(new Random().Next(_images.Count())))).ConfigureAwait(false);
}