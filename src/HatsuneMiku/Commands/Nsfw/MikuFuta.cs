using DSharpPlus.CommandsNext;
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
public class MikuFutaCommand : BaseCommandModule
{
    //public IEnumerable<GoogleImageResult> Images { private get; init; }

    private readonly IImageService _imageService;

    public MikuFutaCommand(IImageService imageService) => _imageService = imageService;

    // Seed Random
    [Hidden]
    [Command("mikufuta"), Aliases("mikufutanari", "mikudick")]
    [Description("")]
    public async Task MikuFuta(CommandContext ctx)
    {
        // Move to ctor
        // Store query into variable
        IEnumerable<ImageResultEntity> imageResults = await _imageService.GetAsync("Hatsune Miku Futanari", safeSearchLevel: SafeSearchLevel.Off).ConfigureAwait(false);

        if (!imageResults.Any())
            return;

        // Seed random and get through service provider
        await ctx.Channel.SendMessageAsync(new DiscordEmbedBuilder().WithImageUrl(imageResults.ElementAt(new Random().Next(imageResults.Count())).Url)).ConfigureAwait(false);
    }

    [Hidden]
    [Command("mikufutaadd"), Aliases("mikufutanariadd", "mikudickadd")]
    public async Task MikuFutaAdd(CommandContext ctx) =>
        await _imageService.AddAsync("Hatsune Miku Futanari", safeSearchLevel: SafeSearchLevel.Off).ConfigureAwait(false);
}