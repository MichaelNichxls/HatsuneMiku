using HatsuneMiku.Data.Entities.Music;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku.Data.MediatR.Requests.Music;

internal static class GetOrCreateProducerByName
{
    public sealed record class Request(string Name) : IRequest<ProducerEntity>;

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class RequestHandler : IRequestHandler<Request, ProducerEntity>
    {
        private readonly IDbContextFactory<MediaContext> _contextFactory;

        public RequestHandler(IDbContextFactory<MediaContext> contextFactory) =>
            _contextFactory = contextFactory;

        public async Task<ProducerEntity> Handle(Request request, CancellationToken cancellationToken)
        {
            await using MediaContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

            ProducerEntity? producer = await context.Producers
                .Include(p => p.Songs)
                .ThenInclude(s => s.Song)
                .FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken)
                .ConfigureAwait(false);

            if (producer is not null)
                return producer;

            producer = new() { Name = request.Name };

            await context.AddAsync(producer, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return producer;
        }
    }
}