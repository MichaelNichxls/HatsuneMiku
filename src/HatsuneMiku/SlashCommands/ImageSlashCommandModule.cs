using DSharpPlus.SlashCommands;
using GScraper;
using HatsuneMiku.Data.MediatR.Requests.Image;
using HatsuneMiku.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.SlashCommands;

public sealed class ImageSlashCommandModule : ApplicationCommandModule
{
    private readonly IMediator _mediator;

    public ImageSlashCommandModule(IMediator mediator) =>
        _mediator = mediator;

    [SlashCommand("mikuimage", "Sends a random image of Hatsune Miku from the Web")]
    public async Task MikuImageAsync(InteractionContext ctx)
    {
        IEnumerable<IImageResult> results   = await _mediator.Send(new GetOrCreateImageResultsByQuery.Request("Hatsune Miku Images")).ConfigureAwait(false);
        IImageResult result                 = Random.Shared.GetItem(results.ToArray());

        await ctx.CreateResponseAsync(result.Url).ConfigureAwait(false);
    }

    [SlashCommand("mikugif", "Sends a random GIF of Hatsune Miku from the Web")]
    public async Task MikuGifAsync(InteractionContext ctx)
    {
        IEnumerable<IImageResult> results   = await _mediator.Send(new GetOrCreateImageResultsByQuery.Request("Hatsune Miku GIFs")).ConfigureAwait(false);
        IImageResult result                 = Random.Shared.GetItem(results.ToArray());

        await ctx.CreateResponseAsync(result.Url).ConfigureAwait(false);
    }
}