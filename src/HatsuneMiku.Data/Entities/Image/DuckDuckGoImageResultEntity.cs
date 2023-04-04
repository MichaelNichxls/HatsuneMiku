using GScraper;

namespace HatsuneMiku.Data.Entities.Image;

public sealed class DuckDuckGoImageResultEntity : Entity, IImageResult
{
    public required string Url { get; init; }
    public required string Title { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required string SourceUrl { get; init; }
    public required string ThumbnailUrl { get; init; }
    public required string Source { get; init; }

    public DuckDuckGoImageQueryResultEntity DuckDuckGoImageQuery { get; set; } = null!;
}