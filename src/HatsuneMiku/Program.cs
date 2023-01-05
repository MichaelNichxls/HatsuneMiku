using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace HatsuneMiku;

// Conserve memory usage
// public?
internal class Program
{
    //private static async Task Main(string[] args)
    //{
    //    using (HatsuneMikuBot bot = await HatsuneMikuBot.CreateAsync())
    //        await bot.RunAsync();
    //}

    // private?
    public static async Task Main(string[] args) =>
        await CreateHostBuilder(args).Build().RunAsync().ConfigureAwait(false);

    // ConfigureAppConfiguration
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(services => services.AddHostedService<HatsuneMikuBot>());
}