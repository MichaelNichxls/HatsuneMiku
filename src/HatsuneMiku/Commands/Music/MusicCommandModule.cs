using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands.Music;

public class MusicCommandModule : BaseCommandModule
{
    [Command]
    public async Task Join(CommandContext ctx, DiscordChannel channel)
    {
        LavalinkExtension lavalink = ctx.Client.GetLavalink(); // Shorthand

        if (!lavalink.ConnectedNodes.Any())
        {
            await ctx.RespondAsync("The Lavalink connection is not established.");
            return;
        }

        LavalinkNodeConnection node = lavalink.ConnectedNodes.Values.First();

        if (channel.Type != ChannelType.Voice)
        {
            await ctx.RespondAsync("Not a valid voice channel.");
            return;
        }

        await node.ConnectAsync(channel); // .ConfigureAwait(false);
        await ctx.RespondAsync($"Joined {channel.Name}!");
    }

    [Command]
    public async Task Leave(CommandContext ctx, DiscordChannel channel)
    {
        LavalinkExtension lavalink = ctx.Client.GetLavalink();

        if (!lavalink.ConnectedNodes.Any())
        {
            await ctx.RespondAsync("The Lavalink connection is not established.");
            return;
        }

        LavalinkNodeConnection node = lavalink.ConnectedNodes.Values.First();

        if (channel.Type != ChannelType.Voice)
        {
            await ctx.RespondAsync("Not a valid voice channel.");
            return;
        }

        LavalinkGuildConnection connection = node.GetGuildConnection(channel.Guild);

        if (connection is null)
        {
            await ctx.RespondAsync("Lavalink is not connected.");
            return;
        }

        await connection.DisconnectAsync();
        await ctx.RespondAsync($"Left {channel.Name}!");
    }

    [Command]
    public async Task Play(CommandContext ctx, [RemainingText] string query)
    {
        if (ctx.Member is { VoiceState: null or { Channel: null } })
        {
            await ctx.RespondAsync("You are not in a voice channel.");
            return;
        }

        LavalinkExtension lavalink = ctx.Client.GetLavalink();
        LavalinkNodeConnection node = lavalink.ConnectedNodes.Values.First();
        LavalinkGuildConnection connection = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

        if (connection is null)
        {
            await ctx.RespondAsync("Lavalink is not connected.");
            return;
        }

        LavalinkLoadResult loadResult = await node.Rest.GetTracksAsync(query);

        if (loadResult.LoadResultType is LavalinkLoadResultType.LoadFailed or LavalinkLoadResultType.NoMatches)
        {
            await ctx.RespondAsync($"Track search failed for {query}.");
            return;
        }

        LavalinkTrack track = loadResult.Tracks.First();
        
        await connection.PlayAsync(track);
        await ctx.RespondAsync($"Now playing {track.Title}!");
    }

    [Command]
    public async Task Play(CommandContext ctx, Uri url)
    {
        if (ctx.Member is { VoiceState: null or { Channel: null } })
        {
            await ctx.RespondAsync("You are not in a voice channel.");
            return;
        }

        LavalinkExtension lavalink = ctx.Client.GetLavalink();
        LavalinkNodeConnection node = lavalink.ConnectedNodes.Values.First();
        LavalinkGuildConnection connection = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

        if (connection is null)
        {
            await ctx.RespondAsync("Lavalink is not connected.");
            return;
        }

        LavalinkLoadResult loadResult = await node.Rest.GetTracksAsync(url);

        if (loadResult.LoadResultType is LavalinkLoadResultType.LoadFailed or LavalinkLoadResultType.NoMatches)
        {
            await ctx.RespondAsync($"Track search failed for {url.OriginalString}.");
            return;
        }

        LavalinkTrack track = loadResult.Tracks.First();

        await connection.PlayAsync(track);
        await ctx.RespondAsync($"Now playing {track.Title}!");
    }

    [Command]
    public async Task Pause(CommandContext ctx)
    {
        if (ctx.Member is { VoiceState: null or { Channel: null } })
        {
            await ctx.RespondAsync("You are not in a voice channel.");
            return;
        }

        LavalinkExtension lavalink = ctx.Client.GetLavalink();
        LavalinkNodeConnection node = lavalink.ConnectedNodes.Values.First();
        LavalinkGuildConnection connection = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

        if (connection is null)
        {
            await ctx.RespondAsync("Lavalink is not connected.");
            return;
        }

        if (connection.CurrentState.CurrentTrack is null)
        {
            await ctx.RespondAsync("There are no tracks loaded.");
            return;
        }

        await connection.PauseAsync();
    }

    [Command]
    public async Task Resume(CommandContext ctx)
    {
        if (ctx.Member is { VoiceState: null or { Channel: null } })
        {
            await ctx.RespondAsync("You are not in a voice channel.");
            return;
        }

        LavalinkExtension lavalink = ctx.Client.GetLavalink();
        LavalinkNodeConnection node = lavalink.ConnectedNodes.Values.First();
        LavalinkGuildConnection connection = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

        if (connection is null)
        {
            await ctx.RespondAsync("Lavalink is not connected.");
            return;
        }

        if (connection.CurrentState.CurrentTrack is null)
        {
            await ctx.RespondAsync("There are no tracks loaded.");
            return;
        }

        await connection.ResumeAsync();
    }
}