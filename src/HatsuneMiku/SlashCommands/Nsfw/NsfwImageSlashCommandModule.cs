using DSharpPlus.SlashCommands;
using GScraper;
using HatsuneMiku.Data.MediatR.Requests.Image;
using HatsuneMiku.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.SlashCommands.Nsfw;

// RequireGuild?
public sealed class NsfwImageSlashCommandModule : ApplicationCommandModule
{
    private readonly IMediator _mediator;

    public NsfwImageSlashCommandModule(IMediator mediator) =>
        _mediator = mediator;

    [SlashCommand("mikufuta", "Sends a random image of Hatsune Miku Futanari from the Web", false)]
    public async Task MikuFutaAsync(InteractionContext ctx)
    {
        IEnumerable<IImageResult> results   = await _mediator.Send(new GetOrCreateImageResultsByQuery.Request("Hatsune Miku Futanari", SafeSearchLevel.Off)).ConfigureAwait(false);
        IImageResult result                 = Random.Shared.GetItem(results.ToArray());

        await ctx.CreateResponseAsync(result.Url).ConfigureAwait(false);
    }

    [SlashCommand("popatitty", "Hatsune Miku pops a titty", false)]
    public async Task PopATittyAsync(InteractionContext ctx)
    {
        IEnumerable<IImageResult> results   = await _mediator.Send(new GetOrCreateImageResultsByQuery.Request("Hatsune Miku Titties", SafeSearchLevel.Off)).ConfigureAwait(false);
        IImageResult result                 = Random.Shared.GetItem(results.ToArray());

        await ctx.CreateResponseAsync(result.Url).ConfigureAwait(false);
    }
}