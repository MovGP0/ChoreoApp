using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MaterialDesignThemes.Maui;

internal sealed class InheritSystemColorTypeConverter : TypeConverter
{
    private const string Inherit = "Inherit";

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType) =>
        destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        if (value is null)
        {
            throw GetConvertFromException(value);
        }

        if (value is string text)
        {
            if (string.Equals(text, Inherit, StringComparison.OrdinalIgnoreCase))
            {
                return GetSystemAccentColor() ?? null;
            }

            if (Color.TryParse(text, out var parsed))
            {
                return parsed;
            }
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is Color color &&
            color != null &&
            color == GetSystemAccentColor())
        {
            return Inherit;
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }

    private static Color? GetSystemAccentColor()
    {
        if (Application.Current?.Resources is not ResourceDictionary resources)
        {
            return null;
        }

        if (resources.TryGetValue(MaterialDesignColorKey.Primary, out var resource) &&
            resource is Color color)
        {
            return color;
        }

        return null;
    }
}
