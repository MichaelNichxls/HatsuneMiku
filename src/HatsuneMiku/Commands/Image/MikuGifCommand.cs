﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Services;
using HatsuneMiku.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands.Image;

public class MikuGifCommand : BaseImageCommandModule
{
    public MikuGifCommand(IImageService imageService)
        : base(imageService, "Hatsune Miku GIFs", imageType: ImageType.Animated)
    {
    }

    [Command("mikugif"), Aliases("mikugiphy", "mikutenor")]
    [Description("Sends a random GIF of Hatsune Miku from off the Web")]
    public async Task MikuGif(CommandContext ctx)
    {
        // Select/index via SQL query
        // Remove default arguments in GetAsync()
        IEnumerable<ImageResultEntity> imageResults = await ImageService.GetOrAddImageResultsAsync(Query, ImageType, SafeSearchLevel).ConfigureAwait(false);

        // Seed random and get through service provider
        await ctx.Channel.SendMessageAsync(imageResults.ElementAt(new Random().Next(imageResults.Count())).Url).ConfigureAwait(false);
    }
}