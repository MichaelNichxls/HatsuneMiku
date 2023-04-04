using GScraper.Google;
using System;

namespace HatsuneMiku.Shared.Models;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class FromGoogleAttribute : Attribute
{
    public GoogleImageSize Size { get; }
    public string? Color { get; }
    public GoogleImageType Type { get; }
    public GoogleImageTime Time { get; }
    public string? License { get; }
    public string? Language { get; }

    public FromGoogleAttribute(
        GoogleImageSize size    = GoogleImageSize.Any,
        string? color           = null,
        GoogleImageType type    = GoogleImageType.Any,
        GoogleImageTime time    = GoogleImageTime.Any,
        string? license         = null,
        string? language        = null)
    {
        Size        = size;
        Color       = color;
        Type        = type;
        Time        = time;
        License     = license;
        Language    = language;
    }

    public void Deconstruct(
        out GoogleImageSize size,
        out string? color,
        out GoogleImageType type,
        out GoogleImageTime time,
        out string? license,
        out string? language)
    {
        size        = Size;
        color       = Color;
        type        = Type;
        time        = Time;
        license     = License;
        language    = Language;
    }
}