using HatsuneMiku.Data;
using HatsuneMiku.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace HatsuneMiku;

internal class Program
{
    // using
    // Change visibilities?
    public static async Task Main(string[] args) =>
        await CreateHostBuilder(args).Build().RunAsync();

    // ConfigureAppConfiguration
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host
            .CreateDefaultBuilder(args)
             // Add environment variables?
            .ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.json"/*, true?*/))
            .ConfigureServices(
                services => services
                    .AddHostedService<HatsuneMikuBot>()
                    // AddDbContextFactory()?
                    .AddDbContext<ImageContext>( 
                        options => options
#if DEBUG
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors()
#endif
                            .UseSqlServer(
                                // Relocate to appsettings.json
                                $@"Server=(localdb)\mssqllocaldb;Database={nameof(ImageContext)};Trusted_Connection=True;MultipleActiveResultSets=true",
                                sqlOptions => sqlOptions.MigrationsAssembly(typeof(ImageContext).Assembly.GetName().Name))
                            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
                        // Temporary
                        ServiceLifetime.Singleton)
                    // Should this be a singleton?
                    .AddSingleton<IImageService, ImageService>());
}