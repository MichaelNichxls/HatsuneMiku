using GScraper;
using GScraper.Brave;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Shared.GScraper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku.Data.MediatR.Requests.Image;

internal static class GetOrCreateBraveImageQuery
{
    public sealed record class Request(
        string Query,
        SafeSearchLevel SafeSearch  = SafeSearchLevel.Moderate,
        string? Country             = null,
        BraveImageSize Size         = BraveImageSize.All,
        BraveImageType Type         = BraveImageType.All,
        BraveImageLayout Layout     = BraveImageLayout.All,
        BraveImageColor Color       = BraveImageColor.All,
        BraveImageLicense License   = BraveImageLicense.All)
        : IRequest<BraveImageQueryEntity>, IImageQuery;

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class RequestHandler : IRequestHandler<Request, BraveImageQueryEntity>
    {
        private readonly IDbContextFactory<MediaContext> _contextFactory;

        public RequestHandler(IDbContextFactory<MediaContext> contextFactory) =>
            _contextFactory = contextFactory;

        public async Task<BraveImageQueryEntity> Handle(Request request, CancellationToken cancellationToken)
        {
            await using MediaContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
            var (query, safeSearch, country, size, type, layout, color, license) = request;

            BraveImageQueryEntity? imageQuery = await context.BraveImageQueries
                .Include(q => q.BraveImageResults)
                .ThenInclude(r => r.BraveImageResult)
                .FirstOrDefaultAsync(
                    q => q.Query == query
                        && q.SafeSearch == safeSearch
                        && q.Country == country
                        && q.Size == size
                        && q.Type == type
                        && q.Layout == layout
                        && q.Color == color
                        && q.License == license,
                    cancellationToken)
                .ConfigureAwait(false);

            if (imageQuery is not null)
                return imageQuery;

            imageQuery = new()
            {
                Query       = query,
                SafeSearch  = safeSearch,
                Country     = country,
                Size        = size,
                Type        = type,
                Layout      = layout,
                Color       = color,
                License     = license
            };

            await context.AddAsync(imageQuery, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return imageQuery;
        }
    }
}