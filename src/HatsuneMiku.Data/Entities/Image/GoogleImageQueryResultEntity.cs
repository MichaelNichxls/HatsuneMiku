using System.ComponentModel.DataAnnotations.Schema;

namespace HatsuneMiku.Data.Entities.Image;

public sealed class GoogleImageQueryResultEntity : Entity
{
    public required int GoogleImageQueryId { get; init; }
    [ForeignKey(nameof(GoogleImageQueryId))]
    public GoogleImageQueryEntity GoogleImageQuery { get; init; } = null!;

    public required int GoogleImageResultId { get; init; }
    [ForeignKey(nameof(GoogleImageResultId))]
    public GoogleImageResultEntity GoogleImageResult { get; init; } = null!;
}