using HatsuneMiku.Data.MediatR.Requests.Music;
using HatsuneMiku.Shared.Models;
using HatsuneMiku.Shared.Types;
using HtmlAgilityPack;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku.Data.DbSetInitializers;

public sealed class SongDbSetInitializer : ProgressibleBackgroundDbSetInitializer<MediaContext>
{
    private readonly IMediator _mediator;

    public SongDbSetInitializer(IDbContextFactory<MediaContext> contextFactory, IMediator mediator)
        : base(contextFactory, mediator) =>
        _mediator = mediator;

    // EnsureCreated()
    // Rename variables
    // Make service
    public override async Task InitializeSetsAsync(MediaContext context, IProgress<ProgressContext> progress, CancellationToken cancellationToken)
    {
        // Maybe make attribute
        Uri uri                 = new("https://vocaloid.fandom.com/wiki/Category:Songs_featuring_Hatsune_Miku");
        HtmlWeb web             = new();
        HtmlDocument document   = await web.LoadFromWebAsync(uri.OriginalString, cancellationToken).ConfigureAwait(false);

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // How to change format globally?
            var fandom = document.DocumentNode.GetEncapsulatedData<Fandom>();
            ProgressContext ??= new(maximum: int.Parse(fandom.SongCount, NumberStyles.Number));
            
            foreach (string path in fandom.SongUrlPaths)
            {
                uri         = new(uri, path);
                document    = await web.LoadFromWebAsync(uri.OriginalString, cancellationToken).ConfigureAwait(false);
                var song    = document.DocumentNode.GetEncapsulatedData<Song>();

                await _mediator.Send(new GetOrCreateSongByTitle.Request(song.Title, song.Producers, song.YouTubeUrl, song.SoundCloudUrl, song.NiconicoUrl), cancellationToken).ConfigureAwait(false);
                progress.Report(++ProgressContext);
            }

            if (fandom.PaginationUrl is null)
                break;

            uri         = new(uri, fandom.PaginationUrl);
            document    = await web.LoadFromWebAsync(uri.OriginalString, cancellationToken).ConfigureAwait(false);
        }
    }
}