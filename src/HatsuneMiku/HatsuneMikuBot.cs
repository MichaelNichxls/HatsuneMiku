using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using GScraper;
using GScraper.Google;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HatsuneMiku;

public class HatsuneMikuBot : IDisposable
{
    private bool _disposed;

    public DiscordClient Client { get; private set; }
    public CommandsNextExtension Commands { get; private set; }
    public SlashCommandsExtension SlashCommands { get; private set; }

    // InitAsync()?
    // Make configurable
    private HatsuneMikuBot()
    {
    }

    public static async Task<HatsuneMikuBot> CreateAsync()
    {
        // ReadOnlySpan?
        // ConfigureAwait(false)?
        string configJson = await File.ReadAllTextAsync("config.json", new UTF8Encoding(false));
        Config config = JsonSerializer.Deserialize<Config>(configJson);

        // Make service
        // Make DB
        // Filter out certain websites
        using GoogleScraper scraper = new();
        // Rename
        IEnumerable<string> mikuImageUrls       = (await scraper.GetImagesAsync("Hatsune Miku Images", type: GoogleImageType.Photo)).Select(image => image.Url);
        IEnumerable<string> mikuImageNsfwUrls   = (await scraper.GetImagesAsync("Hatsune Miku Tits", safeSearch: SafeSearchLevel.Off)).Select(image => image.Url);
        IEnumerable<string> mikuGifUrls         = (await scraper.GetImagesAsync("Hatsune Miku GIFs", type: GoogleImageType.Animated)).Select(image => image.Url);

        // Look over
        ServiceProvider services = new ServiceCollection()
            .AddSingleton(mikuImageUrls)
            .AddSingleton(mikuImageNsfwUrls)
            .AddSingleton(mikuGifUrls)
            // Transient
            // Access by reference
            .AddSingleton<ImageUrlServiceResolver>(serviceProvider => key => key switch
            {
                ImageType.Photo     => serviceProvider.GetServices<IEnumerable<string>>().ElementAt(0),
                ImageType.PhotoNsfw => serviceProvider.GetServices<IEnumerable<string>>().ElementAt(1),
                ImageType.Animated  => serviceProvider.GetServices<IEnumerable<string>>().ElementAt(2),
                _                   => throw new KeyNotFoundException()
            })
            .BuildServiceProvider();

        // Look at configurations
        HatsuneMikuBot bot = new();

        bot.Client = new(new DiscordConfiguration
        {
            Token = config.Token,
            Intents = DiscordIntents.All,
            MinimumLogLevel = LogLevel.Debug
        });
        bot.Client.Ready += bot.Client_Ready;

        bot.Commands = bot.Client.UseCommandsNext(new CommandsNextConfiguration
        {
            StringPrefixes = new[] { config.Prefix, "39" },
            Services = services
        });
        bot.Commands.RegisterCommands(Assembly.GetExecutingAssembly()); //

        bot.SlashCommands = bot.Client.UseSlashCommands(new SlashCommandsConfiguration
        {
            Services = services
        });
        bot.SlashCommands.RegisterCommands(Assembly.GetExecutingAssembly()); //

        return bot;
    }

    private Task Client_Ready(DiscordClient sender, ReadyEventArgs e) => Task.CompletedTask;

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            Client?.Dispose();
            Commands?.Dispose();
        }

        _disposed = true;
    }

    public void Dispose() => Dispose(true);

    public async Task RunAsync()
    {
        await Client.ConnectAsync();
        await Task.Delay(-1);
    }
}