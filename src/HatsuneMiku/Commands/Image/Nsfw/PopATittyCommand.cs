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

namespace HatsuneMiku.Commands.Image.Nsfw;

// Hidden
// RequireGuild?
[RequireGuild, RequireNsfw]
[EditorBrowsable(EditorBrowsableState.Never)]
public class PopATittyCommand : BaseImageCommandModule
{
    public PopATittyCommand(IImageService imageService)
        : base(imageService, "Hatsune Miku Tits", safeSearchLevel: SafeSearchLevel.Off)
    {
    }

    // Seed Random
    [Hidden]
    [Command("popatitty"), Aliases("popatit", "pop", "titty", "tit", "mikutitty", "mikutit")]
    [Description("Hatsune Miku pops a titty")]
    public async Task PopATitty(CommandContext ctx)
    {
        // Remove default arguments in GetAsync()
        IEnumerable<ImageResultEntity> imageResults = await ImageService.GetOrAddImageResultsAsync(Query, ImageType, SafeSearchLevel).ConfigureAwait(false);

        // Seed random and get through service provider
        await ctx.Channel.SendMessageAsync(new DiscordEmbedBuilder().WithImageUrl(imageResults.ElementAt(new Random().Next(imageResults.Count())).Url)).ConfigureAwait(false);
    }
}