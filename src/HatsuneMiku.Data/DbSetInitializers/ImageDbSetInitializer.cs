using HatsuneMiku.Data.MediatR.Requests.Image;
using HatsuneMiku.Shared.Models;
using HatsuneMiku.Shared.Types;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku.Data.DbSetInitializers;

public sealed class ImageDbSetInitializer : ProgressibleBackgroundDbSetInitializer<MediaContext>
{
    private readonly IMediator _mediator;

    public ImageDbSetInitializer(IDbContextFactory<MediaContext> contextFactory, IMediator mediator)
        : base(contextFactory, mediator) =>
        _mediator = mediator;

    public override async Task InitializeSetsAsync(MediaContext context, IProgress<ProgressContext> progress, CancellationToken cancellationToken)
    {
        ProgressContext ??= new(maximum: ImagesResults.GetImageCount());
        IEnumerable<PropertyInfo> queryProperties = typeof(ImagesResults).GetProperties().Where(p => p.IsDefined(typeof(ImageQueryAttribute)));

        foreach (PropertyInfo prop in queryProperties)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var (query, safeSearch) = prop.GetCustomAttribute<ImageQueryAttribute>()!;
            
            if (prop.IsDefined(typeof(FromGoogleAttribute)))
            {
                var (size, color, type, time, license, language) = prop.GetCustomAttribute<FromGoogleAttribute>()!;
                await _mediator
                    .Send(new GetOrCreateGoogleImageResultsByQuery.Request(query, safeSearch, size, color, type, time, license, language), cancellationToken)
                    .ConfigureAwait(false);

                progress.Report(ProgressContext += 100);
            }

            if (prop.IsDefined(typeof(FromDuckDuckGoAttribute)))
            {
                var (time, size, color, type, layout, license, region) = prop.GetCustomAttribute<FromDuckDuckGoAttribute>()!;
                await _mediator
                    .Send(new GetOrCreateDuckDuckGoImageResultsByQuery.Request(query, safeSearch, time, size, color, type, layout, license, region), cancellationToken)
                    .ConfigureAwait(false);

                progress.Report(ProgressContext += 100);
            }

            if (prop.IsDefined(typeof(FromBraveAttribute)))
            {
                var (country, size, type, layout, color, license) = prop.GetCustomAttribute<FromBraveAttribute>()!;
                await _mediator
                    .Send(new GetOrCreateBraveImageResultsByQuery.Request(query, safeSearch, country, size, type, layout, color, license), cancellationToken)
                    .ConfigureAwait(false);

                progress.Report(ProgressContext += 150);
            }
        }
    }
}