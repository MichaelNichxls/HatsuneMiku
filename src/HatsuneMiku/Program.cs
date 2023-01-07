using GScraper;
using GScraper.Google;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku;

// Conserve memory usage
internal class Program
{
    // using
    // Change visibilities?
    private static async Task Main(string[] args) =>
        await CreateHostBuilder(args)
            .ConfigureServices(services =>
            {
                using GoogleScraper scraper = new();

                // Temporary
                // Make DB
                // Filter out certain websites
                IDictionary<ImageType, IEnumerable<string>> imageUrls = new Dictionary<ImageType, IEnumerable<string>>
                {
                    [ImageType.Photo]       = scraper.GetImagesAsync("Hatsune Miku Images", type: GoogleImageType.Photo).GetAwaiter().GetResult().Select(image => image.Url),
                    [ImageType.PhotoNsfw]   = scraper.GetImagesAsync("Hatsune Miku Tits", safeSearch: SafeSearchLevel.Off).GetAwaiter().GetResult().Select(image => image.Url),
                    [ImageType.Animated]    = scraper.GetImagesAsync("Hatsune Miku GIFs", type: GoogleImageType.Animated).GetAwaiter().GetResult().Select(image => image.Url)
                };

                services
                    .AddSingleton(imageUrls)
                    .AddHostedService<HatsuneMikuBot>();
            })
            .Build()
            .RunAsync();
            //.ConfigureAwait(false); ?

    // ConfigureAppConfiguration
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args);
            //.ConfigureServices(services => services.AddHostedService<HatsuneMikuBot>());
}