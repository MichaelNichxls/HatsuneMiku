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

namespace HatsuneMiku
{
    public class Bot : IDisposable
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
            }

            _disposed = true;
        }

        public void Dispose() => Dispose(true);

        // InitAsync()
        public async Task RunAsync()
        {
            // ReadOnlySpan?
            string configJson = await File.ReadAllTextAsync(Path.Combine(Program.ProjectDirectory, "config.json"), new UTF8Encoding(false)).ConfigureAwait(false);
            Config config = JsonSerializer.Deserialize<Config>(configJson);

            // Look at configs

            // using
            Client = new(new DiscordConfiguration
            {
                Token = config.Token,
                Intents = DiscordIntents.All,
                MinimumLogLevel = LogLevel.Debug
            });
            Client.Ready += Client_Ready;

            // using
            Commands = Client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { config.Prefix, "39" },
                //EnableMentionPrefix = false
            });
            Commands.RegisterCommands(Assembly.GetExecutingAssembly()); // ?

            SlashCommands = Client.UseSlashCommands();
            SlashCommands.RegisterCommands(Assembly.GetExecutingAssembly()); // ?

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}