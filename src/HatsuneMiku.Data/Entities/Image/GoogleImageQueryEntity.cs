using GScraper;
using GScraper.Google;
using HatsuneMiku.Shared.GScraper;
using System.Collections.Generic;

namespace HatsuneMiku.Data.Entities.Image;

public sealed class GoogleImageQueryEntity : Entity, IImageQuery
{
    public required string Query { get; init; }
    public SafeSearchLevel SafeSearch { get; init; }    = SafeSearchLevel.Off;
    public GoogleImageSize Size { get; init; }          = GoogleImageSize.Any;
    public string? Color { get; init; }                 = null;
    public GoogleImageType Type { get; init; }          = GoogleImageType.Any;
    public GoogleImageTime Time { get; init; }          = GoogleImageTime.Any;
    public string? License { get; init; }               = null;
    public string? Language { get; init; }              = null;

    public ICollection<GoogleImageQueryResultEntity> GoogleImageResults { get; set; } = new List<GoogleImageQueryResultEntity>();
}