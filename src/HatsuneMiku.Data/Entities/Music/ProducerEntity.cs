using System.Collections.Generic;

namespace HatsuneMiku.Data.Entities.Music;

public sealed class ProducerEntity : Entity
{
    public required string Name { get; init; }

    public ICollection<SongProducerEntity> Songs { get; set; } = new List<SongProducerEntity>();
}