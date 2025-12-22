using System.Collections.ObjectModel;
using System.Globalization;
using Sharpnado.Shades;

namespace ChoreoApp.Styling.Converters;

public sealed class ShadowOpacityMaskConverter : IMultiValueConverter
{
    public static readonly ShadowOpacityMaskConverter Instance = new();

    public object? Convert(object[]? values, Type targetType, object? parameter, CultureInfo culture)
    {
        static double? GetValidSize(object? value)
        {
            return value is double d && !double.IsNaN(d) && !double.IsInfinity(d) ? d : null;
        }

        static Shadow? GetShadow(object? value)
        {
            return value switch
            {
                Elevation elevation => ElevationAssist.GetShadow(elevation),
                Shadow shadow => shadow,
                _ => null
            };
        }

        if (values is null
            || values.Length < 3
            || GetValidSize(values[0]) is null
            || GetValidSize(values[1]) is null
            || GetShadow(values[2]) is not { } shadow)
        {
            return null;
        }

        var shade = CreateShade(shadow);
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

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static Shade CreateShade(Shadow shadow)
    {
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
}
