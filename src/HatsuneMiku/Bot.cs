using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HatsuneMiku;

public class Bot : IDisposable
{
    private bool _disposed;

    public DiscordClient Client { get; }
    public CommandsNextExtension Commands { get; }
    public SlashCommandsExtension SlashCommands { get; }

    // InitAsync()?
    // No await :(
    // Make configurable
    public Bot()
    {
        // ReadOnlySpan?
        string configJson = File.ReadAllText(Path.Combine(Program.ProjectDirectory, "config.json"), new UTF8Encoding(false));
        Config config = JsonSerializer.Deserialize<Config>(configJson);

        // Look at configs

        Client = new(new DiscordConfiguration
        {
            Token = config.Token,
            Intents = DiscordIntents.All,
            MinimumLogLevel = LogLevel.Debug
        });
        Client.Ready += Client_Ready;

        Commands = Client.UseCommandsNext(new CommandsNextConfiguration
        {
            StringPrefixes = new[] { config.Prefix, "39" }
        });
        Commands.RegisterCommands(Assembly.GetExecutingAssembly()); // ?

        SlashCommands = Client.UseSlashCommands();
        SlashCommands.RegisterCommands(Assembly.GetExecutingAssembly()); // ?
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