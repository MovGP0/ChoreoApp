using System.Globalization;

namespace MaterialDesignDemo.Maui.ColorTool;

public sealed class ColorToHexConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Color color)
        {
            return string.Empty;
        }

        var red = (byte)Math.Round(color.Red * 255d);
        var green = (byte)Math.Round(color.Green * 255d);
        var blue = (byte)Math.Round(color.Blue * 255d);

        return $"#{red:X2}{green:X2}{blue:X2}";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException("ColorToHexConverter does not support ConvertBack.");
    }
}
