using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Services;
using HatsuneMiku.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands;

public class MikuImageCommand : BaseCommandModule
{
    // Rename
    private readonly IImageService _imageService;

    public MikuImageCommand(IImageService imageService) => _imageService = imageService;

    [Command("mikuimage"), Aliases("mikuimg")]
    [Description("")]
    public async Task MikuImage(CommandContext ctx)
    {
        // Move to ctor
        // Store query into variable
        IEnumerable<ImageResultEntity> imageResults = await _imageService.GetAsync("Hatsune Miku Images", imageType: ImageType.Photo).ConfigureAwait(false);

        if (!imageResults.Any())
            return;

        // Seed random and get through service provider
        await ctx.Channel.SendMessageAsync(new DiscordEmbedBuilder().WithImageUrl(imageResults.ElementAt(new Random().Next(imageResults.Count())).Url)).ConfigureAwait(false);
    }

    [Hidden]
    [Command("mikuimageadd"), Aliases("mikuimgadd")]
    public async Task MikuImageAdd(CommandContext ctx) =>
        await _imageService.AddAsync("Hatsune Miku Images", imageType: ImageType.Photo).ConfigureAwait(false);
}