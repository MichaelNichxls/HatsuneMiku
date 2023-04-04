using HatsuneMiku.Data.Entities.Music;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku.Data.MediatR.Requests.Music;

// TryGet
// GetSongs or GetAllSongs
public static class GetAllSongs
{
    public sealed record class Request : IRequest<IEnumerable<SongEntity>>;

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class RequestHandler : IRequestHandler<Request, IEnumerable<SongEntity>>
    {
        private readonly IDbContextFactory<MediaContext> _contextFactory;

        public RequestHandler(IDbContextFactory<MediaContext> contextFactory) =>
            _contextFactory = contextFactory;

        public async Task<IEnumerable<SongEntity>> Handle(Request request, CancellationToken cancellationToken)
        {
            await using MediaContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
            
            IEnumerable<SongEntity> songs = await context.Songs
                .Include(s => s.Producers)
                .ThenInclude(p => p.Producer)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return songs;
        }
    }
}