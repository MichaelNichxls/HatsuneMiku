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

public class MikuGifSlashCommand : ApplicationImageCommandModule
{
    public MikuGifSlashCommand(IImageService imageService)
        : base(imageService, "Hatsune Miku GIFs", imageType: ImageType.Animated)
    {
    }

    [SlashCommand("mikugif", "Double L")]
    public async Task MikuGif(InteractionContext ctx)
    {
        // Select/index via SQL query
        // Remove default arguments in GetAsync()
        IEnumerable<ImageResultEntity> imageResults = await ImageService.GetOrAddImageResultsAsync(Query, ImageType, SafeSearchLevel).ConfigureAwait(false);

        // Seed random and get through service provider
        await ctx.CreateResponseAsync(new DiscordEmbedBuilder().WithImageUrl(imageResults.ElementAt(new Random().Next(imageResults.Count())).Url)).ConfigureAwait(false);
    }
}