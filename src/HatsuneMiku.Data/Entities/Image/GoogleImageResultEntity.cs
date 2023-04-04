using GScraper;

namespace HatsuneMiku.Data.Entities.Image;

public sealed class GoogleImageResultEntity : Entity, IImageResult
{
    public required string Url { get; init; }
    public required string Title { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required Color? Color { get; init; }
    public required string DisplayUrl { get; init; }
    public required string SourceUrl { get; init; }
    public required string ThumbnailUrl { get; init; }

    public GoogleImageQueryResultEntity GoogleImageQuery { get; set; } = null!;
}