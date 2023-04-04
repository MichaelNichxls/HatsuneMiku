using HatsuneMiku.Data.Entities.Music;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku.Data.MediatR.Requests.Music;

public static class GetOrCreateSongByTitle
{
    public sealed record class Request(
        string Title,
        IEnumerable<string>? Producers  = null,
        string? YouTubeUrl              = null,
        string? SoundCloudUrl           = null,
        string? NiconicoUrl             = null)
        : IRequest<SongEntity>;

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class RequestHandler : IRequestHandler<Request, SongEntity>
    {
        private readonly IDbContextFactory<MediaContext> _contextFactory;
        private readonly IMediator _mediator;

        public RequestHandler(IDbContextFactory<MediaContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator       = mediator;
        }

        public async Task<SongEntity> Handle(Request request, CancellationToken cancellationToken)
        {
            await using MediaContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
            var (title, producers, youTubeUrl, soundCloudUrl, niconicoUrl) = request;

            // Implement fuzzy searching
            SongEntity? song = await context.Songs
                .Include(s => s.Producers)
                .ThenInclude(p => p.Producer)
                .FirstOrDefaultAsync(s => s.Title == title, cancellationToken)
                .ConfigureAwait(false);

            if (song is not null)
                return song;
            
            song = new()
            {
                Title           = title,
                YouTubeUrl      = youTubeUrl,
                SoundCloudUrl   = soundCloudUrl,
                NiconicoUrl     = niconicoUrl
            };

            await context.AddAsync(song, cancellationToken).ConfigureAwait(false);

            foreach (string prod in producers ?? Enumerable.Empty<string>())
            {
                ProducerEntity producer         = await _mediator.Send(new GetOrCreateProducerByName.Request(prod), cancellationToken).ConfigureAwait(false);
                SongProducerEntity songProducer = new()
                {
                    SongId      = song.Id,
                    ProducerId  = producer.Id
                };

                await context.AddAsync(songProducer, cancellationToken).ConfigureAwait(false);

                song.Producers.Add(songProducer);
                producer.Songs.Add(songProducer);
            }

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return song;
        }
    }
}