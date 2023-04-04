using System.Collections.Generic;

namespace HatsuneMiku.Data.Entities.Music;

public sealed class SongEntity : Entity
{
    public required string Title { get; init; }
    public required string? YouTubeUrl { get; init; }
    public required string? SoundCloudUrl { get; init; }
    public required string? NiconicoUrl { get; init; }

    public ICollection<SongProducerEntity> Producers { get; set; } = new List<SongProducerEntity>();
}