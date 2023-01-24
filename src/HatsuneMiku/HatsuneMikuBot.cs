using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
    public LavalinkExtension Lavalink { get; private set; }
    public CommandsNextExtension Commands { get; private set; }
    public SlashCommandsExtension SlashCommands { get; private set; }

    public HatsuneMikuBot(IServiceProvider services, IConfiguration config)
    {
        _services = services;
        _config = config;
    }

    // Make local?
    private Task Client_Ready(DiscordClient sender, ReadyEventArgs e) => Task.CompletedTask;

    private Task Client_ClientErrored(DiscordClient sender, ClientErrorEventArgs e)
    {
        Console.WriteLine(e.Exception.Message);
        Console.WriteLine(e.Exception.StackTrace);

        return Task.CompletedTask;
    }

    private Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
    {
        Console.WriteLine(e.Exception.Message);
        Console.WriteLine(e.Exception.StackTrace);

        return Task.CompletedTask;
    }

    private Task SlashCommands_SlashCommandErrored(SlashCommandsExtension sender, SlashCommandErrorEventArgs e)
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

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Client.DisconnectAsync();
        //await Lavalink.StopAsync();
    }

    // private?
    // Make configurable
    // InitializeClientAsync()?
    // Configure in ctor?
    // Separate code
    public async Task InitializeAsync()
    {
        // Look at configurations
        // Move some to appsettings.json
        Client = new(new DiscordConfiguration
        {
            Token = _config[$"{nameof(HatsuneMiku)}:Discord:BotToken"],
            Intents = DiscordIntents.All,
            MinimumLogLevel =
#if DEBUG
                LogLevel.Debug
#else
                Enum.Parse<LogLevel>(_config[$"{nameof(HatsuneMiku)}:LogLevel"]!)
#endif
        });
        Client.Ready += Client_Ready;
        Client.ClientErrored += Client_ClientErrored;

        // Async
        Lavalink = Client.UseLavalink();

        Commands = Client.UseCommandsNext(new CommandsNextConfiguration
        {
            StringPrefixes = _config.GetSection($"{nameof(HatsuneMiku)}:Discord:CommandPrefixes").Get<IEnumerable<string>>()!,
            Services = _services
        });
        Commands.RegisterCommands(Assembly.GetExecutingAssembly());
        Commands.CommandErrored += Commands_CommandErrored;

        SlashCommands = Client.UseSlashCommands(new SlashCommandsConfiguration
        {
            Services = _services
        });
        SlashCommands.RegisterCommands(Assembly.GetExecutingAssembly());
        SlashCommands.SlashCommandErrored += SlashCommands_SlashCommandErrored;

        ConnectionEndpoint endpoint = new()
        {
            // GetRequiredSection()
            Port = _config.GetSection($"{nameof(HatsuneMiku)}:Rest:Port").Get<int>(),
            Hostname = _config[$"{nameof(HatsuneMiku)}:Rest:Address"]
        };

        await Client.ConnectAsync();
        await Lavalink.ConnectAsync(new LavalinkConfiguration
        {
            Password = _config[$"{nameof(HatsuneMiku)}:Lavalink:Password"],
            RestEndpoint = endpoint,
            SocketEndpoint = endpoint
        });
    }
}