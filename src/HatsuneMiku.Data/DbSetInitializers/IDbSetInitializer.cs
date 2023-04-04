using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku.Data.DbSetInitializers;

public interface IDbSetInitializer<TContext>
    where TContext : DbContext
{
    Task InitializeSetsAsync(TContext context, CancellationToken cancellationToken);
}