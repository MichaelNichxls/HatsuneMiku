using HatsuneMiku.Data.MediatR.Notifications;
using HatsuneMiku.Shared.Types;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku.Data.DbSetInitializers;

public abstract class ProgressibleBackgroundDbSetInitializer<TContext> : BackgroundDbSetInitializer<TContext>
    where TContext : DbContext
{
    private readonly IPublisher _publisher;
    private readonly Progress<ProgressContext> _progress = new();

    protected ProgressContext? ProgressContext { get; set; }

    // Remove dependencies if possible
    public ProgressibleBackgroundDbSetInitializer(IDbContextFactory<TContext> contextFactory, IPublisher publisher)
        : base(contextFactory) =>
        _publisher = publisher;

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _progress.ProgressChanged += async (_, e) => await _publisher
            .Publish(new ProgressChanged.Notification(this, new ProgressChangedEventArgs(e)), cancellationToken)
            .ConfigureAwait(false);

        return base.StartAsync(cancellationToken);
    }

    public sealed override async Task InitializeSetsAsync(TContext context, CancellationToken cancellationToken)
    {
        try
        {
            await _publisher
                .Publish(new ProgressStarted.Notification(this, new ProgressStartedEventArgs(DateTimeOffset.Now)), cancellationToken)
                .ConfigureAwait(false);

            await InitializeSetsAsync(context, _progress, cancellationToken).ConfigureAwait(false);
            
            await _publisher
                .Publish(new ProgressFinished.Notification(this, new ProgressFinishedEventArgs(DateTimeOffset.Now)), cancellationToken)
                .ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            await _publisher
                .Publish(new ProgressCanceled.Notification(this, new ProgressCanceledEventArgs(ProgressContext, DateTimeOffset.Now)), cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await _publisher
                .Publish(new ProgressExceptionThrown.Notification(this, new ProgressExceptionThrownEventArgs(ProgressContext, ex, DateTimeOffset.Now)), cancellationToken)
                .ConfigureAwait(false);
        }
    }

    public abstract Task InitializeSetsAsync(TContext context, IProgress<ProgressContext> progress, CancellationToken cancellationToken);
}