using HtmlAgilityPack;
using System.Collections.Generic;

// Models/Fandom or something
// or rename
namespace HatsuneMiku.Shared.Models;

[HasXPath]
public sealed class Fandom
{
    [XPath("//*[contains(@class,'mw-selflink')]")]
    public required string SongCount { get; init; }

    [XPath("//*[contains(@class,'category-page__members')]/div[position()>0]/ul/li/a", "href")]
    public required IEnumerable<string> SongUrlPaths { get; init; }

    [SkipNodeNotFound]
    [XPath("//*[contains(@class,'category-page__pagination-next')]", "href")]
    public required string? PaginationUrl { get; init; }
}