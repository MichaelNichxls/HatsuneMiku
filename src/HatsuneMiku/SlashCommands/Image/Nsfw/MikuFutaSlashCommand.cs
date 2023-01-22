using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using GScraper;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.SlashCommands.Image.Nsfw;

[EditorBrowsable(EditorBrowsableState.Never)]
public class MikuFutaSlashCommand : ApplicationImageCommandModule
{
    public MikuFutaSlashCommand(IImageService imageService)
        : base(imageService, "Hatsune Miku Futanari", safeSearchLevel: SafeSearchLevel.Off)
    {
    }

    // Seed Random
    [SlashCommand("mikufuta", "Sends a random image of Hatsune Miku Futanari from off the Web", false)]
    public async Task MikuFuta(InteractionContext ctx)
    {
        // Remove default arguments in GetAsync()
        IEnumerable<ImageResultEntity> imageResults = await ImageService.GetOrAddImageResultsAsync(Query, ImageType, SafeSearchLevel).ConfigureAwait(false);

        // Seed random and get through service provider
        await ctx.CreateResponseAsync(imageResults.ElementAt(new Random().Next(imageResults.Count())).Url).ConfigureAwait(false);
    }
}