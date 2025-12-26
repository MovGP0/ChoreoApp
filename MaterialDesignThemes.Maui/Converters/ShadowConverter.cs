using System.Collections.ObjectModel;
using System.Globalization;
using Sharpnado.Shades;

namespace MaterialDesignThemes.Maui.Converters;

public sealed class ShadowConverter : IValueConverter
{
    public static readonly ShadowConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            Elevation elevation => ToTarget(targetType, CreateShade(ElevationAssist.GetShadow(elevation))),
            Shadow shadow => ToTarget(targetType, CreateShade(shadow)),
            _ => null
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public static Shadow? Convert(Elevation elevation) => ElevationAssist.GetShadow(elevation);

    private static Shade? CreateShade(Shadow? shadow)
    {
        if (shadow is null)
        {
            return null;
        }

        var color = Colors.Black;
        if (shadow.Brush is SolidColorBrush solidColorBrush)
        {
            color = solidColorBrush.Color;
        }

        return new Shade
        {
            Offset = shadow.Offset,
            BlurRadius = shadow.Radius,
            Opacity = shadow.Opacity,
            Color = color
        };
    }

    private static object? ToTarget(Type targetType, Shade? shade)
    {
        if (shade is null)
        {
            return null;
        }

        if (targetType == typeof(Shade) || targetType == typeof(object))
        {
            return shade;
        }

        if (typeof(IEnumerable<Shade>).IsAssignableFrom(targetType)
            || typeof(IReadOnlyCollection<Shade>).IsAssignableFrom(targetType))
        {
            return new ReadOnlyCollection<Shade>([shade]);
        }

        return shade;
    }
}
