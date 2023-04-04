using System.ComponentModel.DataAnnotations.Schema;

namespace HatsuneMiku.Data.Entities.Image;

public sealed class BraveImageQueryResultEntity : Entity
{
    public required int BraveImageQueryId { get; init; }
    [ForeignKey(nameof(BraveImageQueryId))]
    public BraveImageQueryEntity BraveImageQuery { get; set; } = null!;

    public required int BraveImageResultId { get; init; }
    [ForeignKey(nameof(BraveImageResultId))]
    public BraveImageResultEntity BraveImageResult { get; set; } = null!;
}