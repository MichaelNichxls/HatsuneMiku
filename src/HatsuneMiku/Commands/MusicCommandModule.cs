using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using HatsuneMiku.Commands.Attributes;
using HatsuneMiku.Data.Entities.Music;
using HatsuneMiku.Data.MediatR.Requests.Music;
using HatsuneMiku.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands;

[RequireGuild]
[RequireLavalinkNodeConnection]
[Category("Music commands")]
public sealed class MusicCommandModule : BaseCommandModule
{
    // Add Ready event or RequireAttribute

    private readonly IMediator _mediator;
    private readonly ISongQueueService _songQueueService;

    public MusicCommandModule(IMediator mediator, ISongQueueService songQueueService)
    {
        _mediator           = mediator;
        _songQueueService   = songQueueService;
    }

    private async Task LavalinkGuildConnection_PlaybackStarted(LavalinkGuildConnection sender, TrackStartEventArgs e)
    {
        // vs sender.Channel
        await e.Player.Channel.SendMessageAsync($"Now playing {e.Track.Title}").ConfigureAwait(false);
    }

    // Make extension: GetDistinctItems() and GetItem()
    private async Task LavalinkGuildConnection_PlaybackFinished(LavalinkGuildConnection sender, TrackFinishEventArgs e)
    {
        if (e.Reason is not TrackEndReason.Finished)
            return;

        LavalinkTrack track = await GetTrackAsync(sender.Node.Rest).ConfigureAwait(false);
        await sender.PlayAsync(track).ConfigureAwait(false);
    }

    // Make service somehow
    // Cache discord bot properties
    // return LoadResult instead
    private async Task<LavalinkTrack> GetTrackAsync(LavalinkRestClient rest)
    {
        // Extension
        return await _songQueueService.DequeueAsync(FallbackAsync).ConfigureAwait(false);

        async Task<LavalinkTrack> FallbackAsync()
        {
            IEnumerable<SongEntity> songs = await _mediator.Send(new GetAllSongs.Request()).ConfigureAwait(false);

            // How to do this better
            foreach (SongEntity song in Random.Shared.GetItems(songs.ToArray(), 20))
            {
                // Extension, idk
                string? url = song.YouTubeUrl ?? song.SoundCloudUrl;

                if (url is null)
                    continue;

                // ToUri(string)
                LavalinkLoadResult loadResult = await rest.GetTracksAsync(new Uri(url)).ConfigureAwait(false);

                if (loadResult.LoadResultType is LavalinkLoadResultType.LoadFailed or LavalinkLoadResultType.NoMatches)
                    continue;

                _songQueueService.Enqueue(loadResult.Tracks.First());
            }

            return _songQueueService.Dequeue();
        }
    }

    [RequireVoiceState]
    [Command, Aliases("connect", "join", "song", "vocaloid")]
    [Description("")]
    public async Task PlayAsync(CommandContext ctx)
    {
        // Extension or helper
        DiscordVoiceState voiceState            = ctx.Member!.VoiceState;
        LavalinkExtension lavalink              = ctx.Client.GetLavalink();
        LavalinkNodeConnection nodeConnection   = lavalink.ConnectedNodes.Values.First();
        LavalinkGuildConnection guildConnection = await nodeConnection.ConnectAsync(voiceState.Channel).ConfigureAwait(false);
        LavalinkTrack track                     = await GetTrackAsync(nodeConnection.Rest).ConfigureAwait(false);

        await guildConnection.PlayAsync(track).ConfigureAwait(false);

        // Move to ctor?
        guildConnection.PlaybackStarted     -= LavalinkGuildConnection_PlaybackStarted;
        guildConnection.PlaybackStarted     += LavalinkGuildConnection_PlaybackStarted;
        guildConnection.PlaybackFinished    -= LavalinkGuildConnection_PlaybackFinished;
        guildConnection.PlaybackFinished    += LavalinkGuildConnection_PlaybackFinished;
    }

    [RequireVoiceState]
    [RequireLavalinkGuildConnection]
    [Command, Aliases("disconnect", "leave")]
    [Description("")]
    public async Task StopAsync(CommandContext ctx)
    {
        DiscordVoiceState voiceState            = ctx.Member!.VoiceState;
        LavalinkExtension lavalink              = ctx.Client.GetLavalink();
        LavalinkNodeConnection nodeConnection   = lavalink.ConnectedNodes.Values.First();
        LavalinkGuildConnection guildConnection = nodeConnection.GetGuildConnection(voiceState.Guild);

        // Necessary?
        await guildConnection.StopAsync().ConfigureAwait(false);
        await guildConnection.DisconnectAsync().ConfigureAwait(false);

        // Try removing and/or move above this^
        guildConnection.PlaybackStarted     -= LavalinkGuildConnection_PlaybackStarted;
        guildConnection.PlaybackFinished    -= LavalinkGuildConnection_PlaybackFinished;
    }

    [RequireVoiceState]
    [RequireLavalinkGuildConnection]
    [Command, Aliases("next")]
    [Description("")]
    public async Task SkipAsync(CommandContext ctx)
    {
        DiscordVoiceState voiceState            = ctx.Member!.VoiceState;
        LavalinkExtension lavalink              = ctx.Client.GetLavalink();
        LavalinkNodeConnection nodeConnection   = lavalink.ConnectedNodes.Values.First();
        LavalinkGuildConnection guildConnection = nodeConnection.GetGuildConnection(voiceState.Guild);
        LavalinkTrack track                     = await GetTrackAsync(nodeConnection.Rest).ConfigureAwait(false);

        await guildConnection.PlayAsync(track).ConfigureAwait(false);
    }

    [RequireVoiceState]
    [RequireLavalinkGuildConnection]
    [Command]
    [Description("")]
    public async Task PauseAsync(CommandContext ctx)
    {
        DiscordVoiceState voiceState            = ctx.Member!.VoiceState;
        LavalinkExtension lavalink              = ctx.Client.GetLavalink();
        LavalinkNodeConnection nodeConnection   = lavalink.ConnectedNodes.Values.First();
        LavalinkGuildConnection guildConnection = nodeConnection.GetGuildConnection(voiceState.Guild);

        await guildConnection.PauseAsync().ConfigureAwait(false);
    }

    [RequireVoiceState]
    [RequireLavalinkGuildConnection]
    [Command, Aliases("continue")]
    [Description("")]
    public async Task ResumeAsync(CommandContext ctx)
    {
        DiscordVoiceState voiceState            = ctx.Member!.VoiceState;
        LavalinkExtension lavalink              = ctx.Client.GetLavalink();
        LavalinkNodeConnection nodeConnection   = lavalink.ConnectedNodes.Values.First();
        LavalinkGuildConnection guildConnection = nodeConnection.GetGuildConnection(voiceState.Guild);

        await guildConnection.ResumeAsync().ConfigureAwait(false);
    }

    //if (guildConnection.CurrentState.CurrentTrack is null)
    //{
    //    await ctx.RespondAsync("There are no tracks loaded.").ConfigureAwait(false);
    //    return;
    //}
}