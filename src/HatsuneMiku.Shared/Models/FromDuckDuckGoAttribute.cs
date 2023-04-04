using GScraper.DuckDuckGo;
using System;

namespace HatsuneMiku.Shared.Models;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class FromDuckDuckGoAttribute : Attribute
{
    public DuckDuckGoImageTime Time { get; }
    public DuckDuckGoImageSize Size { get; }
    public DuckDuckGoImageColor Color { get; }
    public DuckDuckGoImageType Type { get; }
    public DuckDuckGoImageLayout Layout { get; }
    public DuckDuckGoImageLicense License { get; }
    public string Region { get; }

    public FromDuckDuckGoAttribute(
        DuckDuckGoImageTime time        = DuckDuckGoImageTime.Any,
        DuckDuckGoImageSize size        = DuckDuckGoImageSize.All,
        DuckDuckGoImageColor color      = DuckDuckGoImageColor.All,
        DuckDuckGoImageType type        = DuckDuckGoImageType.All,
        DuckDuckGoImageLayout layout    = DuckDuckGoImageLayout.All,
        DuckDuckGoImageLicense license  = DuckDuckGoImageLicense.All,
        string region                   = DuckDuckGoRegions.UsEnglish)
    {
        Time    = time;
        Size    = size;
        Color   = color;
        Type    = type;
        Layout  = layout;
        License = license;
        Region  = region;
    }

    public void Deconstruct(
        out DuckDuckGoImageTime time,
        out DuckDuckGoImageSize size,
        out DuckDuckGoImageColor color,
        out DuckDuckGoImageType type,
        out DuckDuckGoImageLayout layout,
        out DuckDuckGoImageLicense license,
        out string region)
    {
        time    = Time;
        size    = Size;
        color   = Color;
        type    = Type;
        layout  = Layout;
        license = License;
        region  = Region;
    }
}