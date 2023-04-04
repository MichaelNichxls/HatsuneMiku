using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HatsuneMiku.Shared.Models;

[HasXPath]
public sealed partial class Song
{
    private readonly string _title;
    private readonly IEnumerable<string> _producers;

    [XPath("//*[@data-source='title']/div")]
    public required string Title
    {
        get     => FirstOrLastQuotationMarkRegex().Replace(_title, string.Empty);
        init    => _title = value;
    }

    [XPath("//*[@data-source='producers']/div/ul/li[position()>0]")]
    public required IEnumerable<string> Producers
    {
        get     => _producers.Select(p => InnerParenthesesOrColonRegex().Replace(p, string.Empty)).Distinct();
        init    => _producers = value;
    }

    [SkipNodeNotFound]
    [XPath("//*[@data-source='links']/div/ul/li[not(contains(.,'private') or contains(.,'unavailable') or contains(.,'deleted') or contains(.,'removed'))]//a[contains(.,'YouTube')]", "href")]
    public required string? YouTubeUrl { get; init; }

    [SkipNodeNotFound]
    [XPath("//*[@data-source='links']/div/ul/li[not(contains(.,'private') or contains(.,'unavailable') or contains(.,'deleted') or contains(.,'removed'))]//a[contains(.,'SoundCloud')]", "href")]
    public required string? SoundCloudUrl { get; init; }

    [SkipNodeNotFound]
    [XPath("//*[@data-source='links']/div/ul/li[not(contains(.,'private') or contains(.,'unavailable') or contains(.,'deleted') or contains(.,'removed'))]//a[contains(.,'Niconico')]", "href")]
    public required string? NiconicoUrl { get; init; }

    [GeneratedRegex("""[^\S\r\n]*\(.*?\)|:""")]
    private static partial Regex InnerParenthesesOrColonRegex();

    [GeneratedRegex("""^\s*"|"[^"\r\n]*$""")]
    private static partial Regex FirstOrLastQuotationMarkRegex();
}