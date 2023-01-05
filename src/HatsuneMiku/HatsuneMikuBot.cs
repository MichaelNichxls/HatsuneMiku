using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using GScraper;
using GScraper.Google;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku;

public class HatsuneMikuBot : IHostedService, IDisposable
{
    private bool _disposed;

    public DiscordClient Client { get; private set; }
    public CommandsNextExtension Commands { get; private set; }
    public SlashCommandsExtension SlashCommands { get; private set; }

    private Task Client_Ready(DiscordClient sender, ReadyEventArgs e) => Task.CompletedTask;

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            Client?.Dispose();
            Commands?.Dispose();

            //System.Threading.Channels.ChannelClosedException
            //HResult = 0x80131509
            //Message = The channel has been closed.
            //Source = System.Threading.Channels
            //StackTrace:
            //at System.Threading.Channels.ChannelWriter`1.Complete(Exception error)
            //at DSharpPlus.CommandsNext.Executors.ParallelQueuedCommandExecutor.Dispose()
            //at DSharpPlus.CommandsNext.CommandsNextExtension.Dispose()
            //at HatsuneMiku.HatsuneMikuBot.Dispose(Boolean disposing) in C: \Users\Michael Nichols\source\repos\HatsuneMiku\src\HatsuneMiku\HatsuneMikuBot.cs:line 40
            //at HatsuneMiku.HatsuneMikuBot.Dispose() in C: \Users\Michael Nichols\source\repos\HatsuneMiku\src\HatsuneMiku\HatsuneMikuBot.cs:line 47
            //at Microsoft.Extensions.DependencyInjection.ServiceLookup.ServiceProviderEngineScope.DisposeAsync()
        }

        _disposed = true;
    }

    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
    public void Dispose() => Dispose(true);

    // private?
    // Make configurable
    // InitializeClientAsync?
    public async Task InitializeAsync()
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
        Client = new(new DiscordConfiguration
        {
            Token = config.Token,
            Intents = DiscordIntents.All,
            MinimumLogLevel = LogLevel.Debug
        });
        Client.Ready += Client_Ready;

        Commands = Client.UseCommandsNext(new CommandsNextConfiguration
        {
            StringPrefixes = new[] { config.Prefix, "39" },
            Services = services
        });
        Commands.RegisterCommands(Assembly.GetExecutingAssembly()); //

        SlashCommands = Client.UseSlashCommands(new SlashCommandsConfiguration
        {
            Services = services
        });
        SlashCommands.RegisterCommands(Assembly.GetExecutingAssembly()); //

        await Client.ConnectAsync();
    }

    public async Task StartAsync(CancellationToken cancellationToken) => await InitializeAsync();

    public async Task StopAsync(CancellationToken cancellationToken) => await Client.DisconnectAsync();
}