using System.Globalization;

namespace MaterialDesignDemo.Maui.PaletteSelector;

public sealed class ResourceColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string key || Application.Current is null)
        {
            return Colors.Transparent;
        }

        if (Application.Current.Resources.TryGetValue(key, out var resource))
        {
            if (resource is Color color)
            {
                return color;
            }

            if (resource is SolidColorBrush brush)
            {
                return brush.Color;
            }
        }

        return Colors.Transparent;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException("ResourceColorConverter does not support ConvertBack.");
    }
}
