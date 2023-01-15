using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GScraper.Google;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Services;
using HatsuneMiku.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands;

public class MikuGifCommand : BaseCommandModule
{
    private readonly IImageService _imageService;

    public MikuGifCommand(IImageService imageService) => _imageService = imageService;

    [Command("mikugif"), Aliases("mikugiphy", "mikutenor")]
    [Description("")]
    public async Task MikuGif(CommandContext ctx)
    {
        // Move to ctor
        // Store query into variable
        IEnumerable<ImageResultEntity> imageResults = await _imageService.GetAsync("Hatsune Miku GIFs", imageType: ImageType.Animated).ConfigureAwait(false);

        if (!imageResults.Any())
            return;

        // Seed random and get through service provider
        await ctx.Channel.SendMessageAsync(new DiscordEmbedBuilder().WithImageUrl(imageResults.ElementAt(new Random().Next(imageResults.Count())).Url)).ConfigureAwait(false);
    }

    [Hidden]
    [Command("mikugifadd"), Aliases("mikugiphyadd", "mikutenoradd")]
    public async Task MikuGifAdd(CommandContext ctx) =>
        await _imageService.AddAsync("Hatsune Miku GIFs", imageType: ImageType.Animated).ConfigureAwait(false);
}