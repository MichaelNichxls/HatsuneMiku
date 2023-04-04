using GScraper;
using GScraper.DuckDuckGo;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Shared.GScraper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku.Data.MediatR.Requests.Image;

internal static class GetOrCreateDuckDuckGoImageQuery
{
    public sealed record class Request(
        string Query,
        SafeSearchLevel SafeSearch      = SafeSearchLevel.Moderate,
        DuckDuckGoImageTime Time        = DuckDuckGoImageTime.Any,
        DuckDuckGoImageSize Size        = DuckDuckGoImageSize.All,
        DuckDuckGoImageColor Color      = DuckDuckGoImageColor.All,
        DuckDuckGoImageType Type        = DuckDuckGoImageType.All,
        DuckDuckGoImageLayout Layout    = DuckDuckGoImageLayout.All,
        DuckDuckGoImageLicense License  = DuckDuckGoImageLicense.All,
        string Region                   = DuckDuckGoRegions.UsEnglish)
        : IRequest<DuckDuckGoImageQueryEntity>, IImageQuery;

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class RequestHandler : IRequestHandler<Request, DuckDuckGoImageQueryEntity>
    {
        private readonly IDbContextFactory<MediaContext> _contextFactory;

        public RequestHandler(IDbContextFactory<MediaContext> contextFactory) =>
            _contextFactory = contextFactory;

        public async Task<DuckDuckGoImageQueryEntity> Handle(Request request, CancellationToken cancellationToken)
        {
            await using MediaContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
            var (query, safeSearch, time, size, color, type, layout, license, region) = request;

            DuckDuckGoImageQueryEntity? imageQuery = await context.DuckDuckGoImageQueries
                .Include(q => q.DuckDuckGoImageResults)
                .ThenInclude(r => r.DuckDuckGoImageResult)
                .FirstOrDefaultAsync(
                    q => q.Query == query
                        && q.SafeSearch == safeSearch
                        && q.Time == time
                        && q.Size == size
                        && q.Color == color
                        && q.Type == type
                        && q.Layout == layout
                        && q.License == license
                        && q.Region == region,
                    cancellationToken)
                .ConfigureAwait(false);

            if (imageQuery is not null)
                return imageQuery;

            imageQuery = new()
            {
                Query       = query,
                SafeSearch  = safeSearch,
                Time        = time,
                Size        = size,
                Color       = color,
                Type        = type,
                Layout      = layout,
                License     = license,
                Region      = region
            };

            await context.AddAsync(imageQuery, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return imageQuery;
        }
    }
}