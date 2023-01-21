using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Services;
using HatsuneMiku.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.SlashCommands.Image;

public class MikuImageSlashCommand : ApplicationImageCommandModule
{
    public MikuImageSlashCommand(IImageService imageService)
        : base(imageService, "Hatsune Miku Images", imageType: ImageType.Photo)
    {
    }

    // Disallow extra arguments
    [SlashCommand("mikuimage", "Sends a random image of Hatsune Miku from off the Web")]
    public async Task MikuImage(InteractionContext ctx)
    {
        // Select/index via SQL query
        // Remove default arguments in GetAsync()
        IEnumerable<ImageResultEntity> imageResults = await ImageService.GetOrAddImageResultsAsync(Query, ImageType, SafeSearchLevel).ConfigureAwait(false);

        // Seed random and get through service provider
        await ctx.CreateResponseAsync(new DiscordEmbedBuilder().WithImageUrl(imageResults.ElementAt(new Random().Next(imageResults.Count())).Url)).ConfigureAwait(false);
    }
}