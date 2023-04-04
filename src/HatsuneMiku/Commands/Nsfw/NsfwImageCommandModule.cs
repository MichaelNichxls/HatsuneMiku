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

namespace HatsuneMiku.Commands.Nsfw;

[Hidden]
[RequireNsfw]
[Category("Nsfw Image commands")]
public sealed class NsfwImageCommandModule : BaseCommandModule
{
    private readonly IMediator _mediator;

    public NsfwImageCommandModule(IMediator mediator) =>
        _mediator = mediator;

    [Command, Aliases("mikufutanari", "mikudick", "mikupenis")]
    [Description("Sends a random image of Hatsune Miku Futanari from the Web")]
    public async Task MikuFutaAsync(CommandContext ctx)
    {
        IEnumerable<IImageResult> results   = await _mediator.Send(new GetOrCreateImageResultsByQuery.Request("Hatsune Miku Futanari", SafeSearchLevel.Off)).ConfigureAwait(false);
        IImageResult result                 = Random.Shared.GetItem(results.ToArray());

        await ctx.RespondAsync(result.Url).ConfigureAwait(false);
    }

    [Command, Aliases("popatit", "pop", "mikutitty", "mikutit")]
    [Description("Hatsune Miku pops a titty")]
    public async Task PopATittyAsync(CommandContext ctx)
    {
        IEnumerable<IImageResult> results   = await _mediator.Send(new GetOrCreateImageResultsByQuery.Request("Hatsune Miku Titties", SafeSearchLevel.Off)).ConfigureAwait(false);
        IImageResult result                 = Random.Shared.GetItem(results.ToArray());

        await ctx.RespondAsync(result.Url).ConfigureAwait(false);
    }
}