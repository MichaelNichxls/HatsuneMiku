using GScraper;
using GScraper.Brave;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Shared.GScraper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku.Data.MediatR.Requests.Image;

public static class GetOrCreateBraveImageResultsByQuery
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
        : IRequest<IEnumerable<BraveImageResultEntity>>, IImageQuery;

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class RequestHandler : IRequestHandler<Request, IEnumerable<BraveImageResultEntity>>
    {
        private readonly IDbContextFactory<MediaContext> _contextFactory;
        private readonly IMediator _mediator;

        public RequestHandler(IDbContextFactory<MediaContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator       = mediator;
        }

        // I forgot the inludes but i'm so tired i don't care anymore
        public async Task<IEnumerable<BraveImageResultEntity>> Handle(Request request, CancellationToken cancellationToken)
        {
            await using MediaContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
            var (query, safeSearch, country, size, type, layout, color, license) = request;

            BraveImageQueryEntity imageQuery = await _mediator
                .Send(new GetOrCreateBraveImageQuery.Request(query, safeSearch, country, size, type, layout, color, license), cancellationToken)
                .ConfigureAwait(false);

            if (imageQuery.BraveImageResults.Any())
                return imageQuery.BraveImageResults.Select(r => r.BraveImageResult);
            
            using BraveScraper scraper = new();

            foreach (BraveImageResult result in await scraper.GetImagesAsync(query, safeSearch, country, size, type, layout, color, license).ConfigureAwait(false))
            {
                BraveImageResultEntity imageResult = new()
                {
                    Url             = result.Url,
                    Title           = result.Title,
                    Width           = result.Width,
                    Height          = result.Height,
                    SourceUrl       = result.SourceUrl,
                    PageAge         = result.PageAge,
                    Source          = result.Source,
                    ThumbnailUrl    = result.ThumbnailUrl,
                    ResizedUrl      = result.ResizedUrl,
                    Format          = result.Format
                };

                await context.AddAsync(imageResult, cancellationToken).ConfigureAwait(false);

                BraveImageQueryResultEntity imageQueryResult = new()
                {
                    BraveImageQueryId   = imageQuery.Id,
                    BraveImageResultId  = imageResult.Id
                };

                await context.AddAsync(imageQueryResult, cancellationToken).ConfigureAwait(false);

                imageQuery.BraveImageResults.Add(imageQueryResult);
                imageResult.BraveImageQuery = imageQueryResult;
            }

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return imageQuery.BraveImageResults.Select(r => r.BraveImageResult);
        }
    }
}