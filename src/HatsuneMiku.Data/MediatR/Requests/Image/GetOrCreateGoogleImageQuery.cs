using GScraper;
using GScraper.Google;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Shared.GScraper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku.Data.MediatR.Requests.Image;

internal static class GetOrCreateGoogleImageQuery
{
    public sealed record class Request(
        string Query,
        SafeSearchLevel SafeSearch  = SafeSearchLevel.Off,
        GoogleImageSize Size        = GoogleImageSize.Any,
        string? Color               = null,
        GoogleImageType Type        = GoogleImageType.Any,
        GoogleImageTime Time        = GoogleImageTime.Any,
        string? License             = null,
        string? Language            = null)
        : IRequest<GoogleImageQueryEntity>, IImageQuery;

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class RequestHandler : IRequestHandler<Request, GoogleImageQueryEntity>
    {
        private readonly IDbContextFactory<MediaContext> _contextFactory;

        public RequestHandler(IDbContextFactory<MediaContext> contextFactory) =>
            _contextFactory = contextFactory;

        public async Task<GoogleImageQueryEntity> Handle(Request request, CancellationToken cancellationToken)
        {
            await using MediaContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
            var (query, safeSearch, size, color, type, time, license, language) = request;

            GoogleImageQueryEntity? imageQuery = await context.GoogleImageQueries
                .Include(q => q.GoogleImageResults)
                .ThenInclude(r => r.GoogleImageResult)
                // Overide equality method
                .FirstOrDefaultAsync(
                    q => q.Query == query
                        && q.SafeSearch == safeSearch
                        && q.Size == size
                        && q.Color == color
                        && q.Type == type
                        && q.Time == time
                        && q.License == license
                        && q.Language == language,
                    cancellationToken)
                .ConfigureAwait(false);

            if (imageQuery is not null)
                return imageQuery;

            imageQuery = new()
            {
                Query       = query,
                SafeSearch  = safeSearch,
                Size        = size,
                Color       = color,
                Type        = type,
                Time        = time,
                License     = license,
                Language    = language
            };

            await context.AddAsync(imageQuery, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return imageQuery;
        }
    }
}