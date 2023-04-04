using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace HatsuneMiku.Shared.Types;

// Update interfaces
public readonly struct Percent : IComparable, IComparable<Percent>, IEquatable<Percent>, IFormattable // IConvertible
{
    //private const double MinValue = 0.0;
    //private const double MaxValue = 1.0;

    private readonly double _value;
    
    public static readonly Percent MinValue = 0.0;
    public static readonly Percent MaxValue = 1.0;

    public Percent(float value)
        : this((double)value)
    {
    }

    public Percent(double value) =>
        _value = value is < 0.0 or > 1.0
            ? throw new ArgumentOutOfRangeException(message: "Value must be between 0 (0.00%) and 1 (100.00%).", null)
            : value;

    public Percent(decimal value)
        : this((double)value)
    {
    }

    public int CompareTo(object? value) =>
        value switch
        {
            null            => 1,
            Percent percent => _value.CompareTo(percent._value),
            _               => throw new ArgumentException($"Object must be of type {nameof(Percent)}.")
        };

    public int CompareTo(Percent value) =>
        _value.CompareTo(value._value);

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Percent percent && _value.Equals(percent._value);

    // More overloads
    public bool Equals(Percent obj) =>
        _value.Equals(obj._value);
    
    public override int GetHashCode() =>
        _value.GetHashCode();

    public override string ToString() =>
        ToString(null, null);

    public string ToString(string? format) =>
        ToString(format, null);

    public string ToString(IFormatProvider? provider) =>
        ToString(null, provider);

    public string ToString(string? format, IFormatProvider? provider) =>
        format?.ToUpperInvariant() switch
        {
            var fmt when string.IsNullOrEmpty(fmt)              => _value.ToString("P", provider ?? CultureInfo.CurrentCulture),
            var fmt when fmt![0] is 'G' or 'P' or 'N' or 'F'    => _value.ToString(fmt.Replace("G", "P"), provider ?? CultureInfo.CurrentCulture),
            _                                                   => throw new FormatException("Format specifier was invalid.")
        };

    public static Percent Parse(string s) =>
        double.Parse(s);

    public static Percent Parse(string s, NumberStyles style) =>
        double.Parse(s, style);

    public static Percent Parse(string s, IFormatProvider? provider) =>
        double.Parse(s, provider);

    public static Percent Parse(string s, NumberStyles style, IFormatProvider? provider) =>
        double.Parse(s, style, provider);

    // Bound checks
    // Update method sometime
    public static bool TryParse([NotNullWhen(true)] string? s, out Percent result)
    {
        bool isSuccess = double.TryParse(s, out double percent);
        result = percent;

        return isSuccess;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Percent result)
    {
        bool isSuccess = double.TryParse(s, style, provider, out double percent);
        result = percent;

        return isSuccess;
    }

    public static Percent Add(Percent left, Percent right) =>
        left + right;

    public static Percent Subtract(Percent left, Percent right) =>
        left - right;

    //public static Percent Multiply(Percent left, Percent right) =>
    //    left * right;

    //public static Percent Divide(Percent left, Percent right) =>
    //    left / right;

    //public static Percent Remainder(Percent left, Percent right) =>
    //    left % right;

    public static bool TryAdd(Percent left, Percent right, out Percent result)
    {
        bool predicate = left._value + right._value is not (< 0.0 or > 1.0);
        result = predicate ? left + right : left;

        return predicate;
    }

    public static bool TrySubtract(Percent left, Percent right, out Percent result)
    {
        bool predicate = left._value - right._value is not (< 0.0 or > 1.0);
        result = predicate ? left - right : left;

        return predicate;
    }

    // etc.
    
    // Round, Floor, Ceiling, etc.

    public static implicit operator Percent(float value) =>
        new(value);

    public static implicit operator Percent(double value) =>
        new(value);

    // implicit/explicit for the sake of ToString()?
    public static explicit operator double(Percent value) =>
        value._value;

    public static explicit operator float(Percent value) =>
        (float)value._value;

    public static explicit operator decimal(Percent value) =>
        (decimal)value._value;

    public static explicit operator Percent(decimal value) =>
        new((double)value);

    public static bool operator ==(Percent left, Percent right) =>
        left._value == right._value;

    public static bool operator !=(Percent left, Percent right) =>
        left._value != right._value;

    public static bool operator <(Percent left, Percent right) =>
        left._value < right._value;

    public static bool operator >(Percent left, Percent right) =>
        left._value > right._value;

    public static bool operator <=(Percent left, Percent right) =>
        left._value <= right._value;

    public static bool operator >=(Percent left, Percent right) =>
        left._value >= right._value;

    // Bound checks
    public static Percent operator +(Percent left, Percent right) =>
        left._value + right._value;

    public static Percent operator -(Percent left, Percent right) =>
        left._value - right._value;

    //public static Percent operator *(Percent left, Percent right) =>
    //    left._value * right._value;

    //public static Percent operator /(Percent left, Percent right) =>
    //    left._value / right._value;

    //public static Percent operator %(Percent left, Percent right) =>
    //    left._value % right._value;

    // Add step param
    public static Percent operator ++(Percent value) =>
        value._value + 1;

    public static Percent operator --(Percent value) =>
        value._value - 1;
}