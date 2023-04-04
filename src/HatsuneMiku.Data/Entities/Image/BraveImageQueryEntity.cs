using GScraper;
using GScraper.Brave;
using HatsuneMiku.Shared.GScraper;
using System.Collections.Generic;

namespace HatsuneMiku.Data.Entities.Image;

public sealed class BraveImageQueryEntity : Entity, IImageQuery
{
    public required string Query { get; init; }
    public SafeSearchLevel SafeSearch { get; init; }    = SafeSearchLevel.Moderate;
    public string? Country { get; init; }               = null;
    public BraveImageSize Size { get; init; }           = BraveImageSize.All;
    public BraveImageType Type { get; init; }           = BraveImageType.All;
    public BraveImageLayout Layout { get; init; }       = BraveImageLayout.All;
    public BraveImageColor Color { get; init; }         = BraveImageColor.All;
    public BraveImageLicense License { get; init; }     = BraveImageLicense.All;

    // IReadOnlyList
    public ICollection<BraveImageQueryResultEntity> BraveImageResults { get; set; } = new List<BraveImageQueryResultEntity>();
}