using DSharpPlus.Lavalink;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace HatsuneMiku.Services;

public interface ISongQueueService
{
    void Enqueue(LavalinkTrack track);
    LavalinkTrack Dequeue();
    LavalinkTrack Dequeue(Func<LavalinkTrack> fallback);
    Task<LavalinkTrack> DequeueAsync(Func<Task<LavalinkTrack>> fallback);
    LavalinkTrack Peek();
    LavalinkTrack Peek(Func<LavalinkTrack> fallback);
    Task<LavalinkTrack> PeekAsync(Func<Task<LavalinkTrack>> fallback);
    bool TryDequeue([NotNullWhen(true)] out LavalinkTrack? result);
    bool TryPeek([NotNullWhen(true)] out LavalinkTrack? result);
}

// SUBJECT FOR REMOVAL

// QueueService?
// LavalinkTrackQueueService?
//                                                                              v interface v
public sealed class SongQueueService : ISongQueueService//, INotificationHandler<ProgressChanged.Notification<SongDbSetInitializer, ProgressContext>>
{
    private readonly ISender _sender;
    private readonly Queue<LavalinkTrack> _queue = new();

    // events? or notifications?

    // Publisher too, with queue
    // or just mediator ffs
    public SongQueueService(ISender sender) =>
        _sender = sender;

    public void Enqueue(LavalinkTrack track) =>
        _queue.Enqueue(track);

    // Use events
    public LavalinkTrack Dequeue() =>
        _queue.Dequeue();

    public LavalinkTrack Dequeue(Func<LavalinkTrack> fallback) =>
        _queue.TryDequeue(out LavalinkTrack? track)
            ? track
            : fallback();

    public async Task<LavalinkTrack> DequeueAsync(Func<Task<LavalinkTrack>> fallback) =>
        _queue.TryDequeue(out LavalinkTrack? track)
            ? track
            : await fallback().ConfigureAwait(false);

    public LavalinkTrack Peek() =>
        _queue.Peek();

    public LavalinkTrack Peek(Func<LavalinkTrack> fallback) =>
        _queue.TryPeek(out LavalinkTrack? track)
            ? track
            : fallback();

    public async Task<LavalinkTrack> PeekAsync(Func<Task<LavalinkTrack>> fallback) =>
        _queue.TryPeek(out LavalinkTrack? track)
            ? track
            : await fallback().ConfigureAwait(false);

    public bool TryDequeue([NotNullWhen(true)] out LavalinkTrack? result) =>
        _queue.TryDequeue(out result);

    public bool TryPeek([NotNullWhen(true)] out LavalinkTrack? result) =>
        _queue.TryPeek(out result);
}