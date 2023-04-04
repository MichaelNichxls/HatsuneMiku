using GScraper;
using GScraper.DuckDuckGo;
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

public static class GetOrCreateDuckDuckGoImageResultsByQuery
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
        : IRequest<IEnumerable<DuckDuckGoImageResultEntity>>, IImageQuery;

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class RequestHandler : IRequestHandler<Request, IEnumerable<DuckDuckGoImageResultEntity>>
    {
        private readonly IDbContextFactory<MediaContext> _contextFactory;
        private readonly IMediator _mediator;

        public RequestHandler(IDbContextFactory<MediaContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator       = mediator;
        }

        public async Task<IEnumerable<DuckDuckGoImageResultEntity>> Handle(Request request, CancellationToken cancellationToken)
        {
            await using MediaContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
            var (query, safeSearch, time, size, color, type, layout, license, region) = request;

            DuckDuckGoImageQueryEntity imageQuery = await _mediator
                .Send(new GetOrCreateDuckDuckGoImageQuery.Request(query, safeSearch, time, size, color, type, layout, license, region), cancellationToken)
                .ConfigureAwait(false);

            if (imageQuery.DuckDuckGoImageResults.Any())
                return imageQuery.DuckDuckGoImageResults.Select(r => r.DuckDuckGoImageResult);

            using DuckDuckGoScraper scraper = new();

            foreach (DuckDuckGoImageResult result in await scraper.GetImagesAsync(query, safeSearch, time, size, color, type, layout, license, region).ConfigureAwait(false))
            {
                DuckDuckGoImageResultEntity imageResult = new()
                {
                    Url             = result.Url,
                    Title           = result.Title,
                    Width           = result.Width,
                    Height          = result.Height,
                    SourceUrl       = result.SourceUrl,
                    ThumbnailUrl    = result.ThumbnailUrl,
                    Source          = result.Source
                };

                await context.AddAsync(imageResult, cancellationToken).ConfigureAwait(false);

                DuckDuckGoImageQueryResultEntity imageQueryResult = new()
                {
                    DuckDuckGoImageQueryId  = imageQuery.Id,
                    DuckDuckGoImageResultId = imageResult.Id
                };

                await context.AddAsync(imageQueryResult, cancellationToken).ConfigureAwait(false);

                imageQuery.DuckDuckGoImageResults.Add(imageQueryResult);
                imageResult.DuckDuckGoImageQuery = imageQueryResult;
            }

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return imageQuery.DuckDuckGoImageResults.Select(r => r.DuckDuckGoImageResult);
        }
    }
}