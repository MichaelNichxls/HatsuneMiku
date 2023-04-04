using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using GScraper;
using HatsuneMiku.Data.MediatR.Requests.Image;
using HatsuneMiku.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands;

[Category("Image commands")]
public sealed class ImageCommandModule : BaseCommandModule
{
    private readonly IMediator _mediator;

    public ImageCommandModule(IMediator mediator) =>
        _mediator = mediator;

    // Make query const
    // Make attributes assembly level if possible
    [Command, Aliases("mikuimg")]
    [Description("Sends a random image of Hatsune Miku from the Web")]
    public async Task MikuImageAsync(CommandContext ctx)
    {
        IEnumerable<IImageResult> results   = await _mediator.Send(new GetOrCreateImageResultsByQuery.Request("Hatsune Miku Images")).ConfigureAwait(false);
        IImageResult result                 = Random.Shared.GetItem(results.ToArray());

        await ctx.RespondAsync(result.Url).ConfigureAwait(false);
    }

    [Command, Aliases("mikugiphy", "mikutenor")]
    [Description("Sends a random GIF of Hatsune Miku from the Web")]
    public async Task MikuGifAsync(CommandContext ctx)
    {
        IEnumerable<IImageResult> results   = await _mediator.Send(new GetOrCreateImageResultsByQuery.Request("Hatsune Miku GIFs")).ConfigureAwait(false);
        IImageResult result                 = Random.Shared.GetItem(results.ToArray());

        await ctx.RespondAsync(result.Url).ConfigureAwait(false);
    }
}