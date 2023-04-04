using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HatsuneMiku.Data.Converters;

public sealed class ColorConverter : ValueConverter<Color, KnownColor>
{
    public ColorConverter()
        : base(
            c => c.ToKnownColor(),
            kc => Color.FromKnownColor(kc))
    {
    }
}