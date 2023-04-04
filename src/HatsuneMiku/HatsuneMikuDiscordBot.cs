using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using HatsuneMiku.Shared.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace HatsuneMiku;

public sealed class HatsuneMikuDiscordBot : IHostedService, IDisposable
{
    private readonly IServiceProvider _provider;
    private readonly IOptions<HatsuneMikuConfigurationOptions> _options;

    private bool _disposed;

    public DiscordClient Client { get; }
    public LavalinkExtension Lavalink { get; }
    public CommandsNextExtension Commands { get; }
    public SlashCommandsExtension SlashCommands { get; }
    public ConnectionEndpoint ConnectionEndpoint { get; }

    public HatsuneMikuDiscordBot(IServiceProvider provider, IOptions<HatsuneMikuConfigurationOptions> options)
    {
        _provider   = provider;
        _options    = options;

        Client = new(new DiscordConfiguration
        {
            Token           = _options.Value.Discord.Token,
            Intents         = _options.Value.Discord.Intents.Aggregate((x, y) => x | y),
            MinimumLogLevel =
#if DEBUG
                Microsoft.Extensions.Logging.LogLevel.Debug
#else
                _options.Value.Logging.LogLevel.Default
#endif
        });
        Client.Ready            += Client_Ready;
        Client.ClientErrored    += Client_ClientErrored;

        // Make not required
        Lavalink = Client.UseLavalink();

        Commands = Client.UseCommandsNext(new CommandsNextConfiguration
        {
            StringPrefixes  = _options.Value.Discord.CommandPrefixes,
            Services        = _provider
        });
        Commands.RegisterCommands(Assembly.GetExecutingAssembly());
        Commands.CommandErrored += Commands_CommandErrored;

        SlashCommands = Client.UseSlashCommands(new SlashCommandsConfiguration
        {
            Services = _provider
        });
        SlashCommands.RegisterCommands(Assembly.GetExecutingAssembly());
        SlashCommands.SlashCommandErrored += SlashCommands_SlashCommandErrored;

        ConnectionEndpoint = new()
        {
            Port        = _options.Value.Lavalink.ConnectionEndpoint.Port,
            Hostname    = _options.Value.Lavalink.ConnectionEndpoint.Address
        };
    }

    private Task Client_Ready(DiscordClient sender, ReadyEventArgs e) =>
        Task.CompletedTask;

    // Use Logger
    private Task Client_ClientErrored(DiscordClient sender, ClientErrorEventArgs e)
    {
        Console.WriteLine($"{e.Exception.Message}\n{e.Exception.StackTrace}");
        return Task.CompletedTask;
    }

    private Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
    {
        Console.WriteLine($"{e.Exception.Message}\n{e.Exception.StackTrace}");
        return Task.CompletedTask;
    }

    private Task SlashCommands_SlashCommandErrored(SlashCommandsExtension sender, SlashCommandErrorEventArgs e)
    {
        Console.WriteLine($"{e.Exception.Message}\n{e.Exception.StackTrace}");
        return Task.CompletedTask;
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            Client?.Dispose();

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Client.ConnectAsync();
        await Lavalink.ConnectAsync(new LavalinkConfiguration
        {
            Password        = _options.Value.Lavalink.Password,
            RestEndpoint    = ConnectionEndpoint,
            SocketEndpoint  = ConnectionEndpoint
        });
    }

    // Handle Lavalink
    public async Task StopAsync(CancellationToken cancellationToken) =>
        await Client.DisconnectAsync();
}