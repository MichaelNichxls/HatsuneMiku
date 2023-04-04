using HatsuneMiku.Data.DbSetInitializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace HatsuneMiku.Extensions;

// ServiceCollectionHostedServiceExtensions
public static class HostedDbSetInitializerServiceCollectionExtensions
{
    public static IServiceCollection AddHostedDbSetInitializer<TContext, TSetInitializer>(this IServiceCollection services)
        where TContext : DbContext
        where TSetInitializer : class, IDbSetInitializer<TContext>, IHostedService =>
        services.AddHostedService<TSetInitializer>();
    
    // Necessary?
    public static IServiceCollection AddHostedDbSetInitializer<TContext, TSetInitializer>(this IServiceCollection services, Func<IServiceProvider, TSetInitializer> implementationFactory)
        where TContext : DbContext
        where TSetInitializer : class, IDbSetInitializer<TContext>, IHostedService =>
        services.AddHostedService(implementationFactory);
}