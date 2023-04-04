using GScraper;
using System;

namespace HatsuneMiku.Data.Entities.Image;

public sealed class BraveImageResultEntity : Entity, IImageResult
{
    public required string Url { get; init; }
    public required string Title { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required string SourceUrl { get; init; }
    public required DateTimeOffset PageAge { get; init; }
    public required string Source { get; init; }
    public required string ThumbnailUrl { get; init; }
    public required string ResizedUrl { get; init; }
    public required string Format { get; init; }

    public BraveImageQueryResultEntity BraveImageQuery { get; set; } = null!;
}