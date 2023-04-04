using System.ComponentModel.DataAnnotations.Schema;

namespace HatsuneMiku.Data.Entities.Image;

public sealed class DuckDuckGoImageQueryResultEntity : Entity
{
    public required int DuckDuckGoImageQueryId { get; init; }
    [ForeignKey(nameof(DuckDuckGoImageQueryId))]
    public DuckDuckGoImageQueryEntity DuckDuckGoImageQuery { get; init; } = null!;

    public required int DuckDuckGoImageResultId { get; init; }
    [ForeignKey(nameof(DuckDuckGoImageResultId))]
    public DuckDuckGoImageResultEntity DuckDuckGoImageResult { get; init; } = null!;
}