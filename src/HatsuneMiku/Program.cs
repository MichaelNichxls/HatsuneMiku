using GScraper;
using GScraper.Google;
using HatsuneMiku.Data;
using HatsuneMiku.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku;

// InternalsVisibleTo
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
            .ConfigureServices(services =>
            {
                using GoogleScraper scraper = new();

                // Temporary
                // Filter out certain websites
                IDictionary<ImageType, IEnumerable<string>> imageUrls = new Dictionary<ImageType, IEnumerable<string>>
                {
                    [ImageType.Photo]       = scraper.GetImagesAsync("Hatsune Miku Images", type: GoogleImageType.Photo).GetAwaiter().GetResult().Select(image => image.Url),
                    [ImageType.PhotoNsfw]   = scraper.GetImagesAsync("Hatsune Miku Tits", safeSearch: SafeSearchLevel.Off).GetAwaiter().GetResult().Select(image => image.Url),
                    [ImageType.Animated]    = scraper.GetImagesAsync("Hatsune Miku GIFs", type: GoogleImageType.Animated).GetAwaiter().GetResult().Select(image => image.Url)
                };

                services
                    .AddHostedService<HatsuneMikuBot>()
                    // AddDbContextFactory()?
                    .AddDbContext<ImageContext>(options =>
                        options.UseSqlServer(
                            $@"Server=(localdb)\mssqllocaldb;Database={nameof(ImageContext)};Trusted_Connection=True;MultipleActiveResultSets=true",
                            sqlOptions => sqlOptions.MigrationsAssembly(typeof(ImageContext).Assembly.GetName().Name)))
                    .AddSingleton(imageUrls);
            });
}