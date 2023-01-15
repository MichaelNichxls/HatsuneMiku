using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku;

public class HatsuneMikuBot : IHostedService, IDisposable
{
    private bool _disposed;

    private readonly IServiceProvider _services;
    private readonly IConfiguration _config;

    public DiscordClient Client { get; private set; }
    public CommandsNextExtension Commands { get; private set; }
    public SlashCommandsExtension SlashCommands { get; private set; }

    // I think this is "right"
    public HatsuneMikuBot(IServiceProvider services, IConfiguration config)
    {
        _services = services;
        _config = config;
    }

    // Make local?
    private Task Client_Ready(DiscordClient sender, ReadyEventArgs e) => Task.CompletedTask;

    private Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
    {
        Console.WriteLine(e.Exception.Message);
        Console.WriteLine(e.Exception.StackTrace);

        return Task.CompletedTask;
    }

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task StartAsync(CancellationToken cancellationToken) => await InitializeAsync();

    public async Task StopAsync(CancellationToken cancellationToken) => await Client.DisconnectAsync();

    // private?
    // Make configurable
    // InitializeClientAsync()?
    // Configure in ctor?
    public async Task InitializeAsync()
    {
        // Look at configurations
        // Move some to appsettings.json
        Client = new(new DiscordConfiguration
        {
            Token = _config["token"],
            Intents = DiscordIntents.All,
            MinimumLogLevel = LogLevel.Debug // Move
        });
        Client.Ready += Client_Ready;
        //Client.ClientErrored

        Commands = Client.UseCommandsNext(new CommandsNextConfiguration
        {
            // I think this is "right"
            StringPrefixes = _config.GetSection("prefixes").Get<string[]>()!,
            Services = _services
        });
        Commands.RegisterCommands(Assembly.GetExecutingAssembly());
        Commands.CommandErrored += Commands_CommandErrored; // Temporary

        SlashCommands = Client.UseSlashCommands(new SlashCommandsConfiguration
        {
            Services = _services
        });
        SlashCommands.RegisterCommands(Assembly.GetExecutingAssembly());

        await Client.ConnectAsync();
    }
}