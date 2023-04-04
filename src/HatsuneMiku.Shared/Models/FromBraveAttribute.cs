using GScraper.Brave;
using System;

namespace HatsuneMiku.Shared.Models;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class FromBraveAttribute : Attribute
{
    public string? Country { get; }
    public BraveImageSize Size { get; }
    public BraveImageType Type { get; }
    public BraveImageLayout Layout { get; }
    public BraveImageColor Color { get; }
    public BraveImageLicense License { get; }

    public FromBraveAttribute(
        string? country             = null,
        BraveImageSize size         = BraveImageSize.All,
        BraveImageType type         = BraveImageType.All,
        BraveImageLayout layout     = BraveImageLayout.All,
        BraveImageColor color       = BraveImageColor.All,
        BraveImageLicense license   = BraveImageLicense.All)
    {
        Country = country;
        Size    = size;
        Type    = type;
        Layout  = layout;
        Color   = color;
        License = license;
    }

    public void Deconstruct(
        out string? country,
        out BraveImageSize size,
        out BraveImageType type,
        out BraveImageLayout layout,
        out BraveImageColor color,
        out BraveImageLicense license)
    {
        country = Country;
        size    = Size;
        type    = Type;
        layout  = Layout;
        color   = Color;
        license = License;
    }
}