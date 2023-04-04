using GScraper;
using GScraper.DuckDuckGo;
using HatsuneMiku.Shared.GScraper;
using System.Collections.Generic;

namespace HatsuneMiku.Data.Entities.Image;

public sealed class DuckDuckGoImageQueryEntity : Entity, IImageQuery
{
    public required string Query { get; init; }
    public SafeSearchLevel SafeSearch { get; init; }        = SafeSearchLevel.Moderate;
    public DuckDuckGoImageTime Time { get; init; }          = DuckDuckGoImageTime.Any;
    public DuckDuckGoImageSize Size { get; init; }          = DuckDuckGoImageSize.All;
    public DuckDuckGoImageColor Color { get; init; }        = DuckDuckGoImageColor.All;
    public DuckDuckGoImageType Type { get; init; }          = DuckDuckGoImageType.All;
    public DuckDuckGoImageLayout Layout { get; init; }      = DuckDuckGoImageLayout.All;
    public DuckDuckGoImageLicense License { get; init; }    = DuckDuckGoImageLicense.All;
    public string Region { get; init; }                     = DuckDuckGoRegions.UsEnglish;

    public ICollection<DuckDuckGoImageQueryResultEntity> DuckDuckGoImageResults { get; set; } = new List<DuckDuckGoImageQueryResultEntity>();
}