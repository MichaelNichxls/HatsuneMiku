using GScraper;
using GScraper.Google;
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

public static class GetOrCreateGoogleImageResultsByQuery
{
    // Make abstract record
    public sealed record class Request(
        string Query,
        SafeSearchLevel SafeSearch  = SafeSearchLevel.Off,
        GoogleImageSize Size        = GoogleImageSize.Any,
        string? Color               = null,
        GoogleImageType Type        = GoogleImageType.Any,
        GoogleImageTime Time        = GoogleImageTime.Any,
        string? License             = null,
        string? Language            = null)
        : IRequest<IEnumerable<GoogleImageResultEntity>>, IImageQuery;

    // Does this even matter since it's internal
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class RequestHandler : IRequestHandler<Request, IEnumerable<GoogleImageResultEntity>>
    {
        private readonly IDbContextFactory<MediaContext> _contextFactory;
        private readonly IMediator _mediator;

        public RequestHandler(IDbContextFactory<MediaContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator       = mediator;
        }

        public async Task<IEnumerable<GoogleImageResultEntity>> Handle(Request request, CancellationToken cancellationToken)
        {
            await using MediaContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
            var (query, safeSearch, size, color, type, time, license, language) = request;

            GoogleImageQueryEntity imageQuery = await _mediator
                .Send(new GetOrCreateGoogleImageQuery.Request(query, safeSearch, size, color, type, time, license, language), cancellationToken)
                .ConfigureAwait(false);

            if (imageQuery.GoogleImageResults.Any())
                return imageQuery.GoogleImageResults.Select(r => r.GoogleImageResult);

            using GoogleScraper scraper = new();

            foreach (GoogleImageResult result in await scraper.GetImagesAsync(query, safeSearch, size, color, type, time, license, language).ConfigureAwait(false))
            {
                GoogleImageResultEntity imageResult = new()
                {
                    Url             = result.Url,
                    Title           = result.Title,
                    Width           = result.Width,
                    Height          = result.Height,
                    Color           = result.Color,
                    DisplayUrl      = result.DisplayUrl,
                    SourceUrl       = result.SourceUrl,
                    ThumbnailUrl    = result.ThumbnailUrl
                };

                await context.AddAsync(imageResult, cancellationToken).ConfigureAwait(false);

                GoogleImageQueryResultEntity imageQueryResult = new()
                {
                    GoogleImageQueryId  = imageQuery.Id,
                    GoogleImageResultId = imageResult.Id
                };

                await context.AddAsync(imageQueryResult, cancellationToken).ConfigureAwait(false);

                imageQuery.GoogleImageResults.Add(imageQueryResult);
                imageResult.GoogleImageQuery = imageQueryResult;
            }

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return imageQuery.GoogleImageResults.Select(r => r.GoogleImageResult);
        }
    }
}