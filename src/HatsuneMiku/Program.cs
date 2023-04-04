using HatsuneMiku.Data;
using HatsuneMiku.Data.DbSetInitializers;
using HatsuneMiku.Extensions;
using HatsuneMiku.Services;
using HatsuneMiku.Shared.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace HatsuneMiku;

internal sealed class Program
{
    public static async Task Main(string[] args) =>
        await CreateHostBuilder(args).Build().RunAsync();

    // Separate
    // ConfigureAppConfiguration
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config => config.AddEnvironmentVariables("HATSUNEMIKU_"))
            .ConfigureServices(
                (context, services) => services
                    .AddOptions<HatsuneMikuConfigurationOptions>(
                        options => options
                            .Bind(context.Configuration.GetRequiredSection(HatsuneMikuConfigurationOptions.SectionKey))
                            .ValidateDataAnnotations()
                            .ValidateOnStart())
                    .AddDbContextFactory<MediaContext>(
                        (provider, options) => options
#if DEBUG
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors()
#endif
                            .UseSqlServer(
                                //// Extension
                                //context.Configuration
                                //    .GetRequiredSection(HatsuneMikuConfigurationOptions.SectionKey)
                                //    .Get<HatsuneMikuConfigurationOptions>()!.Persistence.ConnectionString,
                                provider.GetRequiredService<IOptions<HatsuneMikuConfigurationOptions>>().Value.Persistence.ConnectionString,
                                sqlOptions => sqlOptions.MigrationsAssembly(typeof(MediaContext).Assembly.GetName().Name))
                            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking))
                    // I don't know what i'm doing
                    .AddHostedDbSetInitializer<MediaContext, ImageDbSetInitializer>()
                    .AddHostedDbSetInitializer<MediaContext, SongDbSetInitializer>()
                    // Add the other Mediator for speeeed
                    .AddMediatR(config => config.RegisterServicesFromAssemblyContaining<MediaContext>())
                    .AddHostedService<HatsuneMikuDiscordBot>()
                    .AddSingleton<ISongQueueService, SongQueueService>());
}