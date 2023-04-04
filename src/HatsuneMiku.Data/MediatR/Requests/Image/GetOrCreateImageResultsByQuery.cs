using GScraper;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Shared.GScraper;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku.Data.MediatR.Requests.Image;

public static class GetOrCreateImageResultsByQuery
{
    public sealed record class Request(string Query, SafeSearchLevel SafeSearch = SafeSearchLevel.Moderate)
        : IRequest<IEnumerable<IImageResult>>, IImageQuery;

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class RequestHandler : IRequestHandler<Request, IEnumerable<IImageResult>>
    {
        private readonly IMediator _mediator;

        public RequestHandler(IMediator mediator) =>
            _mediator = mediator;

        public async Task<IEnumerable<IImageResult>> Handle(Request request, CancellationToken cancellationToken)
        {
            var (query, safeSearch) = request;

            // Don't do this you can't do this

            // Do reflection instead
            IEnumerable<GoogleImageResultEntity> googleImageResults = await _mediator
                .Send(new GetOrCreateGoogleImageResultsByQuery.Request(query, safeSearch), cancellationToken)
                .ConfigureAwait(false);

            IEnumerable<DuckDuckGoImageResultEntity> duckDuckGoImageResults = await _mediator
                .Send(new GetOrCreateDuckDuckGoImageResultsByQuery.Request(query, safeSearch), cancellationToken)
                .ConfigureAwait(false);

            IEnumerable<BraveImageResultEntity> braveImageResults = await _mediator
                .Send(new GetOrCreateBraveImageResultsByQuery.Request(query, safeSearch), cancellationToken)
                .ConfigureAwait(false);

            // Extension
            return Enumerable.Empty<IImageResult>()
                .Concat(googleImageResults)
                .Concat(duckDuckGoImageResults)
                .Concat(braveImageResults);
        }
    }
}