using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Globalization;

namespace ChoreoMasterMobile.Json;

[System.ComponentModel.TypeConverter(typeof(DancerIdTypeConverter))]
[JsonConverter(typeof(DancerIdSystemTextJsonConverter))]
public readonly struct DancerId(int value):
    ISpanFormattable,
    IParsable<DancerId>,
    ISpanParsable<DancerId>,
    IUtf8SpanParsable<DancerId>,
    IUtf8SpanFormattable,
    IComparable<DancerId>,
    IEquatable<DancerId>,
    IFormattable
{
    public int Value { get; } = value;

    public static readonly DancerId Empty = new(0);

    /// <inheritdoc cref="global::System.IEquatable{T}"/>
    public bool Equals(DancerId other) => Value.Equals(other.Value);
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is DancerId other && Equals(other);
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

    public static bool operator ==(DancerId a, DancerId b) => a.Equals(b);
    public static bool operator !=(DancerId a, DancerId b) => !(a == b);
    public static bool operator >  (DancerId a, DancerId b) => a.CompareTo(b) > 0;
    public static bool operator <  (DancerId a, DancerId b) => a.CompareTo(b) < 0;
    public static bool operator >=  (DancerId a, DancerId b) => a.CompareTo(b) >= 0;
    public static bool operator <=  (DancerId a, DancerId b) => a.CompareTo(b) <= 0;
    public static explicit operator int(DancerId value) => value.Value;
    public static implicit operator DancerId(int value) => new(value);

    /// <inheritdoc cref="global::System.IComparable{TSelf}"/>
    public int CompareTo(DancerId other) => Value.CompareTo(other.Value);

    public static DancerId Parse(string input)
        => new(int.Parse(input));

    /// <inheritdoc cref="global::System.IParsable{TSelf}"/>
    public static DancerId Parse(string input, IFormatProvider? provider)
        => new(int.Parse(input, provider));

    /// <inheritdoc cref="global::System.IParsable{TSelf}"/>
    public static bool TryParse(
        [NotNullWhen(true)] string? input,
        IFormatProvider? provider,
        out DancerId result)
    {
        if (input is null)
        {
            result = default;
            return false;
        }

        if (int.TryParse(input, provider, out var value))
        {
            result = new(value);
            return true;
        }

        result = default;
        return false;
    }


    /// <inheritdoc cref="global::System.IFormattable"/>
    public string ToString(
        [StringSyntax(StringSyntaxAttribute.NumericFormat)]
        string? format,
        IFormatProvider? formatProvider)
        => Value.ToString(format, formatProvider);

    public static DancerId Parse(ReadOnlySpan<char> input)
        => new(int.Parse(input));

    /// <inheritdoc cref="global::System.ISpanParsable{TSelf}"/>
    public static DancerId Parse(ReadOnlySpan<char> input, IFormatProvider? provider)
        => new(int.Parse(input, provider));

    /// <inheritdoc cref="global::System.ISpanParsable{TSelf}"/>
    public static bool TryParse(ReadOnlySpan<char> input, IFormatProvider? provider, out DancerId result)
    {
        if (int.TryParse(input, provider, out var value))
        {
            result = new(value);
            return true;
        }

        result = default;
        return false;
    }

    /// <inheritdoc cref="global::System.ISpanFormattable"/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        [StringSyntax(StringSyntaxAttribute.NumericFormat)]
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => Value.TryFormat(destination, out charsWritten, format);

    /// <inheritdoc cref="global::System.ISpanFormattable"/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        [StringSyntax(StringSyntaxAttribute.NumericFormat)]
        ReadOnlySpan<char> format = default)
        => Value.TryFormat(destination, out charsWritten, format);

    /// <inheritdoc cref="global::System.IUtf8SpanFormattable.TryFormat" />
    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        [StringSyntax(StringSyntaxAttribute.NumericFormat)]
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null)
        => Value.TryFormat(utf8Destination, out bytesWritten, format, provider);

    /// <inheritdoc cref="global::System.IUtf8SpanParsable{TSelf}.Parse(global::System.ReadOnlySpan{byte}, global::System.IFormatProvider?)" />
    public static DancerId Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
        => new(int.Parse(utf8Text, provider));

    /// <inheritdoc cref="global::System.IUtf8SpanParsable{TSelf}.TryParse(global::System.ReadOnlySpan{byte}, global::System.IFormatProvider?, out TSelf)" />
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out DancerId result)
    {
        if (int.TryParse(utf8Text, provider, out var intResult))
        {
            result = new DancerId(intResult);
            return true;
        }

        result = default;
        return false;
    }
}
