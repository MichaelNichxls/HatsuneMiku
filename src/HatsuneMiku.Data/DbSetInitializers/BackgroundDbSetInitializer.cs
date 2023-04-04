using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku.Data.DbSetInitializers;

public abstract class BackgroundDbSetInitializer<TContext> : BackgroundService, IDbSetInitializer<TContext>
    where TContext : DbContext
{
    private readonly IDbContextFactory<TContext> _contextFactory;

    public BackgroundDbSetInitializer(IDbContextFactory<TContext> contextFactory) =>
        _contextFactory = contextFactory;

    protected sealed override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await using TContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await InitializeSetsAsync(context, cancellationToken).ConfigureAwait(false);
    }

    public abstract Task InitializeSetsAsync(TContext context, CancellationToken cancellationToken);
}