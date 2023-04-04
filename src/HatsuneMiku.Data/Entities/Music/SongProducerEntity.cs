using System.ComponentModel.DataAnnotations.Schema;

namespace HatsuneMiku.Data.Entities.Music;

public sealed class SongProducerEntity : Entity
{
    public required int SongId { get; init; }
    [ForeignKey(nameof(SongId))]
    public SongEntity Song { get; init; } = null!;

    public required int ProducerId { get; init; }
    [ForeignKey(nameof(ProducerId))]
    public ProducerEntity Producer { get; init; } = null!;
}