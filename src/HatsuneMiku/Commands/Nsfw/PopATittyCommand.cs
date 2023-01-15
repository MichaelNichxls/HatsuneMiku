﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GScraper;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using DescriptionAttribute = DSharpPlus.CommandsNext.Attributes.DescriptionAttribute;

namespace HatsuneMiku.Commands.Nsfw;

// Hidden
// RequireGuild?
[RequireGuild, RequireNsfw]
[EditorBrowsable(EditorBrowsableState.Never)]
public class PopATittyCommand : BaseCommandModule
{
    //public IEnumerable<GoogleImageResult> Images { private get; init; }

    private readonly IImageService _imageService;

    public PopATittyCommand(IImageService imageService) => _imageService = imageService;

    // Seed Random
    [Hidden]
    [Command("popatitty"), Aliases("popatit", "pop", "titty", "tit", "mikutitty", "mikutit")]
    [Description("Hatsune Miku pops a titty")]
    public async Task PopATitty(CommandContext ctx)
    {
        // Move to ctor
        // Store query into variable
        IEnumerable<ImageResultEntity> imageResults = await _imageService.GetAsync("Hatsune Miku Tits", safeSearchLevel: SafeSearchLevel.Off).ConfigureAwait(false);

        if (!imageResults.Any())
            return;

        // Seed random and get through service provider
        await ctx.Channel.SendMessageAsync(new DiscordEmbedBuilder().WithImageUrl(imageResults.ElementAt(new Random().Next(imageResults.Count())).Url)).ConfigureAwait(false);
    }

    [Hidden]
    [Command("popatittyadd"), Aliases("popatitadd", "popadd", "tittyadd", "titadd", "mikutittyadd", "mikutitadd")]
    public async Task PopATittyAdd(CommandContext ctx) =>
        await _imageService.AddAsync("Hatsune Miku Tits", safeSearchLevel: SafeSearchLevel.Off).ConfigureAwait(false);
}